using System.Collections.Generic;

namespace Periodicity_Detection__Complexity_Improvement_
{
    public class CPeriodCollection
    {
        // first int is for period value, then the list of periods indexed on st pos
        // note that the assumption is that 
        // no two periods having the same period value can start with the same position
        // if so is the case, the period with larger length should be kept
        public SortedList<int, SortedList<int, CPeriod>> periodList = new SortedList<int, SortedList<int, CPeriod>>();

        public void Add(CPeriod period)
        {
            if (periodList.ContainsKey(period.p))  // check for stpos redundancy
            {
                SortedList<int, CPeriod> innerList = periodList[period.p];
                List<int> keysToRemove = new List<int>();
                foreach (int sp in innerList.Keys)
                {
                    if (sp <= period.st) continue;
                    /** April 03 2008, 7:25 pm
                     Adding another condition that would check that the new period not only start earlier but also ends later 
                     * */
                    if ((sp % period.p == period.st % period.p) &&
                        (period.lastOccur >= periodList[period.p][sp].lastOccur))
                    {
                        keysToRemove.Add(sp);
                        //innerList.Remove(sp);
                    }
                }

                for (int i = 0; i < keysToRemove.Count; i++)
                    innerList.Remove(keysToRemove[i]);

                innerList.Add(period.st, period);
            }
            else                             // check for period value redundancy
            {
                List<int> keysToRemove = new List<int>();
                foreach (int p in periodList.Keys)
                {
                    if (p <= period.p) continue;
                    if (p % period.p == 0)
                    {
                        if (periodList[p].ContainsKey(period.st) && periodList[p][period.st].strlen <= period.strlen)
                        {
                            periodList[p].Remove(period.st);
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

                SortedList<int, CPeriod> innerList = new SortedList<int, CPeriod>();
                innerList.Add(period.st, period);
                periodList.Add(period.p, innerList);
            }
        }

        public bool Exist(CPeriod p)
        {
            foreach (int i in periodList.Keys)  // 'i' would have existing period values
            {
                if (i > p.p) break;
                if ((p.p % i) == 0)
                {
                    foreach (int j in periodList[i].Keys)   // j would have existing stpos 
                    {
                        if (j > p.st) break;
                        // Nov 5 2007 - Commented and replaced line below to accomodate periodicity within a subsection of series
                        //if (j % i == p.stPos % i || (j+periodList[i][j].length-1) >= p.stPos)
                        if ((periodList[i][j].strlen >= p.strlen && periodList[i][j].lastOccur >= p.st) &&
                            (j % i == p.st % i || (j + periodList[i][j].strlen - 1) >= p.st))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }

        /*
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
        */

    }
}