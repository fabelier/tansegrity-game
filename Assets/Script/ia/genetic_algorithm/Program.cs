using System;



namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            geneticAlgo.gradientDescent GD;
            GD = new geneticAlgo.gradientDescent(1000, 0, 100, 10);
            GD.update();
            Console.WriteLine("eval value obtained : {0}", GD.getBestIndiv().getEvalValue());
        }
    }
}
