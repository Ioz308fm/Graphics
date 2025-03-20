using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        int k = -1, mdown = 0, startfill = 0;
        const int resx = 1920, resy = 1080;
        Bitmap map = new Bitmap(resx, resy);
        Bitmap mapsave = new Bitmap(resx, resy);
        Bitmap mapsave2 = new Bitmap(resx, resy);

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

        bool isconvex()//проверка на выпуклость
        {
            double product, product1;
            PointF ab = new PointF(vertices[0].X - vertices[k].X, vertices[0].Y - vertices[k].Y);
            PointF bc = new PointF(vertices[1].X - vertices[0].X, vertices[1].Y - vertices[0].Y);
            product1 = ab.X * bc.Y - ab.Y * bc.X;
            for (int i = 1; i < k; i++)
            {
                ab = new PointF(vertices[i].X - vertices[i - 1].X, vertices[i].Y - vertices[i - 1].Y);
                bc = new PointF(vertices[i + 1].X - vertices[i].X, vertices[i + 1].Y - vertices[i].Y);
                product = ab.X * bc.Y - ab.Y * bc.X;
                if (product * product1 < 0)
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

        void FillTriangle (PointF a, PointF b, PointF c)
        {
            PointF[] p  = new PointF[3];
            p[0].X = a.X;
            p[0].Y = a.Y;
            p[1].X = b.X;
            p[1].Y = b.Y;
            p[2].X = c.X;
            p[2].Y = c.Y;
            float dx01, dx02, dx12;
            int imax = 0, imin = 0, imid = 0;
            for (int i = 0; i < 3; i++)
            {
                if (p[i].Y < p[imin].Y)
                {
                    imin = i;
                }
                else
                {
                    if (p[i].Y > p[imax].Y)
                        imax = i;
                }
            }
            imid = 3 - imax - imin;

            if (p[imax].Y != p[imin].Y)
                dx01 = (p[imax].X - p[imin].X) / (p[imax].Y - p[imin].Y);//тут
            else
                dx01 = p[imax].Y;

            if (p[imin].Y != p[imid].Y)
                dx02 = (p[imid].X - p[imin].X) / (p[imid].Y - p[imin].Y);
            else
                dx02 = p[imin].Y;

            if (p[imax].Y != p[imid].Y)
                dx12 = (p[imax].X - p[imid].X) / (p[imax].Y - p[imid].Y);
            else
                dx12 = p[imid].Y;

            float x1 = p[imin].X, x2 = p[imin].X;
            for (int i = Convert.ToInt32(p[imin].Y); i < Convert.ToInt32(p[imid].Y); i++)
            {
                line(x1, i, x2, i);
                x1 += dx01;
                x2 += dx02;
            }
            for (int i = Convert.ToInt32(p[imid].Y); i <= Convert.ToInt32(p[imax].Y); i++)
            {
                line(x1, i, x2, i);
                x1 += dx01;
                x2 += dx12;
            }
        }
        void FillPolygon ()
        {
            for (int i=1; i < k; i++)
            {
                FillTriangle(vertices[0], vertices[i], vertices[i+1]);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (k == -1 && startfill == 0)
            {
                vertices.Add(e.Location);
                k++;
            }
            if (e.Button == MouseButtons.Right && startfill == 0)
            {
                if (isconvex() == true)
                {
                    FillPolygon();
                    startfill = 1;
                }
                else
                {
                    k = -1;
                    vertices.Clear();
                    Bitmap t = new Bitmap(resx, resy);
                    map = (Bitmap)t.Clone(); ;
                    mapsave = (Bitmap)t.Clone();
                    pictureBox1.Image = map;
                    MessageBox.Show("многоугольник не выпуклый");
                }
            }


            mdown = 1;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mdown == 1 && startfill == 0 && k != -1)
            {
                Bitmap map1 = new Bitmap(resx, resy);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                f2 = e.Location;
                line(vertices[k], f2);
                pictureBox1.Image = map;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mdown == 1 && startfill == 0 && k != -1)
            {
                Bitmap map1 = new Bitmap(resx, resy);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                vertices.Add(e.Location);
                line(vertices[k], vertices[k + 1]);
                pictureBox1.Image = map;
                mapsave = map;
                k++;
                mdown = 0;
            }
            if (mdown == 1 && startfill == 1)
            {
                Bitmap map1 = new Bitmap(resx, resy);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                line(vertices[k], vertices[0]);
                pictureBox1.Image = map;
                mapsave = map;
                mdown = 0;
                startfill++;
            }
        }
    }
}
