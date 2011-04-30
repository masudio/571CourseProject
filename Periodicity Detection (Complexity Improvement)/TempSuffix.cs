using System;
using System.Collections.Generic;
using System.Text;

namespace Periodicity_Detection__Complexity_Improvement_
{
    public class TempSuffix
    {
        public string TheSuffix { get; set; }
        public int ThePosition { get; set; }

        public TempSuffix(string theSuffix, int thePosition)
        {
            this.ThePosition = thePosition;
            this.TheSuffix = theSuffix;
        }
    }
}
