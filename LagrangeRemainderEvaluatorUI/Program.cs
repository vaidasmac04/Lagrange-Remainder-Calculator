using LagrangeRemainder;
using LagrangeRemainder.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainderEvaluatorUI
{
    class Program
    {
        private static decimal _x = 0.4m;

        static int Main()
        {
            Expr function = null;
            Expr taylor = null;

            try
            {
                var (Function, Taylor) = ParseFunctions("functions.json");
                function = Function;
                taylor = Taylor;
            }
            catch (IOException ioException)
            {
                Console.WriteLine(ioException.Message);
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }

            if (function != null && taylor != null)
            {
                ILagrangeRemainderEvaluator lagrangeRemainderEvaluator =
                    new LagrangeRemainderCalculator();

                var evaluationIntervals = new List<EvaluationInterval>
                {
                    new EvaluationInterval(-0.5m, 0.5m, -0.5m),
                    new EvaluationInterval(-0.1m, 0.1m, -0.1m),
                    new EvaluationInterval(-0.001m, 0.001m, -0.001m)
                };

                var evaluationResults = lagrangeRemainderEvaluator
                    .EvaluateRemainder(function, evaluationIntervals);

                foreach(var evaluationResult in evaluationResults)
                {
                    Console.WriteLine($"Intervalas: {evaluationResult.Interval}, " +
                        $"Lagranžo liekamasis narys: {evaluationResult.Remainder}");
                }
            }

            Console.ReadLine();

            return 0;
        }

        private static (Expr Function, Expr Taylor) ParseFunctions(string filePath)
        {
            var functionsAsJson = JObject.Parse(File.ReadAllText(filePath));

            var function = functionsAsJson["function"].ToString();
            var taylor = functionsAsJson["taylor"].ToString();

            if (string.IsNullOrEmpty(function))
            {
                throw new ArgumentException("Klaida. Funkcija nepateikta.");
            }

            if (string.IsNullOrEmpty(taylor))
            {
                throw new ArgumentException("Klaida. Teiloro eilutė nepateikta.");
            }

            return (Expr.Parse(function), Expr.Parse(taylor));
        }
    }
}
