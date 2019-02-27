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
        Panel pn,championList;
        Button searchBT;
        Form close;

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
            Commons cm = new Commons();

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

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 25));
            hashtable.Add("point", new Point(750, 100));
            hashtable.Add("color", Color.Gray);
            hashtable.Add("name", "searchbt");
            hashtable.Add("text", "검색");
            hashtable.Add("click", (EventHandler)btn_click);
            searchBT = cm.getButton(hashtable, pn);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(990, 700));
            hashtable.Add("point", new Point(0, 150));
            hashtable.Add("color", Color.Black);
            hashtable.Add("name", "ListPN");
            championList = cm.getPanel(hashtable, pn);

            ChampionINFO ci = new ChampionINFO(this, championList);
            //ci = new ChampionINFO(this, championList);
            ci.WindowState = FormWindowState.Maximized;
            ci.FormBorderStyle = FormBorderStyle.None;
            ci.MdiParent = this;
            //ci.Dock = DockStyle.Fill;
            championList.Controls.Add(ci);
            ci.Show();
        }

        private void btn_click(object o,EventArgs e)
        {
            
        }
    }
}
