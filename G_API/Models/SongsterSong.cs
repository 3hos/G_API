namespace G_API.Models
{
    public class SongsterSong
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public Artist Artist { get; set; }
        public bool ChordsPresent { get; set; }
    }
    public class Artist
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
