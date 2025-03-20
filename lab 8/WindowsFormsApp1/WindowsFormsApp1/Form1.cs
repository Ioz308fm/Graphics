using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<PointF> dots = new List<PointF>();
        const int resx = 1920, resy = 1080;
        double boom = 4;
        double tmax = 0;
        const double s = 1.0/400.0;//scaling
        int N = 100;
        int count = 0;
        Bitmap map = new Bitmap(resx, resy);
        int n = 0;
        double maxr = s*s / 4 *(resx * resx + resy * resy);

        double dwell(double x, double y)
        {
            double cx = x - resx / 2, cy = y - resy / 2;
            cx *= s;
            cy *= s;
            double tmp, dx = cx, dy = cy, f = cx * cx + cy * cy;
            for (int i = 0; i <= N && Math.Abs(f)<=boom; i++)
            {
                tmp = dx;
                dx = dx * dx - dy * dy + cx;
                dy = 2 * tmp * dy + cy;
                f = dx * dx + dy * dy;
            }

            return f;
        }
        int dwellnum(int x, int y)
        {
            double cx = x - resx / 2, cy = y - resy / 2;
            cx *= s;
            cy *= s;
            int i;
            double tmp, dx = cx, dy = cy, f = cx * cx + cy * cy;
            for (i = 0; i <= N && Math.Abs(f) <= boom; i++)
            {
                tmp = dx;
                dx = dx * dx - dy * dy + cx;
                dy = 2 * tmp * dy + cy;
                f = dx * dx + dy * dy;
            }

            return i;
        }

        int Rscale(double t)
        {
            return Convert.ToInt32((Math.Abs(t-20)) * 255.0 / (16.0));
        }

        int Rsc(double t)
        {
            return Convert.ToInt32(t * 255.0 / (36.0));
        }

        int bt(int M)
        {
            int x1=M;
            if (M > 35)
                x1 = 35;
            if (M < 2)
                x1 = 2;
            return 255 - 255 * (x1-2) / 33;
        }

        int rt(int M)
        {
            int x2 = M;
            if (M > 68)
                x2 = 68;
            if (M < 35)
                x2 = 35;

            x2 = x2 - 35;
            return 255 - 255 * x2 / 33;
        }

        int gt(int M)
        {
            int x3 = M;
            if (M < 68)
                x3 = 68;
            x3 = x3 - 68;
            return 255 - 255 * x3 / 33;
        }



        void paint(int i, int j, int k)
        {
            if (k==1)//самый красивый
            {
                double t = dwell(i, j);
                if (t > tmax)
                {
                    tmax = t;
                }
                if (Math.Abs(t) <= 4)
                {
                    map.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
                else
                {
                    double ro = s * s * ((i - resx / 2) * (i - resx / 2) + (j - resy / 2) * (j - resy / 2));
                    int M = dwellnum(i, j);
                    int r = Rscale(t);
                    int g = 255 - 255 * M / 101;
                    int b = (r+g)/2;
                    map.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            if (k == 2)//круги и зеленый вариант
            {
                double t = dwell(i, j);
                if (t > tmax)
                {
                    tmax = t;
                }
                if (Math.Abs(t) <= 4)
                {
                    map.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
                else
                {
                    double ro = s * s * ((i - resx / 2) * (i - resx / 2) + (j - resy / 2) * (j - resy / 2));
                    int M = dwellnum(i, j);
                    int r = Rscale(t);
                    int g = 255 - 255 * M / 101;
                    int b = (r + g) % 255;
                    map.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            if (k==3)//серый
            {
                int M = dwellnum(i, j);
                if (M >= 101)
                { 
                    map.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
                else
                {
                    int r = 255 - 255 * M / 101;
                    int g = 255 - 255 * M / 101;
                    int b = 255 - 255 * M / 101;
                    map.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            if (k == 4)//белые круги
            {
                double t = dwell(i, j);
                if (t > tmax)
                {
                    tmax = t;
                }
                if (Math.Abs(t) <= 4)
                {
                    map.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
                else
                {
                    double ro = s * s * ((i - resx / 2) * (i - resx / 2) + (j - resy / 2) * (j - resy / 2));
                    int M = dwellnum(i, j);
                    int r = Rsc(t);
                    int g = 255 - 255 * M / 101;
                    int b = (r + g) % 255;
                    map.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }

            if (k == 5)//желтый
            {
                int M = dwellnum(i, j);
                if (M >= 101)
                {
                    map.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
                else
                {
                    int r = rt(M);
                    int g = gt(M);
                    int b = bt(M);
                    map.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
        }

        void mandelbrot(int k)
        {
            Random rnd = new Random();
            map.SetPixel(resx / 2, resy / 2, Color.Red);
            for(int i = 0; i < resx; i++)
            {
                for (int j = 0; j < resy; j++) 
                {
                    paint(i,j,k);
                }
            }
            pictureBox1.Image = map;
            //picsave();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                n++;
            }
            else
            {
                n--;
            }
            mandelbrot(n);
        }

        void picsave ()
        {
            if (pictureBox1.Image != null) //если в pictureBox есть изображение
            {
                //создание диалогового окна "Сохранить как..", для сохранения изображения
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
                savedialog.OverwritePrompt = true;
                //отображать ли предупреждение, если пользователь указывает несуществующий путь
                savedialog.CheckPathExists = true;
                //список форматов файла, отображаемый в поле "Тип файла"
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                //отображается ли кнопка "Справка" в диалоговом окне
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        pictureBox1.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
