using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medGraph
{
    class Eapp
    {
        double n;
        double[] xList;
        double[] yList;

        public Eapp() { }

        static double Sum(int n, double[] mass)
        {
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += mass[i];
            }
            return sum;
        }
        public void approx(int n, double[] xList, double[] yList, double[] func)
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
            // Sum(n, X2)a1 + Sum(n, X)a0 = Sum(n, XY)
            // Sum(n, X)a1 + na0 = Sum(n, Y)
            a0 = (Sum(n, X2) * Sum(n, Y) - Sum(n, XY) * Sum(n, X)) / (Sum(n, X2) * n - Sum(n, X) * Sum(n, X));
            a1 = (Sum(n, XY) * n - Sum(n, X) * Sum(n, X)) / (Sum(n, X2) * n - Sum(n, X) * Sum(n, X));
            func[0] = Math.Pow(Math.E, a0);
            func[1] = a1;
            //F = a * e^(b*x)
        }
    }
}
