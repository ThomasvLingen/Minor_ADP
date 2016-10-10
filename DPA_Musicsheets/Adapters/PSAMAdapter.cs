using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace DPA_Musicsheets {
    public class PSAMAdapter {
        public static Dictionary<NoteLevel, String> note_levels = new Dictionary<NoteLevel, string>() {
            { NoteLevel.C, "C" },
            { NoteLevel.D, "D" },
            { NoteLevel.E, "E" },
            { NoteLevel.F, "F" },
            { NoteLevel.G, "G" },
            { NoteLevel.A, "A" },
            { NoteLevel.B, "B" }
        };

        public static Dictionary<NoteAlteration, int> note_alterations = new Dictionary<NoteAlteration, int>() {
            { NoteAlteration.sharp, 1 },
            { NoteAlteration.none, 0 },
            { NoteAlteration.flat, -1 }
        };

        public static Dictionary<int, MusicalSymbolDuration> note_lengths = new Dictionary<int, MusicalSymbolDuration>() {
            { 1, MusicalSymbolDuration.Sixteenth },
            { 2, MusicalSymbolDuration.Eighth },
            { 4, MusicalSymbolDuration.Quarter },
            { 8, MusicalSymbolDuration.Half },
            { 16, MusicalSymbolDuration.Whole }
        };

        public static Dictionary<NoteTie, NoteTieType> note_ties = new Dictionary<NoteTie, NoteTieType>() {
            { NoteTie.none, NoteTieType.None },
            { NoteTie.start, NoteTieType.Start },
            { NoteTie.stop, NoteTieType.Stop }
        };

        static String getNoteLevel(NoteLevel level)
        {
            return note_levels[level];
        }

        static int getNoteAlteration(NoteAlteration alteration)
        {
            return note_alterations[alteration];
        }

        static MusicalSymbolDuration getNoteDuration(int length_in_sixteenths)
        {
            return note_lengths[length_in_sixteenths];
        }

        static NoteTieType getNoteTie(NoteTie tie)
        {
            return note_ties[tie];
        }

        static int getNumberOfDots(bool length_modifier)
        {
            if (length_modifier) {
                return 1;
            } else {
                return 0;
            }
        }

        static int getVisualLength(int note_length, bool modifier)
        {
            int visual_length;

            if (modifier) {
                visual_length = note_length / 3;
                visual_length *= 2;
            } else {
                visual_length = note_length;
            }


            return visual_length;
        }

        static NoteStemDirection getStemDirection(int octave)
        {
            if (octave < 5) {
                return NoteStemDirection.Up;
            } else {
                return NoteStemDirection.Down;
            }
        }

        static private MusicalSymbol getNoteMusicalSymbol(D_Note note)
        {
            String note_level = getNoteLevel(note.level);
            int note_alteration = getNoteAlteration(note.alteration);
            int octave = note.octave;
            int numberOfDots = getNumberOfDots(note.length_modifier);
            MusicalSymbolDuration duration = getNoteDuration(getVisualLength(note.length, note.length_modifier));
            NoteStemDirection direction = getStemDirection(note.octave);
            NoteTieType note_tie = getNoteTie(note.note_tie);
            List<NoteBeamType> note_beams = new List<NoteBeamType>() { NoteBeamType.Single };

            return new Note(note_level, note_alteration, octave, duration, direction, note_tie, note_beams) { NumberOfDots = numberOfDots };
        }

        static private MusicalSymbol getRestMusicalSymbol(D_Note note)
        {
            MusicalSymbolDuration duration = getNoteDuration(note.length);

            return new Rest(duration);
        }

        static public MusicalSymbol fromNote(D_Note note)
        {
            if (!note.is_rest) {
                return getNoteMusicalSymbol(note);
            } else {
                return getRestMusicalSymbol(note);
            }
        }

        static public MusicalSymbol fromMeasure(D_Measure measure)
        {
            return new TimeSignature(TimeSignatureType.Numbers, (uint)measure.beats_per_bar, (uint)measure.beat_length);
        }
    }
}
