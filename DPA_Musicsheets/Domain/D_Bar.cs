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
        public D_Measure measure { get; set; }

        public D_Bar(int beats_in_bar)
        {
            notes = new List<D_Note>();
            this.beats_in_bar = beats_in_bar;
        }

        // This returns a overflowed note if present
        public D_Note addNote(D_Note note)
        {
            int overflow = note.length - this.spaceLeft();
            if (overflow > 0) {
                // split
                Tuple<D_Note, D_Note> split_result = D_Note.splitNote(note, this.spaceLeft());

                this.notes.Add(split_result.Item1);
                return split_result.Item2;
            } else {
                this.notes.Add(note);
                return null;
            }
        }

        public void removeNote(D_Note note)
        {
            this.notes.Remove(note);
        }

        private int getLengthOfNotes()
        {
            int length_of_all_notes = 0;

            foreach (D_Note note in this.notes) {
                length_of_all_notes += note.length;
            }

            return length_of_all_notes;
        }

        public bool isFull()
        {
            return this.getLengthOfNotes() >= this.getLengthSixteenthsInBar();
        }

        public int spaceLeft()
        {
            int length_of_all_notes = this.getLengthOfNotes();

            return this.getLengthSixteenthsInBar() - length_of_all_notes;
        }

        private int getLengthSixteenthsInBar()
        {
            return this.beats_in_bar * 4;
        }
    }
}
