using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LAP_API.Modules;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LAP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        string APIKEY = "RGAPI-a3aa9664-a002-41e0-aa00-6b51680ebd89";
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("select_img")]
        [HttpGet]
        public ActionResult<ArrayList> select_img()
        {
            string rotationAPI = string.Format("https://kr.api.riotgames.com/lol/platform/v3/champion-rotations?api_key={0}", APIKEY);
            Database db = new Database();
            Hashtable ht = new Hashtable();

            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(rotationAPI)))
                {
                    ArrayList list = new ArrayList();
                    JObject jsonList = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
                    JToken jt = jsonList.GetValue("freeChampionIds");
                    JArray jarr = jt.Value<JArray>();
                    for (int i = 0; i < jarr.Count; i++)
                    {
                        string a = jarr[i].ToString();
                        Console.WriteLine(a);
                        int num = Convert.ToInt32(a);
                        SqlDataReader sdr = db.Reader2("p_champion_image", num);
                        while (sdr.Read())
                        {
                            string[] arr = new string[sdr.FieldCount];
                            for (int j = 0; j < sdr.FieldCount; j++)
                            {
                                arr[j] = sdr.GetValue(j).ToString();
                            }
                            list.Add(arr);
                        }
                        db.ReaderClose(sdr);
                        db.ConnectionClose();
                    }
                    return list;
                }
            }

            /*
            ArrayList list = new ArrayList();
            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    arr[i] = sdr.GetValue(i).ToString();
                }
                list.Add(arr);
            }*/
        }
    }
}
