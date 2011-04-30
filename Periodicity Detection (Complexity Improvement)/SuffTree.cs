using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
//using System.Collections;

namespace Periodicity_Detection__Complexity_Improvement_
{
    class SuffTree
    {
        StringBuilder ov = new StringBuilder(); // to extract complexity improvement data, 10th sep 2008, 6:09 am
        public string T; // the input string to the stree
        int N;
        public double minThreshold = 0.5;
        OccurList occur = new OccurList();
        //int[] occur;


        // The array of defined nodes.  The count is 1 at the
        // start because the initial tree has the root node
        // defined, with no children.
        //public static List<Node> Nodes = new List<Node>();

        public static Node[] Nodes = null;
        public int tolWin = 0;

        public int dmax = 0;
        public double minLengthOfSegment = 0;
        public int specificPeriod = -1;

        public SuffTree(string T, double minThreshold, int tolWin, int dmax, double minLengthOfSegment, string filename, int specificPeriod)
        {
            this.tolWin = tolWin;
            this.dmax = dmax;
            this.minLengthOfSegment = minLengthOfSegment;
            this.specificPeriod = specificPeriod;
            if (specificPeriod == -1) Console.WriteLine("==== CLASSIC =====");
            if (specificPeriod == -2) Console.WriteLine("==== IMPROVED =====");
            MakeTree(T, minThreshold);
            //PrintNodes(); PrintEdges();
            PrintTree(filename);

        }

        public void MakeTree(string T, double minThreshold)
        {
            Console.WriteLine("Started: " + DateTime.Now.ToString());
            this.minThreshold = minThreshold;
            this.T = T;
            this.N = T.Length;  // might be T.Length - 1;
            Node.Count = 1;
            Suffix.T = T;
            Edge.T = T;

            Nodes = new Node[N * 2];
            int prime = (new Prime((int)((N * 2) + (N * 2 * 0.1)))).next();
            Edge.HASH_TABLE_SIZE = prime;
            Edge.Edges = new Edge[prime];
            InitializeNodesAndEdges();

            // The active point is the first non-leaf suffix in the
            // tree.  We start by setting this to be the empty string
            // at node 0.  The AddPrefix() function will update this
            // value after every new prefix is added.
            Suffix active = new Suffix(0, 0, -1);  // The initial active prefix
            for (int i = 0; i < N; i++)
            {
                AddPrefix(active, i);
            }
            Console.WriteLine("Tree Done: " + DateTime.Now.ToString());

        }

        public void InitializeNodesAndEdges()
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i] = new Node();
            }
            for (int j = 0; j < Edge.Edges.Length; j++)
            {
                Edge.Edges[j] = new Edge();
            }
        }

        List<Period> periods = new List<Period>();

        //
        // This routine constitutes the heart of the algorithm.
        // It is called repetitively, once for each of the prefixes
        // of the input string.  The prefix in question is denoted
        // by the index of its last character.
        //
        // At each prefix, we start at the active point, and add
        // a new edge denoting the new last character, until we
        // reach a point where the new edge is not needed due to
        // the presence of an existing edge starting with the new
        // last character.  This point is the end point.
        //
        // Luckily for use, the end point just happens to be the
        // active point for the next pass through the tree.  All
        // we have to do is update it's last_char_index to indicate
        // that it has grown by a single character, and then this
        // routine can do all its work one more time.
        //

        public void AddPrefix(Suffix active, int last_char_index)
        {
            int parent_node;
            int last_parent_node = -1;

            for (; ; )
            {
                Edge edge = new Edge();
                parent_node = active.origin_node;

                // Step 1 is to try and find a matching edge for the given node.
                // If a matching edge exists, we are done adding edges, so we break
                // out of this big loop.
                if (active.Explicit())
                {
                    edge = Edge.Find(active.origin_node, T[last_char_index]);
                    //if (edge != null) break;
                    if (edge.start_node != -1)
                        break;
                }
                else
                { //implicit node, a little more complicated
                    edge = Edge.Find(active.origin_node, T[active.first_char_index]);
                    int span = active.last_char_index - active.first_char_index;
                    if (T[edge.first_char_index + span + 1] == T[last_char_index])
                        break;
                    parent_node = edge.SplitEdge(active);
                }

                // We didn't find a matching edge, so we create a new one, add
                // it to the tree at the parent node position, and insert it
                // into the hash table.  When we create a new node, it also
                // means we need to create a suffix link to the new node from
                // the last node we visited.
                Edge new_edge = new Edge(last_char_index, N - 1, parent_node);
                new_edge.Insert();
                if (last_parent_node > 0)
                {
                    //Node n = new Node();
                    /****** new edition ******* 
                    //n.idx = last_parent_node;
                    //n.suffix_node = parent_node;
                     */
                    //Nodes.Add(n);
                    Nodes[last_parent_node].suffix_node = parent_node;
                }
                last_parent_node = parent_node;

                // This final step is where we move to the next smaller suffix
                if (active.origin_node == 0)
                    active.first_char_index++;
                else
                {
                    //active.origin_node = FindNode(active.origin_node).suffix_node;
                    active.origin_node = Nodes[active.origin_node].suffix_node;
                }
                active.Canonize();
            }
            if (last_parent_node > 0)
            {
                //Node n = new Node();
                /******* New Edition ************
                n.idx = last_parent_node;
                n.suffix_node = parent_node;
                 */
                //Nodes.Add(n);
                Nodes[last_parent_node].suffix_node = parent_node;
            }
            active.last_char_index++;  //Now the endpoint is the next active point
            active.Canonize();
        }

        public static Node FindNode(int index)
        {
            foreach (Node n in Nodes)
            {
                if (n.idx == index) return n;
            }
            Node n1 = new Node();
            n1.idx = index;
            return n1;
        }


        // This routine prints out the contents of the suffix tree
        // at the end of the program by walking through the
        // hash table and printing out all used edges.  It
        // would be really great if I had some code that will
        // print out the tree in a graphical fashion, but I don't!


        /*** causing memory error with t.length>5mb

        public void dump_edges( int current_n )
        {
            //************ Commenting the Edge Printing - May 26
            //Edge.Edges.Sort();
            Console.WriteLine();
            Console.WriteLine("Start\tEnd\tSuf\tFirst\tLast\tString\tValue\tPeriod");
            for ( int j = 0 ; j < Edge.Edges.Length; j++ ) {
                Edge s = Edge.Edges[j];
                //if ( s.start_node == -1 )
                //    continue;
                //Console.Write(j+1 + "\t");
                Console.Write(s.start_node + "\t");
                Console.Write(s.end_node + "\t");
                Console.Write(FindNode(s.end_node).suffix_node + "\t");
                Console.Write(s.first_char_index + "\t");
                Console.Write(s.last_char_index + "\t");
                /*
                cout << setw( 5 ) << s->start_node << " "
                     << setw( 5 ) << s->end_node << " "
                     << setw( 3 ) << Nodes[ s->end_node ].suffix_node << " "
                     << setw( 5 ) << s->first_char_index << " "
                     << setw( 6 ) << s->last_char_index << "  ";
                 */

        ///************* commenting the label string - 26th May 

        /***** causing memory error with t.length>5mb

        int top;
        if ( current_n > s.last_char_index )
            top = s.last_char_index;
        else
            top = current_n;
        for (int l = s.first_char_index;
                  l <= top;
                  l++)
            Console.Write(T[l]);
            //cout << T[ l ];
        Console.Write("\t" + s.value + "\t");
         *********** end causing memory error with t.length>5mb */

        //*************/
        ///********** New Edition ******************

        /*** causing memory error with t.length>5mb
        string indexes = "";
        s.indexes.Sort();
        int[] idxs =  s.indexes.ToArray();
        for (int i = 0; i < idxs.Length; i++)
        {
            indexes += idxs[i].ToString() + ", ";
        }
        if (indexes.Length > 2) indexes = indexes.Substring(0, indexes.Length - 2);

        Console.Write("(" + indexes + ")" + "\t");
         **** end causing memory error with t.length>5mb */

        //*/
        ///******** commenting continues from the start of method
        /*** causing memory error with t.length>5mb

            Console.Write("\r\n");
            //cout << "\n";
        }
         ********* end causing memory error with t.length>5mb */

        //***********/

        /*** causing memory error with t.length>5mb

        Console.WriteLine("Total Edges: " + Edge.Edges.Length);
    }
          **** end causing memory error with t.length>5mb *******/


        public void PrintTree(string filename)
        {
            Edge.Sort();
            //Console.WriteLine("\r\nEdges Array Sorted (Size: " + Edge.Edges.Length + ") : " + DateTime.Now.ToString());

            List<int> indexes = new List<int>();
            stn = 0;
            if (specificPeriod == -2)
            {
                VisitNode4Imp(0, 0, indexes);
                WritePeriods2Imp(filename);
            }
        }



       public void WritePeriods2Imp(string filename)
        {
            Console.WriteLine("Periods Found: " + DateTime.Now.ToString());
            Console.WriteLine();


            FileStream fs = null;
            StreamWriter sw = null;
            if (filename != "")
            {
                fs = new FileStream(filename +"-Imp.txt", FileMode.Create, FileAccess.Write, FileShare.None);
                sw = new StreamWriter(fs);

                sw.WriteLine("Series Length: " + (T.Length - 1));
                sw.WriteLine();
                sw.WriteLine("Period\tAvgPer\tStPos\tStPos%\tEndPos\tThreshold\tCount\tSymbolString");//\t\t\tRepetition\tSeriesLength");

            }

            List<CPeriod> periodCollection = Program.periodCollection;
            periodCollection.Sort();
            int perCnt = 0, prePer = -5;
            string strPerTotal = "";
            for (int i = 0; i < periodCollection.Count; i++)
            {
                if (sw == null)
                {
                    Console.WriteLine(periodCollection[i].p + "\t" + periodCollection[i].avgP + "\t" + periodCollection[i].st + "\t" + periodCollection[i].st % periodCollection[i].p + "\t" + (periodCollection[i].lastOccur + periodCollection[i].strlen - 1) + "\t" + Math.Round(periodCollection[i].th, 2) + "\t\t" + periodCollection[i].count + "\t" + T.Substring(periodCollection[i].st, periodCollection[i].strlen) + "\t" + (periodCollection[i].lastOccur -periodCollection[i].st));
                }
                else
                {
                    sw.WriteLine(periodCollection[i].p + "\t" + periodCollection[i].avgP + "\t" + periodCollection[i].st + "\t" + periodCollection[i].st % periodCollection[i].p + "\t" + (periodCollection[i].lastOccur + periodCollection[i].strlen - 1) + "\t" + Math.Round(periodCollection[i].th, 2) + "\t\t" + periodCollection[i].count + "\t" + T.Substring(periodCollection[i].st, periodCollection[i].strlen)); //+ "\t\t\t\t" + periodCollection[i].count + "\t" + (periodCollection[i].lastOccur - periodCollection[i].st));
                }
                if (prePer != periodCollection[i].p)
                {
                    strPerTotal += (prePer + "\t" + perCnt + "\r\n");
                    prePer = periodCollection[i].p;
                    perCnt = 0;
                }
                else
                {
                    perCnt++;
                }
            }
            strPerTotal += (prePer + "\t" + perCnt + "\r\n");
            strPerTotal = strPerTotal.Remove(0, strPerTotal.IndexOf("\r\n") +2);

            if (sw != null)
            {
                sw.WriteLine();
                sw.WriteLine("Number of Periods: " + periodCollection.Count);
                sw.WriteLine();
                sw.WriteLine("Period\tCount");
                sw.WriteLine(strPerTotal);

                //fs.Close();
                sw.Close();
            }
        }


        public static int FindAll1Counter = 0;
        public static int PeriodExistCounter = 0;
        public static int AddPeriodCounter = 0;
        Dictionary<int, int> edht = new Dictionary<int, int>();



        public void VisitNode4Imp(int tabs, int pnvalue, List<int> pnIndexes)
        {

            //int occurCounter = 0;
            MakeEdgeVectorHashtable1();
            Console.WriteLine("MakeEdgeVectorHashtable1() finished: " + DateTime.Now.ToString());

            Stack<EdgeStruct> s = new Stack<EdgeStruct>();
            //int[] rootOccurIndexes = new int[2] { -1, -1 };

            OccurNode[] rootOccurSt = new OccurNode[1] { null };
            int[] rootOccurLength = new int[1] { -1 };

            List<Edge> edgeCol = Edge.FindAll1(stn);
            edgeCol.Sort(EdgeComparer);
            foreach (Edge e in edgeCol)
            {
                EdgeStruct es = new EdgeStruct();
                es.e = e;
                es.tabs = 0;
                es.pnValue = 0;
                //es.pnOccurIndexes = rootOccurIndexes;
                es.pnOccurStart = rootOccurSt;
                es.pnOccurLength = rootOccurLength;
                s.Push(es);
            }
            edgeCol.Clear();
            edgeCol = null;
            //int leafCounter = 0;

            while (s.Count != 0)
            {
                EdgeStruct es = s.Pop();
                if (es.e.value != -1)  // node has already been marked
                {
                    es.e.st = es.occurStart[0];
                    es.e.len = es.occurLength[0];
                    //calculatePeriodCounter++;
                    if (IsCalculatePeriod(es.e))
                    {
                        CalculatePeriodImp(es.e);
                        //CalculatePeriod2(es.e);
                    }

                    if (es.pnOccurStart[0] == null)
                    {
                        es.pnOccurStart[0] = es.occurStart[0];
                        es.pnOccurLength[0] = es.occurLength[0];
                    }
                    else
                    {
                        occur.Sort(es.pnOccurStart, es.pnOccurLength[0], es.occurStart, es.occurLength[0]);
                        es.pnOccurLength[0] += es.occurLength[0];
                    }
                }
                else        // node has not been marked yet
                {
                    int myval = 0;
                    if (es.e.last_char_index == (N - 1))        // if it leads to a leaf
                    {
                        myval = N - ((es.e.last_char_index - es.e.first_char_index) + 1 + es.pnValue);

                        occur.Add(myval, es.pnOccurStart, es.pnOccurLength);
                    }
                    else
                    {
                        myval = ((es.e.last_char_index - es.e.first_char_index) + 1 + es.pnValue);
                        es.e.value = myval;


                        //es.occurIndexes = new int[2]{-1,-1};
                        s.Push(es);
                        stn = es.e.end_node;
                        List<Edge> eCol = Edge.FindAll1(stn);
                        eCol.Sort(EdgeComparer);
                        foreach (Edge e in eCol)
                        {
                            EdgeStruct es1 = new EdgeStruct();
                            es1.e = e;
                            es1.tabs = es.tabs + 1;
                            es1.pnValue = es.e.value;
                            es1.pnOccurStart = es.occurStart;
                            es1.pnOccurLength = es.occurLength;
                            //es1.pnOccurIndexes = es.occurIndexes;
                            s.Push(es1);
                        }
                        eCol.Clear();
                        eCol = null;
                    }
                }
            }
        }


        public bool IsCalculatePeriod(Edge e)
        {
            return true;
            //if (edht[e.eid] < (0.05 * T.Length)) return false; else return true;
            //if (e.start_node == 0) return false; else return true;
            /* Commenting temp
            if (e.start_node == 0  || edht[e.eid] < (0.01 * T.Length))
                return false;
            else
                return true;
            // ************ end comment temp************/

            /*
            List<Edge> childEdges = Edge.FindAll1(e.end_node);
            childEdges.Sort(EdgeComparer);
            int restTotal = 0;
            for (int i = 1; i < childEdges.Count; i++)
            {
                restTotal += edht[childEdges[i].eid];
            }
            if (restTotal < (0.02 * T.Length))
                return false;
            else
                return true;
            */
        }

        public List<Edge> SortEdges(List<Edge> eCol)
        {
            return null;
            /*
            //int[] eColArray = new int[eCol.Count];
            List<Edge> inEdges = new List<Edge>();
            List<Edge> lEdges = new List<Edge>();
            for (int i = 0; i < eCol.Count; i++)
            {
                if (eCol[i].last_char_index == (N - 1))
                {
                    lEdges.Add(eCol[i]);
                }
                else
                {
                    inEdges.Add(eCol[i]);
                    //eColArray[i] = edht[eCol[i].eid];
                }
            }
            inEdges.Sort(EdgeComparer);
            lEdges.AddRange(inEdges);
            return lEdges;
            //Array.Sort(eColArray, new EdgeComparer());
             */
            //return eCol.Sort(EdgeComparer);

        }

        public int EdgeComparer(Edge e1, Edge e2)
        {
            return edht[e1.eid].CompareTo(edht[e2.eid]);
        }


        public void MakeEdgeVectorHashtable1()
        {
            edht.Add(-1, 0);
            Stack<EdgeStruct> s = new Stack<EdgeStruct>();

            //Dictionary<int, int> edht = new Dictionary<int, int>();
            int eidCounter = 0;
            edht.Add(eidCounter++, 0);
            int stn = this.stn;
            List<Edge> edgeCol = Edge.FindAll1(stn);
            foreach (Edge e in edgeCol)
            {
                EdgeStruct es = new EdgeStruct();
                es.e = e;
                es.pnIndexesIndex = 0;
                s.Push(es);
            }
            edgeCol.Clear();
            edgeCol = null;

            while (s.Count != 0)
            {
                EdgeStruct es = s.Pop();
                if (es.e.value != -1)  // node has already been marked
                {
                    edht[es.pnIndexesIndex] += edht[es.e.eid];
                    es.e.value = -1;  // commented to keep it useful for visitNode4
                }
                else        // node has not been marked yet
                {
                    //int myval = 0;
                    if (es.e.last_char_index == (N - 1))
                    {
                        edht[es.pnIndexesIndex]++;
                    }
                    else
                    {
                        es.e.value = 0;//myval;
                        edht.Add(eidCounter, 0);
                        es.e.eid = eidCounter++;
                        s.Push(es);
                        stn = es.e.end_node;
                        List<Edge> eCol = Edge.FindAll1(stn);
                        eCol.Sort(EdgeComparer);
                        foreach (Edge e in eCol)
                        {
                            EdgeStruct es1 = new EdgeStruct();
                            es1.e = e;
                            es1.pnIndexesIndex = es.e.eid;
                            s.Push(es1);
                        }
                        eCol.Clear();
                        eCol = null;
                    }
                }
            }
            /*long ccc = 0;
            for (int i = 0; i < edht.Count; i++)
            {
                ccc += edht[i - 1];
            }
            Console.WriteLine("CCC: " + ccc + "Time: " + DateTime.Now);
             */
        }

        long calculatePeriodCounter = 0;
        int periodColExistCounter = 0;
        long outerLoopCounterBeforeCheck = 0;
        long outerLoopCounterAfterCheck = 0;
        long innerLoopCounter = 0;
        public long diffCounter = 0;
        public List<int> ovl = new List<int>();


        public void CalculatePeriodImp(Edge e)
        {
            if (e.value > Program.maxStrLen || e.value < Program.minStrLen)
                return;

            OccurNode current = e.st;

            /* *********** Extracting data for complexity improvement experiments **************
             * Date: 10th Sep 2008, 5:54 AM 
             */
            //if (e.value < 35)
            //{
                for (int i = 0; i < e.len; i++)
                {
                    ovl.Add(current.value);
                    current = current.next;
                }

                current = e.st;
                Program.CalculatePeriod(ovl.ToArray(), e.value);
                ovl.Clear();
                return;
            //}
            /************ End extracting data for complexity improvement experiments ************/

            //     code to find the last value of occur_vector - 2nd nov 2007
            OccurNode last = e.st;
            for (int k = 1; k < e.len; k++)
            {
                last = last.next;
            }
            int lastOccurValue = last.value;
            // end code to find the last value of occur_vector - 2nd nov 2007
            int preDiffValue = -5; // introduced to check the repetitive stPos periods like 0,3,6,9
            calculatePeriodCounter++;


            for (int i = 1; i < e.len; i++)
            {
                outerLoopCounterBeforeCheck++;
                //int diffValue = occur[i + 1] - occur[i];
                int diffValue = current.next.value - current.value;
                /******* Modifying temporarily to check periodicity of 3 
                if (diffValue < 5 || diffValue < e.value || diffValue == 1 || diffValue > (0.33 * T.Length) || current.value > (0.5 * T.Length))
                //*********** End Modifying to check 3 *********/
                /* replacing with line below for packet data
                if (diffValue < 5 || diffValue < e.value  || diffValue > (0.33 * T.Length) || current.value > (0.5 * T.Length) || diffValue == preDiffValue)
                */
                if (diffValue < 2 || diffValue < e.value || diffValue > 20 || diffValue == preDiffValue)
                {
                    diffCounter++;
                    current = current.next;
                    continue;
                }
                Period p = new Period();
                p.periodValue = diffValue;
                p.stPos = current.value;
                p.endPos = lastOccurValue; // introduced to find periodicity in segments - 2nd nov 2007
                p.fci = p.stPos;
                p.length = e.value;
                preDiffValue = diffValue;

                ///*****commenting temporarily - 5th nov 2007 *********
                if (PeriodCollection.Exist(p))
                {
                    periodColExistCounter++;
                    current = current.next;
                    continue;
                }
                //****** end commenting ********/

                outerLoopCounterAfterCheck++;
                /*********** not required after new code aug 27
                int modRes = p.stPos % p.periodValue;
                 */
                p.foundPosCount = 0;

                /******** NEW CODE Aug 27 ************/
                int A, C = 0;
                double B = 0, sumPerVal = 0;

                int preSubCurValue = -5;
                //int preStPos = p.stPos;
                int currStPos = p.stPos;
                /***** END NEW CODE Aug 27 *********/

                OccurNode subCurrent = current;
                for (int j = i; j <= e.len; j++)
                {
                    innerLoopCounter++;


                    ///******** NEW CODE Aug 27 ***********
                    //A = subCurrent.value - p.stPos;
                    A = subCurrent.value - currStPos;
                    B = Math.Round((double)A / p.periodValue);
                    C = A - (p.periodValue * (int)B);
                    if (C >= (-1 * tolWin) && C <= tolWin)
                    {
                        //if (Math.Round((double)((preSubCurValue - p.stPos) / p.periodValue)) != B)
                        if (Math.Round((double)((preSubCurValue - currStPos) / p.periodValue)) != B) // if it is a valid periodic occurence
                        {
                            preSubCurValue = subCurrent.value;
                            //preStPos = currStPos;           // new lines
                            currStPos = subCurrent.value;   // new lines
                            p.foundPosCount++;
                            sumPerVal += (p.periodValue + C);   // new lines
                        }
                    }
                    //***** END NEW CODE Aug 27 *********/

                    /**************** commenting the original one 
                    if (modRes == subCurrent.value % p.periodValue)
                      p.foundPosCount++;
                     ********** end comment ***/

                    ///***** introducing dmax and mu concept - Nov 4 2007
                    if (j != i && j < e.len &&  // if its atleast the 2nd occurence or at maximum 2nd last occurence
                        (subCurrent.next.value - subCurrent.value) >= (dmax /* * p.periodValue*/) && // if difference between this and previous occurence is greater than dmax times period value
                        (currStPos - p.stPos) >= (minLengthOfSegment * (T.Length - 1)/*p.periodValue*/)) // if the length of to be segment is greater than minLengthOfSegment times period value
                    //(subCurrent.value - p.stPos) >= (minLengthOfSegment * p.periodValue)) // if the length of to be segment is greater than minLengthOfSegment times period value
                    {
                        /*if (currStPos == subCurrent.value)  // if this occurence was match
                        {
                            p.endPos = subCurrent.value;
                        }
                        else
                        {
                            p.endPos = currStPos;           // set to the last matched position
                        }
                         */

                        p.endPos = currStPos;           // set to the last matched position

                        break;
                    }
                    //****** end dmax and mu code - Nov 4 2007 **********/


                    subCurrent = subCurrent.next;
                }

                double y = 0;
                /*** commented and replaced with the lines below to avoid > 1 conf using avgPeriodValue instead of periodValue
                if (((T.Length - 1 - p.stPos) % p.periodValue) >= e.value) y = 1; else y = 0;
                double th1 = p.foundPosCount / Math.Floor(((double)(T.Length - 1 - p.stPos) / p.periodValue) + y);
                 */

                ///*** used p.avgPeriodValue instead of p.periodValue in threshold (conf) calculation to avoid th > 1
                /// also added periodicity for segments code by replacing T.length - 1 with lastOccurValue + p.length
                p.avgPeriodValue = (sumPerVal - p.periodValue) / (p.foundPosCount - 1);
                /************ changed lastOccurValue with p.endPos in the following lines ***********
                if (((lastOccurValue + p.length - p.stPos) % ((int)Math.Round(p.avgPeriodValue))) >= e.value) y = 1; else y = 0;
                double th1 = p.foundPosCount / Math.Floor(((double)(lastOccurValue + p.length - p.stPos) / p.avgPeriodValue) + y);
                *********end changed lastOccurValue with p.endPos in the above lines *****************/
                if (((p.endPos + p.length - p.stPos) % ((int)Math.Round(p.avgPeriodValue))) >= e.value) y = 1; else y = 0;
                double th1 = p.foundPosCount / Math.Floor(((double)(p.endPos + p.length - p.stPos) / p.avgPeriodValue) + y);
                //*/
                p.threshold = th1;

                if (p.threshold >= this.minThreshold) //&& (!IsPeriodExist(p)))
                {
                    AddPeriodCounter++;
                    //p.avgPeriodValue = (sumPerVal-p.periodValue) / (p.foundPosCount-1); // moving this to upper lines
                    PeriodCollection.Add(p.periodValue, p);
                    //break;

                    //periods.Add(p);
                }
                current = current.next;
            }
        }


        public void AddOccurNodes(Edge eSource, Edge eDest)
        {
            if (eDest.st == null)
            {
                eDest.st = eSource.st;
                eDest.len = eSource.len;
            }
            else
            {
                OccurNode on = eDest.st;
                while (on.next != null)
                {
                    on = on.next;
                }

                on.next = new OccurNode();
                on.next.next = eSource.st.next;
                //on.next.previous = on.next;
                on.next.value = eSource.st.value;
                eDest.len += eSource.len;
            }

        }

        public bool StartNode(Edge e)
        {
            return e.start_node == stn;
        }

        public bool IsPeriodExist(Period pin)
        {
            PeriodExistCounter++;

            foreach (Period p in periods)
            {
                if (p.periodValue == pin.periodValue)
                {
                    if (p.stPos == pin.stPos)
                    {
                        //if (p.symbolString.Equals(pin.symbolString))
                        if (p.length == pin.length)
                        {
                            return true;
                        }
                        //else if (p.symbolString.Length > pin.symbolString.Length)
                        if (p.length > pin.length)
                        {
                            return true;
                        }
                    }
                    //else if (p.symbolString.Equals(pin.symbolString) && ((pin.stPos % pin.periodValue) == (p.stPos % pin.periodValue)))
                    else if (((T.Substring(p.fci, p.length)).Equals(T.Substring(pin.fci, pin.length)))
                                && ((pin.stPos % pin.periodValue) == (p.stPos % pin.periodValue)))
                    {
                        if (pin.stPos < p.stPos)
                        {

                            p.stPos = pin.stPos;
                            p.foundPosCount = pin.foundPosCount;
                            p.threshold = pin.threshold;
                            //p.symbolString = pin.symbolString;
                            p.fci = pin.fci;
                            p.length = pin.length;
                        }
                        return true;
                    }
                }
            }
            return false;
        }


        public Edge FindEdge(int endNode)
        {
            foreach (Edge e in Edge.Edges)
            {
                if (e.end_node == endNode)
                {
                    return e;
                }
            }
            return null;
        }

        public int stn = 0;

    }
}
