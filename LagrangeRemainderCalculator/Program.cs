using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainderCalculator
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

            if (function != null)
            {
                ILagrangeRemainderCalculator lagrangeRemainderCalculator =
                    new LagrangeRemainderCalculator();

            ShowMainMenuLabel:
                Console.WriteLine("***********************************************************");
                Console.WriteLine($"Current selection: x = {_x}");
                Console.WriteLine("***********************************************************");
                Console.WriteLine();
                Console.WriteLine("Actions:");
                Console.WriteLine("1. Change x");
                Console.WriteLine("2. Calculate remainder");
                Console.WriteLine("3. Close program");

            HandleMainMenuLabel:
                try
                {
                    Console.Write("Your input (actions): ");
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            Console.Write("Your input (x): ");

                            HandleXChoice:
                                try
                                {
                                    _x = decimal.Parse(Console.ReadLine());
                                }
                                catch (FormatException)
                                {
                                    PrintCalculatorMessage("x value selection failed. Try again.");
                                    goto HandleXChoice;
                                }

                            PrintCalculatorMessage("x value selected successfuly");

                            break;
                        case 2:
                            var lagrangeRemainder = lagrangeRemainderCalculator.CalculateRemainder(
                               function,
                               _x);

                            var trueRemainder = lagrangeRemainderCalculator.CalculateTrueRemainder(
                                function,
                                taylor,
                                _x);

                            PrintCalculatorMessage($"Lagrange remainder calculated successfuly: {lagrangeRemainder}");
                            PrintCalculatorMessage($"true remainder calculated successfuly: {trueRemainder}");
                            PrintCalculatorMessage($"error: {Math.Abs(trueRemainder-lagrangeRemainder)}");
                            break;
                        case 4:
                            return 0;
                        default:
                            PrintCalculatorMessage("action selection failed. Try again.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    PrintCalculatorMessage("action selection failed. Try again.");
                    goto HandleMainMenuLabel;
                }
                catch (ArgumentException argumentException) 
                {
                    PrintCalculatorMessage(argumentException.Message);
                    Console.WriteLine();
                    goto ShowMainMenuLabel;
                }

                Console.WriteLine();
                goto ShowMainMenuLabel;
            }

            return 0;
        }

        private static (Expr Function, Expr Taylor) ParseFunctions(string filePath)
        {
            var functionsAsJson = JObject.Parse(File.ReadAllText(filePath));

            var function = functionsAsJson["function"].ToString();
            var taylor = functionsAsJson["taylor"].ToString();

            if (string.IsNullOrEmpty(function))
            {
                throw new ArgumentException("Function is not provided");
            }

            if (string.IsNullOrEmpty(taylor))
            {
                throw new ArgumentException("Taylor expression is not provided");
            }

            return (Expr.Parse(function), Expr.Parse(taylor));
        }

        private static void PrintCalculatorMessage(string message)
        {
            Console.WriteLine($"--> Calculator: {message}");
        }
    }
}
