using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets {
    public class StringUtil {
        public static int getOccurencesOfChar(char needle, string haystack)
        {
            int occurences = 0;

            foreach(char c in haystack) {
               if (c == needle) {
                    occurences++;
                }
            }

            return occurences;
        }

        public static int get_string_index(string needle, List<string> haystack, int start_index)
        {
            int current_index = start_index;

            while (current_index < haystack.Count) {
                if (haystack[current_index] == needle) {
                    return current_index;
                }

                current_index++;
            }

            return -1;
        }

        public static Tuple<int, int> getMeasureFromString(string str)
        {
            List<string> parts = new List<string>(str.Split('/'));
            int top;
            int bottom;

            try {
                top = Int32.Parse(parts[0]);
                bottom = Int32.Parse(parts[1]);
            } catch {
                throw new Exception("Couldn't get int from string in StringUtil:getIntFromString");
            }

            return new Tuple<int, int>(top, bottom);
        }

        public static int getNumberFromString(string str)
        {
            string number_in_string = String.Empty;
            foreach (char c in str) {
                if (Char.IsDigit(c)) {
                    number_in_string += c;
                }
            }
            
            if(number_in_string.Length > 0) {
                return Int32.Parse(number_in_string); 
            }

            return 0;
        }
    }
}
