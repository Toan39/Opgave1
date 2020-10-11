using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;

namespace CirkelKlikker
{
    class CirkelKlikker : Form
    {
        const int lengte = 6, breedte = 6;
        const int diameter = 29;
        int[,] k, kl;
        bool[] insarray;//, waar;
        bool[,] leg;
        private int kleur = 1;
        private int x;
        private int y;
        private int mogzet = 0, overgeslagen = 0;
        private int aantalrood = 2, aantalblauw = 2;
        Panel Speelbord;
        TextBox hallo;
        public CirkelKlikker()
        {
            this.Size = new Size(300, 300);
            this.k = new int[breedte, lengte];
            this.kl = new int[breedte, lengte];
            this.leg = new bool[breedte, lengte];
            this.insarray = new bool[8];
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

            hallo = new TextBox() { Size = new Size(30, 20), Location = new Point(20, 250) };
            this.Controls.Add(hallo);

            k[breedte / 2, lengte / 2] = 1; kl[breedte / 2, lengte / 2] = 1;
            k[breedte / 2 - 1, lengte / 2] = 1; kl[breedte / 2 - 1, lengte / 2] = 2;
            k[breedte / 2, lengte / 2 - 1] = 1; kl[breedte / 2, lengte / 2 - 1] = 2;
            k[breedte / 2 - 1, lengte / 2 - 1] = 1; kl[breedte / 2 - 1, lengte / 2 - 1] = 1;

            legaal(kleur);

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
            x = mea.X / 33;
            y = mea.Y / 33;

            if (mogzet > 0 && overgeslagen < 2)
            {
                overgeslagen--;
                if (leg[x, y])
                {
                    this.veranderkleur(x, y);
                }
            }
            else overgeslagen++;
            this.kleur = 3 - kleur;
            legaal(kleur);
        }

        public void legaal(int kleur)
        {
            for (int t = 0; t < breedte; t++)
            {
                for (int u = 0; u < lengte; u++)
                {
                    leg[t, u] = richtingen(t, u);
                }
            }
        }

        //public bool richtingen(int x, int y, int kleur)
        //{
        //    if (kl[x, y] > 0) return false;
        //    int andere = 3 - kleur;

        //    // checkt of het geklikte vakje niet aan rand van het bord zit en of het een andere steen kan insluiten
        //    if (y > 2 && kl[x, y - 1] == andere && kl[x, y - 2] == kleur) return true; // naar boven
        //    if (y > 2 && x < breedte - 3 && kl[x + 1, y - 1] == andere && kl[x + 2, y - 2] == kleur) return true; // naar rechtsboven
        //    if (x < breedte - 3 && kl[x + 1, y] == andere && kl[x + 2, y] == kleur) return true; // naar rechts
        //    if (y < lengte - 3 && x < breedte - 3 && kl[x + 1, y + 1] == andere && kl[x + 2, y + 2] == kleur) return true; // naar rechtsonder
        //    if (y < lengte - 3 && kl[x, y + 1] == andere && kl[x, y + 2] == kleur) return true; // naar onder
        //    if (y < lengte - 3 && x > 2 && kl[x - 1, y + 1] == andere && kl[x - 2, y + 2] == kleur) return true; // naar linksonder
        //    if (x > 2 && kl[x - 1, y] == andere && kl[x - 2, y] == kleur) return true; // naar links
        //    if (y > 2 && x > 2 && kl[x - 1, y - 1] == andere && kl[x - 2, y - 2] == kleur) return true; // naar linksboven
        //    return false;
        //}
        //public void veranderkleur(int x, int y, int kleur)
        //{
        //    int andere = 3 - kleur;
        //    kl[x, y] = kleur;

        //    if (y > 2                             && kl[x, y - 1] == andere     && kl[x, y - 2] == kleur)       kl[x, y -1] = kleur; // naar boven
        //    if (y > 2 && x < breedte - 3          && kl[x + 1, y - 1] == andere && kl[x + 2, y - 2] == kleur)   kl[x+ 1, y - 1] = kleur; // naar rechtsboven
        //    if (x < breedte - 3                   && kl[x + 1, y] == andere     && kl[x + 2, y] == kleur)       kl[x + 1, y] = kleur; // naar rechts
        //    if (y < lengte - 3 && x < breedte - 3 && kl[x + 1, y + 1] == andere && kl[x + 2, y + 2] == kleur)   kl[x + 1, y + 1] = kleur; // naar rechtsonder
        //    if (y < lengte - 3                    && kl[x, y + 1] == andere     && kl[x, y + 2] == kleur)       kl[x, y + 1] = kleur; // naar onder
        //    if (y < lengte - 3 && x > 2           && kl[x - 1, y + 1] == andere && kl[x - 2, y + 2] == kleur)   kl[x - 1, y + 1] = kleur; // naar linksonder
        //    if (x > 2                             && kl[x - 1, y] == andere     && kl[x - 2, y] == kleur)       kl[x - 1, y] = kleur; // naar links
        //    if (y > 2 && x > 2                    && kl[x - 1, y - 1] == andere && kl[x - 2, y - 2] == kleur)   kl[x - 1, y - 1] = kleur; // naar linksboven
        //}

        public void arrayOpslag(int x, int y)
        {
            this.insarray[0] = insluiten(x, y, 0, -1);
            this.insarray[1] = insluiten(x, y, -1, -1);
            this.insarray[2] = insluiten(x, y, -1, 0);
            this.insarray[3] = insluiten(x, y, -1, 1);
            this.insarray[4] = insluiten(x, y, 0, 1);
            this.insarray[5] = insluiten(x, y, 1, 1);
            this.insarray[6] = insluiten(x, y, 1, 0);
            this.insarray[7] = insluiten(x, y, 1, -1);
        }
        bool richtingen(int x, int y)
        {
            if (kl[x, y] > 0) return false;
            // checkt of het geklikte vakje niet aan rand van het bord zit en of het een andere steen kan 
            this.arrayOpslag(x, y);
            bool allfalse = true;
            for (int t = 0; t < 8; t++)
            { if (insarray[t]) { allfalse = false; mogzet++; } }
            if (allfalse) return false; else return true;
        }


        bool insluiten(int x, int y, int xrichting, int yrichting)
        {
            int t = x + xrichting, u = y + yrichting;
            bool eerste = true;
            while (t < breedte && t >= 0 && u < lengte && u >= 0)
            {
                if (kl[t, u] == 0) return false;
                if (kl[t, u] == kleur) { if (eerste) return false; else return true; }

                t += xrichting;
                u += yrichting;
                eerste = false;
            }
            return false;
        }
        public void veranderkleur(int x, int y)
        {
            this.arrayOpslag(x, y);
            plaats(x,y);

            for (int n = 0; n < 8; n++)
            {
                if (insarray[n])
                {
                    switch (n)
                    {
                        case 0: kleuringesloten(x, y, 0, -1); break;
                        case 1: kleuringesloten(x, y, -1, -1); break;
                        case 2: kleuringesloten(x, y, -1, 0); break;
                        case 3: kleuringesloten(x, y, -1, 1); break;
                        case 4: kleuringesloten(x, y, 0, 1); break;
                        case 5: kleuringesloten(x, y, 1, 1); break;
                        case 6: kleuringesloten(x, y, 1, 0); break;
                        case 7: kleuringesloten(x, y, 1, -1); break;
                    }
                }
            }
            Speelbord.Invalidate();
        }

        public void kleuringesloten(int x, int y, int xrichting, int yrichting)
        {
            {
                int andere = 3 - kleur;
                bool eindebereikt = false;
                x += xrichting;
                y += yrichting;
                while (eindebereikt == false)
                {
                    if (this.kl[x, y] == andere) { vervang(x, y); }
                    x += xrichting;
                    y += yrichting;
                    if (this.kl[x, y] == kleur) { eindebereikt = true; }
                }
            }
        }

        public void plaats(int x, int y)
        {
            kl[x, y] = kleur;
            switch (kleur)
            {
                case 1: aantalblauw++; break;
                case 2: aantalrood++; break;
            }
        }

        public void vervang(int x, int y)
        {
            kl[x, y] = kleur;
            switch (kleur)
            {
                case 1: aantalblauw++; aantalrood--; break;
                case 2: aantalrood++; aantalblauw--; break;
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
            SolidBrush br = new SolidBrush(Color.Transparent);
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            for (int t = 0; t < breedte; t++)
            {
                for (int u = 0; u < lengte; u++)
                {
                    switch (kl[t, u])
                    {
                        case 1: br.Color = Color.Blue; break;
                        case 2: br.Color = Color.Red; break;

                    }
                    if (kl[t, u] > 0) gr.FillEllipse(br,
                                      t * 33 + 2, u * 33 + 2,
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