using System;

namespace Common
{
    public class PalindromeWithSpaces
    {
        public void CheckIfStringPalindrome()
        {
            Console.WriteLine("Hello Mono World");
            string test = "aabbcbbaa";
            char[] testArray = test.ToCharArray();
            int arrayLength = testArray.Length;
            int firstPartLastIndex = (testArray.Length / 2) - 1;
            int secondPartFirstIndex = (testArray.Length % 2) == 0 ? firstPartLastIndex + 1 : firstPartLastIndex + 2;
            bool isPalindrome = true;
            int firstPartValidCharIndex = 0;
            int lastPartValidCharIndex = arrayLength;
            Console.WriteLine("Length={0}, firstPartLastIndex={1}, secondPartFirstIndex={2}", arrayLength, firstPartLastIndex, secondPartFirstIndex);
            int i = 0;
            while (i <= arrayLength - 1)
            {
                if (testArray[i] != ' ')
                {
                    firstPartValidCharIndex = i;
                }
                else
                {
                    while (testArray[i] == ' ' && i + 1 < lastPartValidCharIndex - 1)
                    {
                        i = i + 1;
                    }
                    firstPartValidCharIndex = i;
                }
                lastPartValidCharIndex = lastPartValidCharIndex - 1;
                if (firstPartValidCharIndex == (lastPartValidCharIndex - 1) && testArray[lastPartValidCharIndex] == ' ')
                {
                    break;
                }
                while (testArray[lastPartValidCharIndex] == ' ' && lastPartValidCharIndex - 1 > firstPartValidCharIndex)
                {
                    lastPartValidCharIndex = lastPartValidCharIndex - 1;
                }
                if (testArray[firstPartValidCharIndex] != testArray[lastPartValidCharIndex])
                {
                    Console.WriteLine("For First Part Index={0} Character={1}, And Second Part Index={2} Character={3} ", firstPartValidCharIndex, testArray[firstPartValidCharIndex], lastPartValidCharIndex, testArray[lastPartValidCharIndex]);
                    isPalindrome = false;
                    break;
                }
                i = i + 1;
            }
            Console.WriteLine("Palidrome={0}", isPalindrome);
        }
    }
}
