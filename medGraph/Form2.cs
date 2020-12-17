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

        
        public Form2(double pa, double pb, double pc, double ea, double eb, double lk, double lb, PointPairList testData, PointPairList res, int num)
        {
            if (res == null && num != 0)
            {
                this.Close();
                return;
            }

            testData.Sort();
            InitializeComponent();
            GraphPane pane = zedGraphControl1.GraphPane;

            PointPairList pl = new PointPairList();
            PointPairList pp = new PointPairList();
            PointPairList pe = new PointPairList();

            int limitX = (int)testData[testData.Count - 1].X + 1;

            pl = Eapp.addPointsL(limitX, lk, lb);
            pp = Eapp.addPointsP(limitX, pa, pb, pc);
            pe = Eapp.addPointsE(limitX, ea, eb);

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
            LineItem myCurve;
            if (Form1.checkbox == 0)
                myCurve = pane.AddCurve("Линейная аппроксимация", pl, Color.Black, SymbolType.None);
            else if (Form1.checkbox == 1)
                myCurve = pane.AddCurve("Квадратичная ппроксимация", pp, Color.Black, SymbolType.None);
            else if (Form1.checkbox == 2)
                pane.AddCurve("Экспоненциальная аппроксимация", pe, Color.Black, SymbolType.None);
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
