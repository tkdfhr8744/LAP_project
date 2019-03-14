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
        string APIKEY = "RGAPI-7f04c800-f187-46c3-b413-9af01f00d437";

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

        [Route("item_img")]
        [HttpPost]
        public ActionResult<string> item_img([FromForm]string id)
        {
            Database db = new Database();
            ArrayList list = new ArrayList();
            int num = Convert.ToInt32(id);
            SqlDataReader sdr = db.itemReader("p_item_image",num);
            string temp = "";
            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];
                for (int j = 0; j < sdr.FieldCount; j++)
                {
                    temp = sdr.GetValue(j).ToString();
                }
            }
            db.ReaderClose(sdr);

            db.ConnectionClose();
            //Console.WriteLine(temp);
            return temp;
        }

        [Route("champ_image")]
        [HttpPost]
        public ActionResult<string> champ_image([FromForm]string championId)
        {
            //Console.WriteLine(championId);
            Database db = new Database();
            ArrayList list = new ArrayList();
            int num = Convert.ToInt32(championId);
            SqlDataReader sdr = db.Reader2("p_champion_image", num);

            string temp = "";
            while (sdr.Read())
            {
                for (int j = 0; j < sdr.FieldCount; j++)
                {
                    temp = sdr.GetValue(j).ToString();
                }
            }
            db.ReaderClose(sdr);
            db.ConnectionClose();
            //Console.WriteLine(temp);
            return temp;
        }

        [Route("spell_image")]
        [HttpPost]
        public ActionResult<string> spell_image([FromForm]string spell1Id)
        {
            //Console.WriteLine(spell1Id);
            Database db = new Database();
            ArrayList list = new ArrayList();
            int num = Convert.ToInt32(spell1Id);
            SqlDataReader sdr = db.Reader3("p_spell_image", num);

            string temp = "";
            while (sdr.Read())
            {
                for (int j = 0; j < sdr.FieldCount; j++)
                {
                    temp = sdr.GetValue(j).ToString();
                }
            }
            db.ReaderClose(sdr);
            db.ConnectionClose();
            //Console.WriteLine(temp);
            return temp;
        }

        [Route("spell_image2")]
        [HttpPost]
        public ActionResult<string> spell_image2([FromForm]string spell2Id)
        {
            //Console.WriteLine(spell2Id);
            Database db = new Database();
            ArrayList list = new ArrayList();
            int num = Convert.ToInt32(spell2Id);
            SqlDataReader sdr = db.Reader3("p_spell_image", num);

            string temp = "";
            while (sdr.Read())
            {
                for (int j = 0; j < sdr.FieldCount; j++)
                {
                    temp = sdr.GetValue(j).ToString();
                }
            }
            db.ReaderClose(sdr);
            db.ConnectionClose();
            //Console.WriteLine(temp);
            return temp;
        }

        [Route("champ_list")]
        [HttpPost]
        public ActionResult<string> champ_list([FromForm]string id)
        {
            Database db = new Database();
            ArrayList list = new ArrayList();
            int num = Convert.ToInt32(id);
            SqlDataReader sdr = db.Reader2("p_champion_image", num);

            string temp = "";
            while (sdr.Read())
            {
                for (int j = 0; j < sdr.FieldCount; j++)
                {
                    temp = sdr.GetValue(j).ToString();
                }
            }
            db.ReaderClose(sdr);
            db.ConnectionClose();
            return temp;
        }

        [Route("champinfo")]
        [HttpPost]
        public ActionResult<string> champinfo([FromForm]string id)
        {
            Database db = new Database();
            ArrayList list = new ArrayList();
            SqlDataReader sdr = db.imginfo("p_champinfo", id);

            string temp = "";
            while (sdr.Read())
            {
                for (int j = 0; j < sdr.FieldCount; j++)
                {
                    temp = sdr.GetValue(j).ToString();
                }
            }
            db.ReaderClose(sdr);
            db.ConnectionClose();
            return temp;
        }
    }
}