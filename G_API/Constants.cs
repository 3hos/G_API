using System;

namespace G_API
{
    public class Constants
    {
        public static string adressChords = "https://api.uberchord.com/v1/";
        public static string adressSongster = "http://www.songsterr.com";
        public static string URLSong = "http://www.songsterr.com/a/wa/song?id=";
        public static string APIadress = "https://localhost:44377/G_API/";
        public static string DBadress = "https://theaudiodb.p.rapidapi.com/";
        public static string adressSpotify = "https://api.spotify.com/v1/";

        public static string SpotyClientID = "0956e9d19657409b89d3b7aaf76690d3";
        public static string SpotySecret = Environment.GetEnvironmentVariable("SpotySecret");

        public static string DB_ID = "AKIA2N4FKUFKYHXI6BUQ";
        public static string DB_Secret = Environment.GetEnvironmentVariable("DB_Secret");
        public static string DB_Table_name = "G_API";
    }
}