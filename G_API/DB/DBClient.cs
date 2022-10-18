using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using G_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G_API.DB
{
    public class DBClient : IDBClient
    {
        public string _tableName;
        private readonly IAmazonDynamoDB _dynamoDB;
        public DBClient(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
            _tableName = Constants.DB_Table_name;
        }

        public async Task<UserSongs> GetUser(string user)
        {
            var item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"ID", new AttributeValue{S = user }}
                }
            };
            var response = await _dynamoDB.GetItemAsync(item);
            if (response.Item == null || !response.IsItemSet)
            {
                if (!AddUser(user).Result) throw new InternalServerErrorException("Failed to add a new user");
                return null;
            }

            var result = ToClas.MToUS(response);
            return result;
        }


        public async Task<bool> AddUser(string user)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["ID"] = new AttributeValue { S = user },
                ["Songs"] = new AttributeValue { L = new List<AttributeValue>(), IsLSet = true },
            };

            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = item,
            };
            var response = await _dynamoDB.PutItemAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> AddToFav(string user, SongResponse song)
        {
            var key = new Dictionary<string, AttributeValue>
            {
                ["ID"] = new AttributeValue { S = user },
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["Songs"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.ADD,
                    Value = new AttributeValue
                    {
                        L = new List<AttributeValue>
                        {   new AttributeValue {M = ToClas.ToM(song)}   }
                    }
                },
            };
            var request = new UpdateItemRequest
            {
                AttributeUpdates = updates,
                Key = key,
                TableName = _tableName,
            };

            var response = await _dynamoDB.UpdateItemAsync(request);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> DelFromFav(string user, int n)
        {
            var key = new Dictionary<string, AttributeValue>
            {
                ["ID"] = new AttributeValue { S = user },
            };
            var request = new UpdateItemRequest
            {
                TableName = _tableName,
                Key = key,

                UpdateExpression = $"REMOVE Songs[{n}]"
            };
            var response = await _dynamoDB.UpdateItemAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
