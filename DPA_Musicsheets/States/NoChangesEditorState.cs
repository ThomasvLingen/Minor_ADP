using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States {
    class NoChangesEditorState : EditorState {
        public NoChangesEditorState(Editor editor) : base(editor)
        {

        }

        public override void quit_callback()
        {

        }
    }
}
