using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Periodicity_Detection__Complexity_Improvement_;



namespace Test {

    [Test]
    class Test {

        public static String s = ""; //"abcabcabbabcabcabc" + "$";
        static double minTh = 0.7;
        static double minLengthSegment = 0.9;
        static int minPeriod = 1;
        static int maxPeriod = 500;
        public static int minStrLen = 1;
        public static int maxStrLen = 100000;
        static int tolWin = 0;
        static int dmax = 10000;
        public static List<CPeriod> periodCollection = new List<CPeriod>();
        public static int preCountPerCol = periodCollection.Count;
        public static int candPerCount = 0, addPerCount = 0;
        public static int occVecCount = 0, occVecAddCount = 0;

        public SuffTree st;

        static void Main(string[] args) {
        }

        [SetUp]
        public void setup()
        {

            string path = @"D:\CPSC\CPSC571\571CourseProject\";

            periodCollection = new List<CPeriod>();
            preCountPerCol = periodCollection.Count;
            candPerCount = 0; addPerCount = 0; occVecCount = 0;
            string fn = "data/abracadabra.data";
            FileStream fs = new FileStream(path + fn, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            s = sr.ReadToEnd().Trim() + "$";

            st = new SuffTree(s, minTh, tolWin, dmax, minLengthSegment,
                path + fn + "-Th=" + minTh + ", TolWin=" + tolWin, -2);
/*
            Console.WriteLine(s);
            Console.WriteLine("0123456789012345678901234567890123456789");
            Console.WriteLine();

            Console.WriteLine("Candidate Period Count: " + candPerCount);
            Console.WriteLine("Added Period Count: " + addPerCount);
            Console.WriteLine("Occur Vector Count: " + occVecCount);
            Console.WriteLine();*/

        }

        [Test]
        public void ShouldFindOriginEdges()
        {
            var edgeList = st.GetEdgesWithSource(0);
            int i;

            for (i = 0; i < edgeList.Count; i++)
            {
                Assert.AreEqual(0, edgeList[i].start_node);
            }

            Assert.AreNotEqual(0, i);
        }

        [Test]
        public void ShouldFindNodeAtEndOfSubstring()
        {
            Assert.AreNotEqual(-1, st.FindSubstringNode("ab", 0, st.GetEdgesWithSource(0)));
        }
    }
}
