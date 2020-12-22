using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLGenerator {
    class GetterRandomPerson {

        public static Person.Name getRandomPerson () { 
            string url = "https://randomuser.me/api/";
            try {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string response;

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream())) {
                    response = streamReader.ReadToEnd();
                }

                Person person = JsonConvert.DeserializeObject<Person>(response);
                Person.Name name = JsonConvert.DeserializeObject<Person.Name>(person);
                return name;

            } catch(Exception) {
                return null;
            }
            
        }
       
    }
}
