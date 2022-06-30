using G_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace G_API.Clients
{
    public class SongDBClient
    {
        private HttpClient _client;
        private static string _adress;

        public SongDBClient()
        {
            _adress = Constants.DBadress;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_adress);
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "05f7b97b62mshe122da26f10b378p1da22fjsn5fd0a5899841");
        }
        public async Task<string> GetSong(string artist, string song)
        {
            try
            {
                var responce = await _client.GetAsync($"searchtrack.php?s={artist}&t={song}");
                responce.EnsureSuccessStatusCode();

                var content = responce.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<DBResponse>(content);
                string genre = "Unknown";
                if (res.track != null) if (res.track[0].StrGenre != null) genre = res.track[0].StrGenre;
                return genre;
            }
            catch { return "Unknown"; }
        }
    }
}
