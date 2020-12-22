using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SQLGenerator.News {
    class NewsGetter {
        public static News GetNews() {
            string url = "https://newsapi.org/v2/top-headlines?country=ru&apiKey=9a634718547f4ec1b4552403b026739a";
            try {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string response;

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream())) {
                    response = streamReader.ReadToEnd();
                }

                News news = JsonConvert.DeserializeObject<News>(response);

                return news;

            }
            catch (Exception) {
                return null;
            }

        }
    }
}
