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
            this.parseLilypondTokens(staff, tokens);

            return staff;
        }

        Dictionary<string, clef> lilypondClefToDomainClef = new Dictionary<string, clef> {
            { "treble", clef.g_key },
            { "bass", clef.f_key }
        };

        private void parseLilypondTokens(D_Staff staff, List<string> tokens)
        {
            int current_token_index = 0;
            int token_end_index = tokens.Count - 1;

            while (current_token_index <= token_end_index) {
                string current_token = tokens[current_token_index];

                if (current_token == "\\relative") {
                    // +3 because we have to start from central c
                    int scope_octave = 3 + getNoteOctave(tokens[current_token_index + 1]);
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
                    D_Note note = LilypondNoteParser.noteFromToken(current_token, scope_octave);

                    if (current_token_index + 1 <= token_end_index && tokens[current_token_index + 1] == "~") {
                        note.note_tie = NoteTie.start;
                    }
                    else if(current_token_index > 1 && tokens[current_token_index - 2] == "~") {
                        note.note_tie = NoteTie.stop;
                    }

                    note_buffer.Add(note);
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
