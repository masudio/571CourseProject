using System;

namespace Periodicity_Detection__Complexity_Improvement_
{
    public class CPeriod : IComparable<CPeriod>
    {
        public int st;
        public int currSt;              // introduced for insertion/deletion noise code
        public int p;
        public int sumP;                // introduced for insertion/deletion noise code
        public int preValidVal = -200;    // introduced for insertion/deletion noise code
        public double avgP;                // introduced for insertion/deletion noise code
        public int count = 0;
        public int lastOccur;
        public int strlen = 0;
        public double th = 0;
        //public bool added = false;

        public int CompareTo(CPeriod pObj)
        {
            return p.CompareTo(pObj.p);
        }

    }
}