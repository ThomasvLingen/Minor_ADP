using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets {
    class LilypondNoteParser {

        private static Dictionary<char, NoteLevel> noteLevelDictionary = new Dictionary<char, NoteLevel>() {
            { 'c', NoteLevel.C },
            { 'd', NoteLevel.D },
            { 'e', NoteLevel.E },
            { 'f', NoteLevel.F },
            { 'g', NoteLevel.G },
            { 'a', NoteLevel.A },
            { 'b', NoteLevel.B },
            { 'r', NoteLevel.rest },
        }; 

        public static D_Note noteFromToken(String token, int current_scope_octave, D_Note previous_note)
        {
            NoteLevel noteLevel = noteLevelDictionary[token[0]];
            int length_in_sixteenths = 16 / StringUtil.getNumberFromString(token);
            if (token.Contains('.')) {
                length_in_sixteenths =  (int)((double)length_in_sixteenths * 1.5);
            }

            if(noteLevel == NoteLevel.rest) {
                return D_NoteFactory.create_rest(length_in_sixteenths);
            } else {
                NoteAlteration alteration = getNoteNoteAlteration(token);

                int octave = getNoteOctave(token, current_scope_octave);

                D_Note new_note = D_NoteFactory.create_note(noteLevel, alteration, octave, length_in_sixteenths);
                if(previous_note != null) {
                    setRelativeOctave(new_note, previous_note);
                }
                return new_note;
            }
        }

        private static NoteAlteration getNoteNoteAlteration(String token)
        {
            if (token.Contains("is")) {
                return NoteAlteration.sharp;
            } else if(token.Contains("es")) {
                return NoteAlteration.flat;
            } else {
                return NoteAlteration.none;
            }
        }

        private static int getNoteOctave(string token, int current_scope_octave)
        {
            int note_octave = LilypondParser.getNoteOctave(token);

            return note_octave + current_scope_octave;
        }

        private static void setRelativeOctave(D_Note new_note, D_Note old_note)
        {
            Tuple<int, bool> below, above;
            below = findBelow(new_note, old_note);
            above = findAbove(new_note, old_note);

            if(below.Item1 > above.Item1) {
                if(above.Item2) {
                    new_note.octave++;
                }
            } else if(below.Item1 < above.Item1) {
                if(below.Item2) {
                    new_note.octave--;
                }
            } else {
                if(below.Item1 == 0 && above.Item1 == 0) {
                    return;
                }
                throw new Exception("7 is a even number?");
            }
        }

        private static Dictionary<NoteLevel, int> noteLevelInt = new Dictionary<NoteLevel, int>() {
                { NoteLevel.C, 1 },
                { NoteLevel.D, 2 },
                { NoteLevel.E, 3 },
                { NoteLevel.F, 4 },
                { NoteLevel.G, 5 },
                { NoteLevel.A, 6 },
                { NoteLevel.B, 7 }
            };

        private static Tuple<int, bool> findBelow(D_Note new_note, D_Note old_note)
        {
            int count = 0;
            bool oct_change = false;
            int int_old = noteLevelInt[old_note.level];
            int int_new = noteLevelInt[new_note.level];

            if (int_old > int_new) {
                count += int_old - int_new;
            }
            else if (int_old < int_new) {
                oct_change = true;
                count += int_old;
                count += noteLevelInt.Count - int_new;
            }

            return new Tuple<int, bool>(count, oct_change);
        }

        private static Tuple<int, bool> findAbove(D_Note new_note, D_Note old_note)
        {
            int count = 0;
            bool oct_change = false;
            int int_old = noteLevelInt[old_note.level];
            int int_new = noteLevelInt[new_note.level];

            if(int_old > int_new) {
                oct_change = true;
                count += noteLevelInt.Count - int_old;
                count += int_new;
            } else if (int_old < int_new) {
                count += int_new - int_old;
            }

            return new Tuple<int, bool>(count, oct_change);
        }
    }
}
