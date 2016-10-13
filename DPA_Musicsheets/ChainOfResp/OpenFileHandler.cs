using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DPA_Musicsheets.Command;

namespace DPA_Musicsheets.ChainOfResp {
    public class OpenFileHandler : HotkeyHandler {
        private List<Key> hotkey = new List<Key>() {
            Key.LeftCtrl,
            Key.O
        };

        protected override bool shouldHandle(List<Key> keys_down)
        {
            return keys_down.ContainsSameItems(this.hotkey);
        }

        protected override void handle(EditorCommandList commands)
        {
            commands.executeCommand("open_file");
        }
    }
}
