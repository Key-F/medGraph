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
using FormSerialisation;


namespace medGraph
{
    public partial class Form1 : Form
    {
        
        PointPairList list1 = new PointPairList();
        double pa, pb, pc;
        Form2 zxc;


        public Form1()
        {           
            InitializeComponent();
            try
            {
                FormSerialisor.Deserialise(this, Application.StartupPath + @"\serialise.xml");
            }
            catch (Exception e) { MessageBox.Show("Ошибка десериализации"); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("");
            zxc = new Form2(pa, pb, pc, list1, null, 0);
            zxc.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("");
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)
            {                
                zxc = new Form2(pa, pb, pc, list1, secondPart(i, ""), i);
                zxc.Show();
            }
        }

        public void firstPart(string prefix)
        {
            list1.Clear();
            for (int i = 1; i <= Convert.ToInt32(textBox14.Text); i++)
            {
                string controlNameC = prefix + "c" + i;
                var controls = this.Controls.Find(controlNameC, true);
                var control = controls.FirstOrDefault();
                double.TryParse(control.Text.Replace('.', ','), out double c);
                string controlNameD1 = prefix + "d" + i + "1";
                string controlNameD2 = prefix + "d" + i + "2";
                var controlsD1 = this.Controls.Find(controlNameD1, true);
                var controlD1 = controlsD1.FirstOrDefault();
                var controlsD2 = this.Controls.Find(controlNameD2, true);
                var controlD2 = controlsD2.FirstOrDefault();
                double.TryParse(controlD1.Text.Replace('.', ','), out double d1);
                double.TryParse(controlD2.Text.Replace('.', ','), out double d2);
                double dd = (d1 + d2) / 2;
                if (dd != 0 || c !=0 )
                list1.Add(c, dd);
            }
            IList<double> XandY = PP.PPToAxe(list1);

            Eapp epp = new Eapp();
            double[] X = new double[XandY.Count / 2];
            double[] Y = new double[XandY.Count / 2];
            for (int i = 0; i < XandY.Count / 2; i++)
            {
                X[i] = XandY[i];
                Y[i] = XandY[i + XandY.Count / 2];

            }
            pa = 0;
            pb = 0;
            pc = 0;
            epp.approx2(XandY.Count / 2, X, Y, ref pa, ref pb, ref pc);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormSerialisor.Serialise(this, Application.StartupPath + @"\serialise.xml");
         
            }
            catch (Exception ee) { MessageBox.Show("Ошибка сериализации"); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("a_");
            zxc = new Form2(pa, pb, pc, list1, null, 0);
            zxc.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("a_");
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)
            {
                zxc = new Form2(pa, pb, pc, list1, secondPart(i, "a_"), i);
                zxc.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("m_");
            zxc = new Form2(pa, pb, pc, list1, null, 0);
            zxc.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("m_");
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)
            {
                zxc = new Form2(pa, pb, pc, list1, secondPart(i, "m_"), i);
                zxc.Show();
            }
        }

        public PointPairList secondPart(int i, string prefix)
        {
            string controlNameD1 = prefix + "ed" + i + "1";
            string controlNameD2 = prefix + "ed" + i + "2";
            var controlsD1 = this.Controls.Find(controlNameD1, true);
            var controlD1 = controlsD1.FirstOrDefault();
            var controlsD2 = this.Controls.Find(controlNameD2, true);
            var controlD2 = controlsD2.FirstOrDefault();
            double.TryParse(controlD1.Text.Replace('.', ','), out double d1);
            double.TryParse(controlD2.Text.Replace('.', ','), out double d2);
            double dd = (d1 - d2);          
            string controlNameresultD = prefix + "resultD" + i;
            var controlsNameresultD = this.Controls.Find(controlNameresultD, true);
            var controlresultD = controlsNameresultD.FirstOrDefault();
            controlresultD.Text = Convert.ToString(dd);
            double c = SolveQuadratic(pa, pb, pc, dd);
            string contolNameConc = prefix + "conc" + i;
            var controlsConc = this.Controls.Find(contolNameConc, true);
            var controlConc = controlsConc.FirstOrDefault();

            //controlConc.Text = Convert.ToString(Math.Round(c, 3));
            controlConc.Text = String.Format("{0:f3}", c);

            PointPairList res = new PointPairList();
            res.Add(0, dd);
            res.Add(c, dd);
            res.Add(c, 0);
            return res;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            clearGraph();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clearGraph();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clearGraph();
        }

        public void clearGraph()
        {
            Application.OpenForms
             .OfType<Form>()
             .Where(form => String.Equals(form.Name, "Form2"))
             .ToList()
             .ForEach(form => form.Close());
        }

        public static double SolveQuadratic(double a, double b, double c, double y)
        {
            c = c - y;
            double sqrtpart = b * b - 4 * a * c;
            double x, x1, x2;
            if (sqrtpart > 0)
            { 
                x1 = (-b + System.Math.Sqrt(sqrtpart)) / (2 * a);
                if (x1 > 0) return x1;
                x2 = (-b - System.Math.Sqrt(sqrtpart)) / (2 * a);
                return x2;
            }
            else
            {
                x = (-b + System.Math.Sqrt(sqrtpart)) / (2 * a);
                return x;
            }
        }
    }

   

}
