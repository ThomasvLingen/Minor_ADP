﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class D_NoteFactory
    {

        public static D_Note create_note(int keycode, int beats)
        { 
            Tuple<NoteLevel, NoteAlteration> note_alt = get_level_and_alteration(keycode);
            return new D_Note(
                note_alt.Item1,
                beats,
                get_octave(keycode),
                note_alt.Item2,
                false
            );
        }

        public static D_Note create_note(NoteLevel level, NoteAlteration alteration, int octave, int beats)
        {
            return new D_Note(
                level,
                beats,
                octave,
                alteration,
                false
            );
        }

        public static D_Note create_note(D_Note other, int new_length)
        {
            return new D_Note(other, new_length);
        }

        public static D_Note create_rest(int beats)
        {
            return new D_Note(
                NoteLevel.rest,
                beats,
                0,
                NoteAlteration.none,
                true
            );
        }

        // whether a sharp or a flat is returned is dependent on conventions
        /* http://computermusicresource.com/midikeys.html 
           http://www.electronics.dit.ie/staff/tscarff/Music_technology/midi/midi_note_numbers_for_octaves.htm
        */
        private static Tuple<NoteLevel, NoteAlteration> get_level_and_alteration(int keycode)
        {
            switch (keycode % 12)
            {
                case 0:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.C, NoteAlteration.none);
                case 1:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.C, NoteAlteration.sharp);

                case 2:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.D, NoteAlteration.none);
                case 3:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.E, NoteAlteration.flat);

                case 4:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.E, NoteAlteration.none);

                case 5:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.F, NoteAlteration.none);
                case 6:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.F, NoteAlteration.sharp);

                case 7:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.G, NoteAlteration.none);
                case 8:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.A, NoteAlteration.flat);

                case 9:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.A, NoteAlteration.none);
                case 10:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.B, NoteAlteration.flat);

                case 11:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.B, NoteAlteration.none);

                default:
                    return new Tuple<NoteLevel, NoteAlteration>(NoteLevel.C, NoteAlteration.none); // this is retarded
                                                                                                   
            }
        }

        private static int get_octave(int keycode)
        {
            return (keycode / 12) - 1;
        }
    }
}