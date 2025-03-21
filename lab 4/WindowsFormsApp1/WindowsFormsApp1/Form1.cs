﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        PointF f1, f2;
        List<PointF> vertices = new List<PointF>();
        List<PointF> normals = new List<PointF>();
        List<float> ind = new List<float>();
        List<float> outd = new List<float>();
        int k = -1, mdown = 0, startline=0;
        Bitmap map = new Bitmap(1000, 1000);
        Bitmap mapsave = new Bitmap(1000, 1000);
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

        PointF lineloc(PointF A, PointF B, float t)//параметрическое вычисление координат точки на прямой
        {
            PointF a = new PointF();
            a.X = A.X + t * (B.X - A.X);
            a.Y = A.Y + t * (B.Y - A.Y);
            return a;
        }

        bool isconvex()//проверка на выпуклость
        {
            double product,product1;
            PointF ab = new PointF(vertices[0].X - vertices[k].X, vertices[0].Y - vertices[k].Y);
            PointF bc = new PointF(vertices[1].X - vertices[0].X, vertices[1].Y - vertices[0].Y);
            product1 = ab.X * bc.Y - ab.Y * bc.X;
            for (int i = 1; i < k; i++)
            {
                ab = new PointF(vertices[i].X - vertices[i - 1].X, vertices[i].Y - vertices[i - 1].Y);
                bc = new PointF(vertices[i + 1].X - vertices[i].X, vertices[i + 1].Y - vertices[i].Y);
                product = ab.X * bc.Y - ab.Y * bc.X;
                if(product * product1 <0)
                {
                    return false;
                }
                product1 = product;
            }
            ab = new PointF(vertices[k].X - vertices[k - 1].X, vertices[k].Y - vertices[k - 1].Y);
            bc = new PointF(vertices[0].X - vertices[k].X, vertices[0].Y - vertices[k].Y);
            product = ab.X * bc.Y - ab.Y * bc.X;
            if (product * product1 < 0)
            {
                return false;
            }

            return true;
        }

        float dotproduct(PointF a, PointF b)//скалярное произведение
        {
            return a.X * b.X + a.Y * b.Y;
        }

        PointF normal(PointF a, PointF b)//нормаль к прямой между двумя точками
        {
            PointF result = new Point();

            result.X = (b.Y - a.Y);
            result.Y = (a.X - b.X);
            return result;
        }

        void LineInPolygon(PointF a, PointF b)
        {
            PointF c = new PointF();
            PointF p = new PointF();//возможно здесь ошибка
            p.X = vertices[k].X - a.X;
            p.Y = vertices[k].Y - a.Y;
            c.X = b.X - a.X;
            c.Y = b.Y - a.Y;
            if (dotproduct(c, normals[k]) < 0)
            {
                ind.Add(dotproduct(p, normals[k]) / dotproduct(c, normals[k]));
            }
            else
            {
                outd.Add(dotproduct(p, normals[k]) / dotproduct(c, normals[k]));
            }
            for (int i = 0; i < k; i++)
            {
                //p.X = vertices[i+1].X - vertices[i].X + a.X;
                //p.Y = vertices[i+1].Y - vertices[i].Y + a.Y;
                p.X = vertices[i].X - a.X;
                p.Y = vertices[i].Y - a.Y;
                if (dotproduct(c, normals[i]) < 0)
                {
                    ind.Add(dotproduct(p, normals[i]) / dotproduct(c, normals[i]));
                }
                else
                {
                    outd.Add(dotproduct(p, normals[i]) / dotproduct(c, normals[i]));
                }
            }
            if (Math.Abs(ind.Max()) < 1 && Math.Abs(outd.Min()) < 1 && Math.Abs(outd.Min()) > 0 && Math.Abs(ind.Max()) > 0) 
            {
                PointF draw1 = new PointF();
                PointF draw2 = new PointF();
                draw1 = lineloc(a, b, ind.Max());
                draw2 = lineloc(a, b, outd.Min());
                line(Convert.ToInt32(draw1.X), Convert.ToInt32(draw1.Y), Convert.ToInt32(draw2.X), Convert.ToInt32(draw2.Y));
            }
            ind.Clear();
            outd.Clear();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (k == -1 && startline == 0) 
            {
                vertices.Add(e.Location);
                k++;
            }
            if (e.Button == MouseButtons.Right && startline == 0) 
            {
                if (isconvex() == true) 
                {
                    for (int i = 0; i < k; i++) //считаем нормали ко всем ребрам
                    {
                        normals.Add(normal(vertices[i], vertices[i+1]));
                      //  line(Convert.ToInt32((vertices[i].X + vertices[i+1].X)/2), Convert.ToInt32((vertices[i].Y + vertices[i + 1].Y) / 2), Convert.ToInt32((vertices[i].X + vertices[i + 1].X) / 2 + normals[i].X), Convert.ToInt32((vertices[i].Y + vertices[i + 1].Y) / 2 - normals[i].Y));
                      //  pictureBox1.Image = map;
                    }
                    normals.Add(normal(vertices[k], vertices[0]));
                    //line(Convert.ToInt32((vertices[k].X + vertices[0].X) / 2), Convert.ToInt32((vertices[k].Y + vertices[0].Y) / 2), Convert.ToInt32((vertices[k].X + vertices[0].X) / 2 + normals[k].X), Convert.ToInt32((vertices[k].Y + vertices[0].Y) / 2 - normals[k].Y));
                    //pictureBox1.Image = map;

                    startline = 1;
                }
                else
                {
                    k = -1;
                    vertices.Clear();
                    Bitmap t = new Bitmap(1000, 1000);
                    map = (Bitmap)t.Clone(); ;
                    mapsave = (Bitmap)t.Clone();
                    pictureBox1.Image = map;
                    MessageBox.Show("многоугольник не выпуклый") ;
                }    
            }
            if (startline == 2) 
            {
                f1 = e.Location;
                startline++;
            }
            else
            {
                f1 = f2;
            }
            mdown = 1;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mdown == 1 && startline == 0 && k != -1)  
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                f2 = e.Location;
                line(Convert.ToInt32(vertices[k].X), Convert.ToInt32(vertices[k].Y), Convert.ToInt32(f2.X), Convert.ToInt32(f2.Y));
                pictureBox1.Image = map;
            }
            if (mdown == 1 && startline > 1) 
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                f2 = e.Location;
                line(Convert.ToInt32(f1.X), Convert.ToInt32(f1.Y), Convert.ToInt32(f2.X), Convert.ToInt32(f2.Y));
                pictureBox1.Image = map;
            }    
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mdown == 1 && startline == 0 && k != -1) 
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                vertices.Add(e.Location);
                line(Convert.ToInt32(vertices[k].X), Convert.ToInt32(vertices[k].Y), Convert.ToInt32(vertices[k+1].X), Convert.ToInt32(vertices[k+1].Y));
                pictureBox1.Image = map;
                mapsave = map;
                k++;
                mdown = 0;
            }
            if (mdown == 1 && startline == 1)
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                line(Convert.ToInt32(vertices[k].X), Convert.ToInt32(vertices[k].Y), Convert.ToInt32(vertices[0].X), Convert.ToInt32(vertices[0].Y));
                pictureBox1.Image = map;
                mapsave = map;
                mdown = 0;
                startline++;
            }
            if (mdown == 1 && startline > 1)
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                f2 = e.Location;
                LineInPolygon(f1, f2);
                //line(Convert.ToInt32(f1.X), Convert.ToInt32(f1.Y), Convert.ToInt32(f2.X), Convert.ToInt32(f2.Y));
                pictureBox1.Image = map;
                mapsave = map;
                mdown = 0;
            }
        }
    }
}
