using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets
{
    class MidiParser
    {
        public static D_Staff parseMidi(string midiFileLocation)
        {
            var sequence = new Sequence();
            sequence.Load(midiFileLocation);

            return good_parseSequence(sequence);
        }

        private static void set_tempo(Sequence sequence, D_Staff staff)
        {
            Track track = sequence[0];

            foreach (var midiEvent in track.Iterator()) {
                // Elke messagetype komt ook overeen met een class. Daarom moet elke keer gecast worden.
                switch (midiEvent.MidiMessage.MessageType) {
                    case MessageType.Meta:
                        var metaMessage = midiEvent.MidiMessage as MetaMessage;
                        byte[] meta_bytes = metaMessage.GetBytes();

                        // BPM
                        if (metaMessage.MetaType == MetaType.Tempo) {
                            // Bitshifting is nodig om het tempo in BPM te berekenen
                            int tempo = (meta_bytes[0] & 0xff) << 16 | (meta_bytes[1] & 0xff) << 8 | (meta_bytes[2] & 0xff);
                            staff.tempo = 60000000 / tempo;
                            return;
                        }
                        break;
                }
            }

            staff.tempo = -1;
        }

        private static void set_measures(Sequence sequence, D_Staff staff)
        {
            Track track = sequence[0];

            foreach (var midiEvent in track.Iterator()) {
                // Elke messagetype komt ook overeen met een class. Daarom moet elke keer gecast worden.
                switch (midiEvent.MidiMessage.MessageType) {
                    case MessageType.Meta:
                        var metaMessage = midiEvent.MidiMessage as MetaMessage;
                        byte[] meta_bytes = metaMessage.GetBytes();

                        // Time signature
                        if (metaMessage.MetaType == MetaType.TimeSignature) {
                            int top_number = meta_bytes[0];
                            int bottom_number = (int)(Math.Pow(2, meta_bytes[1]));
                            int time_signature_event = midiEvent.AbsoluteTicks;

                            staff.addMeasure(time_signature_event, top_number, bottom_number);
                        }

                        break;
                }

            }
        }

        private static List<D_Note> get_notes_from_track(Track track, D_Staff staff, int ticks_per_beat)
        {
            List<D_Note> notes = new List<D_Note>();

            MidiEvent previous_midi_event = null;
            foreach (var midiEvent in track.Iterator()) {
                switch (midiEvent.MidiMessage.MessageType) {
                    case MessageType.Channel:
                        var channelMessage = midiEvent.MidiMessage as ChannelMessage;

                        if (previous_midi_event != null) {
                            if (channelMessage.Command.ToString() == "NoteOn" && channelMessage.Data2 == 0) {
                                Tuple<int, int> current_measure = staff.getMeasure(previous_midi_event.AbsoluteTicks);

                                double note_beats = ((double)midiEvent.AbsoluteTicks - (double)previous_midi_event.AbsoluteTicks) / (double)ticks_per_beat;
                                //int note_beats_measured = (int)(note_beats / ((double)current_measure.Item2 / (double)16));
                                int note_beats_measured = (int)(note_beats * 4);
                                // Console.WriteLine(String.Format("note_beats = {0} = (curTicks {1} - prevTicks {2}) / tpb {3}", note_beats, midiEvent.AbsoluteTicks, previous_midi_event.AbsoluteTicks, ticks_per_beat));
                                // Console.WriteLine(String.Format("note_beats_measured = {0} = note_beats {1} / (beat {2} / 16)", note_beats_measured, note_beats, current_measure.Item2));

                                notes.Add(D_NoteFactory.create_note(channelMessage.Data1, note_beats_measured));
                                // Console.WriteLine(String.Format("Tellen: {0}", note_beats));
                                // Console.WriteLine(String.Format("Tellen measured: {0}", note_beats_measured));
                            }
                            else if (channelMessage.Command.ToString() == "NoteOn" && channelMessage.Data2 == 90) {
                                // Make rest
                                if (midiEvent.DeltaTicks > 0) {
                                    int note_beats = midiEvent.DeltaTicks / ticks_per_beat;
                                    int note_beats_measured = (int)(note_beats * 4);
                                    notes.Add(D_NoteFactory.create_rest(note_beats_measured));
                                }
                            }
                        }

                        previous_midi_event = midiEvent;
                    break;
                }
            }

            return notes;
        }

        private static D_Staff good_parseSequence(Sequence sequence)
        {
            D_Staff staff = new D_Staff();

            set_tempo(sequence, staff);
            set_measures(sequence, staff);
            int ticks_per_beat = sequence.Division;
            List<D_Note> notes = get_notes_from_track(sequence[1], staff, ticks_per_beat);

            Console.WriteLine(String.Format("Ticks per beat: {0}", ticks_per_beat));
            Console.WriteLine(String.Format("Tempo: {0}", staff.tempo));

            return staff;
        }

        private static D_Staff parseSequence(Sequence sequence)
        {
            D_Staff staff = new D_Staff();
            Tuple<int, int> measure = new Tuple<int, int>(-1, -1);

            
            Track track = sequence[1];
            int keycode_on = -1;
            int delta_time_on; // plz kill me

            int delta_time_off; // plz kill me

            foreach (var midiEvent in track.Iterator())
            {
                // Elke messagetype komt ook overeen met een class. Daarom moet elke keer gecast worden.
                switch (midiEvent.MidiMessage.MessageType)
                {
                    // ChannelMessages zijn de inhoudelijke messages.
                    case MessageType.Channel:
                        var channelMessage = midiEvent.MidiMessage as ChannelMessage;
                        // Data1: De keycode. 0 = laagste C, 1 = laagste C#, 2 = laagste D etc.
                        // 160 is centrale C op piano.

                        if (channelMessage.Command.ToString() == "NoteOn")
                        {
                            keycode_on = channelMessage.Data1;
                            delta_time_off = midiEvent.DeltaTicks;
                            Console.WriteLine(String.Format("note: none, length: {0}", delta_time_off));
                        }
                        else if (channelMessage.Command.ToString() == "NoteOff")
                        {
                            delta_time_on = midiEvent.DeltaTicks;
                            Console.WriteLine(String.Format("note: {0}, length: {1}", keycode_on, delta_time_on));
                        }
                        else
                        {
                            Console.WriteLine("YOU DONE FUCKED UP!");
                            Console.WriteLine(
                                String.Format(
                                    "Keycode: {0}, Command: {1}, absolute time: {2}, delta time: {3}"
                                    , channelMessage.Data1, channelMessage.Command, midiEvent.AbsoluteTicks, midiEvent.DeltaTicks
                                )
                            );
                        }



                        break;
                    case MessageType.Meta:
                        var metaMessage = midiEvent.MidiMessage as MetaMessage;
                        byte[] meta_bytes = metaMessage.GetBytes();
                        if (metaMessage.MetaType == MetaType.TimeSignature) {
                            int top_number = meta_bytes[0];
                            int bottom_number = (int) (1 / Math.Pow(meta_bytes[1], -2));
                            measure = new Tuple<int, int>(top_number, bottom_number);
                            //TODO: MEASURE MIGHT NOT BE FILLED ON FIRST COMMAND 
                            Console.WriteLine(String.Format("Time signature is {0}/{1}", measure.Item1, measure.Item2));
                        }
                        break;
                    default:
                        break;
                }
                
            }
            return staff;
        }

    }
}
