using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Periodicity_Detection__Complexity_Improvement_
{
    public class SuffixArray
    {
        private string TheString { get; set; }
        private List<TempSuffix> TheSuffixArray { get; set; }

        public SuffixArray(string theString)
        {
            this.TheString = theString;
            this.TheSuffixArray = new List<TempSuffix>();

            TempSuffix the_suffix;
            int the_position;
            for (int i = 0; i < theString.Length; i++)
            {
                the_position = i;
                the_suffix = new TempSuffix(
                    theString.Substring(i), the_position);
                this.TheSuffixArray.Add(the_suffix);
            }

            TheSuffixArray = SortArray<SortBySuffix>();
        }

        public List<TempSuffix> SortArray<TSortingAlgorithm>()
            where TSortingAlgorithm : IComparer, new()
        {
            var sortedList = new List<TempSuffix>(TheSuffixArray);
            sortedList.Sort((IComparer<TempSuffix>) new TSortingAlgorithm());

            return sortedList;
        }

        public bool FindSubstring(string theSubstring)
        {
            if (theSubstring == null || TheString == null || theSubstring.Equals(""))
                return false;

            var high = TheSuffixArray.Count - 1;
            var low = 0;
            int mid;

            string thisSuffix;
            int compare_len;
            string comparison;

            while (low <= high)
            {
                mid = (high + low)/2;
                thisSuffix = this.TheSuffixArray[mid].TheSuffix;
                compare_len = theSubstring.Length;
                comparison = thisSuffix;
                if (thisSuffix.Length > theSubstring.Length)
                    comparison = thisSuffix.Substring(0, compare_len);

                if (comparison.CompareTo(theSubstring) > 0)
                    high = mid - 1;
                else if (comparison.CompareTo(theSubstring) < 0)
                    low = mid + 1;
                else
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * Postcondition: finds all occurences of the parameter in
         * the suffix array.
         * 
         * @param theSubstring the substring to search for.
         */

        public List<int> FindAllSubstrings(string theSubstring)
        {
            var high = TheSuffixArray.Count - 1;
            var low = 0;
            int mid;

            string thisSuffix;
            string comparison;
            int compareLength = theSubstring.Length;

            var substringIndexList = new List<int>();

            while (low <= high)
            {
                mid = (high + low)/2;
                thisSuffix = this.TheSuffixArray[mid].TheSuffix;
                comparison = thisSuffix;
                if (thisSuffix.Length > theSubstring.Length)
                    comparison = thisSuffix.Substring(0, compareLength);

                if (comparison.CompareTo(theSubstring) > 0)
                    high = mid - 1;
                else if (comparison.CompareTo(theSubstring) < 0)
                    low = mid + 1;
                else
                {
                    //found a match
                    substringIndexList.Add(this.TheSuffixArray[mid].ThePosition);

                    //now check for multiple occurences after mid
                    var probeUp = mid;
                    probeUp++;
                    while (probeUp < this.TheSuffixArray.Count)
                    {
                        thisSuffix = this.TheSuffixArray[probeUp].TheSuffix;
                        comparison = thisSuffix.Substring(0, compareLength);

                        if (comparison.CompareTo(theSubstring) == 0)
                        {
                            substringIndexList.Add(this.TheSuffixArray[probeUp].ThePosition);
                            probeUp++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    //now check for multiple occurences before mid
                    var probeDown = mid;
                    probeDown--;
                    while (probeDown >= 0)
                    {
                        thisSuffix = this.TheSuffixArray[probeDown].TheSuffix;
                        comparison = thisSuffix.Substring(0, compareLength);

                        if (comparison.CompareTo(theSubstring) == 0)
                        {
                            substringIndexList.Add(this.TheSuffixArray[probeDown].ThePosition);
                            probeDown--;
                        }
                        else
                        {
                            break;
                        }
                    }

                    break;
                }
            }

            return substringIndexList;
        }
    }
}