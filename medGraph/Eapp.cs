using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace medGraph
{
    class Eapp
    {
        static double Sum(int n, double[] mass)
        {
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += mass[i];
            }
            return sum;
        }
        public static void Gauss3(double[] o, ref double a2, ref double a1, ref double a0)
        {
            double[] p = new double[12];
            p = o;
            for (int i = 0; i < 3; i++)
            {
                p[i + 5] = p[i + 5] - p[4] / p[0] * p[i + 1];
                p[i + 9] = p[i + 9] - p[8] / p[0] * p[i + 1];
            }
            //a0 = (a * f - c * d) / (a * e - b * d);
            //a1 = (c * e - b * f) / (a * e - b * d);
            a0 = (p[5] * p[11] - p[7] * p[9]) / (p[5] * p[10] - p[6] * p[9]);
            a1 = (p[7] * p[10] - p[6] * p[11]) / (p[5] * p[10] - p[6] * p[9]);
            a2 = (p[3] - p[2] * a0 - p[1] * a1) / p[0];
        }
        public void approxl(int n, double[] xList, double[] yList, ref double k, ref double b)
        {
            double[] x = new double[n];
            double[] y = new double[n];
            double[] x2 = new double[n];
            double[] xy = new double[n];
            x = xList;
            y = yList;
            for (int i = 0; i < n; i++)
            {
                x2[i] = x[i] * x[i];
            }
            for (int i = 0; i < n; i++)
            {
                xy[i] = x[i] * y[i];
            }
            double X = Sum(n, x);
            double Y = Sum(n, y);
            double X2 = Sum(n, x2);
            double XY = Sum(n, xy);
            k = (n * XY - (X * Y)) / (n * X2 - X * X);
            b = (Y - k * X) / n;
        }
        public void approx(int n, double[] xList, double[] yList, ref double aa, ref double bb)
        {
            double[] x = new double[n];
            double[] y = new double[n];
            double[] X = new double[n];
            double[] Y = new double[n];
            double[] X2 = new double[n];
            double[] XY = new double[n];
            double[] F = new double[n];
            double[] FF = new double[n];
            double a1 = 0;
            double a0 = 0;

            x = xList;
            y = yList;
            X = x;
            for (int i = 0; i < n; i++)
            {
                Y[i] = Math.Log(y[i]);
                X2[i] = x[i] * x[i];
                XY[i] = X[i] * Y[i];
            }
            // Решаем СЛАУ:
            // Sum(n, X2)*a1 + Sum(n, X)*a0 = Sum(n, XY)
            // Sum(n, X)*a1 + n*a0 = Sum(n, Y)

            double a = Sum(n, X2);
            double b = Sum(n, X);
            double c = Sum(n, XY);
            double d = Sum(n, X);
            double e = n;
            double f = Sum(n, Y);
            a0 = (a * f - c * d) / (a * e - b * d);
            a1 = (c * e - b * f) / (a * e - b * d);
            // a0 = (Sum(n, X2) * Sum(n, Y) - Sum(n, XY) * Sum(n, X)) / (Sum(n, X2) * n - Sum(n, X) * Sum(n, X));
            // a1 = (Sum(n, XY) * n - Sum(n, X) * Sum(n, Y)) / (Sum(n, X2) * n - Sum(n, X) * Sum(n, X));
            aa = Math.Pow(Math.E, a0);
            bb = a1;
            //F = func[0] * e^(func[1]*x)
        }
        public void approx2(int n, double[] xList, double[] yList, ref double a, ref double b, ref double c)
        {
            double[] X4 = new double[n];
            double[] X3 = new double[n];
            double[] X2 = new double[n];
            double[] X = new double[n];
            double[] X2Y = new double[n];
            double[] XY = new double[n];
            double[] Y = new double[n];
            double[] KoeffUr = new double[12];
            double a2 = 0, a1 = 0, a0 = 0;

            X = xList;
            Y = yList;
            for (int i = 0; i < n; i++)
            {
                X2[i] = X[i] * X[i];
                X3[i] = X2[i] * X[i];
                X4[i] = X3[i] * X[i];
                XY[i] = X[i] * Y[i];
                X2Y[i] = XY[i] * X[i];
            }

            KoeffUr[0] = Sum(n, X4);
            KoeffUr[1] = Sum(n, X3);
            KoeffUr[2] = Sum(n, X2);
            KoeffUr[3] = Sum(n, X2Y);
            KoeffUr[4] = Sum(n, X3);
            KoeffUr[5] = Sum(n, X2);
            KoeffUr[6] = Sum(n, X);
            KoeffUr[7] = Sum(n, XY);
            KoeffUr[8] = Sum(n, X2);
            KoeffUr[9] = Sum(n, X);
            KoeffUr[10] = n;
            KoeffUr[11] = Sum(n, Y);
    
            Gauss3(KoeffUr, ref a2, ref a1, ref a0);
            a = a2;
            b = a1;
            c = a0;
        }
        public static PointPairList addPointsL(int limitX, double lk, double lb)
        {
            PointPairList p = new PointPairList();
            for (double i = 0; i <= limitX; i += 0.1)
            {
                p.Add(i, lk * i + lb);
            }
            return p;
        }
        public static PointPairList addPointsP(int limitX, double pa, double pb, double pc)
        {
            PointPairList p = new PointPairList();
            for (double i = 0; i <= limitX; i += 0.1)
            {
                p.Add(i, pa * i * i + pb * i + pc);
            }
            return p;
        }
        public static PointPairList addPointsE(int limitX, double ea, double eb)
        {
            PointPairList p = new PointPairList();
            for (double i = 0; i <= limitX; i += 0.1)
            {
                p.Add(i, ea * Math.Pow(Math.E, eb * i));
            }
            return p;
        }
    }
}
