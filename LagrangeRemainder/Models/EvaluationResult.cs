namespace LagrangeRemainder.Models
{
    public class EvaluationResult
    {
        public decimal Remainder { get; init;  }

        public Interval Interval { get; init; }

        public decimal MaxValueAt { get; init; }

    }
}
