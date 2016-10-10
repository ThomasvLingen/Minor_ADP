using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States {
    public class EditorStateManager {
        public EditorState state { get; set; }

        public void quit_callback()
        {
            this.state.quit_callback();
        }
    }
}
