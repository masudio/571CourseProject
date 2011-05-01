using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periodicity_Detection__Complexity_Improvement_;



namespace ArrayTests
{
    [TestClass]
    public class ArrayTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

         [TestMethod]
        public void successfulStringsArrays()
        {
            string testString = "abracadabra";
            SuffixArray theSuffixArray = new SuffixArray(testString);
            bool result = theSuffixArray.FindSubstring("bra");
            Assert.AreEqual(true, result);

            result = theSuffixArray.FindSubstring("ab");
            Assert.AreEqual(true, result);

            result = theSuffixArray.FindSubstring("cada");
            Assert.AreEqual(true, result);

            result = theSuffixArray.FindSubstring("dab");
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void failedStringsArrays()
        {
            string testString = "abracadabra";
            SuffixArray theSuffixArray = new SuffixArray(testString);
            bool result;

            result = theSuffixArray.FindSubstring("jason");
            Assert.AreEqual(false, result);

            result = theSuffixArray.FindSubstring("masud");
            Assert.AreEqual(false, result);

            result = theSuffixArray.FindSubstring("testing");
            Assert.AreEqual(false, result);

            result = theSuffixArray.FindSubstring("notastring");
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void nullTestingArrays()
        {
            string nonNullString = "abracadabra";
            SuffixArray theSuffixArray = new SuffixArray(nonNullString);
            bool result = theSuffixArray.FindSubstring(null);
            Assert.AreEqual(false, result);

            result = theSuffixArray.FindSubstring("");
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void testsuccessfulFindAllStringsArrays()
        {
            string theString = "abracadabraababbra";
            SuffixArray theArray = new SuffixArray(theString);
            List<int> result = new List<int>(theArray.FindAllOccurrences("bra"));
            List<int> correct = new List<int>();
            correct.Add(1); correct.Add(8); correct.Add(15);
            //Assert.AreEqual(result, correct);
            Assert.AreEqual(result.Count(), 3);

            result.Clear(); correct.Clear();

            result = new List<int>(theArray.FindAllOccurrences("brac"));
            Assert.AreEqual(result.Count(), 1);
            correct.Add(1);
            //Assert.AreEqual(result, correct);

            result.Clear(); correct.Clear();

            result = new List<int>(theArray.FindAllOccurrences("ab"));
            correct.Add(0); correct.Add(7); correct.Add(11); correct.Add(13);
            //Assert.AreEqual(result, correct);
            Assert.AreEqual(result.Count(), 4);
        }

        [TestMethod]
        public void testfailedFindAllOccurrencesArrays()
        {
            string theString = "abracadabraababbra";
            SuffixArray theArray = new SuffixArray(theString);
            List<int> result = new List<int>(theArray.FindAllOccurrences("jason"));
            Assert.AreEqual(result.Count(), 0);

            result = new List<int>(theArray.FindAllOccurrences("testword"));
            Assert.AreEqual(result.Count(), 0);
        }
    }
}
