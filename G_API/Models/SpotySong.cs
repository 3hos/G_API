using System.Collections.Generic;

namespace G_API.Models
{
    public class SpotyRecks
    {
        public List<Items> Tracks { get; set; }
    }
    public class SpotySong
    {
        public Tracks Tracks { get; set; }
    }
    public class Tracks
    {
        public List<Items> Items { get; set; }
    }
    public class Items
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public List<Artists> Artists { get; set; }
    }
    public class Artists
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public List<string> genres { get; set; }
    }
}