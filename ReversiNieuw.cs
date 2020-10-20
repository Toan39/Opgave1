using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Reversi
{
    class Reversi : Form
    {
        //Declaraties
        Font ScoreFont = new Font("Comic Sans MS", 14);
        const int lengte = 6, breedte = 6; //Het aantal vakjes van het speelbord in de lengte en breedte, het spel werkt perfect zolang deze beiden groter zijn dan 2 
        const int diameter = 29;
        const int steenoffset = (34 - diameter) / 2; //Hoeveel de steen vanaf de rand van een vakje moet worden verschoven om in het midden te komen
        bool geeindigd, hulpNodig;
        int[,] kl; //2D array die opslaat welke kleur elk vakje heeft: 0 voor leeg, 1 voor een blauwe steen en 2 voor een rode
        bool[,] leg; //Slaat op of een vakje legaal is
        bool[] insarray; //Slaat voor elke richting op of er stenen ingesloten kunnen worden ingesloten
        private int kleur; //Kleur die aan de beurt is: 1 is blauw, 2 = rood. Blauw begint.
        private int x;
        private int y; //x en y van het geklikte vakje
        private int mogelijkeZetten, overgeslagen;
        private int aantalrood, aantalblauw;

        //Onderdelen v/d interface die ook na de initialisatie worden aangeroepen
        Panel Speelbord;
        Panel Scorebord;
        Label Status;
        CheckBox Helplock;

        public Reversi()
        {
            //Properties van de window
            this.Size = new Size(Math.Max(300, 60 + breedte * 33), 196 + lengte * 33);//Met een minimale breedte van 300, omdat anders de knoppen niet volledig getoont zouden worden
            this.Text = "Reversi";
            this.Paint += tekenscherm;

            //alle onderdelen van het scherm

            //Speelbord
            Speelbord = new Panel()
            {
                Location = new Point(20, 20),
                Size = new Size(breedte * 33, lengte * 33),
                BackColor = Color.Transparent
            };
            Speelbord.Paint += teken;
            Speelbord.MouseClick += klik;
            this.Controls.Add(Speelbord);

            //Score bord met aantal stenen per kleur
            Scorebord = new Panel()
            {
                Location = new Point(20, Speelbord.Height + 77),
                Size = new Size(300, 60)
            };
            Scorebord.Paint += score;
            this.Controls.Add(Scorebord);

            //Nieuw Spel-knop
            Button NieuwKnop = new Button()
            {
                Size = new Size(80, 20),
                Location = new Point(20, Speelbord.Height + 52),
                Text = "Nieuw Spel"
            };
            NieuwKnop.Click += NieuwSpel;
            this.Controls.Add(NieuwKnop);

            //Hulpknop
            Button HulpKnop = new Button()
            {
                Size = new Size(80, 20),
                Location = new Point(110, Speelbord.Height + 52),
                Text = "Help"
            };
            HulpKnop.Click += Hulp;
            this.Controls.Add(HulpKnop);

            //Chekbox waarmee de helpknop wordt vergrendelt en de "hulpcirkels" na het einde van een beurt niet verdwijnen
            Helplock = new CheckBox()
            {
                Location = new Point(200, Speelbord.Height + 56),
                Size = new Size(15, 15)
            };
            this.Controls.Add(Helplock);

            //Label naast de checkbox
            Label Lock = new Label()
            {
                Text = "Lock Help",
                Size = new Size(60, 30),
                Location = new Point(215, Speelbord.Height + 56)
            };
            this.Controls.Add(Lock);

            //Status
            Status = new Label()
            {
                Font = new Font("Comic Sans MS", 11),
                Size = new Size(250, 30),
                Location = new Point(20, Speelbord.Height + 24)
            };
            this.Controls.Add(Status);

            this.insarray = new bool[8];
            defaultWaarden();
        }
        public void defaultWaarden()
        {
            kl = null;
            leg = null;

            this.kl = new int[breedte, lengte];
            this.leg = new bool[breedte, lengte];

            geeindigd = false;
            hulpNodig = false;
            kleur = 1;
            aantalrood = 2;
            aantalblauw = 2;
            mogelijkeZetten = 0;
            overgeslagen = 0;
            Helplock.Checked = false;
            Status.Text = "Blauw is aan zet";

            //De 4 stenen in het midden
            kl[breedte / 2, lengte / 2] = 1;
            kl[breedte / 2 - 1, lengte / 2] = 2;
            kl[breedte / 2, lengte / 2 - 1] = 2;
            kl[breedte / 2 - 1, lengte / 2 - 1] = 1;

            legaal();
        }

        public void klik(object sender, MouseEventArgs mea)//De eventhandler voor wanneer je op het speelbord klikt
        {
            x = mea.X / 33; //Met het x- en y-coordinaat wordt berekent op welk vakje de speler heeft geklikt, elk vakje is 33*33 pixels
            y = mea.Y / 33;

            if (leg[x, y])//Kijkt of het geklikte vakje legaal is
            {
                if (!Helplock.Checked) hulpNodig = false;//Als de hulp niet gelockd is, wordt de hulpfunctie uitgezet 
                this.veranderkleur(x, y);//Update het bord
                this.kleur = 3 - kleur;//Verandert de kleur die aan de beurt is 3 - 2(rood) = 1(blauw) en vice versa
            }
            mogelijkeZetten = 0;//Reset de mogelijke zetten voor de volgende beurt
            legaal();
            while (mogelijkeZetten == 0 && !geeindigd)//Zolang het aantal mogelijke zetten 0 is(wanneer er een beurt moet worden overgeslagen) en het spel niet is geeindigd
            {
                overgeslagen++;
                if (overgeslagen == 2)
                {
                    EindeSpel();//Als er 2 beurten achter elkaar zijn overgeslagen is het spel afgelopen, het spel kan namelijk niet verder als niemand een zet kan doen
                }
                this.kleur = 3 - kleur;//volgende speler aan de beurt
                legaal();
            }
            overgeslagen = 0;//Aantal overgeslagen beurten in een rij wordt gereset
            if (!geeindigd) //Zorgt ervoor dat de uitslag van het potje niet wordt overschreven met wie er aan zet is
                //Laat zien wie er aan zet is
                switch (kleur)
                {
                    case 1: Status.Text = "Blauw is aan zet"; break;
                    case 2: Status.Text = "Rood is aan zet"; break;
                }
        }

        public void legaal() //Gaat elk vakje op het bord af en checkt of er een legale zet gedaan kan worden.
        {
            for (int t = 0; t < breedte; t++)
            {
                for (int u = 0; u < lengte; u++)
                {
                    leg[t, u] = richtingen(t, u);//"Legale vakjes" worden opgeslagen in een 2-dimensionale array.
                    if (leg[t, u]) mogelijkeZetten++;//Voor elke mogelijke zet wordt het aantal mogelijke zetten geincrement 
                }
            }
        }

        // Methode die bepaalt welke richtingen ingeklikt kunnen worden
        bool richtingen(int x, int y)
        {
            if (kl[x, y] > 0) return false; // Checkt of het geklikte vakje niet aan rand van het bord zit en of het een andere steen kan 

            this.arrayOpslag(x, y);
            
            for (int t = 0; t < 8; t++) //checkt of er minstens 1 mogelijke richting is voor het vakje
            { if (insarray[t]) return true; }
            return false;
        }

        public void arrayOpslag(int x, int y) //Voert insluiten() uit in de 8 nodige richtingen en slaat deze waarden op in een bool array. 
        {
            this.insarray[0] = insluiten(x, y, 0, -1);//Naar boven,
            this.insarray[1] = insluiten(x, y, -1, -1);//linksboven,
            this.insarray[2] = insluiten(x, y, -1, 0);//links,
            this.insarray[3] = insluiten(x, y, -1, 1);//linksonder,
            this.insarray[4] = insluiten(x, y, 0, 1);//onder,
            this.insarray[5] = insluiten(x, y, 1, 1);//rechtsonder,
            this.insarray[6] = insluiten(x, y, 1, 0);//rechts
            this.insarray[7] = insluiten(x, y, 1, -1);//en rechtsboven.
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
            plaats(x, y);
            this.arrayOpslag(x, y); //wordt uitgevoerd voor dit specifieke vakje

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
            Scorebord.Invalidate();
            Speelbord.Invalidate();
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

        //Het plaatsen van een steen op een leeg vakje
        public void plaats(int x, int y)
        {
            kl[x, y] = kleur;
            switch (kleur)
            {
                case 1: aantalblauw++; break;
                case 2: aantalrood++; break;
            }
        }

        //Het vervangen van een ingesloten steen naar de kleur die aan de beurt is
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
        //Methode die de gekleurde stenen
        //en de transparante stenen voor de hulp-knop tekent (als op de hulp-knop wordt gedrukt)
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
                    if (kl[t, u] > 0) gr.FillEllipse(br, t * 33 + steenoffset, u * 33 + steenoffset, diameter, diameter);
                }

            }
        }

        //Tekent het scorebord
        private void score(object sender, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;
            Pen pen = Pens.Blue;
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gr.FillEllipse(Brushes.Blue, 0, 0, diameter, diameter);
            gr.FillEllipse(Brushes.Red, 0, 30, diameter, diameter);
            gr.DrawString("Blauw heeft " + aantalblauw + " stenen", ScoreFont, Brushes.Blue, 32, 2);
            gr.DrawString("Rood heeft " + aantalrood + " stenen", ScoreFont, Brushes.Red, 32, 32);
        }

        //Update de label die aangeeft wie aan de beurt is naar wie heeft gewonnen of dat het gelijkspel als het spel is geëindigd
        private void EindeSpel()
        {
            geeindigd = true;
            if (aantalblauw > aantalrood) Status.Text = "Blauw heeft gewonnen!!!";
            else
            if (aantalrood > aantalblauw) Status.Text = "Rood heeft gewonnen!!!";
            else Status.Text = "Remise.";
        }

        //Methode die het spel opnieuw start als geklikt is op de "Nieuw Spel"-knop
        private void NieuwSpel(object sender, EventArgs ea)
        {
            defaultWaarden(); //De spelsituatie wordt weer hetzelfde als aan het begin van het spel
            Speelbord.Invalidate();
            Scorebord.Invalidate();
        }

        //Methode die hulpNodig als het ware omschakelt en het speelbord opnieuw tekent, zodat de mogelijke zetten verschijnen of verdwijnen.
        private void Hulp(object sender, EventArgs ea)
        {
            if (!Helplock.Checked)
            {
                hulpNodig = !hulpNodig;
                Speelbord.Invalidate();
            }
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