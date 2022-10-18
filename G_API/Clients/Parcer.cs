using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace G_API.Clients
{
    public class Parcer
    {
        static string GetHtmlPage(string url)
        {
            var HtmlText = string.Empty;
            HttpWebRequest myHttwebrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebresponse = (HttpWebResponse)myHttwebrequest.GetResponse();
            StreamReader strm = new StreamReader(myHttpWebresponse.GetResponseStream());
            HtmlText = strm.ReadToEnd();
            return HtmlText;
        }
        static string ParDificulty(string HtmlText)
        {
            string HtmlDif = HtmlText;
            string patternImg = @"<span id=.track-difficulty. (.*) (.*) title=.(.*) Tier .";
            string pattern = @"title=.(.*) Tier .";
            MatchCollection matches = Regex.Matches(HtmlDif, patternImg, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var match = matches[0].ToString();
                matches = Regex.Matches(match, pattern, RegexOptions.IgnoreCase);
                return matches[0].ToString().Replace("title=\"", "");
            }
            else return null;
        }
        public static string GetDificulty(string url)
        {
            var HtmlText = GetHtmlPage(url);
            var response = ParDificulty(HtmlText);
            return response;
        }
    }
}
