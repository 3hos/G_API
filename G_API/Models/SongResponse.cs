using System.Collections.Generic;

namespace G_API.Models
{
    public class SongResponse
    {
        public SongResponse(string title, string artistName, string uRL)
        {
            Title = title;
            ArtistName = artistName;
            URL = uRL;
        }
        public SongResponse() { }

        public string Title { get; set; }
        public string ArtistName { get; set; }
        public string URL { get; set; }
        public string genre { get; set; }
        public string difficulty { get; set; }
        public string YoutubeURL { get; set; }

    }
    public class UserSongs
    {
        public string ID { get; set; }
        public List<SongResponse> Songs { get; set; }
    }
}
