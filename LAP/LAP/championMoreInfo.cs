using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAP
{
    public partial class championMoreInfo : Form
    {
        private Form1 f1;
        private WebBrowser wb,skillset;
        private Hashtable hashtable;
        private PictureBox champImage, back;
        private Commons cm;
        private Label champName;
        private Panel pn;
        private string index;
        
        public championMoreInfo()
        {
            InitializeComponent();
            Load += ChampionMoreInfo_Load;
        }
        public championMoreInfo(Form1 f1,string index)
        {
            InitializeComponent();
            Load += ChampionMoreInfo_Load;
            this.f1 = f1;
            this.index = index;
        }

        private void ChampionMoreInfo_Load(object sender, EventArgs e)
        {
            string name=index.Substring(26);
            string chamName = name.Replace(".png", "");
            string chamNameselect= champresult("http://gdc3.gudi.kr:42001/champinfo", index);
            this.BackColor = Color.WhiteSmoke;
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
            hashtable.Add("size", new Size(150,150));
            hashtable.Add("point", new Point(115, 60));
            hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
            champImage = cm.getPictureBox(hashtable, this);
            champImage.Load(index);
            
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 35));
            hashtable.Add("point", new Point(275, 175));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "champName");
            hashtable.Add("text", chamNameselect);
            champName = cm.getLabel(hashtable, this);
            champName.Font = new Font("맑은 고딕", 15, FontStyle.Bold);

            skillset = new WebBrowser();
            skillset.Location = new Point(275, 60);
            skillset.Size = new Size(300,60);
            skillset.Name = "skill";
            skillset.TabIndex = 0;
            this.Controls.Add(skillset);

            skillset.DocumentCompleted += skillset_DocumentCompleted;
            skillset.Visible = false;
            skillset.ScriptErrorsSuppressed = true;
            skillset.IsWebBrowserContextMenuEnabled = false;
            skillset.Navigate(string.Format("https://www.op.gg/champion/{0}/statistics", chamName));

            
            wb = new WebBrowser();
            wb.Location = new Point(0, 215);
            wb.Size = new Size(985, 400);
            wb.Name = "web1";
            wb.TabIndex = 1;
            this.Controls.Add(wb);

            wb.DocumentCompleted += WebBrowser1_DocumentCompleted;
            wb.Visible = false;
            wb.ScriptErrorsSuppressed = true;
            wb.IsWebBrowserContextMenuEnabled = false;
            wb.Navigate(string.Format("https://www.op.gg/champion/{0}/statistics", chamName));
        }

        

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElementCollection hec = wb.Document.GetElementsByTagName("div");
            for (int i = 0; i < hec.Count; i++)
            {
                if ("l-champion-statistics-content__main" == hec[i].GetAttribute("className").ToString())
                {
                    wb.Document.GetElementsByTagName("body")[0].InnerHtml = hec[i].InnerHtml;
                    wb.Visible = true;
                }
            }
        }

        private void skillset_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElementCollection hec = skillset.Document.GetElementsByTagName("div");
            for (int i = 0; i < hec.Count; i++)
            {
                if ("champion-stats-header-info__skill" == hec[i].GetAttribute("className").ToString())
                {
                    skillset.Document.GetElementsByTagName("body")[0].InnerHtml = hec[i].InnerHtml;
                    skillset.Visible = true;
                }
            }
        }

        private void Back_click(object sender, EventArgs e)
        {
            this.Dispose();
        }
       
        public string champresult(string url, string champkey)
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
                return resultStr;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }
        
    }
}
