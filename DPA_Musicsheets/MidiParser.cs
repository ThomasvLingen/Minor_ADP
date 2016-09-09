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
            

            
            Track track = sequence[1];

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
                        Console.WriteLine(
                            String.Format(
                                "Keycode: {0}, Command: {1}, absolute time: {2}, delta time: {3}"
                                , channelMessage.Data1, channelMessage.Command, midiEvent.AbsoluteTicks, midiEvent.DeltaTicks
                            )
                        );

                        break;
                    case MessageType.Meta:
                        var metaMessage = midiEvent.MidiMessage as MetaMessage;
                        byte[] meta_bytes = metaMessage.GetBytes();
                        if (metaMessage.MetaType == MetaType.TimeSignature) {
                            int top_number = meta_bytes[0];
                            int bottom_number = (int) (1 / Math.Pow(meta_bytes[1], -2));
                            Console.WriteLine(String.Format("Time signature is {0}/{1}", top_number, bottom_number));
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
