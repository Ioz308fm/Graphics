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
        Point f1, f2;
        Bitmap map = new Bitmap(20000, 20000);
        int k=0;
        void pixel (int x,int y, int x0, int y0)
        {
            map.SetPixel(x, y, Color.Black);//1 четверть
            map.SetPixel(2 * x0 - x, y, Color.Black);//2четверть 
            map.SetPixel(2 * x0 - x, 2 * y0 - y, Color.Black);//3четверть 
            map.SetPixel(x, 2 * y0 - y, Color.Black);//4четверть 
            
        }

        void drawrect (int xa,int xb,int ya,int yb)
        {
            int signx=Math.Sign(xb-xa);
            int signy=Math.Sign(yb-ya);
            for(int x=xa;x*signx<=xb*signx;x+=signx)
            {
                map.SetPixel(x, ya, Color.Black);
                map.SetPixel(x, yb, Color.Black);
            }
            for(int y=ya;y*signy<=yb*signy;y+=signy)
            {
                map.SetPixel(xa, y, Color.Black);
                map.SetPixel(xb, y, Color.Black);
            }
        }

        void ellipse (int a,int b,int X0,int Y0 )
        {
            int x = 0;//"1/8" эллипса
            int y = b;
            double d = b * b - a * a * b + a * a / 4;
            pixel(x+X0, y+Y0, X0, Y0);
            while (y*a*a>=x*b*b)
            {
                if (d<0)
                {
                    d += 2 * b * b * (x + 1) + b * b;
                    x++;
                }
                else
                {
                    d += 2 * b * b * (x + 1) + b * b - 2 * a * a * (y - 0.5) + a * a / 4;
                    x++;
                    y--;
                }
                pixel(x+X0, y+Y0, X0, Y0);
            }
        
            x = a ;//"1/4" эллипса
            y = 0;
            d = a * a - b * b * a + b * b / 4;
            pixel(x+X0, y+Y0, X0, Y0);
            while (y * a * a <= x * b * b)
            {
                if (d < 0)
                {
                    d += 2 * a * a * (y + 1) + a * a;
                    y++;
                }
                else
                {
                    d += 2 * a * a * (y + 1) + a * a - 2 * b * b * (x - 0.5) + b * b / 4;
                    y++;
                    x--;
                }
                pixel(x+X0, y+Y0, X0, Y0);
            }
         
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            f1 = e.Location;
            k = 1;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (k==1)
            {
                drawrect(f1.X, e.X, f1.Y, e.Y);
                pictureBox1.Image = map;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            k = 0;
            f2 = e.Location;
            int a, b,X0,Y0;
            a= Math.Abs((f1.X-f2.X)/2);
            b= Math.Abs((f1.Y-f2.Y)/2);
            X0 = (f1.X + f2.X) / 2;
            Y0 = (f1.Y + f2.Y) / 2;
            drawrect(f1.X, f2.X, f1.Y, f2.Y);
            ellipse(a, b,X0,Y0);
            pictureBox1.Image = map;
        }
    }
}
