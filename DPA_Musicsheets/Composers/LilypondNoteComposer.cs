using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Composers {
    public class LilypondNoteComposer {

        public static string placeNoteInLilypondString(D_Note note, D_Note previous_note, ref int current_scope_octave)
        {
            string lilypondString = "";

            string note_alteration = "";
            string octave_mod = "";
            char note_level = getNoteLevel(note);
            if (!note.is_rest) {
                note_alteration = getNoteAlteration(note);
                octave_mod = getOctaveMod(note, previous_note, ref current_scope_octave);
            }
            string note_length = getNoteLength(note);
            string length_mod = getNoteModifier(note);

            lilypondString += String.Format("{0}{1}{2}{3}{4}", note_level, note_alteration, octave_mod, note_length, length_mod);

            if(note.note_tie == NoteTie.start) {
                lilypondString += " ~";
            }

            return lilypondString;
        }

        private static Dictionary<NoteLevel, char> noteLeveltoChar = new Dictionary<NoteLevel, char>() {
            { NoteLevel.C, 'c' },
            { NoteLevel.D, 'd' },
            { NoteLevel.E, 'e' },
            { NoteLevel.F, 'f' },
            { NoteLevel.G, 'g' },
            { NoteLevel.A, 'a' },
            { NoteLevel.B, 'b' },
            { NoteLevel.rest, 'r' }
        };

        private static char getNoteLevel(D_Note note)
        {
            return noteLeveltoChar[note.level];
        }

        private static string getNoteAlteration(D_Note note)
        {
            if(note.alteration == NoteAlteration.none) {
                return "";
            } else if(note.alteration == NoteAlteration.sharp) {
                return "is";
            } else {
                return "es";
            }
        }

        private static string getNoteLength(D_Note note)
        {
            int note_length_int = (note.length_modifier) ? (int)((double)note.length / 1.5) : note.length;
            note_length_int = (int)(16 / (double)note_length_int);

            return String.Format("{0}", note_length_int);
        }

        private static string getNoteModifier(D_Note note)
        {
            return (note.length_modifier) ? "." : "";
        }

        private static string getOctaveMod(D_Note note, D_Note previous_note, ref int current_scope_octave)
        {
            int difference, notes_octave_difference;
            string sign = "";

            if (previous_note != null) {
                notes_octave_difference = getOctaveDifference(note, previous_note);
                current_scope_octave += notes_octave_difference;
            }

            difference = (note.octave - current_scope_octave);
            
            if(current_scope_octave > note.octave) {
                sign = ",";
            } else if( current_scope_octave < note.octave) {
                sign = "'";
            } else {
                return "";
            }

            current_scope_octave += difference;

            return StringUtil.duplicateString(sign, Math.Abs(difference));
        }

        private static int getOctaveDifference(D_Note note, D_Note previous_note)
        {
            int steps = LilypondUtil.getClosestNotePositionInSteps(previous_note, note);
            if(LilypondUtil.newNoteOctaveChange(previous_note, steps)) {
                if (steps < 0) {
                    return -1;
                } else if ( steps > 0) {
                    return 1;
                }
                throw new Exception("Octave changes with 0 steps");
            }
            return 0;
        }

    }
}
