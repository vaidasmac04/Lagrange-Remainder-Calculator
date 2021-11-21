using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainderCalculator
{
    class Program
    {
        private static Interval _interval = new(-0.5m, 0.5m);
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
                Console.WriteLine($"Current selection: interval - {_interval}, x = {_x}");
                Console.WriteLine("***********************************************************");

                Console.WriteLine("1. Change interval");
                Console.WriteLine("2. Change x");
                Console.WriteLine("3. Calculate remainder");
                Console.WriteLine("4. Close program");

            HandleMainMenuLabel:
                try
                {
                    Console.Write("Your input (actions): ");
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Choose interval: ");
                            Console.WriteLine("1. [-0,5; 0,5]");
                            Console.WriteLine("2. [-0,1; 0,1]");
                            Console.WriteLine("3. [-0,001; 0,001]");

                        IntervalSelectionLabel:
                            Console.Write("Your input (interval): ");
                            try
                            {
                                int intervalChoice = int.Parse(Console.ReadLine());

                                switch (intervalChoice)
                                {
                                    case 1:
                                        _interval = new Interval(-0.5m, 0.5m);
                                        break;
                                    case 2:
                                        _interval = new Interval(-0.1m, 0.1m);
                                        break;
                                    case 3:
                                        _interval = new Interval(-0.001m, 0.001m);
                                        break;
                                    default:
                                        PrintCalculatorMessage("interval selection failed. Try again.");
                                        goto IntervalSelectionLabel;
                                }

                                PrintCalculatorMessage("interval selected successfuly");
                            }
                            catch (FormatException)
                            {
                                PrintCalculatorMessage("interval selection failed. Try again.");
                                goto IntervalSelectionLabel;
                            }
                            break;
                        case 2:
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
                        case 3:
                            var lagrangeRemainder = lagrangeRemainderCalculator.CalculateRemainder(
                               function,
                               _interval,
                               _x);

                            var trueRemainder = lagrangeRemainderCalculator.CalculateTrueRemainder(
                                function,
                                taylor,
                                _interval,
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
