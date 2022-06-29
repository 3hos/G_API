
using G_API.Clients;
using G_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class G_APIController : ControllerBase
    {
        private readonly ILogger<G_APIController> _logger;
        private readonly SongsterClient _songsterClient;
        private readonly ChordsClient _chordsClient;
        private readonly SpotifyClient _SpotifyClient;
        private readonly SongDBClient _songDBClient;
        public G_APIController(ILogger<G_APIController> logger, SongsterClient songsterClient, ChordsClient chordsClient, SpotifyClient spotifyClient, SongDBClient songDBClient)
        {
            _logger = logger;
            _songsterClient = songsterClient;
            _chordsClient = chordsClient;
            _SpotifyClient = spotifyClient;
            _songDBClient = songDBClient;
        }

        [HttpGet("songs")]
        public async Task<List<SongResponse>> GetSongsByPattern(string pattern, int number, string user)
        {
            var songsList = await _songsterClient.GetSong(pattern);
            var response = new List<SongResponse>();
            if (number > songsList.Count()) number = songsList.Count(); 
            for (int i=0;i<number;i++)
            {
                response.Add(new SongResponse(songsList[i].Title, songsList[i].Artist.Name, Constants.URLSong + songsList[i].ID));
                response[i].YoutubeURL = $"https://www.youtube.com/results?search_query={response[i].ArtistName.Replace(' ','+')}+{response[i].Title.Replace(' ','+')}+guitar+tutorial";
                response[i].genre = await _songDBClient.GetSong(response[i].ArtistName, response[i].Title);
                response[i].difficulty = Parcer.GetDificulty(response[i].URL);
                try
                {
                var song = await _SpotifyClient.GetSong(songsList[i].Title);
                response[i].genres = await _SpotifyClient.GetArtist(song.Artists[0].ID);
                }
                catch { }
                if (response[i].genre == "Unknown" && response[i].genres!=null)
                {
                    if (response[i].genres.Count>=1)
                    {
                        var rnd = new Random();
                        response[i].genre = "Probably " + response[i].genres[rnd.Next(0, response[i].genres.Count - 1)];
                    }
                }
            }
            try
            {
                if (!USERS.ListOfLast.ContainsKey(user)) USERS.ListOfLast.Add(user, new List<SongResponse>());
                USERS.ListOfLast[user] = response;
            }
            catch { }
            return response;
        }
        [HttpGet("favorites")]
        public List<SongResponse> GetFavorites(string user)
        {
            if (!USERS.ListOfLast.ContainsKey(user)) USERS.ListOfLast.Add(user, new List<SongResponse>());
            if (!USERS.ListOfUsers.ContainsKey(user)) USERS.ListOfUsers.Add(user, new List<SongResponse>());
            USERS.ListOfLast[user] = USERS.ListOfUsers[user];
            return USERS.ListOfUsers[user];
        }
        [HttpGet("addtofav")]
        public void AddToFav(string user, int number) 
        {
            if (number<= USERS.ListOfLast[user].Count)
            {
                if (!USERS.ListOfUsers.ContainsKey(user)) USERS.ListOfUsers.Add(user, new List<SongResponse>());
                USERS.ListOfUsers[user].Add(USERS.ListOfLast[user][number-1]);
                USERS.Save();
            }
        }
        [HttpDelete("delete")]
        public void DellFromFav(string user, int number)
        {

            if (number<= USERS.ListOfUsers[user].Count)
            {
                USERS.ListOfUsers[user].RemoveAt(number-1);
                USERS.Save();
            }
        }
        [HttpGet("chord")]
        public async Task<List<ChordResponse>> GetChord(string chord)
        {
            var response = await _chordsClient.GetChord(Chords.ChordName(chord));

            return response;
        }
        [HttpGet("chords")]
        public async Task<List<ChordResponse>> GetLike(string chord)
        {
            var response = await _chordsClient.GetLike(Chords.ChordName(chord));

            return response;
        }
        [HttpGet("recommendations")]
        public async Task<List<SongResponse>> GetRecommendations(string user)
        {
            var rnd = new Random();
            var track = USERS.ListOfUsers[user][rnd.Next(0, USERS.ListOfUsers[user].Count)];
            var song = await _SpotifyClient.GetSong(track.Title);
            var tracks = song.ID;
            var artists = song.Artists[0].ID;
            var genres = track.genres[rnd.Next(0, track.genres.Count)];
            var recks = await _SpotifyClient.GetRecommendations(artists, genres, tracks);
            List<SongResponse> response = new List<SongResponse>();
            foreach(var reck in recks)
            {
                var new_reck = await GetSongsByPattern(reck.Name, 1,user);
                if(new_reck.Count>=1) response.Add(new_reck[0]);
            }
            if (!USERS.ListOfLast.ContainsKey(user)) USERS.ListOfLast.Add(user, new List<SongResponse>());
            USERS.ListOfLast[user] = response;
            return response;
        }
    }
}
 