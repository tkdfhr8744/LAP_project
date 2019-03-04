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
using System.Windows.Forms.DataVisualization.Charting;

namespace LAP
{
    public partial class SummonerINFO : Form
    {
        Hashtable hashtable;
        PictureBox back,Icon;
        Label SummonerName;
        Chart chart1;
        Panel chartpn;

        Commons cm;

        public SummonerINFO()
        {
            InitializeComponent();
            Load += SummonerINFO_Load;
        }

        private void SummonerINFO_Load(object sender, EventArgs e)
        {
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
            Icon = cm.getPictureBox(hashtable, this);
            Icon.BackColor = Color.Gray;

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(300, 40));
            hashtable.Add("point", new Point(270, 60));
            hashtable.Add("color", Color.Yellow);
            hashtable.Add("name", "summonerName");
            hashtable.Add("text", "소환사명");
            SummonerName = cm.getLabel(hashtable, this);
            SummonerName.Font = new Font("맑은 고딕",20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 40));
            hashtable.Add("point", new Point(270, 110));
            hashtable.Add("color", Color.Yellow);
            hashtable.Add("name", "tier");
            hashtable.Add("text", "티어");
            SummonerName = cm.getLabel(hashtable, this);
            SummonerName.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(100, 40));
            hashtable.Add("point", new Point(380,110));
            hashtable.Add("color", Color.Yellow);
            hashtable.Add("name", "LP");
            hashtable.Add("text", "점수");
            SummonerName = cm.getLabel(hashtable, this);
            SummonerName.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(150,150));
            hashtable.Add("point", new Point(600, 60));
            hashtable.Add("color", Color.Coral);
            hashtable.Add("name", "BackgroundPN");
            chartpn = cm.getPanel(hashtable, this);

            hashtable = new Hashtable();
            hashtable.Add("size", new Size(150, 40));
            hashtable.Add("point", new Point(270, 160));
            hashtable.Add("color", Color.Yellow);
            hashtable.Add("name", "position");
            hashtable.Add("text", "주포지션");
            SummonerName = cm.getLabel(hashtable, this);
            SummonerName.Font = new Font("맑은 고딕", 20, FontStyle.Bold);

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
            chart1.Titles.Add("현 성별 구성 비율");
            chart1.Series.Add(series1);
            chart1.Series["Series1"].IsValueShownAsLabel = false;
            chartpn.Controls.Add(chart1);
            chart1.Series["Series1"].Points.AddXY(string.Format("승 {0}", 5), 5);
            chart1.Series["Series1"].Points[0].Color = Color.Blue;

            chart1.Series["Series1"].Points.AddXY(string.Format("패 {0}", 6), 6);
            chart1.Series["Series1"].Points[0].Color = Color.Red;
        }

        private void Back_click(object sender, EventArgs e)
        {
            MessageBox.Show("클릭");
        }


    }
}
