using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
