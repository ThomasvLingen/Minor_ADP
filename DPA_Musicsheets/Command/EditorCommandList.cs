using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command {
    public class EditorCommandList {
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

            this._addCommand(new SaveFileCommand());
            this._addCommand(new OpenFileCommand());
            this._addCommand(new AddTempoCommand());
            this._addCommand(new AddTrebleCommand());
            this._addCommand(new AddTimeCommand_44());
            this._addCommand(new AddTimeCommand_34());
            this._addCommand(new AddTimeCommand_68());
        }

        private void _addCommand(EditorCommand command)
        {
            this._commands.Add(command.name, command);
        }

        public void executeCommand(string command_name)
        {
            EditorCommandParameters parameters = new EditorCommandParameters(
                this._target.getCurrentSelection(),
                this._target.getCurrentIndex()
            );

            EditorCommand command;
            if (this._commands.TryGetValue(command_name, out command)) {
                command.execute(this._target, parameters);
            }

            this._target.setCurrentIndex(parameters.current_selection_index);
        }
    }
}
