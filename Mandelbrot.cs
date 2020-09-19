using System.Windows.Forms;
using System.Drawing;
using System;
using System.Runtime.CompilerServices;

class OpgaveForm : Form
{
    public OpgaveForm()
    {



        this.BackColor = Color.Black;
        this.Size = new Size(800, 800);
        this.Paint += this.tekenScherm;
        //this.Text = "TextBox Example";
        //this.ResumeLayout(false);
        //this.PerformLayout();


        //TextBox tekst;
        //tekst = new TextBox();

        //
        // textBox1
        //
        //tekst.AcceptsReturn = true;
        //tekst.AcceptsTab = true;
        //tekst.Dock = DockStyle.Fill;
        //tekst.Multiline = true;
        //tekst.ScrollBars = ScrollBars.Vertical;

        //this.Controls.Add(tekst);//


    }
    //voert de methode uit met bepaalde waarde//
    void tekenScherm(object obj, PaintEventArgs pea)
    {
        //gekke shit hier toegevoegd//
        double MiddenX = 0, MiddenY = 0, Schaal = 0.01;
        int Max = 100;
        int n = 1, m = 1;
        int mg;
        Pen pen = new Pen(Color.Black, 1);
        Graphics gr = pea.Graphics;

        while (n <= 400)
        {
            while (m <= 400)
            {
                double X = MiddenX + Schaal * (m - 200);
                double Y = MiddenY + Schaal * (200 - n);
                mg = this.Mandelbrot(X, Y, Max);
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
        //Application.EnableVisualStyles();
        //Application.Run(new OpgaveForm());
    }
}