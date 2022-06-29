using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace G_API.Clients
{
    public class ChordsClient
    {
        private HttpClient _client;
        private static string _adress;

        public ChordsClient()
        {
            _adress = Constants.adressChords;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_adress);
        }
        public async Task<List<Models.ChordResponse>> GetChord(string chord)
        {
            var responce = await _client.GetAsync($"chords?names={chord}");
            responce.EnsureSuccessStatusCode();

            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<Models.ChordResponse>>(content);
            return result;
        }
        public async Task<List<Models.ChordResponse>> GetLike(string chord)
        {
            var responce = await _client.GetAsync($"chords?nameLike={chord}");
            responce.EnsureSuccessStatusCode();

            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<Models.ChordResponse>>(content);
            return result;
        }
    }
}
