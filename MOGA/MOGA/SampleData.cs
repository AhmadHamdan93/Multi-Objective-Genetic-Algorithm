using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOGA
{
    class SampleData
    {
        public double X;
        public double Y;
        public double f1;
        public double f2;

        public SampleData()
        {
            this.X = 0.0; this.Y = 0.0; this.f1 = 0.0; this.f2 = 0.0;
        }
        public void initialData(Random r)
        {
            do
            {

                this.X = Math.Round(r.NextDouble() * 5, 2);
                this.Y = Math.Round(r.NextDouble() * 3, 2);

            } while (checkValues());

            evaluate();
        }
        public Boolean checkValues()
        {
            double c1 = Math.Pow((this.X - 5), 2) + Math.Pow((this.Y), 2) - 25;
            double c2 = -Math.Pow((this.X - 8), 2) - Math.Pow((this.Y + 3), 2) + 7.7;
            return !(c1 <= 0) || !(c2 <= 0);
        }
        public void evaluate()
        {
            this.f1 = Math.Round((4 * Math.Pow(this.X, 2) + 4 * Math.Pow(this.Y, 2)), 2);
            this.f2 = Math.Round((Math.Pow((this.X - 5), 2) + Math.Pow((this.Y - 5), 2)), 2);
        }
    }
}
