using System;

namespace Periodicity_Detection__Complexity_Improvement_
{
    public class Period : IComparable
    {
        //public string symbolString = "";
        public int fci = 0;
        public int length = 0;
        public int periodValue;
        public int stPos;
        public double threshold;
        public int foundPosCount;
        public double avgPeriodValue;
        public int endPos; // introduced to find periodicity in segments - 2nd Nov 2007

        public int CompareTo(object obj)
        {
            if (obj is Period)
            {
                Period p = (Period)obj;
                return periodValue.CompareTo(p.periodValue);
            }
            throw new ArgumentException("object is not a Period");
        }

    }
}