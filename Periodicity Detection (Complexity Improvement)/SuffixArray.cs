using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Periodicity_Detection__Complexity_Improvement_
{
    class SuffixArray
    {
        private string the_string { get; set; }
        private List<TempSuffix> suffix_array { get; set; }

        public SuffixArray(string the_string)
        {
            this.the_string = the_string;
            this.suffix_array = new List<TempSuffix>();

            TempSuffix the_suffix;
            int the_position;
            for (int i = 0; i < the_string.Length; i++)
            {
                the_position = i;
                the_suffix = new TempSuffix(
                    the_string.Substring(i), the_position);
                this.suffix_array.Add(the_suffix);
            }

            suffix_array = SortArray<SortBySuffix>();
        }

        public List<TempSuffix> SortArray<TSortingAlgorithm>()
            where TSortingAlgorithm : IComparer, new()
        {
            var sortedList = new List<TempSuffix>(suffix_array);
            sortedList.Sort((IComparer<TempSuffix>) new TSortingAlgorithm());

            return sortedList;
        }

        public int find_substring(string the_substring)
        {
            int high = suffix_array.Count - 1;
            int low = 0;
            int mid;

            string this_suffix;
            int compare_len;
            string comparison;

            while(low <= high)
            {
                mid = (high + low)/2;
                this_suffix = this.suffix_array[mid].the_suffix;
                compare_len = the_substring.Length - 1;
                comparison = this_suffix.Substring(0, compare_len);

                if (comparison.CompareTo(the_substring) > 0)
                    high = mid - 1;
                else if (comparison.CompareTo(the_substring) < 0)
                    low = mid + 1;
                else
                {
                    return this.suffix_array[mid].the_position;
                }
            }

            return -1;
        }
    }
}
