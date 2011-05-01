using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Periodicity_Detection__Complexity_Improvement_;



namespace Test {

    [TestFixture]
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
            string fn = "data/abracadabra.data";
            s = "abcabcabcabcdefghiihgabcabababab$";

            st = new SuffTree(s, minTh, tolWin, dmax, minLengthSegment,
                path + fn + "-Th=" + minTh + ", TolWin=" + tolWin, -2);
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

        [Test]
        public void ShouldFindExistingSubstring()
        {
            Assert.True(st.FindSubstring("defghii"));
            Assert.True(st.FindSubstring("bcabc"));
            Assert.True(st.FindSubstring("bab"));
            Assert.True(st.FindSubstring("bcabababab"));
            Assert.True(st.FindSubstring("ghiih"));
        }

        [Test]
        public void ShouldNotFindNonExistingSubstring()
        {
            Assert.False(st.FindSubstring("aaab"));
        }

        [Test]
        public void ShouldFindAllSubstringOccurences()
        {
            var substringOccurences = st.FindAllOccurrences("abc");
            Assert.AreEqual(5, substringOccurences.Count());
            substringOccurences = st.FindAllOccurrences("c");
            Assert.AreEqual(5, substringOccurences.Count());
            substringOccurences = st.FindAllOccurrences("def");
            Assert.AreEqual(1, substringOccurences.Count());
            substringOccurences = st.FindAllOccurrences("aba");
            Assert.AreEqual(3, substringOccurences.Count());
            substringOccurences = st.FindAllOccurrences("bc");
            Assert.AreEqual(5, substringOccurences.Count());
        }

        [Test]
        public void ShouldNotFindSubstringsThatDontExistInTree()
        {
            var substringOccurences = st.FindAllOccurrences("jason");
            Assert.AreEqual(0, substringOccurences.Count());
            substringOccurences = st.FindAllOccurrences("abbbb");
            Assert.AreEqual(0, substringOccurences.Count());
            substringOccurences = st.FindAllOccurrences("abcdd");
            Assert.AreEqual(0, substringOccurences.Count());
            substringOccurences = st.FindAllOccurrences("z");
            Assert.AreEqual(0, substringOccurences.Count());
        }
    }
}
