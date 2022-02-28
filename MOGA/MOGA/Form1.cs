using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MOGA
{
    public partial class Form1 : Form
    {
        List<SampleData> population;
        int x_step = 0;
        int y_step = 0;
        int x_max = 0;
        int y_max = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Start_btn_Click(object sender, EventArgs e)
        {
            label16.Text = "";
            label15.Text = "";
            population = new List<SampleData>();
            int pop = Int32.Parse(txt_pop.Text);
            int cross = Int32.Parse(txt_cross.Text);
            int mut = Int32.Parse(txt_mut.Text);
            int iter = Int32.Parse(txt_iter.Text);

            //Moga moga = new Moga(pop, cross, mut);
            //population = moga.population;
            //draw();

            //--------- for progressbar ------------------
            double percint_data = 100.0 / iter;
            percint_data = Math.Round(percint_data, 2);
            double percint_sum = 0.0;

            progressBar1.Maximum = iter;
            progressBar1.Value = 0;
            //-----------------------------------------

            Moga moga = new Moga(pop, cross, mut);



            for (int i = 0; i < iter; i++)
            {
                moga.crossover();
                moga.mutation();
                moga.selection();
                population = moga.getParetoPoints();
                draw();


                progressBar1.Value += 1;

                percint_sum += percint_data;
                percint_sum = Math.Round(percint_sum, 2);
                percint.Text = percint_sum.ToString() + " %";

                Application.DoEvents();
            }

            SampleData sd = moga.findBestSolution(population);
            label15.Text = "X = " + sd.X + " , Y = " + sd.Y;
            label16.Text = "F1 = " + sd.f1 + " , F2 = " + sd.f2;
        }



        void draw()
        {
            // ---------------
            pictureBox1.Image = null;
            pictureBox1.Update();
            pictureBox1.Refresh();
            // -------------------------

            Graphics g = pictureBox1.CreateGraphics();


            int wid = pictureBox1.Width;
            int high = pictureBox1.Height;
            find_steps();
            // draw xox' 
            Pen pen_x = new Pen(Color.Blue);
            pen_x.EndCap = LineCap.ArrowAnchor;
            Point p1_x = new Point(10, high - 10);
            Point p2_x = new Point(wid - 10, high - 10);
            g.DrawLine(pen_x, p1_x, p2_x);
            
            int w = (int) wid / x_step - 10;
            for (int i = 0; i < w ; i++)
            {
                if (i % 10 == 0)
                {
                    x_step_draw(i, 0, g);
                }
                
            }
            //------------
            // draw yoy'
            Pen pen_y = new Pen(Color.Blue);
            pen_y.EndCap = LineCap.ArrowAnchor;
            Point p1_y = new Point(10, high - 10);
            Point p2_y = new Point(10, 10);
            g.DrawLine(pen_y, p1_y, p2_y);

            int h = (int)high / y_step ;
            for (int j = 0; j < h ; j++)
            {
                if(j % 5 == 0)
                {
                    y_step_draw(0, j, g);
                }
                
            }
            //pen_y.DashStyle = DashStyle.DashDotDot;
            //g.DrawLine(pen_y, 50, 50, 100, 100);
            // -------------

            // ------ draw points of solutions ------------

            for (int i = 0; i < population.Count; i++)
            {
                drawPoint(population[i].f1, population[i].f2, g, 0, false);
            }


        }
        void x_step_draw(int x, int y, Graphics g)
        {
            int high = pictureBox1.Height;

            int new_x = 10 + x_step * x;
            int new_y = high - 10 - (y_step * y);



            Color color = Color.Blue;
            SolidBrush brush = new SolidBrush(color);
            g.FillEllipse(brush, new_x, new_y, 3, 3);
            // ---------------------
            Font myFont = new Font("Arial", 7);
            SolidBrush brush1 = new SolidBrush(Color.Blue);
            if (x != 0)
            {
                g.DrawString("" + x, myFont, brush1, new_x - 2, new_y - 10);
            }

        }
        void y_step_draw(int x, int y, Graphics g)
        {
            int high = pictureBox1.Height;

            int new_x = 10 + x_step * x;
            int new_y = high - 10 - (y_step * y);

            Color color = Color.Blue;
            SolidBrush brush = new SolidBrush(color);
            g.FillEllipse(brush, new_x, new_y, 3, 3);
            // ---------------------
            Font myFont = new Font("Arial", 7);
            SolidBrush brush1 = new SolidBrush(Color.Blue);
            if (y != 0)
            {
                g.DrawString("" + y, myFont, brush1, new_x - 10, new_y - 5);
            }

        }
        void drawPoint(double x, double y, Graphics g, int idx_point, Boolean check)
        {
            //int wid = pictureBox1.Width;
            int high = pictureBox1.Height;

            int new_x = (int)(10 + x_step * x - 3);          // -3 for exacut point in center because think 7
            int new_y = (int)(high - 10 - (y_step * y) - 3); // -3 for exacut point in center because think 7

            //Pen p = new Pen(Color.Red, 2);

            int r, b, green, c = idx_point * 220 + 150;
            c = c % 255;
            r = 255 - c;
            green = c;
            b = (c * 2) % 255;
            if (check)
            {
                Color color = Color.FromArgb(255, 255, 0, 255);
                SolidBrush brush = new SolidBrush(color);
                Pen p = new Pen(brush);
                g.FillRectangle(brush, new_x, new_y, 7, 7);

            }
            else
            {
                Color color = Color.FromArgb(255, 0, 0, 0);
                SolidBrush brush = new SolidBrush(color);
                g.FillEllipse(brush, new_x, new_y, 7, 7);
            }

        }
        void find_steps()
        {
            int wid = pictureBox1.Width;
            int high = pictureBox1.Height;

            x_max = (int)population[0].f1;
            y_max = (int)population[0].f2;
            for (int i = 0; i < population.Count; i++)
            {
                if (x_max < population[i].f1)
                {
                    x_max = (int)population[i].f1;
                }
                if (y_max < population[i].f2)
                {
                    y_max = (int)population[i].f2;
                }
            }
            x_step = (wid - 20) / (x_max + 1);
            y_step = (high - 20) / (y_max + 1);

        }

        
    }
}
