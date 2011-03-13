using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Periodicity_Detection__Complexity_Improvement_
{
    class FileReading
    {
        public static List<int> ReadIntFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            List<int> nums = new List<int>();
            while (sr.Peek() >= 0)
            {
                nums.Add(int.Parse(sr.ReadLine()));
            }
            sr.Close();
            fs.Close();
            return nums;
        }

        public static string IntroduceMissing(string seq, List<int> nums)
        {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < nums.Count; i++)
            {
                for (int j = 0; j < nums[i]; j++)
                {
                    res.Append("X");
                }
                res.Append(seq[i]);
            }
            return res.ToString();
        }

        public static void WriteFile(string path, string s)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(s);

            sw.Close();
            fs.Close();
        }

        public static string ReadFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string res = sr.ReadToEnd();
            res = res.Replace("\r\n", "");
            sr.Close();
            fs.Close();
            return res;
        }
    }
}
