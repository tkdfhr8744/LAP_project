﻿using Newtonsoft.Json;
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
    public partial class Form1 : Form
    {
        Hashtable hashtable, ht;
        TextBox tb;
        Panel pn, championList;
        Button searchBT;
        Form close;
        ChampionINFO ci;
        championMoreInfo cmi;
        PictureBox logo, back;
        WebapiLibrary wal;
        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //LAP start
            this.Size = new Size(1000, 800);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "LAP";
            
            Commons cm = new Commons();

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100,100));
            hashtable.Add("point", new Point(40, 20));
            hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
            logo = cm.getPictureBox(hashtable, this);
            logo.Image = Properties.Resources.LAP_logo;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(990, 770));
            hashtable.Add("point", new Point(0, 0));
            hashtable.Add("color", Color.Black);
            hashtable.Add("name", "BackgroundPN");
            pn = cm.getPanel2(hashtable, this);

            hashtable = new Hashtable();
            hashtable.Add("width", "500");
            hashtable.Add("point", new Point(245, 70));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "search");
            hashtable.Add("enabled", true);
            tb = cm.getTextBox(hashtable, pn);
            tb.Font = new Font("맑은 고딕", 20, FontStyle.Bold);
            tb.BorderStyle = BorderStyle.None;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 38));
            hashtable.Add("point", new Point(750, 70));
            hashtable.Add("color", Color.Gray);
            hashtable.Add("name", "searchbt");
            hashtable.Add("text", "검색");
            hashtable.Add("click", (EventHandler)btn_click);
            searchBT = cm.getButton(hashtable, pn);
            searchBT.FlatStyle = FlatStyle.Flat;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(990, 700));
            hashtable.Add("point", new Point(0, 150));
            hashtable.Add("color", Color.Black);
            hashtable.Add("name", "ListPN");
            championList = cm.getPanel(hashtable, pn);

            ci = new ChampionINFO(this);
            ci.WindowState = FormWindowState.Maximized;
            ci.FormBorderStyle = FormBorderStyle.None;
            ci.MdiParent = this;
            ci.Dock = DockStyle.Fill;
            championList.Controls.Add(ci);
            ci.Show();
        }
        
        public bool Post(string url, Hashtable ht)
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

                if ("1" == resultStr)
                {
                    //MessageBox.Show("DB 성공");
                }
                else
                {
                    MessageBox.Show("DB 실패");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

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
                            idKey = ht["id"].ToString();
                        }
                    }
                }
            }
            return idKey;
        }

        private void btn_click(object o, EventArgs e)
        {
            wal = new WebapiLibrary();
            try
            {
                string nameAPI = string.Format("https://kr.api.riotgames.com/lol/summoner/v4/summoners/by-name/{0}?api_key={1}", tb.Text, wal.myapikey());
                close = new SummonerINFO(this, suminfo(nameAPI));
                close.WindowState = FormWindowState.Maximized;
                close.FormBorderStyle = FormBorderStyle.None;
                close.MdiParent = this;
                close.Dock = DockStyle.Fill;
                championList.Controls.Add(close);
                close.Show();
            }
                
            catch
            {
                MessageBox.Show("올바른 아이디를 입력해주세요.");
                this.Show();
            }
           
        }

        public void champinfo(string index)
        {
            close = new championMoreInfo(this,index);
            close.WindowState = FormWindowState.Maximized;
            close.FormBorderStyle = FormBorderStyle.None;
            close.MdiParent = this;
            close.Dock = DockStyle.Fill;
            championList.Controls.Add(close);
            close.Show();
        }
    }
}