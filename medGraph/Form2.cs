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
        PointPairList list1 = new PointPairList();
        public Form2()
        {
            InitializeComponent();
        }

        
        public Form2(double pa, double pb, double pc, PointPairList list1)
        {
            list1.Sort();
            InitializeComponent();
            GraphPane pane = zedGraphControl1.GraphPane;

            PointPairList p = new PointPairList();

            
            for (double i = 0; i < list1[list1.Count - 1].X; i += 0.5)
            {
                p.Add(i, pa*i*i + pb*i +pc);
                // p[i].X = i;
                // p[i].Y = Math.Log(o + b * i);
            }

            pane.XAxis.Title.Text = "C"; //подпись оси X
            pane.YAxis.Title.Text = "D"; //подпись оси Y

            pane.Title.Text = "le graffique";


            // Очистим список кривых 
            pane.CurveList.Clear();


            // Обводка ромбиков будут рисоваться голубым цветом (Color.Blue),
            // Опорные точки - ромбики (SymbolType.Diamond)
            LineItem myCurve = pane.AddCurve("Аппроксимация", p, Color.Black, SymbolType.None);
            LineItem myCurve1 = pane.AddCurve("Эксперементальные данные", list1, Color.BlueViolet, SymbolType.Circle);



            zedGraphControl1.AxisChange();

            zedGraphControl1.Invalidate();

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
        }
            
    }
}
