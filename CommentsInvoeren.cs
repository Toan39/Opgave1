using System.Windows.Forms;
using System.Drawing;
using System;
using System.Globalization;

class OpgaveForm : Form
{

    TextBox PaneelX;
    TextBox PaneelY;
    TextBox PaneelS;
    TextBox PaneelM;
    ListBox LijstVB;
    Button knopOK;
    Panel Plaatje;

    public OpgaveForm()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
        //Control//
        this.Size = new Size(800, 500);
        this.BackColor = Color.GhostWhite;

        //PaneelX//
        PaneelX = new TextBox();
        PaneelX.Text = "0";
        PaneelX.Size = new Size(100, 20);
        PaneelX.Location = new Point(500, 10);
        this.Controls.Add(PaneelX);

        //PaneelY//
        PaneelY = new TextBox();
        PaneelY.Text = "0";
        PaneelY.Size = new Size(100, 20);
        PaneelY.Location = new Point(650, 10);
        this.Controls.Add(PaneelY);

        //Schaal//
        PaneelS = new TextBox();
        PaneelS.Text = "0.01";
        PaneelS.Size = new Size(100, 20);
        PaneelS.Location = new Point(500, 70);
        this.Controls.Add(PaneelS);

        //Max//
        PaneelM = new TextBox();
        PaneelM.Text = "100";
        PaneelM.Size = new Size(100, 20);
        PaneelM.Location = new Point(650, 70);
        this.Controls.Add(PaneelM);

        //Label for Mid X coordinate//
        Label label1;
        label1 = new Label();
        label1.Text = "MidX:";
        label1.Location = new Point(450, 10);
        this.Controls.Add(label1);

        //Label for Mid Y coordinate//
        Label label2;
        label2 = new Label();
        label2.Text = "MidY:";
        label2.Location = new Point(600, 10);
        this.Controls.Add(label2);

        //Label for Max value//
        Label label3;
        label3 = new Label();
        label3.Text = "Max:";
        label3.Location = new Point(600, 70);
        this.Controls.Add(label3);

        //Label for Scale value//
        Label label4;
        label4 = new Label();
        label4.Text = "Schaal:";
        label4.Location = new Point(450, 70);
        this.Controls.Add(label4);

        //Label for voorbeeldenmenu//
        Label label5;
        label5 = new Label();
        label5.Text = "Voorbeelden:";
        label5.Location = new Point(550, 200);
        this.Controls.Add(label5);

        //Listbox//
        LijstVB = new ListBox();
        LijstVB.Location = new Point(650, 200);
        LijstVB.Size = new Size(100, 60);
        LijstVB.BackColor = Color.White;
        LijstVB.ForeColor = Color.Black;
        LijstVB.Items.Add("Basis");
        LijstVB.Items.Add("Regenboog");
        LijstVB.Items.Add("Blauwe plek");
        LijstVB.Items.Add("Gras");
        this.Controls.Add(LijstVB);
        LijstVB.SelectedIndexChanged += new EventHandler(VoorbeeldenMenu);


        //Button okay//
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


    void VoorbeeldenMenu(object sender, EventArgs e)
    {

        if (LijstVB.SelectedIndex == 0)
        {
            PaneelX.Text = "0";
            PaneelY.Text = "0";
            PaneelS.Text = "0.01";
            PaneelM.Text = "100";
            Plaatje.Invalidate();
        }

        if (LijstVB.SelectedIndex == 1)
        {
            PaneelX.Text = "-0.108625";
            PaneelY.Text = "0.9014428";
            PaneelS.Text = "0.000000038147";
            PaneelM.Text = "400";
            Plaatje.Invalidate();
        }

        if (LijstVB.SelectedIndex == 2)
        {
            PaneelX.Text = "-1.0079296875";
            PaneelY.Text = "0.31112109375";
            PaneelS.Text = "0.00001953125";
            PaneelM.Text = "3000";
            Plaatje.Invalidate();
        }

        if (LijstVB.SelectedIndex == 3)
        {
            PaneelX.Text = "-0.15781255";
            PaneelY.Text = "1.0328125";
            PaneelS.Text = " 0.00015625";
            PaneelM.Text = "200";
            Plaatje.Invalidate();
        }



    }

    void Muis(object sender, MouseEventArgs mea)
    {
        LijstVB.ClearSelected();

        double MiddenX = Convert.ToDouble(PaneelX.Text), MiddenY = Convert.ToDouble(PaneelY.Text), Schaal = Convert.ToDouble(PaneelS.Text);

        MiddenX = MiddenX + Schaal * (mea.X - 200);
        MiddenY = MiddenY + Schaal * (200 - mea.Y);

        PaneelX.Text = Convert.ToString(MiddenX);
        PaneelY.Text = Convert.ToString(MiddenY);
        PaneelS.Text = Convert.ToString(Schaal * 0.5);


        Plaatje.Invalidate();
    }


    void knopOK_Click(object obj, EventArgs e)
    {
        LijstVB.ClearSelected();
        Plaatje.Invalidate();

    }

    void Teken(object sender, PaintEventArgs pea)
    {
        Graphics gr = pea.Graphics;
        int mg, Max = Int32.Parse(PaneelM.Text);
        SolidBrush br = new SolidBrush(Color.Black);
        double MiddenX = Convert.ToDouble(PaneelX.Text), MiddenY = Convert.ToDouble(PaneelY.Text), Schaal = Convert.ToDouble(PaneelS.Text);


        for (int n = 1; n <= 400; n++)
        {
            for (int m = 1; m <= 400; m++)
            {
                double X = MiddenX + Schaal * (m - 200);
                double Y = MiddenY + Schaal * (200 - n);
                mg = Mandelbrot(X, Y, Max);
                if (mg == Max) { br.Color = Color.Black; }
                else { br.Color = Color.FromArgb(32 * (mg % 8), 16 * (mg % 16), 8 * (mg % 32)); }
                gr.FillRectangle(br, m, n, 1, 1);
            }
        }
    }

    int Mandelbrot(double X, double Y, int Max)
    {

        double a = 0.0;
        double b = 0.0;
        int n = 0;
        double nieuweA;
        double nieuweB;

        while ((Math.Pow(a, 2) + Math.Pow(b, 2) <= 4.0) && (n < Max))
        {
            nieuweA = Math.Pow(a, 2) - Math.Pow(b, 2) + X;
            nieuweB = 2.0 * a * b + Y;
            a = nieuweA;
            b = nieuweB;
            n = n + 1;
        }
        return n;
    }
}

class HalloWin3
{
    static void Main()
    {
        OpgaveForm scherm;
        scherm = new OpgaveForm();
        Application.Run(scherm);
    }
}

