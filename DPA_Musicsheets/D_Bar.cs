using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class D_Bar
    {
        public List<D_Note> notes { get; private set; }
        private int beats_in_bar = -1;

        public D_Bar(int beats_in_bar)
        {
            notes = new List<D_Note>();
            this.beats_in_bar = beats_in_bar;
        }

        public void addNote(D_Note note)
        {
            this.notes.Add(note);
        }

        public void removeNote(D_Note note)
        {
            this.notes.Remove(note);
        }

        public bool isFull()
        {
            int length_of_all_notes = 0;

            foreach(D_Note note in this.notes) {
                length_of_all_notes += note.length;
            }

            return length_of_all_notes >= beats_in_bar * 4;
        }
    }
}
