using System;

namespace Luhn
{
    class Luhn16Guid
    {
        private static int CodePointFromCharacter(char character)
        {
            return Convert.ToInt32(character.ToString(), 16);
        }

        private static char CharacterFromCodePoint(int codePoint)
        {
            return Char.Parse(String.Format("{0:X}", codePoint));
        }

        private static char GenerateCheckCharacter(string input)
        {
            int factor = 2;
            int sum = 0;
            int n = 16;

            // Starting from the right and working leftwards is easier since 
            // the initial "factor" will always be "2" 
            for (int i = input.Length - 1; i >= 0; --i)
            {
                int codePoint = CodePointFromCharacter(input[i]);
                int addend = factor * codePoint;

                // Alternate the "factor" that each "codePoint" is multiplied by
                factor = (factor == 2) ? 1 : 2;

                // Sum the digits of the "addend" as expressed in base "n"
                addend = (addend / n) + (addend % n);
                sum += addend;
            }

            // Calculate the number that must be added to the "sum" 
            // to make it divisible by "n"
            int remainder = sum % n;
            int checkCodePoint = n - remainder;
            checkCodePoint %= n;

            return CharacterFromCodePoint(checkCodePoint);
        }

        public static Guid GenerateLuhn16Guid()
        {
            // Generate a guid.
            Guid guid = Guid.NewGuid();

            // Generate string version of the guid with no dashes or brackets.
            string guidStr = guid.ToString("N");

            // Remove the last character.
            guidStr = guid.ToString("N").Substring(0, guidStr.Length - 1);

            // Create calculate the check digit.
            guidStr = guidStr + GenerateCheckCharacter(guidStr);

            // Return our new guid.
            return Guid.Parse(guidStr);
        }

        public static bool Validate(Guid inGuid)
        {
            var input = inGuid.ToString("N");

            int factor = 1;
            int sum = 0;
            int n = 16;

            // Starting from the right, work leftwards
            // Now, the initial "factor" will always be "1" 
            // since the last character is the check character
            for (int i = input.Length - 1; i >= 0; i--)
            {
                int codePoint = CodePointFromCharacter(input[i]);
                int addend = factor * codePoint;

                // Alternate the "factor" that each "codePoint" is multiplied by
                factor = (factor == 2) ? 1 : 2;

                // Sum the digits of the "addend" as expressed in base "n"
                addend = (addend / n) + (addend % n);
                sum += addend;
            }

            int remainder = sum % n;

            return (remainder == 0);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var theGuid = Luhn16Guid.GenerateLuhn16Guid();

            Console.Out.WriteLine(theGuid.ToString());

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

                if (Luhn16Guid.Validate(enteredGuid))
                {
                    Console.Out.WriteLine("GUID was entered correctly");
                }
                else
                {
                    Console.Out.WriteLine("GUID was entered incorrectly");
                }
            }
        }
    }
}
