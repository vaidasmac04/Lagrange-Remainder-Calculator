using LagrangeRemainder.Models;
using System.Collections.Generic;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LagrangeRemainder
{
    public interface ILagrangeRemainderEvaluator
    {
        List<EvaluationResult> EvaluateRemainder(
           Expr function,
           List<EvaluationInterval> EvaluationIntervals,
           int stepCount = 10);
    }
}
