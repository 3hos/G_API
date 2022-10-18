using G_API.Models;
using System.Threading.Tasks;

namespace G_API.DB
{
    public interface IDBClient
    {
        public Task<UserSongs> GetUser(string user);
        public Task<bool> AddUser(string user);
        public Task<bool> AddToFav(string user, SongResponse song);
        public Task<bool> DelFromFav(string user, int n);

    }
}
