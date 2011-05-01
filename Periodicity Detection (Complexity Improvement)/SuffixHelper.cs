using System;
using System.Collections.Generic;
using System.Text;

namespace Periodicity_Detection__Complexity_Improvement_ {
    interface SuffixHelper
    {
        /**
        * returns true if the given substring exists in this object's tree, false
        * otherwise.
        */
        bool FindSubstring(string theSubstring);

        /**
         * Returns a list of all occurrences of the given substring, or an empty list if
         * none are found.
         */
        IEnumerable<int> FindAllOccurrences(string theSubstring);
    }
}
