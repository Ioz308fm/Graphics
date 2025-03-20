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

        Point f1, f2;
        int k = 1;
        Bitmap map = new Bitmap(2000, 2000);

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
                // Начальные значения
                int x = x1;
                int y = y1;
                int d = -lengthX;

                // Основной цикл
                length++;
                while (length>0)
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
                // Начальные значения
                int x = x1;
                int y = y1;
                int d = -lengthY;

                // Основной цикл
                length++;
                while (length>0)
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
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (k == 1)//ставим первую точку
            {
                f1 = e.Location;
                k --;
            }
            else if (k==0)
            {
                f2 = e.Location;
                k --;
            }

            if(k == -1 )
            {
                line(f1.X, f1.Y, f2.X, f2.Y);
                pictureBox1.Image = map;
                k = 0;
                f1 = f2;
            }
        }
    }
}
