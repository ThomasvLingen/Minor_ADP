using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento {
    public class EditorHistoryCaretaker {
        private LinkedList<EditorMemento> _memento_list;
        private LinkedListNode<EditorMemento> _current_item;

        public EditorHistoryCaretaker()
        {
            this._memento_list = new LinkedList<EditorMemento>();
        }

        public void save(EditorMemento memento)
        {
            if (_current_item == null) {
                this._memento_list.AddFirst(memento);
                this._current_item = _memento_list.First;
            } else {
                this._current_item = _current_item.ReplaceNext(memento);
            }

            Console.WriteLine(String.Format("Current mementos {0}", this._memento_list.Count()));
        }

        public EditorMemento undo()
        {
            if (this.canUndo()) {
                _current_item = _current_item.Previous;
                return _current_item.Value;
            }

            return null;
        }

        public EditorMemento redo()
        {
            if (this.canRedo()) {
                _current_item = _current_item.Next;
                return _current_item.Value;
            }

            return null;
        }

        public bool canUndo() {
            return _current_item != null && _current_item.Previous != null;
        }

        public bool canRedo() {
            return _current_item != null && _current_item.Next != null;
        }
    }
}
