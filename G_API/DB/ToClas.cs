using Amazon.DynamoDBv2.Model;
using G_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G_API
{
    public static class ToClas
    {
        public static SongResponse MToSR(AttributeValue resp)
        {
            var song = new SongResponse
            {
                difficulty = resp.M["difficulty"].S,
                YoutubeURL = resp.M["YoutubeURL"].S,
                genre = resp.M["genre"].S,
                ArtistName = resp.M["ArtistName"].S,
                Title = resp.M["Title"].S,
                URL = resp.M["URL"].S
            };
            return song;
        }
        public static UserSongs MToUS(GetItemResponse resp)
        {
            var result = new UserSongs();

            result.ID = resp.Item["ID"].S;
            result.Songs = new List<SongResponse>();
            foreach (var s in resp.Item["Songs"].L)
            {
                var song = MToSR(s);
                result.Songs.Add(song);
            }

            return result;
        }
        public static Dictionary<string, AttributeValue> ToM(SongResponse song)
        {
            var M = new Dictionary<string, AttributeValue>
            {
                ["ArtistName"] = new AttributeValue { S = song.ArtistName },
                ["difficulty"] = new AttributeValue { S = song.difficulty },
                ["genre"] = new AttributeValue { S = song.genre },
                ["Title"] = new AttributeValue { S = song.Title },
                ["URL"] = new AttributeValue { S = song.URL },
                ["YoutubeURL"] = new AttributeValue { S = song.YoutubeURL },
            };
            return M;
        }
    }
}
