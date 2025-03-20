using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Color> colors = new List<Color>();
        const int resx = 1920, resy = 1080;
        double boom = 4.0;
        int N = 5000;
        const double s = 1.0 / 400.0;//scaling
        Bitmap map = new Bitmap(resx, resy);
        int n = 0;


        void redupfill()
        {
            int K = (int)Math.Pow(N + 1, 1.0/ 3.0);
            for (int i=0;i<K;i++)
            {
                for (int j=0;j<K;j++)
                {
                    for(int m=0;m<K ;m++)
                    {
                        Color a = new Color();
                        a = Color.FromArgb(255*i/K, 255 - 255 * j / K, 255 - 255 * m / K);
                        colors.Add(a);
                    }
                }
            }
            while (colors.Count<101)
            {
                Color a = new Color();
                a = Color.FromArgb(colors.Count*255/101 ,0,0);
                colors.Add(a);
            }
        }

        void bluedownfill()
        {
            int K = (int)Math.Pow(N + 1, 1.0 / 3.0);
            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    for (int m = 0; m < K; m++)
                    {
                        Color a = new Color();
                        a = Color.FromArgb(255 * i / K, 255 - 255 * j / K, 255 - 255 * m / K);
                        colors.Add(a);
                    }
                }
            }
            while (colors.Count < 101)
            {
                Color a = new Color();
                a = Color.FromArgb(colors.Count * 255 / 101, 0, 0);
                colors.Add(a);
            }
        }

        double dwell(double cx, double cy,double xs,double ys)
        {

            double sx = xs - resx / 2,sy= ys - resy / 2; ;
            sx *= s;
            sy *= s;
            double tmp, dx = sx, dy = sy, f = dx * dx + dy * dy;
            for (int i = 0; i <= N && Math.Abs(f) <= boom; i++)
            {
                tmp = dx;
                dx = dx * dx - dy * dy + cx;
                dy = 2 * tmp * dy + cy;
                f = dx * dx + dy * dy;
            }

            return f;
        }

        int dwellnum(double cx, double cy, double xs, double ys)
        {
            double sx = xs - resx / 2, sy = ys - resy / 2; ;
            sx *= s;
            sy *= s;
            int i=0;
            double tmp, dx = sx, dy = sy, f = dx * dx + dy * dy;
            for (i = 0; i <= N && Math.Abs(f) <= boom; i++)
            {
                tmp = dx;
                dx = dx * dx - dy * dy + cx;
                dy = 2 * tmp * dy + cy;
                f = dx * dx + dy * dy;
            }

            return i;
        }

        void paint(int i, int j,int k)
        {
            if(k==1)
            {
                double cx = -0.74543;
                double cy = 0.11301;
                int M = dwellnum(cx, cy, i, j);
                if (M >= 101)
                    map.SetPixel(i, j, Color.Red);
                else
                    map.SetPixel(i, j, colors[M]);
            }
            if (k == 2)
            {
                double cx = -0.70176;
                double cy = -0.3842;
                int M = dwellnum(cx, cy, i, j);
                if (M >= 101)
                    map.SetPixel(i, j, Color.Red);
                else
                    map.SetPixel(i, j, Color.FromArgb(255, (M * 12) % 255, 0, (M * 12) % 255));
            }
        }

        void julia(int k)
        {
            if (k == 1)
                redupfill();
            for (int i=0;i<resx;i++)
            {
                for(int j=0;j<resy;j++)
                {
                    paint (i,j,k);
                }
            }
            pictureBox1.Image = map;
        }

       


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                n++;
            }
            else
            {
                n--;
            }
            
            julia(n);
        }

        PointF f1, f2;
    }
}
