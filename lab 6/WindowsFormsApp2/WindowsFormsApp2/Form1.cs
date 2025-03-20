using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<PointF> dots = new List<PointF>();
        Bitmap map = new Bitmap(1000, 1000);
        Bitmap mapsave = new Bitmap(1000, 1000);
        Bitmap mapsave2 = new Bitmap(1000, 1000);
        int n = -1;
        PointF f1, f2;

        void line(float xx1, float yy1, float xx2, float yy2)
        {
            int x1 = Convert.ToInt32(xx1);
            int y1 = Convert.ToInt32(yy1);
            int x2 = Convert.ToInt32(xx2);
            int y2 = Convert.ToInt32(yy2);

            int dx = (x2 - x1 >= 0 ? 1 : -1);
            int dy = (y2 - y1 >= 0 ? 1 : -1);

            int lengthX = Math.Abs(x2 - x1);
            int lengthY = Math.Abs(y2 - y1);

            int length = Math.Max(lengthX, lengthY);

            if (length == 0)
            {
                map.SetPixel(x1, y1, Color.Black);
            }

            if (lengthY <= lengthX)
            {
                int x = x1;
                int y = y1;
                int d = -lengthX;

                length++;
                while (length > 0)
                {
                    length--;
                    map.SetPixel(x, y, Color.Black);
                    x += dx;
                    d += 2 * lengthY;
                    if (d > 0)
                    {
                        d -= 2 * lengthX;
                        y += dy;
                    }
                }
            }
            else
            {
                int x = x1;
                int y = y1;
                int d = -lengthY;

                length++;
                while (length > 0)
                {
                    length--;
                    map.SetPixel(x, y, Color.Black);
                    y += dy;
                    d += 2 * lengthX;
                    if (d > 0)
                    {
                        d -= 2 * lengthY;
                        x += dx;
                    }
                }
            }
        }

        void line(int x1, int y1, int x2, int y2)
        {
            int dx = (x2 - x1 >= 0 ? 1 : -1);
            int dy = (y2 - y1 >= 0 ? 1 : -1);

            int lengthX = Math.Abs(x2 - x1);
            int lengthY = Math.Abs(y2 - y1);

            int length = Math.Max(lengthX, lengthY);

            if (length == 0)
            {
                map.SetPixel(x1, y1, Color.Black);
            }

            if (lengthY <= lengthX)
            {
                int x = x1;
                int y = y1;
                int d = -lengthX;

                length++;
                while (length > 0)
                {
                    length--;
                    map.SetPixel(x, y, Color.Black);
                    x += dx;
                    d += 2 * lengthY;
                    if (d > 0)
                    {
                        d -= 2 * lengthX;
                        y += dy;
                    }
                }
            }
            else
            {
                int x = x1;
                int y = y1;
                int d = -lengthY;

                length++;
                while (length > 0)
                {
                    length--;
                    map.SetPixel(x, y, Color.Black);
                    y += dy;
                    d += 2 * lengthX;
                    if (d > 0)
                    {
                        d -= 2 * lengthY;
                        x += dx;
                    }
                }
            }
        }

        void line(PointF A, PointF B)
        {
            int x1 = Convert.ToInt32(A.X);
            int y1 = Convert.ToInt32(A.Y);
            int x2 = Convert.ToInt32(B.X);
            int y2 = Convert.ToInt32(B.Y);
            int dx = (x2 - x1 >= 0 ? 1 : -1);
            int dy = (y2 - y1 >= 0 ? 1 : -1);

            int lengthX = Math.Abs(x2 - x1);
            int lengthY = Math.Abs(y2 - y1);

            int length = Math.Max(lengthX, lengthY);

            if (length == 0)
            {
                map.SetPixel(x1, y1, Color.Black);
            }

            if (lengthY <= lengthX)
            {
                int x = x1;
                int y = y1;
                int d = -lengthX;

                length++;
                while (length > 0)
                {
                    length--;
                    map.SetPixel(x, y, Color.Black);
                    x += dx;
                    d += 2 * lengthY;
                    if (d > 0)
                    {
                        d -= 2 * lengthX;
                        y += dy;
                    }
                }
            }
            else
            {
                int x = x1;
                int y = y1;
                int d = -lengthY;

                length++;
                while (length > 0)
                {
                    length--;
                    map.SetPixel(x, y, Color.Black);
                    y += dy;
                    d += 2 * lengthX;
                    if (d > 0)
                    {
                        d -= 2 * lengthY;
                        x += dx;
                    }
                }
            }
        }

        void over()
        {
            dots.Clear();
            f1.X = 200;
            f2.X = 600;
            f1.Y = 300;
            f2.Y = 300;
            dots.Add(f1);
            dots.Add(f2);
        }

        float polarangle(PointF a, PointF b)
        {
            PointF c = new PointF();
            c.X = b.X - a.X;
            c.Y = b.Y - a.Y;
            if (c.X > 0 && c.Y >= 0)
                return (float)Math.Atan(c.Y / c.X);
            if (c.X > 0 && c.Y < 0)
                return (float)Math.Atan(c.Y / c.X) +2*(float)Math.PI;
            if (c.X < 0)
                return (float)Math.Atan(c.Y / c.X)+(float)Math.PI;
            if (c.X == 0 && c.Y > 0) 
                return (float)Math.PI/2;
            if (c.X == 0 && c.Y < 0)
                return 3*(float)Math.PI / 2;
            return 0;
        }

        void mirror ()
        {
            for (int i = 0; i < dots.Count; i++)//отражение относитеельно изначальной прямой f1,f2
            {
                PointF a = new PointF();
                a.X = dots[i].X;
                a.Y = 2 * f1.Y - dots[i].Y;
                dots.Remove(dots[i]);
                dots.Insert(i, a);
            }
        }

        void koh(int n)
        {
            Bitmap map1 = new Bitmap(1000, 1000);
            map1 = (Bitmap)mapsave.Clone();
            map = map1;
            over();
            for (int i = 0; i < n; i++)
            {
                int count = dots.Count - 1;
                for(int j=0;j<count;j++)//суть алгоритма проста, между 2 точками создаем еще 3
                {//первая точка лежит на 1/3 отрезка=r соединяющего 2 точки
                    PointF A = new PointF();//далеее откладываем это же r но с поворотом на 60 градусов
                    PointF B = new PointF();//против часовой стрелки, далеее от этой точки поворот по часовой
                    A = dots[4 * j];//на 120 градусов и мы попадем в точку лежащую между 2 первыми точками
                    B = dots[4 * j + 1];//но на длине 2/3 отрезка
                    PointF a=new PointF();
                    float phi=polarangle(A,B);
                    float r = (float)Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y)) /3;
                    a.X = r * (float)Math.Cos(phi) + A.X;
                    a.Y = r * (float)Math.Sin(phi) + A.Y;
                    dots.Insert(4 * j + 1, a);
                    a.X += r * (float)Math.Cos(phi + Math.PI / 3);
                    a.Y += r * (float)Math.Sin(phi + Math.PI / 3) ;
                    dots.Insert(4 * j + 2, a);
                    a.X += r * (float)Math.Cos(phi - Math.PI / 3);
                    a.Y += r * (float)Math.Sin(phi - Math.PI / 3);
                    dots.Insert(4 * j + 3, a);
                }
            }

            mirror();//отражение

            //соединяем
            for (int i=0;i<dots.Count-1;i++)
            {
                line(dots[i], dots[i + 1]);
            }
            pictureBox1.Image = map;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                n++;
            }
            if (e.Button == MouseButtons.Right)
            {
                n--;
            }
            if (n >= 0)
            {
                koh(n);
            }
        }
    }
}
