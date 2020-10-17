using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Reversi
{
    class Reversi : Form
    {
        //declaraties
        Font ComicSans = new Font("Comic Sans MS", 14);//Het font dat wordt gebruikt bij het scorebord
        const int lengte = 6, breedte = 6;//Het aantal vakjes van het speelbord in de lengte en breedte
        const int diameter = 29;//Diameter van de stenen
        const int steenoffset = (34 - diameter) / 2;//Hoeveel de steen vanaf de rand van een vakje moet worden verschoven om in het midden te komen
        bool geeindigd = false, hulpNodig = false;
        int[,] kl;//2D array die opslaat welke kleur elk vakje heeft: 0 voor leeg, 1 voor een blauwe steen en 2 voor een rode
        bool[,] leg;//Slaat op of een vakje legaal is
        bool[] insarray;//Slaat voor elke richting op of er stenen ingesloten kunnen worden ingesloten
        private int kleur = 1;//Kleur die aan de beurt is: 1 is blauw, 2 = rood. Blauw begint.
        private int x;
        private int y;//x en y van het geklikte vakje
        private int mogzet = 0, overgeslagen = 0;
        private int aantalrood = 2, aantalblauw = 2;

        //onderdelen v/d interface die ook na de initialisatie worden aangeroepen
        Panel Speelbord;
        Panel Scorebord;
        Label Status;
        CheckBox Helplock;

        public Reversi()
        {
            //Properties van de window
            this.Size = new Size(300, 380);
            this.Text = "Reversi";
            this.Paint += tekenscherm;

            //Domeinen van de arrays
            this.kl = new int[breedte, lengte];
            this.leg = new bool[breedte, lengte];
            this.insarray = new bool[8];

            //alle onderdelen van het scherm

            //Speelbord//
            Speelbord = new Panel()
            {
                Location = new Point(20, 20),
                Size = new Size(breedte * 33, lengte * 33),
                BackColor = Color.Transparent
            };
            Speelbord.Paint += teken;
            Speelbord.MouseClick += klik;
            this.Controls.Add(Speelbord);

            //Score bord met aantal stenen per kleur//
            Scorebord = new Panel()
            {
                Location = new Point(20, 270),
                Size = new Size(150, 60)
            };
            Scorebord.Paint += score;
            this.Controls.Add(Scorebord);

            //nieuw spel-knop//
            Button NieuwKnop = new Button()
            {
                Size = new Size(80, 20),
                Location = new Point(20, 250),
                Text = "Nieuw Spel"
            };
            NieuwKnop.Click += NieuwSpel;
            this.Controls.Add(NieuwKnop);

            //Hulpknop//
            Button HulpKnop = new Button()
            {
                Size = new Size(80, 20),
                Location = new Point(110, 250),
                Text = "Help"
            };
            HulpKnop.Click += Hulp;
            this.Controls.Add(HulpKnop);

            //Chekbox waardoor de "hulpcirkels" na het eindigen van een beurt blijven//
            Helplock = new CheckBox()
            {
                Location = new Point(200, 254),
                Size = new Size(15,15)
            };
            this.Controls.Add(Helplock);

            //Label naast de checkbox//
            Label Lock = new Label()
            {
                Text = "Lock Help",
                Size = new Size(60, 30),
                Location = new Point(215, 254)
            };
            this.Controls.Add(Lock);

            //Status//
            Status = new Label()
            {
                Size = new Size(150, 20),
                Location = new Point(20, 230),
                Text = "Blauw is aan zet"
            };
            this.Controls.Add(Status);

            //De 4 stenen in het midden
            kl[breedte / 2, lengte / 2] = 1;
            kl[breedte / 2 - 1, lengte / 2] = 2;
            kl[breedte / 2, lengte / 2 - 1] = 2;
            kl[breedte / 2 - 1, lengte / 2 - 1] = 1;

            legaal(kleur); //wordt aan het begin van het spel uitgevoerd, zodat de speler op de eerste legale vakjes kan klikken
        }

        public void klik(object sender, MouseEventArgs mea)//De eventhandler voor wanneer je op het speelbord klikt
        {
            x = mea.X / 33; //met het x- en y-coordinaat wordt berekent op welk vakje de speler heeft geklikt, elk vakje is 33*33 pixels
            y = mea.Y / 33;

            if (!geeindigd)
            {
                if (leg[x, y])//kijkt of het geklikte vakje legaal is
                {
                    overgeslagen = 0;//aantal overgeslagen beurten in een rij wordt gereset
                    if(!Helplock.Checked) hulpNodig = false;//Als de hulp niet gelockd is, wordt de hulpfunctie uitgezet 
                    this.veranderkleur(x, y);//Update het bord
                    this.kleur = 3 - kleur;//Verandert de kleur die aan de beurt is 3 - 2(rood) = 1(blauw) en vice versa
                }
                mogzet = 0;//Reset de mogelijke zetten voor de volgende beurt
                legaal(kleur);//wordt uigevoerd (zie de methode)
                while (mogzet == 0 && !geeindigd)//zolang het aantal mogelijke zetten 0 is(wanneer er een beurt moet worden overgeslagen) en het spel niet is geeindigd
                {
                    overgeslagen++;
                    if (overgeslagen == 2)
                    {
                        EindeSpel();//als er 2 beurten achter elkaar zijn overgeslagen is het spel afgelopen, het spel kan namelijk niet verder als niemand een zet kan doen
                    }
                    this.kleur = 3 - kleur;//volgende speler aan de beurt
                    legaal(kleur);
                }
            }
            if (!geeindigd) //Deze if-statement zorgt ervoor dat de uitslag van het potje niet wordt overschreven met wie er aan zet is
                switch (kleur)
                {
                    case 1: Status.Text = "Blauw is aan zet"; break;
                    case 2: Status.Text = "Rood is aan zet"; break;
                } //Laat zien wie er aan zet is
        }

        public void legaal(int kleur) //Gaat elk vakje op het bord af en checkt of er een legale zet gedaan kan worden.
        {
            for (int t = 0; t < breedte; t++)
            {
                for (int u = 0; u < lengte; u++)
                {
                    leg[t, u] = richtingen(t, u);//"Legale vakjes" worden opgeslagen in een 2-dimensionale array.
                    if (leg[t, u]) mogzet++;//voor elke mogelijke zet wordt het aantal mogelijke zetten geincrement 
                }
            }
        }

        public void arrayOpslag(int x, int y) //Voert insluiten() uit in de 8 nodige richtingen en slaat deze waarden op in een bool array. 
        {
            this.insarray[0] = insluiten(x, y, 0, -1);//naar boven,
            this.insarray[1] = insluiten(x, y, -1, -1);//linksboven,
            this.insarray[2] = insluiten(x, y, -1, 0);//links,
            this.insarray[3] = insluiten(x, y, -1, 1);//linksonder,
            this.insarray[4] = insluiten(x, y, 0, 1);//onder,
            this.insarray[5] = insluiten(x, y, 1, 1);//rechtsonder,
            this.insarray[6] = insluiten(x, y, 1, 0);//rechts
            this.insarray[7] = insluiten(x, y, 1, -1);//en rechtsboven.
        // Methode die bepaald welke richtingen ingeklikt kunnen worden
          bool richtingen(int x, int y)
        {
            if (kl[x, y] > 0) return false;
            // checkt of het geklikte vakje niet aan rand van het bord zit en of het een andere steen kan 
            this.arrayOpslag(x, y);
            bool allfalse = true;
            for (int t = 0; t < 8; t++)
            { if (insarray[t]) allfalse = false; }
            if (allfalse) return false; else return true;
        }

        //Methode die ervoor zorgt dat alleen een steen geplaatst kan worden die een insluiting veroorzaken
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

        //Methode die na het plaatsen van een steen de ingesloten stenen naar dezelfde kleur veranderen als de insluiters.
        public void veranderkleur(int x, int y)
        {
            this.arrayOpslag(x, y);
            plaats(x, y);

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
            Scorebord.Invalidate();  // De scorebord wordt getekend met de recenste score tussen rood en blauw
            Speelbord.Invalidate();  //Het speelbord wordt getekened met de geplaatste stenen
        }

        //Methode die controleert of één steen of meerdere stenen ingesloten is door 2 stenen van de tegenovergestelde kleur (bv. 2 rood ingesloten door 2 blauw)
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

        //Methode die het aantal blauwe en rode stenen telt.
        public void plaats(int x, int y)
        {
            kl[x, y] = kleur;
            switch (kleur)
            {
                case 1: aantalblauw++; break;
                case 2: aantalrood++; break;
            }
        }

        //Update het aantal stenen respectief per kleur als dus ingesloten stenen van kleur zijn veranderd.
        public void vervang(int x, int y)
        {
            kl[x, y] = kleur;
            switch (kleur)
            {
                case 1: aantalblauw++; aantalrood--; break;
                case 2: aantalrood++; aantalblauw--; break;
            }
        }

        //Methode dat de speelbord tekent
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
        //Methode die de gekleurde stenen. 
        //En de transparante stenen voor de hulp-knop tekent (als op de hulp-knop wordt gedrukt)
        private void teken(object sender, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;
            SolidBrush br = new SolidBrush(Color.Blue);
            Pen pen = Pens.Black;
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            for (int t = 0; t < breedte; t++)
            {
                for (int u = 0; u < lengte; u++)
                {
                    if (leg[t, u] && hulpNodig) gr.DrawEllipse(pen, t * 33 + steenoffset, u * 33 + steenoffset, diameter, diameter);
                    switch (kl[t, u])
                    {
                        case 1: br.Color = Color.Blue; break;
                        case 2: br.Color = Color.Red; break;
                    }
                    if (kl[t, u] > 0) gr.FillEllipse(br,
                                      t * 33 + steenoffset, u * 33 + steenoffset,
                                      diameter, diameter);
                }

            }
        }

        //Tekent de scoreboard
        private void score(object sender, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;
            Pen pen = Pens.Blue;
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gr.FillEllipse(Brushes.Blue, 0, 0, diameter, diameter);
            gr.FillEllipse(Brushes.Red, 0, 30, diameter, diameter);
            gr.DrawString(aantalblauw + " stenen", ComicSans, Brushes.Blue, 32, 2);
            gr.DrawString(aantalrood + " stenen", ComicSans, Brushes.Red, 32, 32);
        }

        //Update de label die aangeeft wie aan de beurt is naar wie heeft gewonnen of dat het gelijkspel als het spel is ge-eindigd
        private void EindeSpel()
        {
            geeindigd = true;
            if (aantalblauw > aantalrood) Status.Text = "Blauw heeft gewonnen";
            else
            if (aantalrood > aantalblauw) Status.Text = "Rood heeft gewonnen";
            else Status.Text = "Remise";
        }

        //Methode die het spel opnieuw start als geklikt is op de "nieuw spel" knop
        private void NieuwSpel(object sender, EventArgs ea)
        {
            Application.Restart();
        }
        
        //Methode die de transparante stenen die worden getekend als op de hulp-knop is geklikt weghaald, als een steen wordt geplaatst
        private void Hulp(object sender, EventArgs ea)
        {
            hulpNodig = !hulpNodig;
            Speelbord.Invalidate();
        }
    }

    
    class Reversi2
    {
        //Main void, wordt dus als eerste uitgevoerd, start 'Form', wat het interactieve scherm is.
        static void Main()
        {
            Reversi scherm;
            scherm = new Reversi();
            Application.Run(scherm);
        }
    }

}