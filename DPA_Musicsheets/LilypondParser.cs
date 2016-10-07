using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets {
    class LilypondParser : MusicFileParser {
        static LilypondParser instance;

        public static LilypondParser getInstance()
        {
            if (instance == null) {
                instance = new LilypondParser();
            }

            return instance;
        }

        public D_Staff parseFile(string lilypondFilePath)
        {
            D_Staff staff = new D_Staff();

            List<string> file_lines = new List<string>(System.IO.File.ReadAllLines(lilypondFilePath));
            List<string> tokens = this.getLilypondTokens(file_lines);
            this.new_parseLilypondTokens(staff, tokens);

            return staff;
        }

        Dictionary<string, clef> lilypondClefToDomainClef = new Dictionary<string, clef> {
            { "treble", clef.g_key },
            { "bass", clef.f_key }
        };

        private void set_num_of_beats(D_Staff staff, List<D_Note> notes, int current_scope_octave)
        {
            double num_of_beats = 0;

            foreach(D_Note note in notes) {
                num_of_beats += ((double)note.length / (double)4);
            }

            staff.num_of_beats = (int)num_of_beats;
        }

        private void set_tempo(D_Staff staff, List<string> tokens)
        {
            int current_token_index = 0;
            int token_end_index = tokens.Count - 1;

            while (current_token_index <= token_end_index) {
                string current_token = tokens[current_token_index];

                if (current_token == "\\tempo") {
                    string tempo_arg = tokens[current_token_index + 1];

                    staff.tempo = StringUtil.extractTempoFromLilypondString(tempo_arg);
                    return;
                }

                current_token_index++;
            }
        }

        private List<D_Note> get_notes(List<string> tokens, int current_scope_octave)
        {
            // This does not take into account repeats
            int current_token_index = 0;
            int token_end_index = tokens.Count - 1;
            List<D_Note> notes = new List<D_Note>();
            D_Note previous_note = null;

            while (current_token_index <= token_end_index) {
                string current_token = tokens[current_token_index];

                // Special case, when we encounter \relative, skip a token
                if (current_token == "\\relative") {
                    current_token_index++;
                }
                else if (isNote(current_token)) {
                    D_Note note = LilypondNoteParser.noteFromToken(current_token, current_scope_octave, previous_note);

                    if (!note.is_rest) {
                        current_scope_octave = note.octave;
                    }

                    if (current_token_index + 1 <= token_end_index && tokens[current_token_index + 1] == "~") {
                        note.note_tie = NoteTie.start;
                    }
                    else if (current_token_index > 1 && tokens[current_token_index - 2] == "~") {
                        note.note_tie = NoteTie.stop;
                    }

                    notes.Add(note);
                    if (!note.is_rest) {
                        previous_note = note;
                    }
                }

                current_token_index++;
            }

            return notes;
        }

        private int get_relative_octave(List<string> tokens)
        {
            int current_token_index = 0;
            int token_end_index = tokens.Count - 1;

            while (current_token_index <= token_end_index) {
                string current_token = tokens[current_token_index];

                if (current_token == "\\relative") {
                    // +3 because we have to start from central c
                    int scope_octave = 2 + getNoteOctave(tokens[current_token_index + 1]);
                    return scope_octave;
                }
            }

            return -1;
        }

        private void set_measures(D_Staff staff, List<string> tokens)
        {
            // This function requires staff.num_of_beats to be set
            int current_token_index = 0;
            int token_end_index = tokens.Count - 1;
            double current_beat = 0;

            while (current_token_index <= token_end_index) {
                string current_token = tokens[current_token_index];

                // Special case, when we encounter \relative, skip a token
                if (current_token == "\\relative") {
                    current_token_index++;
                }
                else if (isNote(current_token)) {
                    // We don't care about octaves here so we pass 0 and no previous note
                    // All we care about is note length so that we may increase current_beat
                    D_Note note = LilypondNoteParser.noteFromToken(current_token, 0, null);
                    current_beat += ((double)note.length / (double)4);
                }
                else if (current_token == "\\time") {
                    string time_signature = tokens[current_token_index + 1];
                    Tuple<int, int> time_signature_tuple = StringUtil.getMeasureFromString(time_signature);
                    staff.addMeasure(time_signature_tuple.Item1, time_signature_tuple.Item2, (int)current_beat);
                }

                current_token_index++;
            }

            staff.setMeasureEndTimes();
        }

        private void set_clef(D_Staff staff, List<string> tokens)
        {
            int current_token_index = 0;
            int token_end_index = tokens.Count - 1;

            while (current_token_index <= token_end_index) {
                string current_token = tokens[current_token_index];

                if (current_token == "\\clef") {
                    string clef_type = tokens[current_token_index + 1];
                    staff.clef = getClef(clef_type);
                    return;
                }

                current_token_index++;
            }
        }

        private D_Staff new_parseLilypondTokens(D_Staff staff, List<string> tokens)
        {
            int relative_octave = this.get_relative_octave(tokens);
            List<D_Note> notes = this.get_notes(tokens, relative_octave);

            this.set_clef(staff, tokens);
            this.set_num_of_beats(staff, notes, relative_octave);
            this.set_tempo(staff, tokens);
            this.set_measures(staff, tokens);
            staff.make_bars();
            staff.fillBarsWithNotes(notes);

            return staff;
        }

        private void parseLilypondTokens(D_Staff staff, List<string> tokens)
        {
            int current_token_index = 0;
            int token_end_index = tokens.Count - 1;

            while (current_token_index <= token_end_index) {
                string current_token = tokens[current_token_index];

                if (current_token == "\\relative") {
                    // +3 because we have to start from central c
                    int scope_octave = 2 + getNoteOctave(tokens[current_token_index + 1]);
                    int scope_begin_index = current_token_index + 3;
                    int scope_end_index = StringUtil.get_string_index("}", tokens, scope_begin_index) - 1;
                    List<string> scope_tokens = tokens.GetRange(scope_begin_index, scope_end_index - scope_begin_index);

                    this.parseLilypondScope(staff, scope_tokens, scope_octave);
                }

                current_token_index++;
            }

            // 2nd pass to set bar measures
            double current_beat = 0;
            foreach (D_Bar bar in staff.bars) {
                bar.measure = staff.getMeasure((int)current_beat);

                foreach (D_Note note in bar.notes) {
                    current_beat += (note.length / 16);
                }
            }

            staff.setNumOfBeats();
            staff.setMeasureEndTimes();
        }

        public static int getNoteOctave(string note)
        {
            int num_of_singlequotes = StringUtil.getOccurencesOfChar('\'', note);
            int num_of_commas = StringUtil.getOccurencesOfChar(',', note);

            return num_of_singlequotes - num_of_commas;
        }

        private void parseLilypondScope(D_Staff staff, List<string> tokens, int scope_octave)
        {
            int current_token_index = 0;
            int token_end_index = tokens.Count - 1;
            List<D_Note> note_buffer = new List<D_Note>();
            int current_scope_octave = scope_octave;
            D_Note previous_note = null;

            while (current_token_index <= token_end_index) {
                string current_token = tokens[current_token_index];
                
                if (current_token == "\\clef") {
                    string clef_type = tokens[current_token_index + 1];
                    staff.clef = getClef(clef_type);
                }

                else if (current_token == "\\time") {
                    string time_signature = tokens[current_token_index + 1];
                    Tuple<int,int> time_signature_tuple = StringUtil.getMeasureFromString(time_signature);
                    staff.addMeasure(time_signature_tuple.Item1, time_signature_tuple.Item2);
                }
                
                else if (isNote(current_token)) {
                    D_Note note = LilypondNoteParser.noteFromToken(current_token, current_scope_octave, previous_note);

                    if(!note.is_rest) {
                        current_scope_octave = note.octave;
                    }

                    if (current_token_index + 1 <= token_end_index && tokens[current_token_index + 1] == "~") {
                        note.note_tie = NoteTie.start;
                    }
                    else if(current_token_index > 1 && tokens[current_token_index - 2] == "~") {
                        note.note_tie = NoteTie.stop;
                    }

                    note_buffer.Add(note);
                    if(!note.is_rest) {
                        previous_note = note;
                    }
                }

                else if (current_token == "|") {
                    addToStaff(note_buffer, staff);

                    note_buffer = new List<D_Note>();
                }

                current_token_index++;
            }

            if (note_buffer.Count > 0) {
                addToStaff(note_buffer, staff);
            }
        }

        private static void addToStaff(List<D_Note> note_buffer, D_Staff staff)
        {
            D_Bar bar = new D_Bar();

            foreach (D_Note note in note_buffer) {
                bar.addNoteNoOverflow(note);
            }

            staff.addBar(bar);
        }

        private static List<char> valid_notes = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'r'};
        private bool isNote(string token)
        {
            if(!valid_notes.Contains(token[0])) {
                return false;
            }
            return true;
        }

        private clef getClef(string lilypondClef)
        {
            return this.lilypondClefToDomainClef[lilypondClef];
        }

        private List<string> getLilypondTokens(List<string> file_lines)
        {
            List<string> tokens = new List<string>();

            foreach(string line in file_lines) {
                string[] line_tokens = line.Split(' ');

                foreach (string token in line_tokens) {
                    if (token != "") {
                        tokens.Add(token);
                    }
                }
            }

            return tokens;
        }
    }
}
