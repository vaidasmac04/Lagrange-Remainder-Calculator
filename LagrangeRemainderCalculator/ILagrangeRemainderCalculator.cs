using MathNet.Symbolics;
using System.Collections.Generic;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainderCalculator
{
    public interface ILagrangeRemainderCalculator
    {
        decimal CalculateRemainder(
            Expr function, 
            Interval interval,
            decimal x,
            int stepCount = 10);

        decimal CalculateTrueRemainder(
            Expr function,
            Expr taylor,
            Interval interval,
            decimal x,
            int stepCount = 10);
    }
}
