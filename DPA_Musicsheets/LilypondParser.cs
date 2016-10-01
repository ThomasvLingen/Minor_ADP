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

            return staff;
        }
    }
}
