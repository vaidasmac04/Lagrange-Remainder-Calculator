using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagrangeRemainderCalculator
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
    }
}
