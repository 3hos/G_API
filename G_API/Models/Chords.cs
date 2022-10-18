using System.Collections.Generic;

namespace G_API.Models
{
    public class Chords
    {
        public static List<string> roots = new List<string> { "A", "Ab", "B", "Bb", "C", "D", "Db", "E", "Eb", "F", "G", "Gb" };

        public static string ChordName(string chord)
        {
            string root = "";
            string quality = "";
            string bass = "";
            foreach (var R in roots)
            {
                if (chord.StartsWith(R)) root = R;
                if (chord.Contains("/" + R)) bass = R;
            }
            quality = chord.Replace(root, "").Replace("/" + bass, "");
            var result = $"{root}_{quality}_{bass}";
            return result;
        }
    }
}
