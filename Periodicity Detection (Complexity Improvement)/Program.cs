using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace Periodicity_Detection__Complexity_Improvement_
{
    class Program
    {
        public static String s = ""; //"abcabcabbabcabcabc" + "$";
        static double minTh = 0.7;
        static double minLengthSegment = 0.1;
        static int minPeriod = 2;
        static int maxPeriod = 500;
        public static int minStrLen = 1;
        public static int maxStrLen = 100000;
        static int tolWin = 0;
        static int dmax = 10000;
        public static List<CPeriod> periodCollection = new List<CPeriod>();
        public static int preCountPerCol = periodCollection.Count;
        public static int candPerCount = 0, addPerCount = 0;
        public static int occVecCount = 0, occVecAddCount = 0;
        

        static void Main(string[] args)
        {
            ///////////////////////////////////////////////////////////////////////
            /////////// This is the complexity improved version working \\\\\\\\\\\
            //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

            //int[] occur = new int[]{0,3,5,6,9,13,15,18,19,21};
            //CalculatePeriod(occur, s.Length);
             
            /*Console.WriteLine(s);
            Console.WriteLine("01234567890123456789");
            Console.WriteLine();
             */
            
            //String fn = @"c:\output";

            string path = @"C:\data for experiments\";
            //for (int i = 1; i <= 1; i++)
            //{
                periodCollection = new List<CPeriod>();
                preCountPerCol = periodCollection.Count;
                candPerCount = 0; addPerCount = 0; occVecCount = 0;
                string fn = "test,p=4,gap";
                FileStream fs = new FileStream(path + fn + ".data", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                s = sr.ReadToEnd().Trim() + "$";

                SuffTree st = new SuffTree(s, minTh, tolWin, dmax, minLengthSegment, path + fn + "-Th=" + minTh + ", TolWin=" + tolWin, -2);
                Console.WriteLine("Candidate Period Count: " + candPerCount);
                Console.WriteLine("Added Period Count: " + addPerCount);
                Console.WriteLine("Occur Vector Count: " + occVecCount);
                Console.WriteLine();
                //Console.ReadLine();
            //}
            Console.ReadLine();

            /* int y = 0;
           for (int i = 0; i < periodCollection.Count; i++)
           {
               if (((periodCollection[i].lastOccur + periodCollection[i].strlen - periodCollection[i].st) % periodCollection[i].p) >= periodCollection[i].strlen) y = 1; else y = 0;
               periodCollection[i].th = periodCollection[i].count / Math.Floor(((double)(periodCollection[i].lastOccur + periodCollection[i].strlen - periodCollection[i].st) / periodCollection[i].p) + y);

               Console.WriteLine();
               Console.WriteLine("String: " + s.Substring(periodCollection[i].st, periodCollection[i].strlen));
               Console.WriteLine("Period: " + periodCollection[i].p);
               Console.WriteLine("St: " + periodCollection[i].st);
               Console.WriteLine("End: " + periodCollection[i].lastOccur);
               Console.WriteLine("Count: " + periodCollection[i].count);
               Console.WriteLine("Conf: " + periodCollection[i].th);
           }
           Console.WriteLine("\r\nTotal Periods: " + periodCollection.Count);
           */
        }

        

        public static void CalculatePeriod(int[] occur, int strlen)
        {
            occVecCount++;
            //occVecAddCount = addPerCount;
            //if (strlen < minPeriod || strlen > maxPeriod) return;
            if (strlen < minStrLen || strlen > maxStrLen) return;
            candPerCount += (occur.Length - 1);
            preCountPerCol = periodCollection.Count;
            int prePer = -5;
            for (int i = 0; i < occur.Length; i++)
            {
                CPeriod p = new CPeriod();
                p.st = occur[i];
                p.count = 0;
                p.lastOccur = p.st;
                p.strlen = strlen;

                if(i < (occur.Length -1))
                {
                    p.p = occur[i + 1] - occur[i];
                    // adding code for dmax, 11th Feb 2009 1:38 pm
                    if (p.p > dmax)
                    {
                        int y1 = 0;
                        for (int k = preCountPerCol; k < periodCollection.Count; k++)
                        {
                            // new code for A,B,C, i.e., insertion/deletion noise accomodation - 7th Feb 2009, 4:04 am
                            int A1 = occur[i] - periodCollection[k].currSt;
                            double B1 = Math.Round((double)A1 / periodCollection[k].p);
                            int C1 = A1 - (periodCollection[k].p * (int)B1);
                            if (C1 >= (-1 * tolWin) && C1 <= tolWin)
                            {
                                if (Math.Round((double)((periodCollection[k].preValidVal - periodCollection[k].currSt) / periodCollection[k].p)) != B1)
                                {
                                    periodCollection[k].preValidVal = occur[i];
                                    periodCollection[k].currSt = occur[i];
                                    periodCollection[k].sumP += (periodCollection[k].p + C1);
                                    periodCollection[k].lastOccur = occur[i];
                                    periodCollection[k].count++;
                                }
                            }
                            // end new code for A,B,C, i.e., insertion/deletion noise accomodation

                            double avgPeriodValue = (double)(periodCollection[k].sumP - periodCollection[k].p) / (periodCollection[k].count - 1);
                            periodCollection[k].avgP = Math.Round(avgPeriodValue, 1);
                            if (((periodCollection[k].lastOccur + periodCollection[k].strlen - periodCollection[k].st) % ((int)Math.Round(avgPeriodValue))) >= periodCollection[k].strlen) y1 = 1; else y1 = 0;
                            periodCollection[k].th = periodCollection[k].count / Math.Floor(((double)(periodCollection[k].lastOccur + periodCollection[k].strlen - periodCollection[k].st) / avgPeriodValue) + y1);
                            if (periodCollection[k].th < minTh || ((periodCollection[k].lastOccur + periodCollection[k].strlen - periodCollection[k].st) < (minLengthSegment * s.Length)))
                            {
                                periodCollection.RemoveAt(k);
                                k--;
                            }
                        }
                        preCountPerCol = periodCollection.Count;
                        /************ may be add code to update prePer ************ Feb 23 2009, 4:28 am */
                        // added code to update prePer, March 3, 2009 1:52 pm
                        prePer = -5;
                        // end added code to update prePer
                        continue;
                    }
                    // end code for dmax, 11th Feb 2009 1:38 pmB052436764
                }

                if ( (p.p!= 0) && (prePer != p.p) && (occur[occur.Length-1] +strlen  - p.st) > (minLengthSegment*s.Length) && !AlreadyThere(p) )
                {
                    p.currSt = p.st;    // added for insertion/deletion noise accomodation
                    periodCollection.Add(p);
                    addPerCount++;
                }
                
                prePer = p.p;

                for (int j = preCountPerCol; j < periodCollection.Count; j++)
                {
                    // new code for A,B,C, i.e., insertion/deletion noise accomodation - 7th Feb 2009, 4:04 am
                    int A = occur[i] - periodCollection[j].currSt;
                    double B = Math.Round((double)A / periodCollection[j].p);
                    int C = A - (periodCollection[j].p * (int)B);
                    if (C >= (-1 * tolWin) && C <= tolWin)
                    {
                        if (Math.Round((double)((periodCollection[j].preValidVal - periodCollection[j].currSt) / periodCollection[j].p)) != B)
                        {
                            periodCollection[j].preValidVal = occur[i];
                            periodCollection[j].currSt = occur[i];
                            periodCollection[j].sumP += (periodCollection[j].p + C);
                            periodCollection[j].lastOccur = occur[i];
                            periodCollection[j].count++;
                        }
                    }

                    // end new code for A,B,C, i.e., insertion/deletion noise accomodation
                    /********* Commenting code working prior to adding A,B,C stuff
                    if ((periodCollection[j].st % periodCollection[j].p) == (p.st % periodCollection[j].p))
                    {
                        periodCollection[j].count++;
                        periodCollection[j].lastOccur = occur[i];
                    }
                    ****** End commenting code working prior to A,B,C stuff*/
                }
            }
            //Console.WriteLine("Occ Vector Length =\t\t" + occur.Length);
            //Console.WriteLine("Occ Vector Added Period Count =\t" + (addPerCount - occVecAddCount));
            int y = 0;
            for (int i = preCountPerCol; i < periodCollection.Count; i++)
            {
                //if (((p.endPos + p.length - p.stPos) % ((int)Math.Round(p.avgPeriodValue))) >= e.value) y = 1; else y = 0;
                //double th1 = p.foundPosCount / Math.Floor(((double)(p.endPos + p.length - p.stPos) / p.avgPeriodValue) + y);

                double avgPeriodValue = (double) (periodCollection[i].sumP - periodCollection[i].p) / (periodCollection[i].count - 1);
                periodCollection[i].avgP = Math.Round(avgPeriodValue, 1);
                if (((periodCollection[i].lastOccur + periodCollection[i].strlen - periodCollection[i].st) % ((int)Math.Round(avgPeriodValue))) >= periodCollection[i].strlen) y = 1; else y = 0;
                periodCollection[i].th = periodCollection[i].count / Math.Floor(((double)(periodCollection[i].lastOccur + periodCollection[i].strlen - periodCollection[i].st) / avgPeriodValue) + y);
                if (periodCollection[i].th < minTh || ((periodCollection[i].lastOccur + periodCollection[i].strlen  - periodCollection[i].st) < (minLengthSegment * s.Length)))
                {
                    periodCollection.RemoveAt(i);
                    i--;
                }
            }

        }

        public static bool AlreadyThere(CPeriod p)
        {
            if (/*p.strlen > 0.33 * s.Length || */p.strlen > p.p || p.p < minPeriod || p.p > maxPeriod)
            {
                return true;
            }
            else
            {
                return false; /////////// putting temporarily 18th August 2010

                for (int i = 0; i < preCountPerCol; i++)
                {
                    if (periodCollection[i].p == p.p || p.p % periodCollection[i].p == 0)
                    {
                        if (p.st == periodCollection[i].st) return true;
                        if (p.st % p.p == periodCollection[i].st % periodCollection[i].p && p.strlen <= periodCollection[i].strlen)
                        {
                            int k=0;
                            for (k = 0; k < p.strlen; k++)
                            {
                                if (s[p.st + k] != s[periodCollection[i].st + k]) break;
                            }
                            if (k == p.strlen) return true;
                            else continue;
                        }
                    }
                }
            }
            return false;
        }

    }

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

