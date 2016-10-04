using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public enum NoteLevel { C, D, E, F, G, A, B, rest };
    public enum NoteAlteration { flat, sharp, none};
    public enum NoteTie { start, stop, none};

    public class D_Note
    {
        public bool is_rest { get; }
        public NoteLevel level { get; }
        public int octave { get; }
        public NoteAlteration alteration { get; }
        public int length { get; } // in 16th notes
        public bool length_modifier { get; }
        public NoteTie note_tie { get; set; }

        public D_Note(NoteLevel level, int length, int octave = 1,
                    NoteAlteration alteration = NoteAlteration.none, bool is_rest = false)
        {
            this.level              = level;
            this.length             = length;
            this.octave             = octave;
            this.alteration         = alteration;
            this.is_rest            = is_rest;
            this.length_modifier    = has_length_modifier(length);
            this.note_tie           = NoteTie.none;
        }

        public D_Note(NoteLevel level, int length, NoteTie note_tie = NoteTie.none, int octave = 1,
                    NoteAlteration alteration = NoteAlteration.none, bool is_rest = false)
        {
            this.level = level;
            this.length = length;
            this.octave = octave;
            this.alteration = alteration;
            this.is_rest = is_rest;
            this.length_modifier = has_length_modifier(length);
            this.note_tie = note_tie;
        }

        public D_Note(D_Note other, int new_length)
        {
            this.level = other.level;
            this.length = new_length;
            this.octave = other.octave;
            this.alteration = other.alteration;
            this.is_rest = other.is_rest;
            this.length_modifier = has_length_modifier(this.length);
        }

        private static List<int> valid_lengths = new List<int> { 1, 2, 4, 8, 16 };
        private static bool has_length_modifier(int length)
        {
            return !valid_lengths.Contains(length);
        }

        public static Tuple<D_Note, D_Note> splitNote(D_Note note_to_split, int first_note_length)
        {
            int second_note_length = note_to_split.length - first_note_length;
            D_Note first = D_NoteFactory.create_note(note_to_split, first_note_length);
            first.note_tie = NoteTie.start;
            D_Note second = D_NoteFactory.create_note(note_to_split, second_note_length);
            second.note_tie = NoteTie.stop;

            return new Tuple<D_Note, D_Note>(first, second);
        }
    }
}
