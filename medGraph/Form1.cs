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
        double pa, pb, pc, ea, eb, lk, lb;
        public static int checkbox = 0;
        public Form1()
        {           
            InitializeComponent();
            try
            {
                FormSerialisor.Deserialise(this, Application.StartupPath + @"\serialise.xml");
            }
            catch (Exception) { MessageBox.Show("Ошибка десериализации"); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("");
            new Form2(pa, pb, pc, ea, eb, lk, lb, list1, null, 0);            
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
                if (dd != 0 && c !=0 )
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
            ea = 0;
            eb = 0;
            lk = 0;
            lb = 0;
            if (radioButton1.Checked == true)
            {
                epp.approxl(XandY.Count / 2, X, Y, ref lk, ref lb);
                checkbox = 0;
            }
            else if (radioButton2.Checked == true)
            {                 
                epp.approx2(XandY.Count / 2, X, Y, ref pa, ref pb, ref pc);
                checkbox = 1;
            }
            else if (radioButton3.Checked == true)
            {
                epp.approx(XandY.Count / 2, X, Y, ref ea, ref eb);
                checkbox = 2;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormSerialisor.Serialise(this, Application.StartupPath + @"\serialise.xml");
         
            }
            catch (Exception) { MessageBox.Show("Ошибка сериализации"); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("a_");
            new Form2(pa, pb, pc, ea, eb, lk, lb, list1, null, 0);           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("a_");
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)            
                new Form2(pa, pb, pc, ea, eb, lk, lb, list1, secondPart(i, "a_"), i);                            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("m_");
            new Form2(pa, pb, pc, ea, eb, lk, lb, list1, null, 0);            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clearGraph();
            firstPart("m_");
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)           
                new Form2(pa, pb, pc, ea, eb, lk, lb, list1, secondPart(i, "m_"), i);            
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
            double dd = Math.Abs(d1 - d2);
            if (dd == 0) return null;
            string controlNameresultD = prefix + "resultD" + i;
            var controlsNameresultD = this.Controls.Find(controlNameresultD, true);
            var controlresultD = controlsNameresultD.FirstOrDefault();
            controlresultD.Text = Convert.ToString(dd);
            double c = 0;
            if (radioButton1.Checked == true)
                c = SolveLinear(lk, lb, dd);
            else if (radioButton2.Checked == true)
                c = SolveQuadratic(pa, pb, pc, dd);
            else if (radioButton3.Checked == true)
                c = SolveExp(ea, eb, dd);
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

        private void label97_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            firstPart("a_");
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)            
                secondPart(i, "a_");
        }

        private void button11_Click(object sender, EventArgs e)
        {       
            clearGraph();
            firstPart("");
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)            
            new Form2(pa, pb, pc, ea, eb, lk, lb, list1, secondPart(i, ""), i);                           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            firstPart("");
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)
                secondPart(i, "");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            firstPart("m_");
            for (int i = 1; i <= Convert.ToInt32(textBox48.Text); i++)
                secondPart(i, "m_");
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
        public static double SolveLinear(double k, double b, double y)
        {
            return (y - b) / k;
        }
        public static double SolveExp(double a, double b, double y)
        {
            return Math.Log(y / a) / b;
        }
    }

   

}
