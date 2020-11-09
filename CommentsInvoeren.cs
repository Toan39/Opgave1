using System.Windows.Forms;
using System.Drawing;
using System;
using System.Globalization;

class OpgaveForm : Form
{
    //Declaratie van alle Interface-onderdelen die buiten OpgavenForm() aangeroepen moeten kunnen worden //hoi12345
    TextBox PaneelX;
    TextBox PaneelY;
    TextBox PaneelS;
    TextBox PaneelM;
    ListBox LijstVB;
    ListBox LijstKL;
    Button knopOK;
    Panel Plaatje;

    //integer die belangrijk is voor de kleurkeuze, deze wordt door een listbox aangepast en in een methode gebruikt om de kleur te kiezen//
    int kleur;

    public OpgaveForm()
    {
        //Zorgt ervoor dat een ingevoerde "." wordt geregristreed als een komma, hierdoor gebruiken de invoer en de code allebei een "."
        CultureInfo.CurrentCulture = new CultureInfo("en-US", false);

        //Eigenschappen van de Window zelf
        //Control//
        this.Size = new Size(800, 500);
        this.BackColor = Color.GhostWhite;

        //Maakt voor alle in te voegen variabelen een soort invoerpaneel aan in de vorm van TextBoxes
        ////
        //PaneelX//
        PaneelX = new TextBox();
        PaneelX.Text = "0";
        PaneelX.Size = new Size(100, 20);
        PaneelX.Location = new Point(500, 10);
        this.Controls.Add(PaneelX);
        ////
        //PaneelY//
        PaneelY = new TextBox();
        PaneelY.Text = "0";
        PaneelY.Size = new Size(100, 20);
        PaneelY.Location = new Point(650, 10);
        this.Controls.Add(PaneelY);
        ////
        //PaneelSchaal//
        PaneelS = new TextBox();
        PaneelS.Text = "0.01";
        PaneelS.Size = new Size(100, 20);
        PaneelS.Location = new Point(500, 70);
        this.Controls.Add(PaneelS);
        ////
        //PaneelMax//
        PaneelM = new TextBox();
        PaneelM.Text = "100";
        PaneelM.Size = new Size(100, 20);
        PaneelM.Location = new Point(650, 70);
        this.Controls.Add(PaneelM);

        //Om duidelijk te maken waar de TextBoxes voor staan, worden er de volgende Labels aan toegevoegd:
        ////
        //Label voor MiddenX//
        Label label1;
        label1 = new Label();
        label1.Text = "Midden X:";
        label1.Location = new Point(450, 10);
        this.Controls.Add(label1);
        ////
        //Label voor MiddenY//
        Label label2;
        label2 = new Label();
        label2.Text = "Midden Y:";
        label2.Location = new Point(600, 10);
        this.Controls.Add(label2);
        ////
        //Label voor Max//
        Label label3;
        label3 = new Label();
        label3.Text = "Max:";
        label3.Location = new Point(600, 70);
        this.Controls.Add(label3);
        ////
        //Label voor de Schaal//
        Label label4;
        label4 = new Label();
        label4.Text = "Schaal:";
        label4.Location = new Point(450, 70);
        this.Controls.Add(label4);
        ////
        //Label voor de lijst met voorbeelden//
        Label label5;
        label5 = new Label();
        label5.Text = "Voorbeelden:";
        label5.Location = new Point(550, 200);
        this.Controls.Add(label5);
        ////
        //Label voor de KleurenLijst//
        Label label6;
        label6 = new Label();
        label6.Text = "Kleurinstellingen:";
        label6.Location = new Point(550, 300);
        this.Controls.Add(label6);

        //Lijst van de gegeven voorbeelden//
        LijstVB = new ListBox();
        LijstVB.Location = new Point(650, 200);
        LijstVB.Size = new Size(100, 60);
        LijstVB.BackColor = Color.White;
        LijstVB.ForeColor = Color.Black;
        LijstVB.Items.Add("Basis");
        LijstVB.Items.Add("Takken");
        LijstVB.Items.Add("Kleine Mandelbrot");
        LijstVB.Items.Add("Scheuren");
        LijstVB.SelectedIndexChanged += new EventHandler(VoorbeeldenMenu);
        this.Controls.Add(LijstVB);

        //Lijst van kleurinstellingen//
        LijstKL = new ListBox();
        LijstKL.Location = new Point(650, 300);
        LijstKL.Size = new Size(100, 45);
        LijstKL.Items.Add("Spectrum");
        LijstKL.Items.Add("Regenboog");
        LijstKL.Items.Add("Organisch");
        LijstKL.SelectedIndexChanged += new EventHandler(KleurenMenu);
        this.Controls.Add(LijstKL);

        
        //OK-knop//
        knopOK = new Button();
        knopOK.Text = "OK";
        knopOK.Size = new Size(50, 20);
        knopOK.Location = new Point(600, 120);
        this.Controls.Add(knopOK);
        knopOK.Click += new EventHandler(knopOK_Click);

        //Plaatje van het Mandelbrot//
        Plaatje = new Panel();
        Plaatje.Location = new Point(25, 25);
        Plaatje.Size = new Size(400, 400);
        Plaatje.Paint += new PaintEventHandler(Teken);
        this.Controls.Add(Plaatje);

        //Mouse//
        Plaatje.MouseClick += new MouseEventHandler(Muis);

    }

    //Maakt een methode aan voor de ListBox "LijstVB"
    void VoorbeeldenMenu(object sender, EventArgs e)
    {

        //Vult de tekstboxes in met de waardes voor de voorbeeld "Basis" als deze optie wordt geselecteerd.
        if (LijstVB.SelectedIndex == 0)  
            PaneelX.Text = "0";
        PaneelY.Text = "0";
        PaneelS.Text = "0.01";
        PaneelM.Text = "100";
        Plaatje.Invalidate(); // Tekent het bijbehorende plaatje als de if-statement is voldaan.
        {
        }

        //Vult de tekstboxes in met de waardes voor de voorbeeld "Takken" als deze optie wordt geselecteerd.
        if (LijstVB.SelectedIndex == 1) 
        {
            PaneelX.Text = "-0.108625";
            PaneelY.Text = "0.9014428";
            PaneelS.Text = "0.000000038147";
            PaneelM.Text = "400";
            Plaatje.Invalidate(); // Tekent het bijbehorende plaatje als de if-statement is voldaan.
        }

        //Vult de tekstboxes in met de waardes voor de voorbeeld "Kleine Mandelbrot" als deze optie wordt geselecteerd.
        if (LijstVB.SelectedIndex == 2) 
        {
            PaneelX.Text = "-1.0079296875";
            PaneelY.Text = "0.31112109375";
            PaneelS.Text = "0.00001953125";
            PaneelM.Text = "3000";
            Plaatje.Invalidate(); // Tekent het bijbehorende plaatje als de if-statement is voldaan.
        }

        //Vult de tekstboxes in met de waardes voor de voorbeeld "Scheuren" als deze optie wordt geselecteerd.
        if (LijstVB.SelectedIndex == 3) 
        {
            PaneelX.Text = "-0.15781255";
            PaneelY.Text = "1.0328125";
            PaneelS.Text = " 0.00015625";
            PaneelM.Text = "200";
            Plaatje.Invalidate(); // Tekent het bijbehorende plaatje als de if-statement is voldaan.
        }
    }
    //Wanneer met de muis wordt geklikt op het plaatje, dan wordt er ingezoomed op het plaatje met de aangeklikte geplek als midden.
    void Muis(object sender, MouseEventArgs mea)
    {
        LijstVB.ClearSelected(); // Deselecteert de geselecteerde in de voorbeeldenmenu

        //Converteert de momentele ingevulde X,Y en schaal waardes van een string  naar een double. 
        double MiddenX = Convert.ToDouble(PaneelX.Text), MiddenY = Convert.ToDouble(PaneelY.Text), Schaal = Convert.ToDouble(PaneelS.Text);

        //Berekent de nieuwe MiddenX en MiddenY-coordinaat uit op basis van de aangeklikte x en y-positie van de muis
        //(x,y) = (0,0) staat in een assenstelsel linksonderin, maar in en Panel/Window staat deze linksbovenin, dus wat er tussen de haakjes staat is omgedraaid bij Y.
        MiddenX = MiddenX + Schaal * (mea.X - 200);
        MiddenY = MiddenY + Schaal * (200 - mea.Y);

        //Converteert de ingevulde nieuwe MiddenX, MiddenY en Schaal coordinaat naar een string, zodat het daarna in de tekstblokken wordt gezet.
        PaneelX.Text = Convert.ToString(MiddenX);
        PaneelY.Text = Convert.ToString(MiddenY);
        PaneelS.Text = Convert.ToString(Schaal * 0.5);

        Plaatje.Invalidate(); //Tekent de nieuwe plaatje met de nieuwe schaal, MiddenX en MiddenY waardes.
    }

    //Bij het indrukken van de OK-knop worden de ingevoerde variabelen bevestigd en wordt een nieuw plaatje getekent.
    void knopOK_Click(object obj, EventArgs e)
    {
        LijstVB.ClearSelected();
        Plaatje.Invalidate();
    }

    //Methode dat het mandelbrot-plaatje tekent
    void Teken(object sender, PaintEventArgs pea)
    {
        Graphics gr = pea.Graphics;
        int mg, Max = Int32.Parse(PaneelM.Text);
        SolidBrush br = new SolidBrush(Color.Black);
        double MiddenX = Convert.ToDouble(PaneelX.Text), MiddenY = Convert.ToDouble(PaneelY.Text), Schaal = Convert.ToDouble(PaneelS.Text);

        //Tekent elke pixel met een kleur gebaseerd op de uitgerekende mandelgetal.
        for (int n = 1; n <= 400; n++)
        {
            for (int m = 1; m <= 400; m++)
            {
                double X = MiddenX + Schaal * (m - 200);
                double Y = MiddenY + Schaal * (200 - n);
                mg = Mandelbrot(X, Y, Max);       //Roept de methode aan dat de mandelbrot getal uitrekend.
                if (mg == Max) { br.Color = Color.Black; }
                else { br.Color = KleurKeuze(kleur, mg); }
                gr.FillRectangle(br, m, n, 1, 1);
            }
        }


    }

    //Dit is de methode dat het mandelbrot getal uitrekent. 
    int Mandelbrot(double X, double Y, int Max)
    {

        double a = 0.0;
        double b = 0.0;
        int n = 0;
        double nieuweA;

        //Wiskundige formule voor de mandelbrot getal.
        //NieuweA is er zodat de oude a nog wordt gebruikt in het berekenen van b
        while ((Math.Pow(a, 2) + Math.Pow(b, 2) <= 4.0) && (n < Max))
        {
            nieuweA = Math.Pow(a, 2) - Math.Pow(b, 2) + X;
            b = 2 * a * b + Y;
            a = nieuweA;
            n++;
        }
        return n;
    }

    //methode voor de listbox van kleureninstellingen
    void KleurenMenu(object sender, EventArgs ea) 
    {
        kleur = LijstKL.SelectedIndex;
        Plaatje.Invalidate();
    }

    //methode die de kleur van de 'geselecteerde' pixel in Teken() bepaalt
    Color KleurKeuze(int k, int mg) 
    {
        Color Kleur = new Color();
        switch (k)
        {
            case 0: Kleur = Color.FromArgb(32 * (mg % 8), 16 * (mg % 16), 8 * (mg % 32)); break; //gaat veel verschillende kleuren af per 32 waarden van het mandelgetal
            case 1:
                switch (mg % 7)
                {
                    case 0: Kleur = Color.Red; break;
                    case 1: Kleur = Color.Orange; break;
                    case 2: Kleur = Color.Yellow; break;
                    case 3: Kleur = Color.Green; break;
                    case 4: Kleur = Color.Blue; break;
                    case 5: Kleur = Color.Indigo; break;
                    case 6: Kleur = Color.Violet; break;
                } //'mg%' heeft een waarde tussen 0 en 6, de kleurenkeuze gaat dus alle 7 kleuren van de regenboog af
                break;
            case 2: Kleur = Color.FromArgb(127 / (mg % 8 + 1), 63 + 16 * (mg % 5), 64); break; //heeft een variabele rode en groene waarde; geeft verschillende bruine/groene tinten
        }

        return Kleur;
    }
}

class HalloWin3
{
    static void Main() //main void, wordt dus als eerste uitgevoerd, start 'OpgaveForm', wat het interactieve scherm is.
    {
        OpgaveForm scherm;
        scherm = new OpgaveForm();
        Application.Run(scherm);
    }
}