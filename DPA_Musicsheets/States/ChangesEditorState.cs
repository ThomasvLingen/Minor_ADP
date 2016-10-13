using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace DPA_Musicsheets.States {
    class ChangesEditorState : EditorState {
        public ChangesEditorState(Editor editor) : base(editor)
        {

        }

        public override void quit_callback()
        {
            DialogResult result = System.Windows.Forms.MessageBox.Show("Do you want to save your work?", "Maybe save?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes) {
                this._editor.saveFile();
            }
        }
    }
}
