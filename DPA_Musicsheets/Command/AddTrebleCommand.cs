using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command {
    public class AddTrebleCommand : EditorCommand {
        public string name { get; }

        public AddTrebleCommand()
        {
            this.name = "add_treble";
        }

        public void execute(Editor editor, EditorCommandParameters parameters)
        {
            string current_text = editor.getText();
            string new_text = current_text.Insert(parameters.current_selection_index, "\\clef treble\r\n");

            editor.setText(new_text);
        }
    }
}
