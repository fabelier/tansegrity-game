using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            geneticAlgo.gradientDescent GD = new geneticAlgo.gradientDescent(100,2);
            GD.update();
        }
    }
}
