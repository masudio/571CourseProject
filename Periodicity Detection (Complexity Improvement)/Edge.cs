using System;
using System.Collections.Generic;

namespace Periodicity_Detection__Complexity_Improvement_
{
    //
    // The suffix tree is made up of edges connecting nodes.
    // Each edge represents a string of characters starting
    // at first_char_index and ending at last_char_index.
    // Edges can be inserted and removed from a hash table,
    // based on the Hash() function defined here.  The hash
    // table indicates an unused slot by setting the
    // start_node value to -1.
    //
    public class Edge : IComparable
    {
        public int eid = -1;
        //public int st = -1;
        //public int end = -1;

        public OccurNode st = null;
        public int len = 0;
        /*** causing memory error with t.length>5mb
        public List<int> indexes = new List<int>();
         */
        public int value = -1;
        public int CompareTo(object obj)
        {
            if (obj is Edge)
            {
                Edge e = (Edge)obj;
                return start_node.CompareTo(e.start_node);
            }
            throw new ArgumentException("object is not an Edge");
        }

        public static string T;
        // This is the hash table where all the currently
        // defined edges are stored.  You can dump out
        // all the currently defined edges by iterating
        // through the table and finding edges whose start_node
        // is not -1.
        //Edge Edges[ HASH_TABLE_SIZE ];
        //public static List<Edge> Edges = new List<Edge>();

        public static int HASH_TABLE_SIZE = 0;
        public static Edge[] Edges = null;
        public static int firstEdgesIndex = -1;
        public static int[] stNodeArray;

        public int first_char_index;
        public int last_char_index;
        public int end_node;
        public int start_node;

        // The default ctor for Edge just sets start_node
        // to the invalid value.  This is done to guarantee
        // that the hash table is initially filled with unused
        // edges.
        public Edge()
        {
            start_node = -1;
        }

        // I create new edges in the program while walking up
        // the set of suffixes from the active point to the
        // endpoint.  Each time I create a new edge, I also
        // add a new node for its end point.  The node entry
        // is already present in the Nodes[] array, and its
        // suffix node is set to -1 by the default Node() ctor,
        // so I don't have to do anything with it at this point.
        public Edge(int init_first, int init_last, int parent_node)
        {
            first_char_index = init_first;
            last_char_index = init_last;
            start_node = parent_node;
            end_node = Node.Count++;
        }

        // Edges are inserted into the hash table using this hashing
        // function.

        /*replacing int with long to avoid it getting -ve after shifts - nov 7 2007*/
        public static int Hash(long node, int c)
        {
            return (int)(((node << 8) + c) % HASH_TABLE_SIZE);
        }

        // A given edge gets a copy of itself inserted into the table
        // with this function.  It uses a linear probe technique, which
        // means in the case of a collision, we just step forward through
        // the table until we find the first unused slot.
        public void Insert()
        {
            //Edges.Add(this);

            int i = Hash(start_node, T[first_char_index]);
            while (Edges[i].start_node != -1)
                i = ++i % HASH_TABLE_SIZE;
            ///**** changed: memberwise copy
            // Edges[ i ] = this;
            Edges[i] = new Edge();
            Edges[i].start_node = this.start_node;
            Edges[i].end_node = this.end_node;
            Edges[i].first_char_index = this.first_char_index;
            Edges[i].last_char_index = this.last_char_index;
            Edges[i].value = this.value;
            /*** causing memory error with t.length>5mb
            Edges[i].indexes = new List<int>();
             */
        }

        // Removing an edge from the hash table is a little more tricky.
        // You have to worry about creating a gap in the table that will
        // make it impossible to find other entries that have been inserted
        // using a probe.  Working around this means that after setting
        // an edge to be unused, we have to walk ahead in the table,
        // filling in gaps until all the elements can be found.
        // Knuth, Sorting and Searching, Algorithm R, p. 527
        public void Remove()
        {
            //Edges.Remove(this);

            int i = Hash(start_node, T[first_char_index]);
            while (Edges[i].start_node != start_node ||
                   Edges[i].first_char_index != first_char_index)
            {
                i = ++i % HASH_TABLE_SIZE;
            }
            for (; ; )
            {
                //Edges[ i ].start_node = -1;
                Edges[i] = new Edge();
                int j = i;
                for (; ; )
                {
                    i = ++i % HASH_TABLE_SIZE;
                    if (Edges[i].start_node == -1)
                        return;
                    int r = Hash(Edges[i].start_node, T[Edges[i].first_char_index]);
                    if (i >= r && r > j)
                        continue;
                    if (r > j && j > i)
                        continue;
                    if (j > i && i >= r)
                        continue;
                    break;
                }
                // copying member wise
                //Edges[ j ] = Edges[ i ];
                Edges[j] = new Edge();
                Edges[j].start_node = Edges[i].start_node;
                Edges[j].end_node = Edges[i].end_node;
                Edges[j].first_char_index = Edges[i].first_char_index;
                Edges[j].last_char_index = Edges[i].last_char_index;
                Edges[j].value = Edges[i].value;
                /*** causing memory error with t.length>5mb
                Edges[j].indexes =          new List<int>();
                 */

            }

        }

        // The whole reason for storing edges in a hash table is that it
        // makes this function fairly efficient.  When I want to find a
        // particular edge leading out of a particular node, I call this
        // function.  It locates the edge in the hash table, and returns
        // a copy of it.  If the edge isn't found, the edge that is returned
        // to the caller will have start_node set to -1, which is the value
        // used in the hash table to flag an unused entry.
        public static Edge Find(int node, char c)
        { 
            /*foreach (Edge edge in Edges)
            {
                if (edge.start_node == node)
                {
                    if (c == T[edge.first_char_index])
                        return edge;
                }
            }
            return null;
            */

            int i = Hash(node, c);
            for (; ; )
            {
                if (Edges[i].start_node == node)
                    if (c == T[Edges[i].first_char_index])
                        return Edges[i];
                if (Edges[i].start_node == -1)
                    return Edges[i];
                i = ++i % HASH_TABLE_SIZE;
            }
        }

        public static int Hash(int node)
        {
            return (node << 8) % HASH_TABLE_SIZE;
        }

        public static List<Edge> FindAll(int st_node)
        {
            List<Edge> result = new List<Edge>();
            int i = Hash(st_node);
            for (; ; )
            {
                if (Edges[i].start_node == st_node)
                {
                    result.Add(Edges[i]);
                    //if (c == T[Edges[i].first_char_index])
                    //    return Edges[i];
                }
                if (Edges[i].start_node == -1)
                {
                    return result;
                    //return Edges[i];
                }
                i = ++i % HASH_TABLE_SIZE;
            }
        }

        public static List<Edge> FindAll1(int st_node)
        {
            SuffTree.FindAll1Counter++;
            /*if (SuffTree.FindAll1Counter == 100000)
            {
                Console.WriteLine("5% more done");
                SuffTree.FindAll1Counter = 0;
            }
             */

            int idx = FindIndex(st_node);

            /*Edge ed = new Edge();
            ed.start_node = st_node;
            int idx = Array.BinarySearch(Edges, ed);
             */
            List<Edge> result = new List<Edge>();
            if (idx < 0)
            {
                return result;
            }

            for (int i = idx; ; i--)
            {
                if (Edges[i].start_node < st_node)
                {
                    idx = i + 1;
                    break;
                }
            }

            for (int i = idx; i < Edges.Length; i++)
            {
                if (Edges[i].start_node > st_node)
                {
                    break;
                }
                else if (Edges[i].start_node == st_node)
                {
                    result.Add(Edges[i]);
                }
                //if (e.start_node == st_node)
                //    result.Add(e);
            }
            return result;
        }

        public static int FindIndex(int st_node)
        {
            return Array.BinarySearch(stNodeArray, st_node);
        }

        public static int FindFirstEdgeIndex()
        {
            for (int i = 0; i < Edges.Length; i++)
            {
                if (Edges[i].start_node != -1)
                {
                    firstEdgesIndex = i;
                    break;
                }
            }

            Edge[] nEdges = new Edge[Edges.Length - firstEdgesIndex + 1];
            for (int k = firstEdgesIndex - 1, c = 0; k < Edges.Length; k++, c++)
            {
                nEdges[c] = Edges[k];
            }
            Edges = nEdges;

            return firstEdgesIndex;
        }


        // When a suffix ends on an implicit node, adding a new character
        // means I have to split an existing edge.  This function is called
        // to split an edge at the point defined by the Suffix argument.
        // The existing edge loses its parent, as well as some of its leading
        // characters.  The newly created edge descends from the original
        // parent, and now has the existing edge as a child.
        //
        // Since the existing edge is getting a new parent and starting
        // character, its hash table entry will no longer be valid.  That's
        // why it gets removed at the start of the function.  After the parent
        // and start char have been recalculated, it is re-inserted.
        // The number of characters stolen from the original node and given
        // to the new node is equal to the number of characters in the suffix
        // argument, which is last - first + 1;
        public int SplitEdge(Suffix s)
        {
            Remove();
            Edge new_edge =
                new Edge(first_char_index,
                         first_char_index + s.last_char_index - s.first_char_index,
                         s.origin_node);
            new_edge.Insert();
            //SuffTree.FindNode(new_edge.end_node).suffix_node = s.origin_node;
            SuffTree.Nodes[new_edge.end_node].suffix_node = s.origin_node;
            first_char_index += s.last_char_index - s.first_char_index + 1;
            start_node = new_edge.end_node;
            Insert();
            return new_edge.end_node;
        }

        public static void Sort()
        {
            Array.Sort(Edges);
            FindFirstEdgeIndex();

            stNodeArray = new int[Edges.Length];
            for (int i = 0; i < Edges.Length; i++)
            {
                stNodeArray[i] = Edges[i].start_node;
            }
        }

        public static bool StopSubstringNodeSearch; // used in FindSubstring algorithm (PrefixMatch)

        /**
         * Checks if this edge's string is a prefix of the searched-for substring.
         */
        public bool PrefixMatch(string theSubstring)
        {
            StopSubstringNodeSearch = false;

            var edgeStringLength = this.last_char_index - this.first_char_index + 1;

            if(theSubstring.Length < edgeStringLength)
            {
                // need some way to tell caller that IF this returns true, then this node is
                // THE substring node, and not to go further down in tree
                StopSubstringNodeSearch = true;

                return theSubstring.Equals(
                    T.Substring(this.first_char_index, theSubstring.Length));
            }

            for(int i = 0; i < edgeStringLength; i++)
            {
                if (!T[i].Equals(theSubstring[i]))
                    return false;
            }

            return true;
        }
    }
}