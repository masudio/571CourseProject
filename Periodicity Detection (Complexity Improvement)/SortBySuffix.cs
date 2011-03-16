using System;
using System.Collections.Generic;
using System.Text;

namespace Periodicity_Detection__Complexity_Improvement_
{
    class SortBySuffix : Comparer<TempSuffix>
    {
        public override int Compare(TempSuffix x, TempSuffix y)
        {
            return x.the_suffix.CompareTo(y.the_suffix);
        }
    }
}
