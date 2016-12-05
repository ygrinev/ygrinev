using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnippets
{
    class Program
    {
        static void GetPrimesInRange()
        {
            int topN, btmN;
            while (true)
            {
                Console.WriteLine("Give the comma-separated range to find all primes in:");
                string[] range = Console.ReadLine().Split(',');
                if (range.Length < 2)
                    break;
                if (int.TryParse(range[0], out btmN) && int.TryParse(range[1], out topN))
                {
                    Console.Write("Primes:  ");
                    for (int i = btmN; i <= topN; i++)
                        Console.Write(IsPrime(i) ? (i.ToString() + ',') : "");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Please provide a valid integer range!");
                }
            }
        }
        static bool IsPrime(int num)
        {
            bool isPrime = true;
            for (int i = 2; i * i <= num && isPrime; i++)
                isPrime = num % i != 0;
            return isPrime;
        }
        static void Main(string[] args)
        {
            GetPrimesInRange();
        }
    }
}
