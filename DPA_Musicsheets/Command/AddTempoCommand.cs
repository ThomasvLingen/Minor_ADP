using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command {
    public class AddTempoCommand : EditorCommand {
        public string name { get; }

        public AddTempoCommand()
        {
            this.name = "add_tempo";
        }

        public void execute(Editor editor, EditorCommandParameters parameters)
        {
            string current_text = editor.getText();
            string new_text = current_text.Insert(parameters.current_selection_index, "\\tempo 4=120\r\n");

            editor.setText(new_text);
        }
    }
}
