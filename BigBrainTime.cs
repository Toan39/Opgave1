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
        //Button button1 = new Button();
        button1 = new Button();
        button1.Text = "OK";
        button1.Size = new Size(50, 20);
        button1.Location = new Point(600, 120);
        this.Controls.Add(button1);
        button1.Click += new EventHandler(button1_Click);
        //button1.Click += this.button1_Click;
        //button1.PerformClick();

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
        /*this.Paint += new PaintEventHandler(start);*/// dit werkt niet iets moet geclicked worden en paint krijg je een repaint en kan niet samen gebruikt worden.
        this.BackColor = Color.GhostWhite;          //pas op als mouse te snel in control zit loopt die vast!
        //start(null, null);
    }


    //void start(object sender, PaintEventArgs pea)
    //{
    //    this.button1_Click(sender, pea);    //deze hele methode wordt alleen uitgevoerd voor console functies.
    //}

    //void call()
    //{
    //       if (String.IsNullOrEmpty(PanelX.Text))
    //        {
    //            this.reken(0, 0, 0.01, 100);             //deze void roept dus ook de reken methode uit, maar moet het maar 1x doen!
    //}   
    //}

    //private void self_click()
    //{
    //   button1.PerformClick();          // dit werkt alleen als button 1 eerder is gezet in de code.
    //}

    void start(object sender, EventArgs e)
    {
        listBox1.SetSelected(0, true);    //Werkt niet helemaal goed, want laadtijd wordt hoger en de figuur wordt niet ge-paint 
        //if (listBox1.Items.Count > 0)
        //{
        //   listBox1.SetSelected(0, true);
        //}
        //button1.PerformClick();
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
        //Mouse values  determined//
        double s2 =0.5* Convert.ToDouble(PanelS.Text);   // punt verandert in comma, net zoals het probleem dat input een comma moet hebben.
        int m2 = Int32.Parse(PanelM.Text);
        double x2 = mea.Location.X;
        double y2 = mea.Location.Y;   //Midden is 200, 200, inplaats van 0, Dus om de goede coordinaten te krijg moet beide respectief naar coordinate systeem.
                                      //reverse mandelbrot lost dat op

        //double x2-s2/((x2 - 200) = MiddenX   ;
        //double y2-s2/(200 - y2) = MiddenY   ;


        double vx = x2/s2;   // x  en y coordinate delen door schaal, dit moet een waarde zijn dat het punt in het midden zet. bv. voor 200 krijg je x,y=0,0, dus de waarde zal 1-400 zijn
        double vy= y2/s2;
                                 //Mandel
                 
        double v1 = x2 / (vx-200 );   //dit zoomt alleen in, formule moet omgeschreven worden.
        double v3 = y2 / (200-vy);
        double xMouseMid = v1-s2;
        double yMouseMid = v3-s2;


        //the new values in the textboxes//
        string MouseX = xMouseMid.ToString();
        string MouseY = yMouseMid.ToString();
        string MouseS = s2.ToString();

        PanelX.Text = MouseX;
        PanelY.Text = MouseY;
        PanelS.Text = MouseS;

        //zooms in//
        Plaatje.Invalidate();
    }


    void button1_Click(object obj, EventArgs e)
    {
        //if (String.IsNullOrEmpty(PanelX.Text))
        //   {
        //       this.reken( 0, 0, 0.01, 100);          //Als er niks ingevuld werd in PanelX voert het reken-method uit, maar knop moet wel ingedrukt worden.
        //   }
        //else
        //    {
        listBox1.ClearSelected();
        Plaatje.Invalidate();
        //}

    }

    void reken(object sender, PaintEventArgs pea)
    {
        Graphics gr = pea.Graphics;
        int n = 1, m = 1;
        int mg;
        Pen pen = new Pen(Color.Black, 1);
        double MiddenX = Convert.ToDouble(PanelX.Text), MiddenY = Convert.ToDouble(PanelY.Text), Schaal = Convert.ToDouble(PanelS.Text);

        while (n <= 400)
        {
            while (m <= 400)
            {
                double X = MiddenX + Schaal * (m - 200);
                double Y = MiddenY + Schaal * (200 - n);
                mg = Mandelbrot(X, Y, Int32.Parse(PanelM.Text));
                pen.Color = Color.FromArgb(32 * (mg % 8), 16 * (mg % 16), 8 * (mg % 32));
                gr.DrawRectangle(pen, m, n, 1, 1);


                m++;
            }
            m = 1;
            n++;
        }
    }

    int Mandelbrot(double X, double Y, int Max)
    {

        double a = 0.0;
        double b = 0.0;
        int n = 0;
        double newA;
        double newB;

        while ((Math.Pow(a, 2) + Math.Pow(b, 2) <= 4.0) && (n < Max + 1))
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

