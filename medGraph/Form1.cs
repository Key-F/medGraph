﻿using System;
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
        static int count = 0;
        PointPairList list1 = new PointPairList();
        List<double> En = new List<double>();
        public Form1()
        {
            InitializeComponent();
            label8.Visible = false;
        }


        private void button1_Click(object sender, EventArgs e) // Начало всего
        {

            label8.Visible = true;
            label8.Update();


            count = 0;
            En.Clear();

            int N = Convert.ToInt32(textBox1.Text);
            int J = Convert.ToInt32(textBox3.Text);
            int M = Convert.ToInt32(textBox2.Text);
            int kolX; // Количество значений X

            if (formula.ParseDouble1(textBox6.Text) == 0)
            {
                if (formula.ParseDouble1(textBox5.Text) == 0)
                {
                    if (formula.ParseDouble1(textBox4.Text) == 0)
                        kolX = 0;
                    else
                        kolX = 1;
                }
                else kolX = 2;
            }
            else kolX = 3;

            if (formula.ParseDouble1(textBox5.Text) == 0) // второй элемент не может быть нулевым
            {
                MessageBox.Show("Входной вектор не может быть нулевым", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                label8.Visible = false;
                return;
            }

            double[] X = new double[kolX - 1];
            for (int i = 5; i < 5 + X.Length; i++)
            {
                string controlName = "textBox" + i;
                var controls = this.Controls.Find(controlName, true); // http://stackoverflow.com/questions/32224320/how-is-the-number-one-pick-textbox-from-text-box
                var control = controls.FirstOrDefault();
                var textBox = (TextBox)control;
                if (control != null)
                    X[i - 5] = formula.ParseDouble1(textBox.Text); // Норм работает
            }


            int kol10t; // Количество значений 10t

            if (formula.ParseDouble1(textBox9.Text) == 0)
            {
                if (formula.ParseDouble1(textBox8.Text) == 0)
                {
                    if (formula.ParseDouble1(textBox7.Text) == 0)
                        kol10t = 0;
                    else
                        kol10t = 1;
                }
                else kol10t = 2;
            }
            else kol10t = 3;
            if (kol10t == 0)
            {
                MessageBox.Show("10t не может быть нулевым", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                label8.Visible = false;
                return;
            }

            double[] t10 = new double[kol10t];
            for (int i = 7; i < 7 + t10.Length; i++)
            {
                string controlName = "textBox" + i;
                var controls = this.Controls.Find(controlName, true); // http://stackoverflow.com/questions/32224320/how-is-the-number-one-pick-textbox-from-text-box
                var control = controls.FirstOrDefault();
                var textBox = (TextBox)control;
                if (control != null)
                    t10[i - 7] = formula.ParseDouble1(textBox.Text); // Норм работает
            }
            int tochnost = Convert.ToInt32(textBox13.Text);

            double[] netSKR = new double[J + 1]; // + 1 для 1 
            double[] netVIH = new double[M + 1]; // комбинированные входы нейронов скрытого и выходного слоев
            double[] OutSKR = new double[J];
            double[] OutVIH = new double[M]; // выходные сигналы нейронов скрытого и выходного слоев;
            double[] SigmaSKR = new double[J + 1];
            double[] SigmaVIH = new double[M + 1]; // ошибки скрытого и выходного слоев.
            double[,] WSKR = new double[N + 1, J];
            double[,] WVIH = new double[J + 1, M]; // веса скрытого и выходного слоев


            if (radioButton2.Checked == true)  // Вывод всей инфы в рабочее поле
            {
                string a1 = textBox11.Text;
                string b1 = textBox12.Text;
                richTextBox1.AppendText("       a = " + a1 + "; b = " + b1 + Environment.NewLine);
            }
            richTextBox1.AppendText("       N-J-M: " + textBox1.Text + "-" + textBox3.Text + "-" + textBox2.Text + Environment.NewLine);
            richTextBox1.AppendText("       x: (" + textBox4.Text + ", " + textBox5.Text + ", " + textBox6.Text + ")" + Environment.NewLine);
            richTextBox1.AppendText("      10t: (" + textBox7.Text + ", " + textBox8.Text + ", " + textBox9.Text + ")" + Environment.NewLine);


            var random = new Random();
            if (radioButton1.Checked == true)
            {
                double WW = formula.ParseDouble1(textBox10.Text);
                for (int j = 0; j < J; j++)
                    for (int i = 0; i < N + 1; i++)
                    {
                        WSKR[i, j] = WW;
                    }
                for (int j = 0; j < M; j++)
                    for (int i = 0; i < J + 1; i++)
                    {
                        WVIH[i, j] = WW;
                    }
            }
            int a, b;
            // int iter = 1;
            // for (int asd = 1; asd < tochnost; asd++)
            //  iter = iter * 10;
            if (radioButton2.Checked == true)
            {
                a = Convert.ToInt32(textBox11.Text);
                b = Convert.ToInt32(textBox12.Text);
                for (int j = 0; j < J; j++)
                    for (int i = 0; i < N + 1; i++)
                    {
                        WSKR[i, j] = (float)random.Next(10 * a, 10 * b) / 10f; // iter * 10f для точности
                    }
                for (int j = 0; j < M; j++)
                    for (int i = 0; i < J + 1; i++)
                    {
                        WVIH[i, j] = (float)random.Next(10 * a, 10 * b) / 10f;
                    }
            }

            for (int i = 0; i < t10.Length; i++)
                t10[i] = t10[i] * 0.1;

            work(tochnost, N, J, M, t10, X, netSKR, netVIH, OutSKR, OutVIH, SigmaSKR, SigmaVIH, WSKR, WVIH);
        }
        public void work(int tochnost, int N, int J, int M, double[] t, double[] X1, double[] net1, double[] net2, double[] Out1, double[] Out2, double[] Sigma1, double[] Sigma2, double[,] W1, double[,] W2)
        {

            {

                net1 = part1.net(W1, X1, N, J);
                Out1 = part1.Out(tochnost, Out1, net1, J);
                net2 = part1.net(W2, Out1, J, M);
                Out2 = part1.Out(tochnost, Out2, net2, M);

                Sigma1 = part2.sigmaJ(net1, J, M, W2, Sigma2);
                Sigma2 = part2.sigmaM(net2, M, t, Out2);

                W1 = formula.SetW(W1, N, J, 1, X1, Sigma1);
                W2 = formula.SetW(W2, J, M, 1, Out1, Sigma2);

                double err = formula.error(tochnost, t, Out2, M);

                En.Add(err);

                if (err > 0)
                {
                    richTextBox1.ScrollToCaret();
                    count++;
                    richTextBox1.AppendText("                   Номер эпохи: (" + count + ") ");
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("   Выход нейронов внешнего слоя: ");

                    for (int i = 0; i < Out2.Count(); i++)
                    {
                        string vvv = Convert.ToString(Math.Round(Out2[i], tochnost + 1));
                        richTextBox1.AppendText(vvv + " ");
                    }
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("  " + "  Веса скрытого слоя W1" + Environment.NewLine);
                    for (int i = 0; i < N + 1; i++)
                    {
                        for (int j = 0; j < J; j++)
                        {
                            richTextBox1.AppendText("   " + Convert.ToString(Math.Round(W1[i, j], tochnost)) + "   ");
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                    richTextBox1.ScrollToCaret();
                    richTextBox1.AppendText("  Веса внешнего слоя W2" + (Environment.NewLine));
                    for (int i = 0; i < J; i++)
                    {
                        for (int j = 0; j < M; j++)
                        {
                            richTextBox1.AppendText("   " + Convert.ToString(Math.Round(W2[i, j], tochnost)) + "   ");
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                    richTextBox1.AppendText("      Среднеквадратическая ошибка: ");
                    richTextBox1.AppendText(Convert.ToString(Math.Round(err, tochnost + 1)) + Environment.NewLine);
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.ScrollToCaret();
                    work(tochnost, N, J, M, t, X1, net1, net2, Out1, Out2, Sigma1, Sigma2, W1, W2);

                }
                else
                {
                    count++;
                    richTextBox1.AppendText("                   Номер эпохи: (" + count + ") ");
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("   Выход нейронов внешнего слоя: ");

                    for (int i = 0; i < Out2.Count(); i++)
                    {
                        string vvv = Convert.ToString(Math.Round(Out2[i], tochnost + 1));
                        richTextBox1.AppendText(vvv + "  ");
                    }
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("   Веса скрытого слоя W1" + Environment.NewLine);
                    for (int i = 0; i < N + 1; i++)
                    {
                        for (int j = 0; j < J; j++)
                        {
                            richTextBox1.AppendText("   " + Convert.ToString(Math.Round(W1[i, j], tochnost)) + "   ");
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }

                    richTextBox1.AppendText("  Веса внешнего слоя W2" + (Environment.NewLine));
                    for (int i = 0; i < J; i++)
                    {
                        for (int j = 0; j < M; j++)
                        {
                            richTextBox1.AppendText("   " + Convert.ToString(Math.Round(W2[i, j], tochnost)) + "   ");
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                    richTextBox1.AppendText("      Среднеквадратическая ошибка: ");
                    richTextBox1.AppendText(Convert.ToString(Math.Round(err, tochnost + 1)) + Environment.NewLine);
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.ScrollToCaret();


                    label8.Visible = false;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e) // Копирование в буфер обмена
        {
            label8.Visible = true;
            label8.Update();
            string text = string.Empty;

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
                text += richTextBox1.Lines[i] + Environment.NewLine;
            if (text == "") MessageBox.Show("Нечего копировать", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            else
            {
                Clipboard.SetText(text);
                MessageBox.Show("Скопировано", "Сообщение", MessageBoxButtons.OK);
            }
            label8.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e) // Запись лога
        {
            label8.Visible = true;
            label8.Update();

            string s = DateTime.Now.ToString("dd_MMMM_yyyy_HH-mm-ss");
            string filename = "log_" + s + ".txt";
            //string path = (Directory.GetCurrentDirectory() + "\\.." + "\\.."); // Плохой способ
            string path = Directory.GetCurrentDirectory();
            string npath = Directory.GetParent(path).FullName;
            string nnpath = Directory.GetParent(npath).FullName;
            if (!Directory.Exists(nnpath + "/logs"))
            {
                Directory.CreateDirectory(nnpath + "/logs");
            }
            //File.Create(path + "/logs/" + filename);
            // File.WriteAllText(filename, richTextBox1.Text);
            FileStream fs = File.Create(nnpath + "/logs/" + filename);
            StreamWriter writer = new StreamWriter(fs);
            string[] tempArray = richTextBox1.Lines;

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
                writer.WriteLine(tempArray[i]); //что-то пишем
            writer.Close();
            MessageBox.Show("Сохранено в " + nnpath + "\\logs\\" + filename, "Saved Log File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            label8.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)  // Мудреная запись              
        {
            // Create a SaveFileDialog to request a path and file name to save to.
            SaveFileDialog saveFile1 = new SaveFileDialog();

            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFile1.DefaultExt = "*.rtf";
            saveFile1.Filter = "TXT Files (*.txt)|*.txt|RTF Files|*.rtf|All files (*.*)|*.*"; // https://msdn.microsoft.com/ru-ru/library/e4a710b1(v=vs.110).aspx

            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
            {
                // Save the contents of the RichTextBox into the file.
                richTextBox1.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

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
            double a1=0, a2=0, a3=0, a4=0, b, o, a5=0, a6=0;
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

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

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
                PointPair p = new PointPair(dd, 0);
                Form2 xc = new Form2(p);
                xc.Show();
            }
        }

        private void C4_TextChanged(object sender, EventArgs e)
        {

        }
    }


    public class Trendline
    {
        private readonly IList<double> xAxisValues;
        private readonly IList<double> yAxisValues;
        private double count;
        private double xAxisValuesSum;
        private double xxSum;
        private double xySum;
        private double yAxisValuesSum;

        public Trendline(IList<double> yAxisValues, IList<double> xAxisValues)
        {
            this.yAxisValues = yAxisValues;
            this.xAxisValues = xAxisValues;

            this.Initialize();
        }

        public double Slope { get; private set; }
        public double doubleercept { get; private set; }
        public double Start { get; private set; }
        public double End { get; private set; }

        private void Initialize()
        {
            this.count = this.yAxisValues.Count;
            this.yAxisValuesSum = this.yAxisValues.Sum();
            this.xAxisValuesSum = this.xAxisValues.Sum();
            this.xxSum = 0;
            this.xySum = 0;

            for (int i = 0; i < this.count; i++)
            {
                this.xySum += (this.xAxisValues[i] * this.yAxisValues[i]);
                this.xxSum += (this.xAxisValues[i] * this.xAxisValues[i]);
            }

            this.Slope = this.CalculateSlope();
            this.doubleercept = this.Calculatedoubleercept();
            this.Start = this.CalculateStart();
            this.End = this.CalculateEnd();
        }

        private double CalculateSlope()
        {
            try
            {
                return ((this.count * this.xySum) - (this.xAxisValuesSum * this.yAxisValuesSum)) / ((this.count * this.xxSum) - (this.xAxisValuesSum * this.xAxisValuesSum));
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        private double Calculatedoubleercept()
        {
            return (this.yAxisValuesSum - (this.Slope * this.xAxisValuesSum)) / this.count;
        }

        private double CalculateStart()
        {
            return (this.Slope * this.xAxisValues.First()) + this.doubleercept;
        }

        private double CalculateEnd()
        {
            return (this.Slope * this.xAxisValues.Last()) + this.doubleercept;
        }
    }

    public class PP
    {
        PointPairList list;
        IList<double> xAxisValues;
        IList<double> yAxisValues;

        public PP(PointPairList pp) {
            IList<double> temp = PPToAxe(pp);
            xAxisValues = new List<double>();
            yAxisValues = new List<double>();
            for (int i = 0; i < temp.Count/2; i++)
            {
                xAxisValues.Add(temp[i]);
            }
            for (int i = 0; i < temp.Count / 2; i++)
            {
                yAxisValues.Add(temp[i + temp.Count / 2]);
            }


        }

        public static IList<double> PPToAxe(PointPairList pp)
        {
            var points = new List<double>(); // первая половина X, вторая - Y
            for (int i = 0; i < pp.Count; i++) 
            {
                points.Add(pp[i].X);
            }
            for (int i = 0; i < pp.Count; i++)
            {
                points.Add(pp[i].Y);
            }
            return points;
        }

    }

}
