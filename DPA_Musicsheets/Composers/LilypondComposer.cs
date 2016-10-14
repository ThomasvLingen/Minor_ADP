using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Composers {
    public class LilypondComposer {

        static LilypondComposer instance;

        public static LilypondComposer getInstance()
        {
            if (instance == null) {
                instance = new LilypondComposer();
            }

            return instance;
        }

        public string composeLilypondFromStaff(D_Staff staff)
        {
            string lilypondString = "";
            int current_scope_octave = 0;

            composeRelativeInformation(staff.bars[0].notes[0], ref lilypondString, ref current_scope_octave);
            composeClefInformation(staff.clef, ref lilypondString);
            composeTempoInformation(staff.tempo, ref lilypondString);
            composeMeasureInformation(staff.measures[0], ref lilypondString);
            composeBars(staff.bars, ref lilypondString, ref current_scope_octave);

            lilypondString += "}";

            return lilypondString;
        }

        private int central_c_ocatave = 2;
        private void composeRelativeInformation(D_Note first_note, ref string lilypondString, ref int current_scope_octave)
        {
            int first_note_octave = first_note.octave;
            string octave_modifiers = "";
            
            if (first_note_octave > central_c_ocatave) {
                octave_modifiers = StringUtil.duplicateString("'", first_note_octave - central_c_ocatave);
            } else if (first_note_octave < central_c_ocatave) {
                octave_modifiers = StringUtil.duplicateString(",", central_c_ocatave - first_note_octave);
            }

            current_scope_octave = first_note_octave;
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

        private void composeBars(List<D_Bar> bars, ref string lilypondString, ref int current_scope_octave)
        {
            D_Note previous_note = null;
            D_Measure current_measure = bars[0].measure;
            foreach(D_Bar bar in bars) {

                lilypondString += "  ";

                if (bar.measure != current_measure) {
                    current_measure = bar.measure;
                    composeMeasureInformation(current_measure, ref lilypondString);
                }

                foreach(D_Note note in bar.notes) {
                    lilypondString += LilypondNoteComposer.placeNoteInLilypondString(note, previous_note, ref current_scope_octave);
                    lilypondString += " ";

                    if (!note.is_rest) {
                        previous_note = note;
                    }
                }

                lilypondString += "|\r\n";
            }
        }
    }
}
