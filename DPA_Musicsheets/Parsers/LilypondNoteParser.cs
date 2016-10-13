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
            int step = LilypondUtil.getClosestNotePositionInSteps(old_note, new_note);

            if (LilypondUtil.newNoteOctaveChange(old_note, step)) {
                if(step < 0) {
                    new_note.octave--;
                } else if (step > 0) {
                    new_note.octave++;
                }
            }
        }
    }
}
