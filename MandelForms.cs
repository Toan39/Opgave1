using System.Windows.Forms;
using System.Drawing;
using System;


class OpgaveForm : Form
{
    public OpgaveForm()
    {

        this.BackColor = Color.White;
        this.Size = new Size(400, 400);
        // this.Text = "TextBox Example";
        // this.ResumeLayout(false);
        // this.PerformLayout();
       
        
        // TextBox tekst;
        // tekst = new TextBox();

        // // 
        // // textBox1
        // // 
        // tekst.AcceptsReturn = true;
        // tekst.AcceptsTab = true;
        // tekst.Dock = DockStyle.Fill;
        // tekst.Multiline = true;
        // tekst.ScrollBars = ScrollBars.Vertical;

        // this.Controls.Add(tekst);


    }
    //voert de methode uit met bepaalde waarde//
    void tekenScherm(object obj, PaintEventArgs pea)
    {
        // double X, Y;
        // Console.Write("X = "); X = Convert.ToDouble(Console.ReadLine());
        // Console.Write("Y = "); Y = Convert.ToDouble(Console.ReadLine());
        this.Mandelbrot(0.5,0.8);
        
    }

    void Mandelbrot( double X, double Y)
    {
        
            double a = 0.0;
            double b = 0.0;
            int n = 0;
            double newA;
            double newB;

            while ((a * a + b * b <= 4.0) && (n < 1000000))
            {
                newA = a * a - b * b + X;
                newB = 2.0 * a * b + Y;
                a = newA;
                b = newB;
                n = n + 1;
            }
    }
}


class HalloWin3
{
    static void Main()
    {
        OpgaveForm scherm;
        scherm = new OpgaveForm();
        Application.Run(scherm);
        //Application.EnableVisualStyles();
        //Application.Run(new OpgaveForm());
    }
}