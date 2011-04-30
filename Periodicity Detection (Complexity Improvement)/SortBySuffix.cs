using System;
using System.Collections.Generic;
using System.Text;

namespace Periodicity_Detection__Complexity_Improvement_
{
    public class SortBySuffix : Comparer<TempSuffix>
    {
        public override int Compare(TempSuffix x, TempSuffix y)
        {
            return x.TheSuffix.CompareTo(y.TheSuffix);
        }
    }
}
