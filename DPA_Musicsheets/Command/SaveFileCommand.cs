using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command {
    public class SaveFileCommand : EditorCommand {
        public string name { get; }

        public SaveFileCommand()
        {
            this.name = "save_file";
        }

        public void execute(Editor editor, EditorCommandParameters parameters)
        {
            editor.saveFile();
        }
    }
}
