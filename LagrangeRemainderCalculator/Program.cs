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

            if (function != null && taylor != null)
            {
                ILagrangeRemainderCalculator lagrangeRemainderCalculator =
                    new LagrangeRemainderCalculator();

                ShowMainMenuLabel:
                    Console.WriteLine("***********************************************************");
                    Console.WriteLine($"Dabartinis pasirinkimas: x = {_x}");
                    Console.WriteLine("***********************************************************");
                    Console.WriteLine();
                    Console.WriteLine("Galimi veiksmai:");
                    Console.WriteLine("1. Pakeisti x");
                    Console.WriteLine("2. Apskaičiuoti liekamąjį narį (Lagranžo forma)");
                    Console.WriteLine("3. Baigti programą");

                HandleMainMenuLabel:
                    try
                    {
                        Console.Write("--> Jūsų pasirinkimas (veiksmas): ");
                        int choice = int.Parse(Console.ReadLine());

                        switch (choice)
                        {
                            case 1:
                                HandleXChoice:
                                    Console.Write("--> Jūsų pasirinkimas (x): ");

                                    try
                                    {
                                        _x = decimal.Parse(Console.ReadLine());
                                    }
                                    catch (FormatException)
                                    {
                                        PrintCalculatorMessage("x pasirinkti nepavyko. Bandykite dar kartą.");
                                        goto HandleXChoice;
                                    }

                                PrintCalculatorMessage("x pasirinktas sėkmingai");

                                break;
                            case 2:
                                var lagrangeRemainder = lagrangeRemainderCalculator.CalculateRemainder(
                                   function,
                                   _x);

                                var trueRemainder = lagrangeRemainderCalculator.CalculateTrueRemainder(
                                    function,
                                    taylor,
                                    _x);

                                PrintCalculatorMessage($"Liekamasis narys (Lagranžo forma): {lagrangeRemainder}");
                                PrintCalculatorMessage($"Tikslusis liekamasis narys: {trueRemainder}");
                                PrintCalculatorMessage($"Paklaida: {Math.Abs(trueRemainder-lagrangeRemainder)}");
                                break;
                            case 3:
                                return 0;
                            default:
                                PrintCalculatorMessage("Veiksmo pasirinkti nepavyko. Bandykite dar kartą.");
                                break;
                        }
                    }
                    catch (FormatException)
                    {
                        PrintCalculatorMessage("Veiksmo pasirinkti nepavyko. Bandykite dar kartą.");
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
                throw new ArgumentException("Klaida. Funkcija nepateikta.");
            }

            if (string.IsNullOrEmpty(taylor))
            {
                throw new ArgumentException("Klaida. Teiloro eilutė nepateikta.");
            }

            return (Expr.Parse(function), Expr.Parse(taylor));
        }

        private static void PrintCalculatorMessage(string message)
        {
            Console.WriteLine($"--> {message}");
        }
    }
}
