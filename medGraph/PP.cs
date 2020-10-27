using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace medGraph
{
    public class PP
    {

        IList<double> xAxisValues;
        IList<double> yAxisValues;

        public PP(PointPairList pp)
        {
            IList<double> temp = PPToAxe(pp);
            xAxisValues = new List<double>();
            yAxisValues = new List<double>();
            for (int i = 0; i < temp.Count / 2; i++)
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
