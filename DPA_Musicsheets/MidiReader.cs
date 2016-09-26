using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DPA_Musicsheets
{
    public static class MidiReader
    {
        public static IEnumerable<MidiTrack> ReadMidi(string midiFileLocation)
        {
            var sequence = new Sequence();
            sequence.Load(midiFileLocation);

            return ReadSequence(sequence);
        }

        public static IEnumerable<MidiTrack> ReadSequence(Sequence sequence)
        {
            // De sequence heeft tracks. Deze zijn per index benaderbaar.
            for (int i = 0; i < sequence.Count; i++)
            {
                Track track = sequence[i];
                MidiTrack trackLog = new MidiTrack() { TrackName = i.ToString() };

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
                            trackLog.Messages.Add(String.Format("Keycode: {0}, Command: {1}, absolute time: {2}, delta time: {3}"
                                , channelMessage.Data1, channelMessage.Command, midiEvent.AbsoluteTicks, midiEvent.DeltaTicks));
                            break;
                        case MessageType.SystemExclusive:
                            break;
                        case MessageType.SystemCommon:
                            break;
                        case MessageType.SystemRealtime:
                            break;
                        // Meta zegt iets over de track zelf.
                        case MessageType.Meta:
                            var metaMessage = midiEvent.MidiMessage as MetaMessage;
                            trackLog.Messages.Add(GetMetaString(metaMessage));
                            if (metaMessage.MetaType == MetaType.TrackName)
                            {
                                trackLog.TrackName += " " + Encoding.Default.GetString(metaMessage.GetBytes());
                            }
                            break;
                        default:
                            trackLog.Messages.Add(String.Format("MidiEvent {0}, absolute ticks: {1}, deltaTicks: {2}", midiEvent.MidiMessage.MessageType, midiEvent.AbsoluteTicks, midiEvent.DeltaTicks));
                            break;
                    }
                }

                yield return trackLog;
            }
        }

        private static string GetMetaString(MetaMessage metaMessage)
        {
            byte[] bytes = metaMessage.GetBytes();
            switch (metaMessage.MetaType)
            {
                case MetaType.Tempo:
                    // Bitshifting is nodig om het tempo in BPM te be
                    int tempo = (bytes[0] & 0xff) << 16 | (bytes[1] & 0xff) << 8 | (bytes[2] & 0xff);
                    int bpm = 60000000 / tempo;
                    return metaMessage.MetaType + ": " + bpm;
                //case MetaType.SmpteOffset:
                //    break;
                case MetaType.TimeSignature:                               //kwart = 1 / 0.25 = 4
                    return metaMessage.MetaType + ": (" + bytes[0] + " / " + 1 / Math.Pow(bytes[1], -2) + ") ";
                //case MetaType.KeySignature:
                //    break;
                //case MetaType.ProprietaryEvent:
                //    break;
                case MetaType.TrackName:
                    return metaMessage.MetaType + ": " + Encoding.Default.GetString(metaMessage.GetBytes());
                default:
                    return metaMessage.MetaType + ": " + Encoding.Default.GetString(metaMessage.GetBytes());
            }
        }
    }
}
