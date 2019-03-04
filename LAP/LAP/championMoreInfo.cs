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
    public partial class championMoreInfo : Form
    {
        Form1 f1;
        Hashtable hashtable;
        PictureBox back,champImage;
        Commons cm;
        Label champName;
        Panel pn;
        
        public championMoreInfo()
        {
            InitializeComponent();
            Load += ChampionMoreInfo_Load;
        }
        public championMoreInfo(Form1 f1)
        {
            InitializeComponent();
            Load += ChampionMoreInfo_Load;
            this.f1 = f1;
        }

        private void ChampionMoreInfo_Load(object sender, EventArgs e)
        {
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
            hashtable.Add("size", new Size(150,150));
            hashtable.Add("point", new Point(115, 60));
            hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
            hashtable.Add("click", (EventHandler)Back_click);
            champImage = cm.getPictureBox(hashtable, this);
            champImage.BackColor = Color.Gray;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(620, 200));
            hashtable.Add("point", new Point(90, 220));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "championlist");
            pn = cm.getPanel(hashtable, this);
            pn.BorderStyle = BorderStyle.FixedSingle;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 35));
            hashtable.Add("point", new Point(275, 175));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "champName");
            hashtable.Add("text", "챔피언 명");
            champName = cm.getLabel(hashtable, this);
            champName.Font = new Font("맑은 고딕", 15, FontStyle.Bold);
            
            skillBox();

        }

        private void Back_click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void skillBox()
        {
            for(int i = 0; i < 5; i++)
            {
                hashtable = new Hashtable();
                hashtable.Add("size", new Size(75, 75));
                hashtable.Add("point", new Point(275+(i*80), 60));
                hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                hashtable.Add("click", (EventHandler)Back_click);
                champImage = cm.getPictureBox(hashtable, this);
                champImage.BackColor = Color.Gray;
            }
        }
    }
}
