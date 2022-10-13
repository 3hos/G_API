
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using G_API.Clients;
using G_API.DB;
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
        private readonly DBClient _dBClient;

        public G_APIController(ILogger<G_APIController> logger, SongsterClient songsterClient, ChordsClient chordsClient, SpotifyClient spotifyClient, DBClient dBClient)
        {
            _logger = logger;
            _songsterClient = songsterClient;
            _chordsClient = chordsClient;
            _SpotifyClient = spotifyClient;
            _dBClient = dBClient;
        }

        [HttpGet("songs")]
        public async Task<List<SongResponse>> GetSongsByPattern(string pattern, int number, string user)
        {
            var songsList = await _songsterClient.GetSong(pattern);
            var response = new List<SongResponse>();
            if (number > songsList.Count()) number = songsList.Count();
            for (int i = 0; i < number; i++)
            {
                response.Add(new SongResponse(songsList[i].Title, songsList[i].Artist.Name, Constants.URLSong + songsList[i].ID));
                response[i].YoutubeURL = $"https://www.youtube.com/results?search_query={response[i].ArtistName.Replace(' ', '+')}+{response[i].Title.Replace(' ', '+')}+guitar+tutorial";
                response[i].difficulty = Parcer.GetDificulty(response[i].URL);
                try
                {
                    var song = await _SpotifyClient.GetSong(songsList[i].Title);
                    response[i].genre = _SpotifyClient.GetArtist(song.Artists[0].ID).Result[0];
                }
                catch { }
            }
                if (!USERS.ListOfLast.ContainsKey(user)) USERS.ListOfLast.Add(user, new List<SongResponse>());
                USERS.ListOfLast[user] = response;
            return response;
        }
        [HttpGet("favorites")]
        public async Task<List<SongResponse>> GetFavorites(string user)
        {
            if (!USERS.ListOfLast.ContainsKey(user)) USERS.ListOfLast.Add(user, new List<SongResponse>());
            var fav = await _dBClient.GetUser(user);
            USERS.ListOfLast[user] = fav.Songs;
            return fav.Songs;
        }
        [HttpPost("addtofav")]
        public async Task AddToFav(string user, int number)
        {
            if (number <= USERS.ListOfLast[user].Count)
            {
                var resp = await _dBClient.AddToFav(user, USERS.ListOfLast[user][number - 1]);
                if (!resp) throw new InternalServerErrorException("Failed to add");
            }
            else throw new InternalServerErrorException("Incorrect data");
        }
        [HttpDelete("delete")]
        public async Task DellFromFavAsync(string user, int number)
        {
            var resp = await _dBClient.DelFromFav(user, number);
            if(!resp) throw new InternalServerErrorException("Failed to delete");
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
            var list = await _dBClient.GetUser(user);
            var track = list.Songs[rnd.Next(0, list.Songs.Count)];
            var song = await _SpotifyClient.GetSong(track.Title);
            var tracks = song.ID;
            var artists = song.Artists[0].ID;
            var genres = track.genre;
            var recks = await _SpotifyClient.GetRecommendations(artists, genres, tracks);
            List<SongResponse> response = new List<SongResponse>();
            foreach (var reck in recks)
            {
                var new_reck = await GetSongsByPattern(reck.Name, 1, user);
                if (new_reck.Count >= 1) response.Add(new_reck[0]);
            }
            if (!USERS.ListOfLast.ContainsKey(user)) USERS.ListOfLast.Add(user, new List<SongResponse>());
            USERS.ListOfLast[user] = response;
            return response;
        }
    }
}
 