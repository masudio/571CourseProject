namespace Periodicity_Detection__Complexity_Improvement_
{
    class Suffix
    {
        public static string T;
        public int origin_node;
        public int first_char_index;
        public int last_char_index;

        public Suffix(int node, int start, int stop)
        {
            this.origin_node = node;
            this.first_char_index = start;
            this.last_char_index = stop;
        }

        public bool Explicit() { return first_char_index > last_char_index; }
        public bool Implicit() { return last_char_index >= first_char_index; }

        // A suffix in the tree is denoted by a Suffix structure
        // that denotes its last character.  The canonical
        // representation of a suffix for this algorithm requires
        // that the origin_node by the closest node to the end
        // of the tree.  To force this to be true, we have to
        // slide down every edge in our current path until we
        // reach the final node.
        public void Canonize()
        {
            if (!Explicit())
            {
                Edge edge = Edge.Find(origin_node, T[first_char_index]);
                int edge_span = edge.last_char_index - edge.first_char_index;
                while (edge_span <= (last_char_index - first_char_index))
                {
                    first_char_index = first_char_index + edge_span + 1;
                    origin_node = edge.end_node;
                    if (first_char_index <= last_char_index)
                    {
                        edge = Edge.Find(edge.end_node, T[first_char_index]);
                        edge_span = edge.last_char_index - edge.first_char_index;
                    };
                }
            }
        }

    }
}