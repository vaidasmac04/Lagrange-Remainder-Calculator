namespace LagrangeRemainder.Models
{
    public class EvaluationInterval : Interval
    {
        private decimal _maxDerivativeValueX;

        public decimal MaxDerivativeValueX 
        { 
            get => _maxDerivativeValueX; 
            set => _maxDerivativeValueX = value; 
        }

        public EvaluationInterval(
            decimal start, 
            decimal end, 
            decimal maxDerivativeValueX) : base(start, end)
        {
            _maxDerivativeValueX = maxDerivativeValueX;
        }
    }
}
