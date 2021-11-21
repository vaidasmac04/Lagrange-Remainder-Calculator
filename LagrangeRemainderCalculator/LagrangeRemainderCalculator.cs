using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainderCalculator
{
    public class LagrangeRemainderCalculator : ILagrangeRemainderCalculator
    {
        private readonly static decimal _x0 = 0;
        private readonly static string _variableName = "x";
        private readonly static Expr _variable = Expr.Variable(_variableName);

        public decimal CalculateRemainder(
            Expr function, 
            Interval interval,
            decimal x,
            int stepCount = 10)
        {

            if(_x0 <= interval.Start || _x0 >= interval.End)
            {
                throw new ArgumentException($"Wrong interval provided: point x0 = '{_x0}' " +
                    $"must be in ({interval.Start};{interval.End})");
            }

            if(x < interval.Start || x > interval.End)
            {
                throw new ArgumentException($"Wrong point provided: x = '{x}' " +
                   $"must be in [{interval.Start};{interval.End}]");
            }

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

            var derivative = function;

            for (int i = 1; i <= 6; i++)
            {
                derivative = derivative.Differentiate(_variable);
            }

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

        public decimal CalculateTrueRemainder(
            Expr function, 
            Expr taylor, 
            Interval interval, 
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
    }
}
