using System;
using System.Collections.Generic;
using System.Text;

namespace Periodicity_Detection__Complexity_Improvement_
{
    public class TempSuffix
    {
        public string the_suffix { get; set; }
        public int the_position { get; set; }

        public TempSuffix(string the_suffix, int the_position)
        {
            this.the_position = the_position;
            this.the_suffix = the_suffix;
        }
    }
}
