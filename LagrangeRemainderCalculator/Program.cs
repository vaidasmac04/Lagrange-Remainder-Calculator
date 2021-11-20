using MathNet.Symbolics;
using System;
using System.Configuration;
using System.IO;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainderCalculator
{
    class Program
    {
        static void Main()
        {
            Expr function = null;

            try
            {
                function = ParseFunction("function.txt");
            }
            catch (IOException ioException)
            {
                Console.WriteLine(ioException.Message);
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }

            if (function != null)
            {
                var interval = new Interval(-0.5m, 0.5m);
                ILagrangeRemainderCalculator lagrangeRemainderCalculator =
                    new LagrangeRemainderCalculator();

                var remainder = lagrangeRemainderCalculator.CalculateRemainder(
                    function, 
                    interval, 
                    0.6m,
                    10);

                Console.WriteLine($"Remainder: {remainder}");
            }
        }

        private static Expr ParseFunction(string filePath)
        {
            var functionAsString = File.ReadAllText(filePath);
            Console.WriteLine(functionAsString);

            if (string.IsNullOrEmpty(functionAsString))
            {
                throw new ArgumentException("Function is not provided");
            }

            return Expr.Parse(functionAsString);
          
        }
    }
}
