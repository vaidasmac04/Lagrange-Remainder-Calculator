using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainder
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
