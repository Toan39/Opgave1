using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotConsole
{
    class Program
    {


        static void Main(string[] args)
        {
            int mg(double X, double Y)
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


                return n;
            }
            double x, y;
            Console.Write("X = "); x = Convert.ToDouble(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            Console.Write("Y = "); y = Convert.ToDouble(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            Console.WriteLine(mg(x, y));
            Console.ReadLine();
        }
    }
}