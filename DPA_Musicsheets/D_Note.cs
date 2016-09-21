﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public enum NoteLevel { C, D, E, F, G, A, B, rest };
    public enum NoteAlteration { flat, sharp, none};

    public class D_Note
    {
        public bool is_rest { get; }
        public NoteLevel level { get; }
        public int octave { get; }
        public NoteAlteration alteration { get; }
        public int length { get; } // in delta time
        public bool length_modifier { get; }

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