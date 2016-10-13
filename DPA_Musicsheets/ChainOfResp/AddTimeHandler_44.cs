using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DPA_Musicsheets.Command;

namespace DPA_Musicsheets.ChainOfResp {
    public class AddTimeHandler_44 : HotkeyHandler {
        private List<Key> hotkey1 = new List<Key>() {
            Key.LeftAlt,
            Key.T
        };

        private List<Key> hotkey2 = new List<Key>() {
            Key.LeftAlt,
            Key.T,
            Key.D4
        };

        protected override bool shouldHandle(List<Key> keys_down)
        {
            return keys_down.ContainsSameItems(this.hotkey1) || keys_down.ContainsSameItems(this.hotkey2);
        }

        protected override void handle(EditorCommandList commands)
        {
            commands.executeCommand("add_time_44");
        }
    }
}
