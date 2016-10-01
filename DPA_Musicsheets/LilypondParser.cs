using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets {
    class LilypondParser : MusicFileParser {
        static LilypondParser instance;

        public static LilypondParser getInstance()
        {
            if (instance == null) {
                instance = new LilypondParser();
            }

            return instance;
        }

        public D_Staff parseFile(string lilypondFilePath)
        {
            D_Staff staff = new D_Staff();

            List<string> file_lines = new List<string>(System.IO.File.ReadAllLines(lilypondFilePath));
            List<string> tokens = this.getLilypondTokens(file_lines);

            return staff;
        }

        private List<string> getLilypondTokens(List<string> file_lines)
        {
            List<string> tokens = new List<string>();

            foreach(string line in file_lines) {
                string[] line_tokens = line.Split(' ');

                foreach (string token in line_tokens) {
                    if (token != "") {
                        tokens.Add(token);
                    }
                }
            }

            return tokens;
        }
    }
}
