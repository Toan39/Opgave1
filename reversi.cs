using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace CirkelKlikker
{
    class CirkelKlikker : Form
    {
        int lengte = 10, breedte = 14;
        const int maxAantal = 100;
        const int diameter = 15;
        const int straal = diameter / 2;
        int[] xs, ys;
        private int aantal;
        Panel Speelbord;
        public CirkelKlikker()
        {
            this.Size = new Size(300, 300);
            this.xs = new int[maxAantal];
            this.ys = new int[maxAantal];
            this.aantal = 0;
            this.Text = "CirkelKlikker";
            this.Paint += tekenscherm;

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
        private void klik(object sender, MouseEventArgs mea)
        {
            if (this.aantal < maxAantal)
            {
                this.xs[aantal] = mea.X;
                this.ys[aantal] = mea.Y;
                this.Invalidate();
                this.aantal++;
            }
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
