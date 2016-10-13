using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DPA_Musicsheets.Command;


namespace DPA_Musicsheets.ChainOfResp {
    public class Hotkeys {
        private HotkeyHandler _chain;
        private EditorCommandList _commands;

        public Hotkeys(Editor target)
        {
            this._commands = new EditorCommandList(target);

            this._setupChain();
        }

        public bool keysPressed(List<Key> keys_down)
        {
            if (this._chain != null) {
                return this._chain.handle(keys_down, this._commands);
            }

            return false;
        }

        private void _setupChain()
        {
            this._addToChain(new SaveFileHandler());
            this._addToChain(new OpenFileHandler());
            this._addToChain(new AddTrebleHandler());
            this._addToChain(new AddTempoHandler());
            this._addToChain(new AddTimeHandler_44());
            this._addToChain(new AddTimeHandler_34());
            this._addToChain(new AddTimeHandler_68());
        }

        private void _addToChain(HotkeyHandler handler)
        {
            if (this._chain != null) {
                this._chain.getLastLink().next = handler;
            } else {
                this._chain = handler;
            }
        }
    }
}
