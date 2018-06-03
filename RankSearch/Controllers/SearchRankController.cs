using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using RankSearch.Models;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Text;

namespace RankSearch.Controllers
{
    public class SearchRankController : Controller
    {
        // GET: SearchRank
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(string search_str)
        {          
            Uri url = new Uri("https://www.alifery.com.au");

            int rank = GetPosition(url, search_str);

            Models.CompanyInfo company = new Models.CompanyInfo { CompanyURL=url,SearchString= search_str, Rank=rank };

            return View("Result", company);
        }

        public static int GetPosition(Uri url, string searchTerm)
        {
            //https: //www.google.com.au/#num=500&q=online+title+search&btnG=Search
            string raw = "http://www.google.com/search?num=100&q={0}&btnG=Search";
            string search = string.Format(raw, HttpUtility.UrlEncode(searchTerm));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search);
//            System.Net.ServicePointManager.Expect100Continue = false;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
//                System.Net.ServicePointManager.Expect100Continue = false;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                {
                    System.Net.ServicePointManager.Expect100Continue = false;
                    string html = reader.ReadToEnd();
                    return FindPosition(html, url);
                }
            }
        }
        
        private static int FindPosition(string html, Uri url)
        {
            string lookup = "(<h3 class=\"r\"><a href=\"/url\\?q=)(\\w+[a-zA-Z0-9.\\-?=/:]*)";
            MatchCollection matches = Regex.Matches(html, lookup);
            for (int i = 0; i < matches.Count; i++)
            {
                string match = matches[i].Groups[2].Value;
                if (match.Contains(url.Host))
                {
                    return i + 1;
                }
            }
            Console.WriteLine("We didn't find your website");
            return 0;
        }

    }
}