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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LAP_API.Controllers
{
    [ApiController]
    public class DataController : Controller
    {
        string APIKEY = "RGAPI-d494941d-4a3c-44b5-8c68-63bfc8cda4ff";
        
        [Route("select_img")]
        [HttpGet]
        public ActionResult<ArrayList> select_img()
        {
            string rotationAPI = string.Format("https://kr.api.riotgames.com/lol/platform/v3/champion-rotations?api_key={0}", APIKEY);
            Database db = new Database();

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
                    }

                    db.ConnectionClose();
                    return list;
                }
            }
        }

        [Route("champion_img")]
        [HttpGet]
        public ActionResult<ArrayList> champion_img()
        {
            Database db = new Database();
            ArrayList list = new ArrayList();
            SqlDataReader sdr = db.Reader("p_championlist");

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
            return list;
        }

        [Route("champ_image")]
        [HttpPost]
        public ActionResult<string> champ_image([FromForm]string summonerName)
        {
            service sv = new service();
            Console.WriteLine(summonerName);
            string nameAPI = string.Format("https://kr.api.riotgames.com/lol/summoner/v4/summoners/by-name/{0}?api_key={1}", summonerName, APIKEY);
            
            return nameAPI;
        }


    }
}