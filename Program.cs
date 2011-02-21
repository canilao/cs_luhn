using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Luhn
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid guid = Guid.NewGuid();
            string guidAsString = "";

            while (true)
            {
                byte[] guidAsBytes = guid.ToByteArray();
                BigInteger guidAsInt = new BigInteger(guidAsBytes);
                guidAsString = guid.ToString("D");

                if (IsValid(guidAsInt.ToString())) break;

                guid = Guid.NewGuid();
            }

            Console.Out.WriteLine(guidAsString);

            bool breakOut = false;
            while (!breakOut)
            {
                Console.Out.WriteLine("Enter the guid:");
                string entered = Console.In.ReadLine();

                if (entered == "end")
                {
                    break;
                }

                Guid enteredGuid;
                try
                {
                    enteredGuid = Guid.Parse(entered);
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.Message);
                    continue;
                }

                byte[] enteredGuidAsBytes = enteredGuid.ToByteArray();
                BigInteger enteredGuidAsInt = new BigInteger(enteredGuidAsBytes);

                if (IsValid(enteredGuidAsInt.ToString()))
                {
                    Console.Out.WriteLine("GUID was entered correctly");
                }
                else
                {
                    Console.Out.WriteLine("GUID was entered incorrectly");
                }
            }
        }

        public static int ComputeChecksum(string value)
        {
            return value.Where(c => Char.IsDigit(c))
                        .Reverse()
                        .SelectMany((c, i) => ((c - '0') << (i & 1)).ToString())
                        .Sum(c => c - '0') % 10;
        }

        public static bool IsValid(string value)
        {
            return ComputeChecksum(value) == 0;
        }
    }
}
