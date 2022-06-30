using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace G_API.Clients
{
    public class SpotifyClient
    {
        private HttpClient _client;
        private static string _adress;

        public SpotifyClient()
        {
            _adress = Constants.adressSpotify;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_adress);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());

        }
        public async Task<Models.Items> GetSong(string song)
        {
            var responce = await _client.GetAsync($"search?q={song}&type=track&limit=5&include_external=true");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var spoty = JsonConvert.DeserializeObject<Models.SpotySong>(content);
            var result = spoty.Tracks.Items[0];
            return result;
        }
        public async Task<List<string>> GetArtist(string id)
        {
            var responce = await _client.GetAsync($"artists/{id}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Models.Artists>(content);
            return result.genres;
        }
        public async Task<List<Models.Items>> GetRecommendations(string artists,string genres,string tracks, int limit=5)
        {
            var responce = await _client.GetAsync($"recommendations?limit={limit}&market=UA&seed_artists={artists}&seed_genres={genres}&seed_tracks={tracks}");
            responce.EnsureSuccessStatusCode();
            if (!responce.IsSuccessStatusCode)
            {
                try
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                    responce = await _client.GetAsync($"recommendations?limit={limit}&market=UA&seed_artists={artists}&seed_genres={genres}&seed_tracks={tracks}");
                    responce.EnsureSuccessStatusCode();
                }
                catch { }
            }
            var content = responce.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<Models.SpotyRecks>(content);
            return res.Tracks;
        }

        public string GetAccessToken()
        {
            SpotifyToken token = new SpotifyToken();
            string url5 = "https://accounts.spotify.com/api/token";
            var clientid = Constants.SpotyClientID;
            var clientsecret = Constants.SpotySecret;

            //request to get the access token
            var encode_clientid_clientsecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", clientid, clientsecret)));

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url5);

            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add("Authorization: Basic " + encode_clientid_clientsecret);

            var request = ("grant_type=client_credentials");
            byte[] req_bytes = Encoding.ASCII.GetBytes(request);
            webRequest.ContentLength = req_bytes.Length;

            Stream strm = webRequest.GetRequestStream();
            strm.Write(req_bytes, 0, req_bytes.Length);
            strm.Close();

            HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
            String json = "";
            using (Stream respStr = resp.GetResponseStream())
            {
                using (StreamReader rdr = new StreamReader(respStr, Encoding.UTF8))
                {
                    //should get back a string i can then turn to json and parse for accesstoken
                    json = rdr.ReadToEnd();
                    rdr.Close();
                }
            }
            token = JsonConvert.DeserializeObject<SpotifyToken>(json);
            return token.access_token;
        }

        private class SpotifyToken
        {
            public string access_token { get; set; }
        }
    }
}
