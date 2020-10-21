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

        public Form2(PointPair point)
        {
            InitializeComponent();
            GraphPane pane = zedGraphControl1.GraphPane;

            pane.XAxis.Title.Text = "C"; //подпись оси X
            pane.YAxis.Title.Text = "D"; //подпись оси Y

            pane.Title.Text = "le graffique";


            // Очистим список кривых 
            pane.CurveList.Clear();

          
            // Обводка ромбиков будут рисоваться голубым цветом (Color.Blue),
            // Опорные точки - ромбики (SymbolType.Diamond)
            LineItem myCurve = pane.AddCurve("", list1, Color.Black, SymbolType.Circle);

            // Создадим список точек
            PointPairList list2 = new PointPairList();
            list2.Add(0, point.Y);
            list2.Add(point.X, point.Y);
            list2.Add(point.X, 0);

            LineItem myCurve2 = pane.AddCurve("", list2, Color.Violet, SymbolType.Circle);

            zedGraphControl1.AxisChange();

            zedGraphControl1.Invalidate();

        }
        public Form2(PointPairList En)
        {
            InitializeComponent();
             GraphPane pane = zedGraphControl1.GraphPane;

            pane.XAxis.Title.Text = "C"; //подпись оси X
            pane.YAxis.Title.Text = "D"; //подпись оси Y
            
                pane.Title.Text = "le graffique"; 
            

            // Очистим список кривых 
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list1 = En; // Для y
            
            int i = 1;

          
            
            
            // Обводка ромбиков будут рисоваться голубым цветом (Color.Blue),
            // Опорные точки - ромбики (SymbolType.Diamond)
            LineItem myCurve = pane.AddCurve("", En, Color.Black, SymbolType.Circle);
            

           
                myCurve.Line.IsVisible = true;  
               
            // !!!
            // Цвет заполнения отметок (ромбиков) - голубой
            myCurve.Symbol.Fill.Color = Color.White;
            

            // !!!
            // Тип заполнения - сплошная заливка
            myCurve.Symbol.Fill.Type = FillType.Solid;
                       
            myCurve.Symbol.Size = 4;

           
            zedGraphControl1.AxisChange();

            
            zedGraphControl1.Invalidate();
           
        }

        public Form2(double b, double o, PointPairList list1)
        {

            InitializeComponent();
            GraphPane pane = zedGraphControl1.GraphPane;

            PointPairList p = new PointPairList();

            for (int i = 0; i <= 100; i++)
            {
                p.Add(i, Math.Pow(Math.E, o + b * i));
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
            LineItem myCurve = pane.AddCurve("", p, Color.Black, SymbolType.Circle);
            LineItem myCurve1 = pane.AddCurve("", list1, Color.Black, SymbolType.Circle);



            zedGraphControl1.AxisChange();

            zedGraphControl1.Invalidate();

        }
       

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
        }
            
    }
}
