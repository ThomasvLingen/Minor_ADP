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

            return parseSequence(sequence);
        }

        private static D_Staff parseSequence(Sequence sequence)
        {
            D_Staff staff = new D_Staff(0,0);
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
