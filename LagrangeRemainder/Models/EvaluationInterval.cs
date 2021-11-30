namespace LagrangeRemainder.Models
{
    public class EvaluationInterval : Interval
    {
        private decimal _x;

        public decimal X
        { 
            get => _x; 
            set => _x = value; 
        }

        public EvaluationInterval(
            decimal start, 
            decimal end, 
            decimal x) : base(start, end)
        {
            _x = x;
        }
    }
}
