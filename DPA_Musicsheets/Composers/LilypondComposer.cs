using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets {
    public class LilypondComposer {

        //TODO add double space for lines, depending on scope
        public string composeLilypondFromStaff(D_Staff staff)
        {
            string lilypondString = "";

            composeRelativeInformation(staff.bars[0].notes[0], ref lilypondString);
            composeClefInformation(staff.clef, ref lilypondString);
            composeTempoInformation(staff.tempo, ref lilypondString);
            composeMeasureInformation(staff.measures[0], ref lilypondString); //TODO not only first, tempo might switch, check for bars

            return lilypondString;
        }

        private int central_c_ocatave = 4;
        private void composeRelativeInformation(D_Note first_note, ref string lilypondString)
        {
            int first_note_octave = first_note.octave;
            string octave_modifiers = "";
            
            if (first_note_octave > central_c_ocatave) {
                octave_modifiers = StringUtil.duplicateString("'", first_note_octave - central_c_ocatave);
            } else if (first_note_octave < central_c_ocatave) {
                octave_modifiers = StringUtil.duplicateString(",", central_c_ocatave - first_note_octave);
            }

            lilypondString += String.Format("\\relative c{0}{{\r\n", octave_modifiers);
        }

        Dictionary<clef, string> lilypondClefToString = new Dictionary<clef, string> {
            { clef.g_key, "treble" },
            { clef.f_key, "bass" }
        };

        private void composeClefInformation(clef staff_cleff, ref string lilypondString)
        {
            lilypondString += String.Format("\\clef {0}\r\n", lilypondClefToString[staff_cleff]);
        }

        private void composeTempoInformation(int tempo, ref string lilypondString)
        {
            lilypondString += String.Format("\\tempo 4={0}\r\n", tempo);
        }

        private void composeMeasureInformation(D_Measure measure, ref string lilypondString)
        {
            lilypondString += String.Format("\\time {0}/{1}\r\n", measure.beats_per_bar, measure.beat_length);
        }

        private void composeBars(List<D_Bar> bars, ref string lilypondString)
        {
            foreach(D_Bar bar in bars) {

            }
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

        private string getNotesInLilypondString(List<D_Note> notes) //perhaps move to different class, equal to lilypondnoteparser but reverse
        {
            string lilypondString = ""; //TODO relative octaves, see the string format

            foreach(D_Note note in notes) {
                char note_level = noteLeveltoChar[note.level];
                string octave_mod = ""; //TODO implement, use StringUtil.duplicateString

                int note_length_int = (note.length_modifier) ? (int)((double)note.length / 1.5) : note.length;
                note_length_int = (int)(16 / (double)note_length_int);
                string note_length = String.Format("{0}", note_length_int); //TODO think about length mod

                string length_mod = (note.length_modifier) ? "." : "";

                lilypondString += String.Format("{0}{1}{2}{3} ", note_level, octave_mod, note_length, length_mod);
            }

            return lilypondString;
        }

    }
}
