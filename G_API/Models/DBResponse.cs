using System.Collections.Generic;

namespace G_API.Models
{
    public class DBResponse
    {
        public List<Track> track { get; set; }
    }
    public class Track
    {
        public string StrGenre { get; set; }
    }
}
