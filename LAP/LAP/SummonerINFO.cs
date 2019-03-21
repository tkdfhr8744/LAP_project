using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LAP
{
    public partial class SummonerINFO : Form
    {
        private Form1 f1;
        private Hashtable hashtable, ht, hta, accountTable, gameIDTable, myitem,itemtable, teamname;
        private PictureBox back, tierpic, pc1, pc2, pc3, pc4, pc5;
        private Label SummonerName, tierlb, KDA, rankPoint, ranknum, mainpostition, WinLoss, TeamList;
        private Chart chart1;
        private Panel chartpn, summonerLog, LogPn;
        private Commons cm;
        private WebapiLibrary wal = new WebapiLibrary();
        private string name = "";
        private string gameID = "";
        private string url = "";
        private string accountapi = "";
        private string matchlist = "";
        private string matchlog = "";
        private string[] namelist;
        private string[] champlist;
        private int winCount, lossCount;
        public SummonerINFO(Form1 f1, string name)
        {
            InitializeComponent();
            Load += SummonerINFO_Load;
            this.f1 = f1;
            this.name = name;
        }

        public SummonerINFO()
        {
            InitializeComponent();
            Load += SummonerINFO_Load;
        }

        private void SummonerINFO_Load(object sender, EventArgs e)
        {
            url = string.Format("https://kr.api.riotgames.com/lol/league/v4/positions/by-summoner/{0}?api_key={1}", name, wal.myapikey());
            this.BackColor = Color.White;
            Summoner_info(url);
            accountapi = string.Format("https://kr.api.riotgames.com/lol/summoner/v4/summoners/by-name/{0}?api_key={1}", ht["summonerName"].ToString(), wal.myapikey());
            matchlist = string.Format("https://kr.api.riotgames.com/lol/match/v4/matchlists/by-account/{0}?endIndex=10&api_key={1}", accountfuc(accountapi), wal.myapikey());

            cm = new Commons();
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(50, 50));
            hashtable.Add("point", new Point(0, 0));
            hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
            hashtable.Add("click", (EventHandler)Back_click);
            back = cm.getPictureBox(hashtable, this);
            back.Image = Properties.Resources.images;
            back.Cursor = Cursors.Hand;
            
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(150, 150));
            hashtable.Add("point", new Point(115, 60));
            hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
            tierpic = cm.getPictureBox(hashtable, this);
            tierpic.BackColor = Color.White;
            tierpic.Load(string.Format("http://gdc3.gudi.kr:42001/emblems/Emblem_{0}.png", ht["tier"].ToString()));

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(300, 40));
            hashtable.Add("point", new Point(270, 60));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "summonerName");
            //hashtable.Add("text", "서머너이름");
            hashtable.Add("text", ht["summonerName"]);
            SummonerName = cm.getLabel(hashtable, this);
            SummonerName.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(150, 40));
            hashtable.Add("point", new Point(270, 110));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "tier");
            //hashtable.Add("text", "티어");
            hashtable.Add("text", ht["tier"]);
            tierlb = cm.getLabel(hashtable, this);
            tierlb.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(30, 40));
            hashtable.Add("point", new Point(430, 110));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "rank");
            //hashtable.Add("text", "랭크");
            hashtable.Add("text", ht["rank"]);
            rankPoint = cm.getLabel(hashtable, this);
            rankPoint.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 40));
            hashtable.Add("point", new Point(470, 110));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "LP");
            //hashtable.Add("text", "포인트");
            hashtable.Add("text", ht["leaguePoints"]);
            ranknum = cm.getLabel(hashtable, this);
            ranknum.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(150, 150));
            hashtable.Add("point", new Point(600, 60));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "BackgroundPN");
            chartpn = cm.getPanel(hashtable, this);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(985, 400));
            hashtable.Add("point", new Point(0, 220));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "summonerLOG");
            summonerLog = cm.getPanel(hashtable, this);
            summonerLog.AutoScroll = true;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(150, 40));
            hashtable.Add("point", new Point(270, 160));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "position");
            //hashtable.Add("text", "포지션");
            hashtable.Add("text", ht["position"]);
            mainpostition = cm.getLabel(hashtable, this);
            mainpostition.Font = new Font("맑은 고딕", 20, FontStyle.Bold);


            gameID_PULL(matchlist);
        }

        private void grape_img()
        {
            chart1 = new Chart();
            ChartArea chartArea1 = new ChartArea();
            Series series1 = new Series();
            chartArea1.Name = "ChartArea1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = SeriesChartType.Doughnut;
            series1.Name = "Series1";

            chart1.Name = "chart1";
            chart1.Dock = DockStyle.Fill;
            chart1.Text = "chart1";
            chart1.ChartAreas.Add(chartArea1);
            chart1.Titles.Add("최근 경기 전적");
            chart1.Series.Add(series1);
            chart1.Series["Series1"].IsValueShownAsLabel = false;
            chartpn.Controls.Add(chart1);
            chart1.Series["Series1"].Points.AddXY(string.Format("승 {0}", winCount), winCount);
            chart1.Series["Series1"].Points[0].Color = Color.Blue;

            chart1.Series["Series1"].Points.AddXY(string.Format("패 {0}", lossCount), lossCount);
            chart1.Series["Series1"].Points[0].Color = Color.Red;

        }

        //소환사 리그정보 해쉬테이블로 담아온것
        public Hashtable Summoner_info(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(url)))
                {
                    ArrayList value = new ArrayList();
                    JArray jsonList = JsonConvert.DeserializeObject<JArray>(streamReader.ReadToEnd());

                    for (int i = 0; i < jsonList.Count; i++)
                    {
                        ht = new Hashtable();
                        JObject jo = (JObject)jsonList[0];

                        foreach (JProperty jp in jo.Properties())
                        {
                            ht.Add(jp.Name, jp.Value);
                        }
                    }
                }
            }
            return ht;
        }
        //걍 클릭
        private void Back_click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //소환사 account 아이디를 가져오는 메소드
        private string accountfuc(string url)
        {
            string accountID = "";
            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(url)))
                {
                    JObject jsonList = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
                    for (int i = 0; i < jsonList.Count; i++)
                    {
                        accountTable = new Hashtable();
                        foreach (JProperty jp in jsonList.Properties())
                        {
                            accountTable.Add(jp.Name, jp.Value);
                        }
                    }
                    accountID = accountTable["accountId"].ToString();
                }
            }
            return accountID;
        }

        /*
        //각 게임 ID 받는 메소드
        public string gameID_PULL(string url, int i)
        {
            //Loglist(gameNo);
            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(url)))
                {
                    JObject jsonList = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
                    JToken jt = jsonList.GetValue("matches");
                    JArray ja = jt.Value<JArray>();
                    for (int k = i; k < i + 1; k++)
                    {
                        gameIDTable = new Hashtable();

                        JObject jo = (JObject)ja[k];
                        foreach (JProperty jp in jo.Properties())
                        {
                            gameIDTable.Add(jp.Name, jp.Value);
                        }
                        gameID = gameIDTable["gameId"].ToString();
                    }
                    return gameID;
                }
            }
        }*/

        public void gameID_PULL(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(url)))
                {
                    JObject jsonList = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
                    JToken jt = jsonList.GetValue("matches");
                    JArray ja = jt.Value<JArray>();
                    string[] arr = new string[ja.Count];
                    for (int k = 0; k < 5; k++)
                    {
                        JObject jo = (JObject)ja[k];
                        JToken jt2 = jo.GetValue("gameId");
                        //MessageBox.Show(jt2.ToString());
                        Loglist(jt2.ToString(),k);
                    }
                    grape_img();
                }
            }
        }

        //게임 리스트에서 자신 인덱스 받아오는 메소드
        public int gameinfofuc(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(url)))
                {
                    int nameindex = 0;
                    JObject jsonList = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
                    JToken jt = jsonList.GetValue("participants");
                    JArray jo = (JArray)jt;
                    JToken summonername = jsonList.GetValue("participantIdentities");
                    JArray jarr = (JArray)summonername;
                    for (int i = 0; i < jarr.Count; i++)
                    {
                        hta = new Hashtable();
                        JObject jObject = (JObject)jarr[i];
                        JToken summonertoken = jObject.GetValue("player");
                        JObject nameobject = (JObject)summonertoken;
                        JToken sumname = nameobject.GetValue("summonerName");
                        if (ht["summonerName"].ToString() == sumname.ToString())
                        {
                            nameindex = i + 1;
                        }
                    }
                    return nameindex;

                }
            }
        }
        //item과 스펠
        public void myitem_spell(string url, int num)
        {
            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(url)))
                {
                    JObject jsonList = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
                    JToken jt = jsonList.GetValue("participants");
                    JArray jo = (JArray)jt;
                    for (int k = num - 1; k < num; k++)
                    {
                        myitem = new Hashtable();
                        JObject job = (JObject)jo[k];
                        foreach (JProperty jp in job.Properties())
                        {
                            myitem.Add(jp.Name, jp.Value);
                        }
                    }
                    for (int k = 0; k < jo.Count; k++)
                    {
                        JObject job = (JObject)jo[k];
                        JToken jt2 = job.GetValue("stats");
                        JObject jo2 = (JObject)jt2;
                        itemtable = new Hashtable();
                        foreach (JProperty jp2 in jo2.Properties())
                        {
                            itemtable.Add(jp2.Name, jp2.Value);
                        }
                    }
                }
            }
        }
        //아이템 및 서머너정보
        public Hashtable item_select(string url,int num)
        {
            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(url)))
                {
                    JObject jsonList = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
                    JToken jt = jsonList.GetValue("participants");
                    JArray jo = (JArray)jt;
                    for (int k = num - 1; k < num; k++)
                    {
                        JObject job = (JObject)jo[k];

                        myitem = new Hashtable();
                        foreach (JProperty jp in job.Properties())
                        {
                            myitem.Add(jp.Name, jp.Value);
                        }
                        JToken jt2 = job.GetValue("stats");
                        JObject jo2 = (JObject)jt2;
                        itemtable = new Hashtable();
                        foreach (JProperty jp2 in jo2.Properties())
                        {
                            itemtable.Add(jp2.Name, jp2.Value);
                        }

                        if (itemtable["win"].ToString() == "True") winCount++;
                        else lossCount++;
                    }
                    return itemtable;
                }
            }
        }
        
        public string Post3(string url, string champkey)
        {
            try
            {
                WebClient wc = new WebClient();
                NameValueCollection nameValue = new NameValueCollection();
                Hashtable ht = new Hashtable();
                ht.Add("id", champkey);
                foreach (DictionaryEntry data in ht)
                {
                    nameValue.Add(data.Key.ToString(), data.Value.ToString());
                }

                byte[] result = wc.UploadValues(url, "POST", nameValue);
                string resultStr = Encoding.UTF8.GetString(result);
                /*if ("1" == resultStr)
                {
                    //MessageBox.Show("DB 성공");
                }
                else
                {
                    MessageBox.Show("DB 실패");
                }*/
                return resultStr;
            }
            catch (Exception)
            {
                MessageBox.Show("오류");
                return null;
            }
        }

        public string Post2(string url, Hashtable ht)
        {
            try
            {
                WebClient wc = new WebClient();
                NameValueCollection nameValue = new NameValueCollection();

                foreach (DictionaryEntry data in ht)
                {
                    nameValue.Add(data.Key.ToString(), data.Value.ToString());
                }

                byte[] result = wc.UploadValues(url, "POST", nameValue);
                string resultStr = Encoding.UTF8.GetString(result);
                /*if ("1" == resultStr)
                {
                    //MessageBox.Show("DB 성공");
                }
                else
                {
                    MessageBox.Show("DB 실패");
                }*/
                return resultStr;
            }
            catch (Exception)
            {
                MessageBox.Show("오류");
                return null;
            }
        }
        //아이템 리스트 POST
        public string itemPost(string url, Hashtable ht,int num)
        {
            try
            {
                WebClient wc = new WebClient();
                NameValueCollection nameValue = new NameValueCollection();
                string a = string.Format("item{0}",num);
                string itemValue = ht[a].ToString();
                Hashtable htimg = new Hashtable();
                htimg.Add("id", itemValue);

                foreach (DictionaryEntry data in htimg)
                {
                    nameValue.Add(data.Key.ToString(), data.Value.ToString());
                }

                byte[] result = wc.UploadValues(url, "POST", nameValue);
                string resultStr = Encoding.UTF8.GetString(result);
                /*if ("1" == resultStr)
                {
                    //MessageBox.Show("DB 성공");
                }
                else
                {
                    MessageBox.Show("DB 실패");
                }*/
                return resultStr;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }

        public void Post(string url, PictureBox pc1, int index)
        {
            try
            {
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url);
                StreamReader sr = new StreamReader(stream);
                string result = sr.ReadToEnd();
                JArray list = JsonConvert.DeserializeObject<JArray>(result);
                for (int i = index; i < index + 1; i++)
                {
                    JArray j = (JArray)list[i];
                    string[] arr = new string[j.Count];
                    for (int k = 0; k < j.Count; k++)
                    {
                        arr[k] = j[k].ToString();
                        pc1.Load(arr[k]);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        //팀 상대 이미지 및 아이디
        public Array summonerlist(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                using (StreamReader streamReader = new StreamReader(webClient.OpenRead(url)))
                {

                    JObject jsonList = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
                    JToken jt = jsonList.GetValue("participants");
                    JArray jar = (JArray)jt;
                    champlist = new string[jar.Count];
                    for (int k = 0; k < jar.Count; k++)
                    {
                        JObject job = (JObject)jar[k];
                        JToken champtk = job.GetValue("championId");
                        champlist[k] = champtk.ToString();
                    }

                    JToken summonername = jsonList.GetValue("participantIdentities");
                    JArray jarr = (JArray)summonername;
                    JArray ja = new JArray();
                    namelist = new string[jarr.Count];
                    for (int i = 0; i < jarr.Count; i++)
                    {
                        teamname = new Hashtable();
                        JObject jObject = (JObject)jarr[i];
                        JToken summonertoken = jObject.GetValue("player");
                        JObject nameobject = (JObject)summonertoken;
                        JToken jtname = nameobject.GetValue("summonerName");
                        namelist[i] = jtname.ToString();
                    }
                    return namelist;
                }
            }
        }

        private void Loglist(string gameNo,int i)
        {
                matchlog = string.Format("https://kr.api.riotgames.com/lol/match/v4/matches/{0}?api_key={1}", gameNo, wal.myapikey());
                //matchlog = string.Format("https://kr.api.riotgames.com/lol/match/v4/matches/{0}?api_key={1}", gameID_PULL(matchlist, i), wal.myapikey());
                myitem_spell(matchlog, gameinfofuc(matchlog));
                item_select(matchlog, gameinfofuc(matchlog));
                string kdainfo = itemtable["kills"].ToString() + "/" + itemtable["deaths"].ToString() + "/" + itemtable["assists"].ToString();
                
                hashtable = new Hashtable();
                hashtable.Add("size", new Size(750, 160));
                hashtable.Add("point", new Point(115, 5 + (i * 170)));
                if (itemtable["win"].ToString() =="True") hashtable.Add("color", Color.FromArgb(163,207,236));
                else if(itemtable["win"].ToString() == "False") hashtable.Add("color", Color.FromArgb(226,182,176));
                hashtable.Add("name", "LOG");
                LogPn = cm.getPanel(hashtable, summonerLog);

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(40, 20));
                hashtable.Add("point", new Point(5, 60));
                hashtable.Add("name", "WinLoss");
                if (itemtable["win"].ToString() == "True") hashtable.Add("color", Color.FromArgb(163,207,236));
                else if (itemtable["win"].ToString() == "False") hashtable.Add("color", Color.FromArgb(226,182, 176));
                if (itemtable["win"].ToString() == "True") hashtable.Add("text", "승리");
                else if(itemtable["win"].ToString()=="False") hashtable.Add("text", "패배");
                WinLoss = cm.getLabel(hashtable, LogPn);
                WinLoss.Font = new Font("맑은 고딕", 10, FontStyle.Bold);

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(60, 60));
                hashtable.Add("point", new Point(50, 40));
                hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                pc1 = cm.getPictureBox(hashtable, LogPn);
                pc1.BackColor = Color.Black;
                pc1.Load(Post2("http://gdc3.gudi.kr:42001/champ_image", myitem));

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(25, 25));
                hashtable.Add("point", new Point(115, 40));
                hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                pc2 = cm.getPictureBox(hashtable, LogPn);
                pc2.BackColor = Color.Black;
                pc2.Load(Post2("http://gdc3.gudi.kr:42001/spell_image", myitem));

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(25, 25));
                hashtable.Add("point", new Point(115, 75));
                hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                pc3 = cm.getPictureBox(hashtable, LogPn);
                pc3.BackColor = Color.Black;
                pc3.Load(Post2("http://gdc3.gudi.kr:42001/spell_image2", myitem));

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(120, 25));
                hashtable.Add("point", new Point(155, 60));
                if (itemtable["win"].ToString() == "True") hashtable.Add("color", Color.FromArgb(163,207,236));
                else if (itemtable["win"].ToString() == "False") hashtable.Add("color", Color.FromArgb(226, 182, 176));
                hashtable.Add("name", "champName");
                hashtable.Add("text", kdainfo);
                KDA = cm.getLabel(hashtable, LogPn);
                KDA.Font = new Font("맑은 고딕", 15, FontStyle.Bold);

                // 아이템리스트
                for (int j = 0; j < 7; j++)
                {
                    hashtable = new Hashtable();
                    hashtable.Add("size", new Size(30, 30));
                    hashtable.Add("point", new Point(290 + (32 * j), 58));
                    hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                    pc4 = cm.getPictureBox(hashtable, LogPn);
                    pc4.BackColor = Color.Black;
                    pc4.Load(itemPost("http://gdc3.gudi.kr:42001/item_img", itemtable, j));
                }
                summonerlist(matchlog);

                //우리팀 상대팀 리스트
                for (int k = 0; k < 2; k++)
                {
                    for(int t = 0; t < 5; t++)
                    {
                        hashtable = new Hashtable();
                        hashtable.Add("size", new Size(25, 25));
                        hashtable.Add("point", new Point(530+(k*171), 5 + (t * 30)));
                        hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                        pc5 = cm.getPictureBox(hashtable, LogPn);
                        pc5.BackColor = Color.Black;
                        if (k == 1) pc5.Load(Post3("http://gdc3.gudi.kr:42001/champ_list", champlist[5 + t].ToString()));
                        else pc5.Load(Post3("http://gdc3.gudi.kr:42001/champ_list", champlist[t].ToString()));

                        hashtable = new Hashtable();
                        hashtable.Add("size", new Size(70, 25));
                        hashtable.Add("point", new Point(556+(k*74), 5 + (t * 30)));
                        if (itemtable["win"].ToString() == "True") hashtable.Add("color", Color.FromArgb(163,207,236));
                        else if (itemtable["win"].ToString() == "False") hashtable.Add("color", Color.FromArgb(226,182,176));
                        hashtable.Add("name", "TeamList");
                        if (k == 1) hashtable.Add("text", namelist[5 + t]);
                        else hashtable.Add("text",namelist[t]);
                        TeamList = cm.getLabel(hashtable, LogPn);
                        TeamList.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                    }
                }
            //grape_img();
        }
    }
}