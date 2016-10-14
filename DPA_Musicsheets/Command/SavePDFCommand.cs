using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command {
    public class SavePDFCommand : EditorCommand {
        public string name { get; }

        public SavePDFCommand()
        {
            this.name = "save_file_pdf";
        }

        public void execute(Editor editor, EditorCommandParameters parameters)
        {
            editor.saveToPDF();
        }
    }
}
