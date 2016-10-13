using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States {
    public abstract class EditorState {
        protected Editor _editor;

        public EditorState(Editor editor)
        {
            this._editor = editor;
        }

        public abstract void quit_callback();
    }
}
