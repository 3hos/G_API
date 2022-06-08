using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G_API.Models
{
    /*
      [
   {
      "id":33984,
      "title":"Karn Evil 9 Part 2",
      "artist":{
         "id":2149,
         "name":"Emerson, Lake & Palmer"
      },
      "chordsPresent":false,
   },
   {
      "id":407134,
      "title":"Karn doen thang",
      "artist":{
         "id":54714,
         "name":"การเดินทาง"
      },
      "chordsPresent":false,
   }
]
     */
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
