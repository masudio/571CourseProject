using System;
using System.Collections.Generic;
using System.Text;

namespace Periodicity_Detection__Complexity_Improvement_
{
    public class Prime
    {
        private int current;

        public Prime()
            : this(0)
        {

        }

        public Prime(int lowerBound)
        {
            current = lowerBound;
        }

        public int next()
        {
            for (; !isPrime(current); current++) ;
            current++;
            return current - 1;
        }

        public bool isPrime(int n)
        {
            if (n < 2)
                return false;
            for (int i = 2; i <= (int)Math.Sqrt(n * 1.0); i++)
                if (n == i * (n / i))
                    return false;

            return true;
        }

        public static void Main(string[] args)
        {
            Prime primeNumbers1 = new Prime(2000000);
            for (int i = 0; i < 10; i++)
                Console.WriteLine(primeNumbers1.next());

        }

    }
}
