using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command {
    public class OpenFileCommand : EditorCommand {
        public string name { get; }

        public OpenFileCommand()
        {
            this.name = "open_file";
        }

        public void execute(Editor editor, EditorCommandParameters parameters)
        {
            editor.openFile();
        }
    }
}
