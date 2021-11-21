using MathNet.Symbolics;
using System.Collections.Generic;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainderCalculator
{
    public interface ILagrangeRemainderCalculator
    {
        decimal CalculateRemainder(
            Expr function, 
            decimal x,
            int stepCount = 10);

        decimal CalculateTrueRemainder(
            Expr function,
            Expr taylor,
            decimal x,
            int stepCount = 10);
    }
}
