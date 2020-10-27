using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZedGraph;


namespace medGraph
{
    public partial class Form1 : Form
    {
        
        PointPairList list1 = new PointPairList();
        List<double> En = new List<double>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            list1.Clear();
            for (int i = 1; i <= Convert.ToInt32(textBox14.Text); i++)
            {
                string controlNameC = "c" + i;
                var controls = this.Controls.Find(controlNameC, true);
                var control = controls.FirstOrDefault();
                double c = Convert.ToDouble(control.Text);
                string controlNameD1 = "d" + i + "1";
                string controlNameD2 = "d" + i + "2";
                var controlsD1 = this.Controls.Find(controlNameD1, true);
                var controlD1 = controlsD1.FirstOrDefault();
                var controlsD2 = this.Controls.Find(controlNameD2, true);
                var controlD2 = controlsD2.FirstOrDefault();
                double dd = (Convert.ToDouble(controlD1.Text) + Convert.ToDouble(controlD2.Text)) / 2;
                list1.Add(c, dd);
            }
            IList<double> XandY =  PP.PPToAxe(list1);

            Eapp epp = new Eapp();
            double[] X = new double[XandY.Count / 2];
            double[] Y = new double[XandY.Count / 2];
            for (int i = 0; i < XandY.Count/2; i++)
            {
                X[i] = XandY[i];
                Y[i] = XandY[i + XandY.Count / 2];

            }
            //double[] func = new double[2];
            double pa = 0, pb = 0, pc = 0;
            epp.approx2(XandY.Count / 2, X, Y, ref pa, ref pb, ref pc);

            double f = XandY[0];
            double a1=0, a2=0, a3=0, a4=0, b, o;
            for (int i = 0; i < XandY.Count/2; i++)
            {
                a1 = a1 + XandY[i] * Math.Log(XandY[i + XandY.Count / 2]);
                a2 = a2 + XandY[i];
                a3 = a3 + Math.Log(XandY[i + XandY.Count / 2]);
                a4 += XandY[i] * XandY[i];
               
            }
            b = (XandY.Count / 2 * a1 - a2 * a3)/(XandY.Count/2 * a4 - Math.Pow(a2, 2));
            o = (1 / (XandY.Count / 2))* a3 - (b / (XandY.Count / 2))*a2;

            PP p = new PP(list1);
            Math.Log(5);
          
            Form2 zxc = new Form2(pa, pb, pc, list1);
            zxc.Show();
        }


        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)
            {
                string controlNameD1 = "ed" + i + "1";
                string controlNameD2 = "ed" + i + "2";
                var controlsD1 = this.Controls.Find(controlNameD1, true);
                var controlD1 = controlsD1.FirstOrDefault();
                var controlsD2 = this.Controls.Find(controlNameD2, true);
                var controlD2 = controlsD2.FirstOrDefault();
                double dd = (Convert.ToDouble(controlD1.Text) - Convert.ToDouble(controlD2.Text));
                string controlNameresultD = "resultD" + i;
                var controlsNameresultD = this.Controls.Find(controlNameresultD, true);
                var controlresultD = controlsNameresultD.FirstOrDefault();
                controlresultD.Text = Convert.ToString(dd);
            }
        }

 
    }

   

}
