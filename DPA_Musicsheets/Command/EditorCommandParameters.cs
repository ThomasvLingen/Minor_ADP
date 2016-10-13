using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command {
    public class EditorCommandParameters {
        public string current_selection { get; set; }
        public int current_selection_index { get; set; }

        public EditorCommandParameters(string current_selection, int current_selection_index)
        {
            this.current_selection = current_selection;
            this.current_selection_index = current_selection_index;
        }
    }
}
