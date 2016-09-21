using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    enum NoteLevel { C, D, E, F, G, A, B, rest };
    enum NoteAlteration { flat, sharp, none};

    public class D_Note
    {
        bool is_rest;
        NoteLevel level;
        int octave;
        NoteAlteration alteration;
        int length; // in delta time
        bool length_modifier;

        public D_Note(NoteLevel level, int length, int octave = 1,
                    NoteAlteration alteration = NoteAlteration.none, bool is_rest = false,
                    bool length_modifier = false)
        {
            this.level              = level;
            this.length             = length;
            this.octave             = octave;
            this.alteration         = alteration;
            this.is_rest            = is_rest;
            this.length_modifier    = length_modifier;
        }
    }
}
