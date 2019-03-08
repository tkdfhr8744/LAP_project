using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private Hashtable hashtable,ht;
        private PictureBox back,tierpic,pc1,pc2,pc3,pc4,pc5,pc6;
        private Label SummonerName,tierlb,KDA,rankPoint,ranknum,mainpostition,WinLoss,TeamList,OpponentTeam,tiernum;
        private Chart chart1;
        private Panel chartpn,summonerLog,LogPn;
        private Commons cm;
        private WebapiLibrary wal = new WebapiLibrary();
        private string name;

        public SummonerINFO(Form1 f1,string name)
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
            string url = string.Format("https://kr.api.riotgames.com/lol/league/v4/positions/by-summoner/{0}?api_key={1}", name, wal.myapikey());
            //.Show(name+"::::::"+wal.myapikey());
            Summoner_info(url);
            this.BackColor = Color.White;
            cm = new Commons();
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(50, 50));
            hashtable.Add("point", new Point(0, 0));
            hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
            hashtable.Add("click", (EventHandler)Back_click);
            back = cm.getPictureBox(hashtable, this);
            back.Image = Properties.Resources.images;
            
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(150, 150));
            hashtable.Add("point", new Point(115, 60));
            hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
            tierpic = cm.getPictureBox(hashtable, this);
            tierpic.BackColor = Color.White;
            //tierpic.Load("http://gdc3.gudi.kr:42001/emblems/Emblem_Bronze.png");

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(300, 40));
            hashtable.Add("point", new Point(270, 60));
            hashtable.Add("color", Color.Yellow);
            hashtable.Add("name", "summonerName");
            hashtable.Add("text", ht["summonerName"]);
            SummonerName = cm.getLabel(hashtable, this);
            SummonerName.Font = new Font("맑은 고딕",20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 40));
            hashtable.Add("point", new Point(270, 110));
            hashtable.Add("color", Color.Yellow);
            hashtable.Add("name", "tier");
            hashtable.Add("text", ht["tier"]);
            tierlb = cm.getLabel(hashtable, this);
            tierlb.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 40));
            hashtable.Add("point", new Point(270, 110));
            hashtable.Add("color", Color.Yellow);
            hashtable.Add("name", "rank");
            hashtable.Add("text", ht["rank"]);
            rankPoint = cm.getLabel(hashtable, this);
            rankPoint.Font = new Font("맑은 고딕", 20, FontStyle.Bold);
            
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 40));
            hashtable.Add("point", new Point(450,110));
            hashtable.Add("color", Color.Yellow);
            hashtable.Add("name", "LP");
            hashtable.Add("text", ht["leaguePoints"]);
            ranknum = cm.getLabel(hashtable, this);
            ranknum.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(150,150));
            hashtable.Add("point", new Point(600, 60));
            hashtable.Add("color", Color.Coral);
            hashtable.Add("name", "BackgroundPN");
            chartpn = cm.getPanel(hashtable, this);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(800, 230));
            hashtable.Add("point", new Point(0, 220));
            hashtable.Add("color", Color.Coral);
            hashtable.Add("name", "summonerLOG");
            summonerLog = cm.getPanel(hashtable, this);
            summonerLog.AutoScroll = true;
            
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(150, 40));
            hashtable.Add("point", new Point(270, 160));
            hashtable.Add("color", Color.Yellow);
            hashtable.Add("name", "position");
            hashtable.Add("text", ht["position"]);
            mainpostition = cm.getLabel(hashtable, this);
            mainpostition.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

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
            chart1.Series["Series1"].Points.AddXY(string.Format("승 {0}", 5), 5);
            chart1.Series["Series1"].Points[0].Color = Color.Blue;
            
            chart1.Series["Series1"].Points.AddXY(string.Format("패 {0}", 6), 6);
            chart1.Series["Series1"].Points[0].Color = Color.Red;
            Loglist();
        }

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
        private void Back_click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Loglist()
        {
            for(int i = 0; i < 20; i++)
            {
                hashtable = new Hashtable();
                hashtable.Add("size", new Size(750, 160));
                hashtable.Add("point", new Point(25, 5+(i*170)));
                hashtable.Add("color", Color.Aqua);
                hashtable.Add("name", "LOG");
                LogPn = cm.getPanel(hashtable, summonerLog);

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(40, 20));
                hashtable.Add("point", new Point(5, 60));
                hashtable.Add("color", Color.Yellow);
                hashtable.Add("name", "WinLoss");
                hashtable.Add("text", "승리");
                WinLoss = cm.getLabel(hashtable, LogPn);
                WinLoss.Font = new Font("맑은 고딕", 10, FontStyle.Bold);

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(60, 60));
                hashtable.Add("point", new Point(50, 40));
                hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                pc1 = cm.getPictureBox(hashtable, LogPn);
                pc1.BackColor = Color.Black;

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(25, 25));
                hashtable.Add("point", new Point(115, 40));
                hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                pc2 = cm.getPictureBox(hashtable, LogPn);
                pc2.BackColor = Color.Black;

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(25, 25));
                hashtable.Add("point", new Point(115, 75));
                hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                pc3 = cm.getPictureBox(hashtable, LogPn);
                pc3.BackColor = Color.Black;

                hashtable = new Hashtable();
                hashtable.Add("size", new Size(120, 25));
                hashtable.Add("point", new Point(155, 60));
                hashtable.Add("color", Color.Yellow);
                hashtable.Add("name", "champName");
                hashtable.Add("text", "KDA");
                KDA = cm.getLabel(hashtable, LogPn);
                KDA.Font = new Font("맑은 고딕", 15, FontStyle.Bold);

                // 아이탬리스트
                for (int j = 0; j < 7; j++)
                {
                    hashtable = new Hashtable();
                    hashtable.Add("size", new Size(30, 30));
                    hashtable.Add("point", new Point(290 + (32 * j), 58));
                    hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                    pc4 = cm.getPictureBox(hashtable, LogPn);
                    pc4.BackColor = Color.Black;
                    //pc4.Load("http://gdc3.gudi.kr:42001/Talon.png");
                }


                //우리팀 상대팀 리스트
                for (int k = 0; k < 5; k++)
                {
                    hashtable = new Hashtable();
                    hashtable.Add("size", new Size(25, 25));
                    hashtable.Add("point", new Point(530, 5 + (k * 30)));
                    hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                    pc5 = cm.getPictureBox(hashtable, LogPn);
                    pc5.BackColor = Color.Black;

                    hashtable = new Hashtable();
                    hashtable.Add("size", new Size(70, 25));
                    hashtable.Add("point", new Point(556, 5 + (k * 30)));
                    hashtable.Add("color", Color.Yellow);
                    hashtable.Add("name", "TeamList");
                    hashtable.Add("text", "우리팀");
                    TeamList = cm.getLabel(hashtable, LogPn);
                    TeamList.Font = new Font("맑은 고딕", 10, FontStyle.Bold);

                    hashtable = new Hashtable();
                    hashtable.Add("size", new Size(70, 25));
                    hashtable.Add("point", new Point(630, 5 + (k * 30)));
                    hashtable.Add("color", Color.Yellow);
                    hashtable.Add("name", "TeamList");
                    hashtable.Add("text", "상대팀");
                    OpponentTeam = cm.getLabel(hashtable, LogPn);
                    OpponentTeam.Font = new Font("맑은 고딕", 10, FontStyle.Bold);

                    hashtable = new Hashtable();
                    hashtable.Add("size", new Size(25, 25));
                    hashtable.Add("point", new Point(701, 5 + (k * 30)));
                    hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                    pc6 = cm.getPictureBox(hashtable, LogPn);
                    pc6.BackColor = Color.Black;

                }
            }
        }
        
        private void txtExction()
        {

        }
        
    }
}
