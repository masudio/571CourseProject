namespace Periodicity_Detection__Complexity_Improvement_
{
    //  The only information contained in a node is the
    //  suffix link. Each suffix in the tree that ends
    //  at a particular node can find the next smaller suffix
    //  by following the suffix_node link to a new node.  Nodes
    //  are stored in a simple array.
    public class Node
    {
        public int idx;
        public int suffix_node;
        public Node() { suffix_node = -1; }
        public static int Count;
    }
}