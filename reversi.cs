using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace CirkelKlikker
{
    class CirkelKlikker : Form
    {
        int lengte = 6, breedte = 6;
        const int maxAantal = 100;
        const int max1 = 1000;
        const int max2 = 1000;
        const int diameter = 29;
        const int straal = diameter / 2;
        int[,] k;
        int[] xs, ys;
        private int x;
        private int y;
        private int aantal;
        Panel Speelbord;
        public CirkelKlikker()
        {
            this.Size = new Size(300, 300);
            this.xs = new int[maxAantal];
            this.ys = new int[maxAantal];
            this.k= new int[max1,max2];
            this.aantal = 0;
            this.x = 0;
            this.y = 0;
            this.Text = "CirkelKlikker";
            this.Paint += tekenscherm;

            Speelbord = new Panel()
            {
                Location = new Point(20, 20),
                Size = new Size(1 + breedte * 33, 1 + lengte * 33),
                BackColor = Color.Transparent,
            };
            Speelbord.Paint += teken;
            Speelbord.MouseClick += klik; 
            this.Controls.Add(Speelbord);

            // for loop voor maken van x * y panels.
            //for (int t = 0; t < 6; t++)
            //{
            //    for (int v = 0; v < 6; v++)
            //    {

            //        Speelbord = new Panel() { Location = new Point(20 + 33 * t, 20 + 33 * v), Size = new Size(32, 32), BackColor = Color.FromArgb(0, Color.White) };
            //        Speelbord.Paint += teken;
            //        Speelbord.MouseClick += klik;
            //        this.Controls.Add(Speelbord);
            //    }
            //}

        }
        public void klik(object sender, MouseEventArgs mea)
        {
            x = mea.X / 33 * 33 + 16;
            y = mea.Y / 33 * 33 + 16;

            if (legaal(x,y))
            {
                if (this.aantal < maxAantal)
                {
                    xs[aantal] = mea.X / 33 * 33 + 16;
                    ys[aantal] = mea.Y / 33 * 33 + 16;
                    this.Invalidate();
                    this.aantal++;
                }
            }
        }

        public bool legaal(int x, int y)
        {
            if (k[x,y] != 1)
            {
                k[x,y] = 1;                           
                return true;
            }
                return false;
        }
        private void tekenscherm(object sender, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;
            for (int t = 1; t < breedte; t++)
            {
                gr.DrawLine(Pens.Black, 20 + 33 * t, 21, 20 + 33 * t, 21 + 33 * lengte);
            }
            for (int t = 1; t < lengte; t++)
            {
                gr.DrawLine(Pens.Black, 21, 20 + 33 * t, 21 + 33 * breedte, 20 + 33 * t);
            }
        }
        private void teken(object sender, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            for (int t = 0; t < aantal; t++)
            {
                if (t % 2 == 0)
                {
                    gr.FillEllipse(Brushes.Blue,
                                    this.xs[t] - straal, this.ys[t] - straal,
                                    diameter, diameter);
                }
                else
                {
                    gr.FillEllipse(Brushes.Red,
                                    this.xs[t] - straal, this.ys[t] - straal,
                                    diameter, diameter);
                }

            }
        }
    }

    class HalloWin3
    {
        static void Main()
        {
            CirkelKlikker scherm;
            scherm = new CirkelKlikker();
            Application.Run(scherm);
        }
    }

}
