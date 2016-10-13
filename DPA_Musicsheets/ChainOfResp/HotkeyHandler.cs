using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DPA_Musicsheets.Command;

namespace DPA_Musicsheets.ChainOfResp {
    public abstract class HotkeyHandler {
        public HotkeyHandler next { get; set; }

        public bool handle(List<Key> keys_down, EditorCommandList commands)
        {
            if (tryHandle(keys_down, commands)) {
                return true;
            } else {
                if (this.next != null) {
                    return this.next.handle(keys_down, commands);
                } else {
                    return false;
                }
            }
        }

        protected bool tryHandle(List<Key> keys_down, EditorCommandList commands)
        {
            if (this.shouldHandle(keys_down)) {
                this.handle(commands);
                return true;
            } else {
                return false;
            }
        }

        public HotkeyHandler getLastLink()
        {
            if (this.next != null) {
                return this.next.getLastLink();
            } else {
                return this;
            }
        }

        protected abstract bool shouldHandle(List<Key> keys_down);
        protected abstract void handle(EditorCommandList commands);
    }
}
