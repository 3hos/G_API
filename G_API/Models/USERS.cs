using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace G_API.Models
{
    public class USERS
    {
        public USERS()
        {
            ListOfUsers = new Dictionary<string, List<SongResponse>>();
            ListOfLast = new Dictionary<string, List<SongResponse>>();
        }

        public static Dictionary<string,List<SongResponse>> ListOfUsers { get; set; }
        public static Dictionary<string, List<SongResponse>> ListOfLast { get; set; }

        public static void Save()
        {
            string Js = JsonSerializer.Serialize(ListOfUsers);
            File.WriteAllText("users.json", Js);
        }
        public static void Open()
        {
            var Js = File.ReadAllText("users.json");
            ListOfUsers = JsonSerializer.Deserialize<Dictionary<string, List<SongResponse>>>(Js);
        }
    }
}
