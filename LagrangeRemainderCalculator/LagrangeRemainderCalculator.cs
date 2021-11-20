﻿using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainderCalculator
{
    public class LagrangeRemainderCalculator : ILagrangeRemainderCalculator
    {
        private readonly decimal _x0 = 0;

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
                throw new ArgumentException($"Wrong interval provided: point x = '{x}' " +
                   $"must be in [{interval.Start};{interval.End}]");
            }

            if (x < _x0)
            {
                interval.Start = x;
                interval.End = 0;
            } 
            else if (x > _x0)
            {
                interval.Start = 0;
                interval.End = x;
            }
            else
            {
                return 0;
            }

            var intervalLength = Math.Abs(interval.End - interval.Start);
            var stepSize = intervalLength / stepCount;

            decimal currentIntervalPoint = x < _x0 ? interval.Start : interval.Start + stepSize;

            var results = new List<decimal>();
            var end = x < _x0 ? interval.End : interval.End + stepSize;
            while (currentIntervalPoint < end)
            {
                var remainderValue = CalculateRemainder(function, currentIntervalPoint, x);
                results.Add(remainderValue);
                currentIntervalPoint += stepSize;
            }

            return results.Max(i => i);
        }

        private static decimal CalculateRemainder(
            Expr function,
            decimal currentIntervalPoint, 
            decimal x)
        {
            var variableName = "x";
            var variable = Expr.Variable(variableName);
            var derivative = function;

            for (int i = 1; i <= 6; i++)
            {
                derivative = derivative.Differentiate(variable);
            }

            var variables = new Dictionary<string, FloatingPoint>
            {
                { variableName, (double)currentIntervalPoint }
            };

            var result = derivative.Evaluate(variables).RealValue / 720 * Math.Pow((double)x, 6);

            return (decimal)result;
        }
    }
}
