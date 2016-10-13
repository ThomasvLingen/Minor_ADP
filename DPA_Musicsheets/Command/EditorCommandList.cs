using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command {
    class EditorCommandList {
        private Dictionary<string, EditorCommand> _commands;
        private Editor _target;


        public EditorCommandList(Editor target)
        {
            this._target = target;
            this._setupCommands();
        }

        private void _setupCommands()
        {
            this._commands = new Dictionary<string, EditorCommand>();

            EditorCommand command;

            command = new AddTrebleCommand();
            this._commands.Add(command.name, command);
        }

        public void executeCommand(string command_name)
        {
            Tuple<string, int> parameters = new Tuple<string, int>(
                this._target.getCurrentSelection(),
                this._target.getCurrentIndex()
            );

            EditorCommand command;
            if (this._commands.TryGetValue(command_name, out command)) {
                command.execute(this._target, parameters);
            }
        }
    }
}
