
using G_API.Clients;
using G_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class G_APIController : ControllerBase
    {
        private readonly ILogger<G_APIController> _logger;
        private readonly SongsterClient _songsterClient;
        public G_APIController(ILogger<G_APIController> logger, SongsterClient songsterClient)
        {
            _logger = logger;
            _songsterClient = songsterClient;
        }

        [HttpGet("songs")]
        public async Task<List<SongsterSong>> GetSongsByPattern(string pattern)
        {
            var songsList = await _songsterClient.GetSong(pattern);

            return songsList;
        }
    }
}
 