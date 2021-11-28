using LagrangeRemainder.Models;
using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainder
{
    public class LagrangeRemainderCalculator : ILagrangeRemainderCalculator, ILagrangeRemainderEvaluator
    {
        private readonly static decimal _x0 = 0;
        private readonly static string _variableName = "x";
        private readonly static Expr _variable = Expr.Variable(_variableName);

        public decimal CalculateRemainder(
            Expr function, 
            decimal x,
            int stepCount = 10)
        {

            var interval = ChooseInterval((double)x);

            Interval newInterval = null;
            if (x < _x0)
            {
                newInterval = new Interval(x, 0);
            } 
            else if (x > _x0)
            {
                newInterval = new Interval(0, x);
            }
            else
            {
                return 0;
            }

            var results = new List<decimal>();

            var intervalLength = Math.Abs(newInterval.End - newInterval.Start);
            var stepSize = intervalLength / stepCount;

            decimal currentIntervalPoint = x < _x0 ? newInterval.Start : newInterval.Start + stepSize;
            var end = x < _x0 ? newInterval.End : newInterval.End + stepSize;

            var derivative = GetDerivative(function, 6);

            while (currentIntervalPoint < end)
            {
                var variables = new Dictionary<string, FloatingPoint>
                {
                    { _variableName, (double)currentIntervalPoint }
                };

                var remainderValue = derivative.Evaluate(variables).RealValue / 720 * Math.Pow((double)x, 6);

                results.Add((decimal)remainderValue);
                currentIntervalPoint += stepSize;
            }

            return results.Max(i => i);
        }

        private static Expr GetDerivative(Expr function, short degree)
        {
            var derivative = function;

            for (int i = 1; i <= degree; i++)
            {
                derivative = derivative.Differentiate(_variable);
            }

            return derivative;
        }

        public decimal CalculateTrueRemainder(
            Expr function, 
            Expr taylor, 
            decimal x, 
            int stepCount = 10)
        {
            var variables = new Dictionary<string, FloatingPoint>
            {
                { _variableName, (double)x }
            };

            var trueValue = function.Evaluate(variables);
            var taylorValue = taylor.Evaluate(variables);

            return (decimal)Math.Abs(trueValue.RealValue - taylorValue.RealValue);
        }

        private static Interval ChooseInterval(double x)
        {
            x = Math.Abs(x);

            if (x <= 0.001)
            {
                return new Interval(-0.001m, 0.001m);
            }
            else if (x <= 0.1)
            {
                return new Interval(-0.1m, 0.1m);
            }
            else if (x <= 0.5)
            {
                return new Interval(-0.5m, 0.5m);
            }

            throw new ArgumentException($"x = {x} nepriklauso reikšmėms tarp {-0.5} ir {0.5}");

        }

        public List<EvaluationResult> EvaluateRemainder(
            Expr function, 
            List<EvaluationInterval> EvaluationIntervals, 
            int stepCount = 10)
        {
            List<EvaluationResult> evaluationResults = new();

            foreach (var evaluationInterval in EvaluationIntervals)
            {
                var intervalLength = Math.Abs(evaluationInterval.End - evaluationInterval.Start);
                var stepSize = intervalLength / stepCount;
                var currentIntervalPoint = evaluationInterval.Start;
                decimal maxRemainderValue = 0;
                var derivative = GetDerivative(function, 6);

                while (currentIntervalPoint < evaluationInterval.End)
                {
                    var variables = new Dictionary<string, FloatingPoint>
                    {
                        { _variableName, (double)currentIntervalPoint }
                    };

                    var remainderValue = (decimal)(derivative.Evaluate(variables).RealValue / 720 
                        * Math.Pow((double)evaluationInterval.MaxDerivativeValueX, 6));

                    if (remainderValue > maxRemainderValue)
                    {
                        maxRemainderValue = remainderValue;
                    }

                    currentIntervalPoint += stepSize;
                }

                evaluationResults.Add(
                    new EvaluationResult
                    {
                        Interval = evaluationInterval,
                        Remainder = maxRemainderValue
                    });
            }

            return evaluationResults;
        }
    }
}
