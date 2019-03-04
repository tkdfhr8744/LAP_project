using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAP
{
    public partial class Form1 : Form
    {
        Hashtable hashtable;
        TextBox tb;
        Panel pn, championList;
        Button searchBT;
        Form close;
        ChampionINFO ci;
        championMoreInfo cmi;
        PictureBox logo;

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
            Commons cm = new Commons();

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(50, 50));
            hashtable.Add("point", new Point(500, 40));
            hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
            logo = cm.getPictureBox(hashtable, this);
            logo.Image = Properties.Resources.LAP_logo;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(990, 770));
            hashtable.Add("point", new Point(0, 0));
            hashtable.Add("color", Color.Coral);
            hashtable.Add("name", "BackgroundPN");
            pn = cm.getPanel2(hashtable, this);

            hashtable = new Hashtable();
            hashtable.Add("width", "500");
            hashtable.Add("point", new Point(245, 100));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "search");
            hashtable.Add("enabled", true);
            tb = cm.getTextBox(hashtable, pn);
            tb.Font = new Font("맑은 고딕", 15, FontStyle.Bold);
            tb.BorderStyle = BorderStyle.None;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 30));
            hashtable.Add("point", new Point(750, 100));
            hashtable.Add("color", Color.Gray);
            hashtable.Add("name", "searchbt");
            hashtable.Add("text", "검색");
            hashtable.Add("click", (EventHandler)btn_click);
            searchBT = cm.getButton(hashtable, pn);
            searchBT.FlatStyle=FlatStyle.Flat;

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

        private void btn_click(object o,EventArgs e)
        {
            MessageBox.Show("클릭");
        }

        public void champinfo()
        {
            close = new championMoreInfo(this);
            close.WindowState = FormWindowState.Maximized;
            close.FormBorderStyle = FormBorderStyle.None;
            close.MdiParent = this;
            close.Dock = DockStyle.Fill;
            championList.Controls.Add(close);
            close.Show();
        }//챔피언리스트창에서 챔피언상세정보로이동
    }
}
