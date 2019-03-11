using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LAP_API.Modules
{
    public class service
    {
        Hashtable ht = new Hashtable();

        public string suminfo(string url)
        {
            string idKey = "";
            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(url)))
                {
                    JObject jsonList = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());

                    for (int i = 0; i < jsonList.Count; i++)
                    {
                        ht = new Hashtable();
                        foreach (JProperty jp in jsonList.Properties())
                        {
                            ht.Add(jp.Name, jp.Value);
                            //Console.WriteLine(jp.Name + ":" + jp.Value);
                            idKey = ht["id"].ToString();
                        }
                    }
                }
            }
            return idKey;
        }
    }
}
