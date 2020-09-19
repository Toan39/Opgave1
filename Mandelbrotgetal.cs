using System;

namespace MandelbrotConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            int mg(double X, double Y, int max)
            {
                double a = 0.0;
                double b = 0.0;
                int n = 0;
                double newA;
                double newB;

                while ((a * a + b * b <= 4.0) && (n < max))
                {
                    newA = a * a - b * b + X;
                    newB = 2.0 * a * b + Y;
                    a = newA;
                    b = newB;
                    n = n + 1;
                }


                return n;
            }
            double x, y;
            int m;
            Console.Write("X = "); x = Convert.ToDouble(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            Console.Write("Y = "); y = Convert.ToDouble(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            Console.Write("Max = "); m = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(mg(x, y, m));
            Console.ReadLine();
        }
    }
}
