using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace medGraph
{
    public partial class Form2 : Form
    {      
        public Form2()
        {
            InitializeComponent();
        }

        
        public Form2(double pa, double pb, double pc, PointPairList testData, PointPairList res, int num)
        {
            if (res == null && num != 0)
            {
                this.Close();
                return;
            }

            testData.Sort();
            InitializeComponent();
            GraphPane pane = zedGraphControl1.GraphPane;

            PointPairList p = new PointPairList();

            
            for (double i = 0; i <= testData[testData.Count - 1].X; i += 0.5)
            {
                p.Add(i, pa*i*i + pb*i +pc);
                // p[i].X = i;
                // p[i].Y = Math.Log(o + b * i);
            }

            pane.XAxis.Title.Text = "C"; //подпись оси X
            pane.YAxis.Title.Text = "D"; //подпись оси Y

            if (num != 0)
            {
                pane.Title.Text = "Номер пробы: " + num;
                this.Text = "Номер пробы: " + num;
            }
            else pane.Title.Text = "le graffique";

            // Очистим список кривых 
            pane.CurveList.Clear();


            // Обводка ромбиков будут рисоваться голубым цветом (Color.Blue),
            // Опорные точки - ромбики (SymbolType.Diamond)
            LineItem myCurve = pane.AddCurve("Аппроксимация", p, Color.Black, SymbolType.None);
            LineItem myCurve1 = pane.AddCurve("Эксперементальные данные", testData, Color.BlueViolet, SymbolType.Circle);


            LineItem myCurve2 = pane.AddCurve("Проекция", res, Color.DarkGray, SymbolType.None);



            zedGraphControl1.AxisChange();

            zedGraphControl1.Invalidate();

            this.Show();

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
        }
            
    }
}
