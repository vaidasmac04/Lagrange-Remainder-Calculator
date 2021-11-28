using System;

namespace LagrangeRemainder.Models
{
    public class Interval
    {
        private decimal _start;
        private decimal _end;

        public decimal Start { get => _start; set => _start = value; }
        public decimal End { get => _end; set => _end = value; }

        public Interval(decimal start, decimal end)
        {
            _start = start;

            if (end <= start)
            {
                throw new ArgumentException("Interval end can't be smaller than start");
            }

            _end = end;
        }

        public override string ToString()
        {
            return $"[{_start};{_end}]";
        }
    }
}
