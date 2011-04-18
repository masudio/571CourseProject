namespace Periodicity_Detection__Complexity_Improvement_
{
    class EdgeStruct
    {
        public Edge e;
        public int tabs;
        public int pnValue;
        /* commenting in order to save memory - 8th nov 2007
        public int indexesIndex = -1;
         ********* end commenting in order to save memory ******/
        public int pnIndexesIndex;

        public OccurNode[] occurStart = new OccurNode[1] { null };
        public int[] occurLength = new int[] { -1 };

        public OccurNode[] pnOccurStart = new OccurNode[1] { null };
        public int[] pnOccurLength = new int[1] { -1 };

        //public int[] occurIndexes = new int[2] { -1, -1 };
        //public int[] pnOccurIndexes = new int[2] { -1, -1 };
    }
}