using System.Windows.Forms;
using System.Drawing;
using System;
using System.Globalization;

class OpgaveForm : Form
{


    Panel Plaatje;
    TextBox PanelX;
    TextBox PanelY;
    TextBox PanelS;
    TextBox PanelM;
    ListBox listBox1;
    Button button1;
    public OpgaveForm()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
        //PanelX//
        PanelX = new TextBox();
        PanelX.Text = "0";
        PanelX.Size = new Size(100, 20);
        PanelX.Location = new Point(500, 10);
        this.Controls.Add(PanelX);

        //PanelY//
        PanelY = new TextBox();
        PanelY.Text = "0";
        PanelY.Size = new Size(100, 20);
        PanelY.Location = new Point(650, 10);
        this.Controls.Add(PanelY);

        //Schaal//
        PanelS = new TextBox();
        PanelS.Text = "0.01";
        PanelS.Size = new Size(100, 20);
        PanelS.Location = new Point(500, 70);
        this.Controls.Add(PanelS);

        //Max//
        PanelM = new TextBox();
        PanelM.Text = "100";
        PanelM.Size = new Size(100, 20);
        PanelM.Location = new Point(650, 70);
        this.Controls.Add(PanelM);

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
        listBox1 = new ListBox();
        listBox1.Location = new Point(650, 200);
        listBox1.Size = new Size(100, 60);
        listBox1.BackColor = Color.White;
        listBox1.ForeColor = Color.Black;
        listBox1.Items.Add("Basis");
        listBox1.Items.Add("Regenboog");
        listBox1.Items.Add("Blauwe plek");
        listBox1.Items.Add("Gras");
        this.Controls.Add(listBox1);
        listBox1.SelectedIndexChanged += new EventHandler(SelectedIndex);


        //Button okay//
        button1 = new Button();
        button1.Text = "OK";
        button1.Size = new Size(50, 20);
        button1.Location = new Point(600, 120);
        this.Controls.Add(button1);
        button1.Click += new EventHandler(button1_Click);

        //Plaatje van het Mandelbrot//
        Plaatje = new Panel();
        Plaatje.Location = new Point(25, 25);
        Plaatje.Size = new Size(400, 400);
        Plaatje.Paint += new PaintEventHandler(reken);
        this.Controls.Add(Plaatje);

        //Mouse//
        Plaatje.MouseClick += new MouseEventHandler(mouse);

        //Control//
        this.Size = new Size(800, 500);
        this.BackColor = Color.GhostWhite;          //pas op als mouse te snel in control zit loopt die vast!
    }



    void start(object sender, EventArgs e)
    {
        listBox1.SetSelected(0, true);    //Werkt niet helemaal goed, want laadtijd wordt hoger en de figuur wordt niet ge-paint 

    }

    void SelectedIndex(object sender, EventArgs e)
    {

        if (listBox1.SelectedIndex == 0)
        {
            PanelX.Text = "0";
            PanelY.Text = "0";
            PanelS.Text = "0.01";
            PanelM.Text = "100";
            Plaatje.Invalidate();
        }

        if (listBox1.SelectedIndex == 1)
        {
            PanelX.Text = "-0.108625";
            PanelY.Text = "0.9014428";
            PanelS.Text = "0.000000038147";
            PanelM.Text = "400";
            Plaatje.Invalidate();
        }

        if (listBox1.SelectedIndex == 2)
        {
            PanelX.Text = "-1.0079296875";
            PanelY.Text = "0.31112109375";
            PanelS.Text = "0.00001953125";
            PanelM.Text = "3000";
            Plaatje.Invalidate();
        }

        if (listBox1.SelectedIndex == 3)
        {
            PanelX.Text = "-0.15781255";
            PanelY.Text = "1.0328125";
            PanelS.Text = " 0.00015625";
            PanelM.Text = "200";
            Plaatje.Invalidate();
        }



    }

    void mouse(object sender, MouseEventArgs mea)
    {
        listBox1.ClearSelected();

        double MiddenX = Convert.ToDouble(PanelX.Text), MiddenY = Convert.ToDouble(PanelY.Text), Schaal = Convert.ToDouble(PanelS.Text);

        MiddenX = MiddenX + Schaal * (mea.X - 200);
        MiddenY = MiddenY + Schaal * (200 - mea.Y);

        PanelX.Text = Convert.ToString(MiddenX);
        PanelY.Text = Convert.ToString(MiddenY);
        PanelS.Text = Convert.ToString(Schaal * 0.5);


        Plaatje.Invalidate();
    }


    void button1_Click(object obj, EventArgs e)
    {
        listBox1.ClearSelected();
        Plaatje.Invalidate();

    }

    void reken(object sender, PaintEventArgs pea)
    {
        Graphics gr = pea.Graphics;
        int mg, Max = Int32.Parse(PanelM.Text);
        SolidBrush br = new SolidBrush(Color.Black);
        double MiddenX = Convert.ToDouble(PanelX.Text), MiddenY = Convert.ToDouble(PanelY.Text), Schaal = Convert.ToDouble(PanelS.Text);
        

        for (int n = 1; n <= 400; n++)
        {
            for (int m = 1;  m <= 400; m++)
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
        double newA;
        double newB;

        while ((Math.Pow(a, 2) + Math.Pow(b, 2) <= 4.0) && (n < Max))
        {
            newA = Math.Pow(a, 2) - Math.Pow(b, 2) + X;
            newB = 2.0 * a * b + Y;
            a = newA;
            b = newB;
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

