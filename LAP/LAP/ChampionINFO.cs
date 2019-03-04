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

namespace LAP
{
    public partial class ChampionINFO : Form
    {
        Form1 f1;
        Panel championList, rotationBack;
        Hashtable hashtable;
        Label rotationTitle;
        PictureBox pc1, pc2;
        Commons cm;

        public ChampionINFO()
        {
            InitializeComponent();
            Load += ChampionINFO_Load;
        }
        public ChampionINFO(Form1 f1, Panel championList)
        {
            InitializeComponent();
            Load += ChampionINFO_Load;
            this.f1 = f1;
            this.championList = championList;
        }

        private void ChampionINFO_Load(object sender, EventArgs e)
        {
            cm = new Commons();
            this.BackColor = Color.White;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(990, 100));
            hashtable.Add("point", new Point(0, 0));
            hashtable.Add("color", Color.Gold);
            hashtable.Add("name", "BackgroundPN");
            rotationBack = cm.getPanel2(hashtable, this);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(990, 512));
            hashtable.Add("point", new Point(0, 100));
            hashtable.Add("color", Color.DarkBlue);
            hashtable.Add("name", "championlist");
            championList = cm.getPanel2(hashtable, this);
            championList.AutoScroll = true;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(140, 30));
            hashtable.Add("point", new Point(10, 5));
            hashtable.Add("color", Color.Gold);
            hashtable.Add("name", "rotationTitle");
            hashtable.Add("text", "금주 로테이션");
            rotationTitle = cm.getLabel(hashtable, rotationBack);
            rotationTitle.Font = new Font("Tahoma", 15, FontStyle.Bold);
            RotationChamp();

            championlistpt();
        }

        private void RotationChamp()
        {
            int margin = 0;
            cm = new Commons();
            for (int i = 0; i < 14; i++)
            {
                hashtable = new Hashtable();
                hashtable.Add("size", new Size(50, 50));
                hashtable.Add("point", new Point(18 + margin, 40));
                hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                hashtable.Add("click", (EventHandler)pic_click);
                pc1 = cm.getPictureBox(hashtable, rotationBack);
                pc1.Cursor = Cursors.Hand;
                Post("http://gdc3.gudi.kr:42001/select_img", pc1, i);
                margin += 69;
            }
        }

        private void championlistpt()
        {
            int margin = 0;
            int height = 0;
            int count = 0;
            cm = new Commons();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    hashtable = new Hashtable();
                    hashtable.Add("size", new Size(90, 90));
                    hashtable.Add("point", new Point(18 + margin, 20 + height));
                    hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                    hashtable.Add("click", (EventHandler)pic_click);
                    pc2 = cm.getPictureBox(hashtable, championList);
                    pc2.Cursor = Cursors.Hand;
                    Post("http://gdc3.gudi.kr:42001/champion_img", pc2, count);
                    margin += 95;
                    count++;
                    if (count == 143) break;
                }
                margin = 0;
                height += 95;
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

        private void pic_click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
        }

    }
}