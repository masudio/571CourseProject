namespace Periodicity_Detection__Complexity_Improvement_
{
    class OccurList
    {
        public int Length = 0;
        public OccurNode firstNode = null;
        public OccurNode currentNode = null;
        public OccurList()
        {
            firstNode = new OccurNode();
            firstNode.value = -1;   // an identification
            currentNode = firstNode;
        }

        public void Add(int value, OccurNode[] pnStart, int[] pnLength)
        {
            OccurNode n = new OccurNode();
            n.value = value;

            if (pnStart[0] == null)
            {
                /*
                if (currentNode.next != null)
                {
                    Console.WriteLine("current node is not the last node");
                }
                 */
                currentNode.next = n;
                n.previous = currentNode;
                currentNode = n;
                pnStart[0] = n;
                pnLength[0] = 1;
            }
            else
            {
                bool flag = false;
                OccurNode k = pnStart[0];
                for (int i = 1; i <= pnLength[0]; i++)
                {
                    if (k.value > value)
                    {
                        n.previous = k.previous;
                        n.next = k;
                        k.previous.next = n;
                        k.previous = n;
                        if (k == pnStart[0])
                        {
                            pnStart[0] = n;
                        }
                        flag = true;
                        break;
                    }
                    if (i != pnLength[0])
                    {
                        k = k.next;
                    }
                }
                if (!flag)
                {
                    k.next = n;
                    n.previous = k;
                    //currentNode.next = n;
                    //n.previous = currentNode;
                    currentNode = n;
                }
                pnLength[0] += 1;
            }

            this.Length++;
        }


        public void Sort(OccurNode[] pnOccurStart, int pnOccurLength, OccurNode[] occurStart, int occurLength)
        {
            bool flag = false;
            OccurNode prePnOccSt = pnOccurStart[0];
            OccurNode currOccSt = occurStart[0];
            OccurNode currPnOccSt = pnOccurStart[0];
            int j = 1;
            OccurNode temp;
            for (int i = 1; i <= occurLength; i++)
            {
                /*
                if (currOccSt == currentNode)
                {
                    Console.WriteLine("current node in the middle found, i=" + i);
                }
                 */
                temp = currOccSt.next;
                for (; j <= pnOccurLength; j++)
                {
                    /*
                    if (currPnOccSt == currentNode)
                    {
                        Console.WriteLine("current node in the middle found, j=" + j);
                    }
                     */
                    if (currPnOccSt.value > currOccSt.value)
                    {
                        //OccurNode temp = currOccSt.previous;
                        if (currOccSt.next != null)
                        {
                            currOccSt.next.previous = currOccSt.previous;
                        }
                        else
                        {
                            flag = true;
                        }
                        currOccSt.previous.next = currOccSt.next;

                        currOccSt.previous = currPnOccSt.previous;
                        currOccSt.previous.next = currOccSt;

                        currOccSt.next = currPnOccSt;
                        currPnOccSt.previous = currOccSt;
                        if (currPnOccSt == prePnOccSt)
                        {
                            pnOccurStart[0] = currOccSt;
                            prePnOccSt = currOccSt;
                        }
                        //currOccSt = temp;
                        break;
                    }
                    currPnOccSt = currPnOccSt.next;
                }
                currOccSt = temp;
            }
            if (flag)
            {
                while (currPnOccSt.next != null)
                {
                    currPnOccSt = currPnOccSt.next;
                }
                currentNode = currPnOccSt;
            }
        }
        //es.pnOccurStart, es.pnOccurLength[0], es.occurStart, es.occurLength[0]);
    }
}