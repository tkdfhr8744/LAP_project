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
    public partial class ChampionINFO : Form
    {
        Form1 f1;
        Panel championList, rotationBack;
        Hashtable hashtable;
        Label rotationTitle;
        PictureBox pc1;
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
            hashtable.Add("size", new Size(990, 770));
            hashtable.Add("point", new Point(0, 0));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "BackgroundPN");
            rotationBack = cm.getPanel2(hashtable, this);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(90, 15));
            hashtable.Add("point", new Point(10, 5));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "rotationTitle");
            hashtable.Add("text", "금주 로테이션");
            rotationTitle = cm.getLabel(hashtable, rotationBack);
            RotationChamp();

        }

        private void RotationChamp()
        {
            int margin = 0;
            cm = new Commons();
            for(int i = 1; i < 15; i++)
            {
                hashtable = new Hashtable();
                hashtable.Add("size", new Size(50, 50));
                hashtable.Add("point", new Point(18+margin, 20));
                hashtable.Add("pictureboxsizemode", PictureBoxSizeMode.Zoom);
                pc1 = cm.getPictureBox(hashtable, rotationBack);
                pc1.Load("https://opgg-static.akamaized.net/images/lol/champion/Talon.png?image=w_140&v=15354684000");
                margin += 69;
            }
            
        }
    }
}
