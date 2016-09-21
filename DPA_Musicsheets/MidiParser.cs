﻿using System;
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

        private static D_Staff good_parseSequence(Sequence sequence)
        {
            D_Staff staff = new D_Staff();
            int bpm = -1;
            int ticks_per_beat = sequence.Division;
            Console.WriteLine(String.Format("Ticks per beat: {0}", ticks_per_beat));

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

                            Console.WriteLine(String.Format("[{2}] Time signature is {0}/{1}", top_number, bottom_number, time_signature_event));
                            staff.addMeasure(time_signature_event, top_number, bottom_number);
                        }

                        // BPM
                        if (metaMessage.MetaType == MetaType.Tempo) {
                            // Bitshifting is nodig om het tempo in BPM te berekenen
                            int tempo = (meta_bytes[0] & 0xff) << 16 | (meta_bytes[1] & 0xff) << 8 | (meta_bytes[2] & 0xff);
                            bpm = 60000000 / tempo;

                            staff.tempo = bpm;
                            Console.WriteLine(String.Format("BPM is {0}", staff.tempo));
                        }
                        break;
                }

            }

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
