using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento {
    public class EditorMemento {
        public string editor_contents { get; set; }

        public EditorMemento(Editor editor)
        {
            this.editor_contents = editor.getText();
        }

        public EditorMemento(string editor_contents)
        {
            this.editor_contents = editor_contents;
        }
    }
}
