using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command {
    public class AddTimeCommand_34 : EditorCommand {
        public string name { get; }

        public AddTimeCommand_34()
        {
            this.name = "add_time_34";
        }

        public void execute(Editor editor, EditorCommandParameters parameters)
        {
            string current_text = editor.getText();
            string new_text = current_text.Insert(parameters.current_selection_index, "\\time 3/4\r\n");

            editor.setText(new_text);
        }
    }
}
