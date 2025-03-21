﻿using System;
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

        Point f1, f2,P1,P2;
        int k = 3, v = 2,b=1;
        Bitmap map = new Bitmap(1000, 1000);

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

        void drawrect(int xa, int xb, int ya, int yb)//прямоугольник
        {
            int x1 = Math.Max(xa, xb);
            int y1 = Math.Max(ya, yb);
            int x0 = Math.Min(xa, xb);
            int y0 = Math.Min(ya, yb);
            for (int x = x0; x <= x1; x++)
            {
                map.SetPixel(x, y0, Color.Black);
                map.SetPixel(x, y1, Color.Black);
            }
            for (int y = y0; y <= y1; y++)
            {
                map.SetPixel(x0 - 1, y, Color.Black);
                map.SetPixel(x1 + 1, y, Color.Black);
            }
        }

        int outcode (int x,int y, int X1, int Y1, int X2, int Y2)
        {
            int code = 0;
            if (x < X1) code += 0x01;
            if (y < Y1) code += 0x02;
            if (x > X2) code += 0x04;
            if (y > Y2) code += 0x08;
            return code;
        }

        void clipline(int xa, int ya, int xb, int yb, int X1, int Y1, int X2, int Y2)
        {
            int code1 = outcode(xa, ya, X1, Y1, X2, Y2);//code для А
            int code2 = outcode(xb, yb, X1, Y1, X2, Y2);//code для B
            int inside = (code1 | code2);// А,В внутри
            int outside = (code1 & code2);//A,B с одной стороны
            while(outside == 0 && inside!=0)
            {
                if (code1==0)//А внутри, меняеем местами А и В
                {
                    int t;
                    t = xa;
                    xa = xb;
                    xb = t;
                    t = ya;
                    ya = yb;
                    yb = t;
                    t = code1;
                    code1 = code2;
                    code2 = t;
                }
                int coox = code1 & 0x01;//A слева
                if (coox!=0)
                {
                    ya += (yb - ya) * (X1 - xa) / (xb - xa);
                    xa = X1;//АВ пересек с х=х1
                }
                coox = code1 & 0x02;//А сверху
                if (coox != 0)
                {
                    xa += (xb - xa) * (Y1 - ya) / (yb - ya);
                    ya = Y1;//АВ пересек с х=х1
                }
                coox = code1 & 0x04;//А справа
                if (coox != 0)
                {
                    ya += (yb - ya) * (X2 - xa) / (xb - xa);
                    xa = X2;//АВ пересек с х=х1
                }
                coox = code1 & 0x08;//А снизу
                if (coox != 0)
                {
                    xa += (xb - xa) * (Y2 - ya) / (yb - ya);
                    ya = Y2;//АВ пересек с х=х1
                }
                code1 = outcode(xa, ya, X1, Y1, X2, Y2);//пересчет всех кодов
                inside = (code1 | code2);
                outside = (code1 & code2);
            }
            line(xa, ya, xb, yb);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (k == 3)
            {
                k=2;
                P1.X = e.X;
                P1.Y = e.Y;
                v = 1;
            }

            if (k == 1)
            {
                k--;
                f1 = e.Location;
            }
            if (k == 0)
            {
                b = 1;
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (k == 2)
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map = map1;
                pictureBox1.Image = map;
                P2.X = e.X;
                P2.Y = e.Y;
                drawrect(P1.X, P2.X, P1.Y, P2.Y);
                
            }
            if (k == 0 && b==1)
            {
                Bitmap map1 = new Bitmap(1000, 1000);
                map = map1;
                pictureBox1.Image = map;
                f2.X = e.X;
                f2.Y = e.Y;
                drawrect(P1.X, P2.X, P1.Y, P2.Y);//если что убрать
                line (f1.X,f1.Y,f2.X,f2.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (k == 1)//ставим первую точку
            {
                f1 = e.Location;
                k--;
            }
            else if (k == 0) 
            {
                f2 = e.Location;
                k--;
                b = 0;
            }

            if (k == -1)
            {
                Bitmap map2 = new Bitmap(1000, 1000);
                map = map2;
                drawrect(P1.X, P2.X, P1.Y, P2.Y);
                pictureBox1.Image = map;
                clipline(f1.X, f1.Y, f2.X, f2.Y, P1.X, P1.Y, P2.X, P2.Y);
                pictureBox1.Image = map;
                k = 0;
                f1 = f2;
            }

            if (v == 1)
            {
                k = 1;
                v--;
            }
        }
    }
}

