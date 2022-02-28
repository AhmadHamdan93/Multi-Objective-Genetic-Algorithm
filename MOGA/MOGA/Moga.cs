using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOGA
{
    class Moga
    {
        public List<SampleData> population = new List<SampleData>();
        
        int cross, mut, pop;
        Random r;
        public Moga(int pop, int cross, int mut)
        {
            this.pop = pop;
            this.cross = cross;
            this.mut = mut;
            
            r = new Random();
            for(int i=0;i<pop;i++)
            {
                SampleData sd = new SampleData();
                sd.initialData(r);
                this.population.Add(sd);
            }
          
        }
        public void crossover()
        {
            int cross_size = (int) (this.pop * (this.cross/100.0));
            for (int i = 0; i < cross_size; i++)
            {
                SampleData sd1,sd2;
                do {
                    int num1, num2;
                    do
                    {
                        num1 = r.Next(this.pop);
                        num2 = r.Next(this.pop);

                    } while (num1 == num2);
                    // ----------------- crossover -------------------
                    sd1 = new SampleData();
                    sd2 = new SampleData();
                
                    sd1.X = this.population[num1].X;
                    sd1.Y = this.population[num2].Y;

                    sd2.X = this.population[num2].X;
                    sd2.Y = this.population[num1].Y;

                }while(sd1.checkValues() || sd2.checkValues());
                sd1.evaluate();
                sd2.evaluate();
                this.population.Add(sd1);
                this.population.Add(sd2);
            }
        }

        public void mutation()
        {
            int mut_size = (int)(this.pop * (this.mut / 100.0));
            for (int i = 0; i < mut_size; i++)
            {
                SampleData sd1;
                do
                {
                    int num1;
                    num1 = r.Next(this.pop); // for x or y
                    double num2 = r.NextDouble();
                    // ----------------- mutation -------------------
                    sd1 = new SampleData();
                    if (num1 <= mut_size / 2)
                    {
                        sd1.X = this.population[num1].X;
                        sd1.Y = Math.Round(r.NextDouble() * 3, 2);
                    }
                    else
                    {
                        sd1.X = Math.Round(r.NextDouble() * 5, 2);
                        sd1.Y = this.population[num1].Y;
                    }


                } while (sd1.checkValues());
                sd1.evaluate();
                this.population.Add(sd1);
                
            }

        }

        public void selection()
        {
            List<SampleData> population_local = new List<SampleData>();
            List<SampleData> tempPop = copyPop();
            List<SampleData> dominPareto = new List<SampleData>();
            

            while(population_local.Count < this.pop)
            {
                dominPareto = pareto(tempPop);
                for (int i = 0; i < dominPareto.Count; i++ )
                {
                    SampleData sd = new SampleData();
                    sd.f1 = dominPareto[i].f1;
                    sd.f2 = dominPareto[i].f2;
                    sd.X = dominPareto[i].X;
                    sd.Y = dominPareto[i].Y;
                    population_local.Add(sd);
                }
            }

            this.population.Clear();
            
            for (int i = 0; i < this.pop; i++)
            {
                SampleData sd = new SampleData();
                sd.f1 = population_local[i].f1;
                sd.f2 = population_local[i].f2;
                sd.X = population_local[i].X;
                sd.Y = population_local[i].Y;
                this.population.Add(sd);
            }
        }

        List<SampleData> pareto(List<SampleData>domin)
        {
            List<SampleData> tempPop = copyPop();
            List<SampleData> dominPareto = new List<SampleData>();
            
            int count = 0;
            SampleData sd = new SampleData();
            if (tempPop.Count > 0)
            {
                sd.f1 = tempPop[0].f1;
                sd.f2 = tempPop[0].f2;
                sd.X = tempPop[0].X;
                sd.Y = tempPop[0].Y;
                tempPop.RemoveAt(0);
            }
            
            while (tempPop.Count > 0)
            {
                if ((count != 0) && (count == tempPop.Count))
                {
                    
                    count = 0;
                    dominPareto.Add(sd);
                    sd = new SampleData();
                    sd.X = tempPop[count].X;
                    sd.Y = tempPop[count].Y;
                    sd.f1 = tempPop[count].f1;
                    sd.f2 = tempPop[count].f2;
                    tempPop.RemoveAt(count);
                    if (tempPop.Count == 1)
                    {
                        dominPareto.Add(sd);
                    }
                    continue;
                }

                if ((sd.f1 < tempPop[count].f1) && (sd.f2 < tempPop[count].f2))
                {
                    SampleData s = new SampleData();
                    s.X = tempPop[count].X;
                    s.Y = tempPop[count].Y;
                    s.f1 = tempPop[count].f1;
                    s.f2 = tempPop[count].f2;
                    domin.Add(s);

                    tempPop.RemoveAt(count);
                    continue;
                }

                if ((sd.f1 >= tempPop[count].f1) && (sd.f2 >= tempPop[count].f2))
                {
                    domin.Add(sd);

                    sd = new SampleData();
                    sd.X = tempPop[count].X;
                    sd.Y = tempPop[count].Y;
                    sd.f1 = tempPop[count].f1;
                    sd.f2 = tempPop[count].f2;
                    tempPop.RemoveAt(count);
                    continue;
                }
                
                count++;

            }
            return dominPareto;
        }

        List<SampleData> copyPop()
        {
            List<SampleData> temp = new List<SampleData>();
            for (int i = 0; i < this.population.Count; i++)
            {
                SampleData sd = new SampleData();
                sd.X = this.population[i].X;
                sd.Y = this.population[i].Y;
                sd.f1 = this.population[i].f1;
                sd.f2 = this.population[i].f2;
                temp.Add(sd);
            }
            return temp;
        }

        public List<SampleData> getParetoPoints()
        {
            List<SampleData> domin = new List<SampleData>();
            List<SampleData> tempPop = copyPop();
            List<SampleData> dominPareto = new List<SampleData>();

            int count = 0;
            SampleData sd = new SampleData();
            if (tempPop.Count > 0)
            {
                sd.f1 = tempPop[0].f1;
                sd.f2 = tempPop[0].f2;
                sd.X = tempPop[0].X;
                sd.Y = tempPop[0].Y;
                tempPop.RemoveAt(0);
            }

            while (tempPop.Count > 0)
            {
                if ((count != 0) && (count == tempPop.Count))
                {

                    count = 0;
                    dominPareto.Add(sd);
                    sd = new SampleData();
                    sd.X = tempPop[count].X;
                    sd.Y = tempPop[count].Y;
                    sd.f1 = tempPop[count].f1;
                    sd.f2 = tempPop[count].f2;
                    tempPop.RemoveAt(count);
                    if (tempPop.Count == 1)
                    {
                        dominPareto.Add(sd);
                    }
                    continue;
                }

                if ((sd.f1 < tempPop[count].f1) && (sd.f2 < tempPop[count].f2))
                {
                    SampleData s = new SampleData();
                    s.X = tempPop[count].X;
                    s.Y = tempPop[count].Y;
                    s.f1 = tempPop[count].f1;
                    s.f2 = tempPop[count].f2;
                    domin.Add(s);

                    tempPop.RemoveAt(count);
                    continue;
                }

                if ((sd.f1 >= tempPop[count].f1) && (sd.f2 >= tempPop[count].f2))
                {
                    domin.Add(sd);

                    sd = new SampleData();
                    sd.X = tempPop[count].X;
                    sd.Y = tempPop[count].Y;
                    sd.f1 = tempPop[count].f1;
                    sd.f2 = tempPop[count].f2;
                    tempPop.RemoveAt(count);
                    continue;
                }

                count++;

            }
            return dominPareto;  // dominPareto    this.population
        }

        public SampleData findBestSolution(List<SampleData> paretoPoints)
        {
            double f1_max = paretoPoints[0].f1, f2_max = paretoPoints[0].f2;
            for (int i = 1; i < paretoPoints.Count; i++)
            {
                if (paretoPoints[i].f1 > f1_max)
                {
                    f1_max = paretoPoints[i].f1;
                }
                if (paretoPoints[i].f2 > f2_max)
                {
                    f2_max = paretoPoints[i].f2;
                }
            }
            SampleData sd = new SampleData();
            double res = (f1_max - paretoPoints[0].f1) * (f2_max - paretoPoints[0].f2);
            for (int i = 1; i < paretoPoints.Count; i++)
            {
                if (res < ((f1_max - paretoPoints[i].f1) * (f2_max - paretoPoints[i].f2)))
                {
                    sd.X = paretoPoints[i].X;
                    sd.Y = paretoPoints[i].Y;
                    sd.f1 = paretoPoints[i].f1;
                    sd.f2 = paretoPoints[i].f2;
                    res = (f1_max - paretoPoints[i].f1) * (f2_max - paretoPoints[i].f2); 
                }
            }
            return sd;
        }

    }
}
