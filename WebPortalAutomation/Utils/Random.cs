using System;
using System.Linq; // ElementAt
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public static class RandomUtils
    {
        // atributes
        private const int MaxLengthRandomString = 64;
        private static Random random = new Random();

        // Non empty lists
        public static object randomListElement(List<object> list)
        {
            return list[random.Next(list.Count)];
        }

        public static string randomKey(Dictionary<string, object> dict)
        {
            int index = random.Next(dict.Count);
            return dict.Keys.ElementAt(index);
        }

        // default is -1 so random length is used in case that length is not passed
        public static string generateString(int length = -1)
        {
            // use random length if is not passed as arg or if its an invalid value
            if (length <= 0 || MaxLengthRandomString < length)
            {
                length = random.Next(1, MaxLengthRandomString);
            }

            // fill the string with random chars
            char[] stringToReturn = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringToReturn[i] = getRandomChar();   
            }

            return new string(stringToReturn);
        }

        private static char getRandomChar()
        {
            char c;

            do
            {
                int index = random.Next(char.MinValue, char.MaxValue);
                c = Convert.ToChar(index);
            } 
            while(char.IsControl(c));

            return c;
        }
    }
}
