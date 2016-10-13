using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Command {
    public interface EditorCommand {
        string name { get; }

        // Tuple structure:
        // - currentSelection      (string)
        // - currentSelectionIndex (int)
        void execute(Editor editor, Tuple<string, int> parameters);
    }
}
