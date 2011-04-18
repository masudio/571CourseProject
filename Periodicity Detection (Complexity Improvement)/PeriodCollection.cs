using System.Collections.Generic;

namespace Periodicity_Detection__Complexity_Improvement_
{
    public class PeriodCollection
    {
        // first int is for period value, then the list of periods indexed on st pos
        // note that the assumption is that 
        // no two periods having the same period value can start with the same position
        // if so is the case, the period with larger length should be kept
        public static SortedList<int, SortedList<int, Period>> periodList = new SortedList<int, SortedList<int, Period>>();

        public static void Add(int periodVal, Period period)
        {
            if (periodList.ContainsKey(periodVal))  // check for stpos redundancy
            {
                SortedList<int, Period> innerList = periodList[period.periodValue];
                List<int> keysToRemove = new List<int>();
                foreach (int sp in innerList.Keys)
                {
                    if (sp <= period.stPos) continue;
                    /** April 03 2008, 7:25 pm
                     Adding another condition that would check that the new period not only start earlier but also ends later 
                     * */
                    // Nov 25 2008 - Added the second check on second line to check that the string to be removed is the substring of new periodic occurence to qualify as redundant

                    if ((sp % period.periodValue == period.stPos % period.periodValue) && 
                        Program.s.Substring(period.stPos, period.length).StartsWith(Program.s.Substring(sp, periodList[periodVal][sp].length)) &&
                        (period.endPos >= periodList[period.periodValue][sp].endPos))
                    {
                        keysToRemove.Add(sp);
                        //innerList.Remove(sp);
                    }
                }

                for (int i = 0; i < keysToRemove.Count; i++)
                    innerList.Remove(keysToRemove[i]);

                innerList.Add(period.stPos, period);
            }
            else                             // check for period value redundancy
            {
                List<int> keysToRemove = new List<int>();
                foreach (int p in periodList.Keys)
                {
                    if (p <= periodVal) continue;
                    if (p % period.periodValue == 0)
                    {
                        if (periodList[p].ContainsKey(period.stPos) && periodList[p][period.stPos].length <= period.length)
                        {
                            periodList[p].Remove(period.stPos);
                            if (periodList[p].Count == 0)
                            {
                                keysToRemove.Add(p);
                                //periodList.Remove(p);
                            }
                        }
                    }
                }
                for (int i = 0; i < keysToRemove.Count; i++)
                    periodList.Remove(keysToRemove[i]);

                SortedList<int, Period> innerList = new SortedList<int, Period>();
                innerList.Add(period.stPos, period);
                periodList.Add(periodVal, innerList);
            }
        }

        public static bool Exist(Period p)
        {
            foreach (int i in periodList.Keys)  // 'i' would have existing period values
            {
                if (i > p.periodValue) break;
                if ((p.periodValue % i) == 0)
                {
                    foreach (int j in periodList[i].Keys)   // j would have existing stpos 
                    {
                        if (j > p.stPos) break;
                        // Nov 5 2007 - Commented and replaced line below to accomodate periodicity within a subsection of series
                        // Nov 25 2008 - Added the last check on first line to check that the supplied string is the substring of an existing periodic occurence to qualify as redundant
                        //if (j % i == p.stPos % i || (j+periodList[i][j].length-1) >= p.stPos)
                        if ((periodList[i][j].length >= p.length && periodList[i][j].endPos >= p.stPos) && Program.s.Substring(j,periodList[i][j].length).StartsWith(Program.s.Substring(p.stPos, p.length)) &&
                            (j % i == p.stPos % i || (j + periodList[i][j].length - 1) >= p.stPos))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

            /*
            if (!periodList.ContainsKey(p.periodValue))
            {
                if (PeriodValueExist(p))
                {
                    return true;
                }
            }
            return false;
             */
        }

        public static bool PeriodValueExist(Period p)
        {
            foreach (int i in periodList.Keys)  // 'i' would have existing period values
            {
                if (i > p.periodValue) break;
                if ((p.periodValue % i) == 0)
                {
                    foreach (int j in periodList[i].Keys)   // j would have existing stpos 
                    {
                        if (j > p.stPos) break;
                        if (j % i == p.stPos % i)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


    }
}