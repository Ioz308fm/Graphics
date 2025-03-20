using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
        List<PointF> VertNConv = new List<PointF>();
        List<PointF> before = new List<PointF>();
        List<PointF> after = new List<PointF>();
        int k = -1, m = -1, mdown = 0, startconv = 0, endconv = 0;
        Bitmap map = new Bitmap(1000, 1000);
        Bitmap mapsave = new Bitmap(1000, 1000);
        Bitmap mapsave2 = new Bitmap(1000, 1000);
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


        int check(PointF a, PointF b, PointF A, PointF B, PointF C)
        {
            double product, producta, productb;
            PointF ab = new PointF(B.X - A.X, B.Y - A.Y);
            PointF bc = new PointF(C.X - B.X, C.Y - B.Y);
            product = ab.X * bc.Y - ab.Y * bc.X;//ориентация ABC
            bc.X = a.X - B.X;
            bc.Y = a.Y - B.Y;
            producta = ab.X * bc.Y - ab.Y * bc.X;//ориентация ABa
            bc.X = b.X - B.X;
            bc.Y = b.Y - B.Y;
            productb = ab.X * bc.Y - ab.Y * bc.X;//ориентация ABa
            
            if(product * producta < 0 && product * productb > 0)//а вне, b внутри
            {
                return 1;
            }
            if (product * producta > 0 && product * productb < 0)//b вне, а внутри
            {
                return 2;
            }
            if (product * producta > 0 && product * productb > 0)//а b внутри
            {
                return 3;
            }
            if (product * producta < 0 && product * productb < 0)//a b снаружи
            {
                return 4;
            }

            return 5;// невозможный и ненужный случай, если BC есть продолжение AB но пускай тут сидит
        }

        PointF findt(PointF a, PointF b, PointF A, PointF B,PointF norm)
        {
            PointF t = new PointF();
            PointF c = new PointF();
            PointF p = new PointF();
            p.X = A.X - a.X;
            p.Y = A.Y - a.Y;
            c.X = b.X - a.X;
            c.Y = b.Y - a.Y;
            t = lineloc(a, b, dotproduct(p, norm) / dotproduct(c, norm));
            return t;
        }

        void adding(PointF a, PointF b, PointF A, PointF B, PointF C, PointF norm)
        {
            if (check(a, b, A, B, C) == 1)
            {
                after.Add(findt(a, b, A, B, norm));
                after.Add(b);
            }
            if (check(a, b, A, B, C) == 2)
            {
                after.Add(findt(a, b, A, B, norm));
            }
            if (check(a, b, A, B, C) == 3)
            {
                after.Add(b);
            }
        }

        void clip ()
        {
            //проверяем для ребра состоящего из последней и первой вершины
            //берем k 0 1 ребра
            //проверяем первую и последнюю
            adding(before[before.Count - 1], before[0], vertices[vertices.Count - 1], vertices[0], vertices[1], normals[normals.Count - 1]);
            for (int j = 0; j < before.Count - 1; j++)
            {
                //проверяем все прямые
                adding(before[j], before[j + 1], vertices[vertices.Count-1], vertices[0], vertices[1], normals[normals.Count-1]);
            }
            //after->before и after очищается
            before.Clear();
            before.AddRange(after.ToArray());
            after.Clear();

            //проверяем для всех остальных**
            for (int i = 0; i < vertices.Count-2; i++) 
            {
                //для каждой пары точек из before проверяем где они и добавляем по надобности в after
                //проверяем первую и последнюю
                adding(before[before.Count - 1], before[0], vertices[i], vertices[i + 1], vertices[i + 2], normals[i]);

                for (int j = 0; j < before.Count - 1; j++)
                {
                    adding(before[j], before[j + 1], vertices[i], vertices[i + 1], vertices[i + 2], normals[i]);
                }

                //after->before и after очищается
                before.Clear();
                before.AddRange(after.ToArray());
                after.Clear();
            }
            //проверяеем для последней тройки ребро k-1 k и 0
            //проверяем первую и последнюю
            adding(before[before.Count - 1], before[0], vertices[vertices.Count - 2], vertices[vertices.Count - 1], vertices[0], normals[normals.Count - 2]);
            for (int j = 0; j < before.Count - 1; j++)
            {
                //проверяем все прямые
                adding(before[j], before[j + 1], vertices[vertices.Count - 2], vertices[vertices.Count - 1], vertices[0], normals[normals.Count - 2]);
            }


            Bitmap t = new Bitmap(1000, 1000);
            map = (Bitmap)t.Clone(); ;

            for (int j = 0; j < after.Count - 1; j++)
            {
                line(Convert.ToInt32(after[j].X), Convert.ToInt32(after[j].Y), Convert.ToInt32(after[j + 1].X), Convert.ToInt32(after[j + 1].Y));
            }
            line(Convert.ToInt32(after[after.Count-1].X), Convert.ToInt32(after[after.Count - 1].Y), Convert.ToInt32(after[0].X), Convert.ToInt32(after[0].Y));


            pictureBox1.Image = map;

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right && endconv==1)
            {
                endconv = 2;
                clip();
            }
            
            if (startconv > 1 && m == -1)
            {
                before.Add(e.Location);
                VertNConv.Add(e.Location);
                m++;
            }
            if (e.Button == MouseButtons.Right && startconv > 1 && endconv < 1) 
            {
                endconv = 1;
            }

            if (k == -1 && startconv == 0)
            {
                vertices.Add(e.Location);
                k++;
            }
            if (e.Button == MouseButtons.Right && startconv == 0)
            {
                if (isconvex() == true)
                {
                    for (int i = 0; i < k; i++) //считаем нормали ко всем ребрам
                    {
                        normals.Add(normal(vertices[i], vertices[i + 1]));
                    }
                    normals.Add(normal(vertices[k], vertices[0]));
                    startconv = 1;
                }
                else
                {
                    k = -1;
                    vertices.Clear();
                    Bitmap t = new Bitmap(1000, 1000);
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
            if (mdown == 1 && startconv == 0 && k != -1)
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                f2 = e.Location;
                line(Convert.ToInt32(vertices[k].X), Convert.ToInt32(vertices[k].Y), Convert.ToInt32(f2.X), Convert.ToInt32(f2.Y));
                pictureBox1.Image = map;
            }
            if (mdown == 1 && endconv == 0 && m != -1)  
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                f2 = e.Location;
                line(Convert.ToInt32(VertNConv[m].X), Convert.ToInt32(VertNConv[m].Y), Convert.ToInt32(f2.X), Convert.ToInt32(f2.Y));
                pictureBox1.Image = map;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mdown == 1 && startconv > 1 && m != -1 && endconv == 0) 
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                before.Add(e.Location);
                VertNConv.Add(e.Location);
                line(Convert.ToInt32(VertNConv[m].X), Convert.ToInt32(VertNConv[m].Y), Convert.ToInt32(VertNConv[m+1].X), Convert.ToInt32(VertNConv[m+1].Y));
                pictureBox1.Image = map;
                mapsave = map;
                m++;
                mdown = 0;
            }
            if (mdown == 1 && endconv == 1)
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                line(Convert.ToInt32(VertNConv[m].X), Convert.ToInt32(VertNConv[m].Y), Convert.ToInt32(VertNConv[0].X), Convert.ToInt32(VertNConv[0].Y));
                pictureBox1.Image = map;
                mapsave = map;
                mdown = 0;
            }
            if (mdown == 1 && startconv == 0 && k != -1)
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                vertices.Add(e.Location);
                line(Convert.ToInt32(vertices[k].X), Convert.ToInt32(vertices[k].Y), Convert.ToInt32(vertices[k + 1].X), Convert.ToInt32(vertices[k + 1].Y));
                pictureBox1.Image = map;
                mapsave = map;
                k++;
                mdown = 0;
            }
            if (mdown == 1 && startconv == 1)
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map1 = (Bitmap)mapsave.Clone();
                map = map1;
                line(Convert.ToInt32(vertices[k].X), Convert.ToInt32(vertices[k].Y), Convert.ToInt32(vertices[0].X), Convert.ToInt32(vertices[0].Y));
                pictureBox1.Image = map;
                mapsave = map;
                mdown = 0;
                startconv++;
            }
        }
    }
}
