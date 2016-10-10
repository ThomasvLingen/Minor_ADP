using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets
{
    class MidiParser : MusicFileParser
    {
        static MidiParser instance;

        public static MidiParser getInstance()
        {
            if (instance == null) {
                instance = new MidiParser();
            }

            return instance;
        }

        public D_Staff parseFile(string midiFileLocation)
        {
            var sequence = new Sequence();
            sequence.Load(midiFileLocation);

            return parseSequence(sequence);
        }

        private static List<Tuple<MidiEvent, MetaMessage>> getMetaMessages(Track track)
        {
            List<Tuple<MidiEvent, MetaMessage>> messages = new List<Tuple<MidiEvent, MetaMessage>>();

            foreach (var midiEvent in track.Iterator()) {
                if (midiEvent.MidiMessage.MessageType == MessageType.Meta) {
                    messages.Add(
                        new Tuple<MidiEvent, MetaMessage>(midiEvent, midiEvent.MidiMessage as MetaMessage)
                    );
                }
            }

            return messages;
        }

        private static List<Tuple<MidiEvent, ChannelMessage>> getChannelMessages(Track track)
        {
            List<Tuple<MidiEvent, ChannelMessage>> messages = new List<Tuple<MidiEvent, ChannelMessage>>();

            foreach (var midiEvent in track.Iterator()) {
                if (midiEvent.MidiMessage.MessageType == MessageType.Channel) {
                    messages.Add(
                        new Tuple<MidiEvent, ChannelMessage>(midiEvent, midiEvent.MidiMessage as ChannelMessage)
                    );
                }
            }

            return messages;
        }

        private static void set_tempo(Sequence sequence, D_Staff staff)
        {
            // Tempo message is in channel 0
            List<Tuple<MidiEvent, MetaMessage>> meta_messages = getMetaMessages(sequence[0]);

            foreach (Tuple<MidiEvent, MetaMessage> metaPair in meta_messages) {
                MetaMessage metaMessage = metaPair.Item2;

                byte[] meta_bytes = metaPair.Item2.GetBytes();

                // BPM
                if (metaMessage.MetaType == MetaType.Tempo) {
                    // Bitshifting is nodig om het tempo in BPM te berekenen
                    int tempo = (meta_bytes[0] & 0xff) << 16 | (meta_bytes[1] & 0xff) << 8 | (meta_bytes[2] & 0xff);
                    staff.tempo = 60000000 / tempo;
                    return;
                }
            }

            staff.tempo = -1;
        }

        private static void set_measures(Sequence sequence, D_Staff staff)
        {
            // This function requires staff.num_of_beats to be set!

            // TimeSignature messages are in channel 0
            List<Tuple<MidiEvent, MetaMessage>> meta_messages = getMetaMessages(sequence[0]);

            foreach (Tuple<MidiEvent, MetaMessage> metaPair in meta_messages) {
                MidiEvent midiEvent = metaPair.Item1;
                MetaMessage metaMessage = metaPair.Item2;

                byte[] meta_bytes = metaMessage.GetBytes();

                // Time signature
                if (metaMessage.MetaType == MetaType.TimeSignature) {
                    int top_number = meta_bytes[0];
                    int bottom_number = (int)(Math.Pow(2, meta_bytes[1]));
                    int start_beat = midiEvent.AbsoluteTicks / sequence.Division;

                    staff.addMeasure(top_number, bottom_number, start_beat);
                }
            }

            staff.setMeasureEndTimes();
        }

        private static void set_num_of_beats(Sequence sequence, D_Staff staff)
        {
            // EndOfTrack messages are in channel 1
            List<Tuple<MidiEvent, MetaMessage>> meta_messages = getMetaMessages(sequence[1]);

            foreach (Tuple<MidiEvent, MetaMessage> metaPair in meta_messages) {
                MidiEvent midiEvent = metaPair.Item1;
                MetaMessage metaMessage = metaPair.Item2;

                byte[] meta_bytes = metaMessage.GetBytes();

                // EndOfTrack
                if (metaMessage.MetaType == MetaType.EndOfTrack) {
                    int beats = midiEvent.AbsoluteTicks / sequence.Division;

                    staff.num_of_beats = beats;
                }
            }
        }

        private static List<D_Note> get_notes_from_track(Track track, D_Staff staff, int ticks_per_beat)
        {
            List<D_Note> notes = new List<D_Note>();

            MidiEvent previous_midi_event = null;

            List<Tuple<MidiEvent, ChannelMessage>> channel_messages = getChannelMessages(track);

            foreach(Tuple<MidiEvent, ChannelMessage> channel_pair in channel_messages) {
                MidiEvent midiEvent = channel_pair.Item1;
                ChannelMessage channelMessage = channel_pair.Item2;

                if (previous_midi_event != null) {
                    D_Note note = get_note(midiEvent, channelMessage, previous_midi_event, ticks_per_beat);

                    if (note != null) {
                        notes.Add(get_note(midiEvent, channelMessage, previous_midi_event, ticks_per_beat));
                    }
                }

                previous_midi_event = midiEvent;
            }

            return notes;
        }

        private static D_Note get_note(MidiEvent midiEvent, ChannelMessage channelMessage, MidiEvent previous_midi_event, int ticks_per_beat)
        {
            if (channelMessage.Command.ToString() == "NoteOn" && channelMessage.Data2 == 0) {
                // Make note
                double note_beats = ((double)midiEvent.AbsoluteTicks - (double)previous_midi_event.AbsoluteTicks) / (double)ticks_per_beat;
                int note_beats_measured = (int)(note_beats * 4);

                return MidiNoteParser.create_note(channelMessage.Data1, note_beats_measured);
            }
            else if (channelMessage.Command.ToString() == "NoteOn" && channelMessage.Data2 == 90) {
                // Make rest
                if (midiEvent.DeltaTicks > 0) {
                    double note_beats = (double)midiEvent.DeltaTicks / (double)ticks_per_beat;
                    int note_beats_measured = (int)(note_beats * 4);
                    return MidiNoteParser.create_rest(note_beats_measured);
                }
            }

            return null;
        }

        private static D_Staff parseSequence(Sequence sequence)
        {
            D_Staff staff = new D_Staff();

            staff.clef = clef.g_key;
            set_num_of_beats(sequence, staff);
            set_tempo(sequence, staff);
            set_measures(sequence, staff);
            int ticks_per_beat = sequence.Division;
            staff.make_bars();
            List<D_Note> notes = get_notes_from_track(sequence[1], staff, ticks_per_beat);
            staff.fillBarsWithNotes(notes);

            return staff;
        }
    }
}
