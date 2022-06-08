using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using G_API.Models;
using Newtonsoft.Json;


namespace G_API.Clients
{
    public class SongsterClient
    {
        private HttpClient _client;
        private static string _adress;
        
        public SongsterClient()
        {
            _adress = Constants.adressSongster;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_adress);
        }
        public async Task<List<SongsterSong>> GetSong(string pattern)
        {
            var responce = await _client.GetAsync($"/a/ra/songs.json?pattern={pattern}");
            responce.EnsureSuccessStatusCode();

            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<SongsterSong>>(content);

            return result;
        }
    }
}
