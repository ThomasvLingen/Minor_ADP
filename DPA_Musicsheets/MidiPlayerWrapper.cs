using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets {

    class MidiPlayerWrapper {

        private static MidiPlayer _player;

        private static OutputDevice _outputDevice = new OutputDevice(0);

        public static void playMidi(String path)
        {
            if (_player != null) {
                _player.Dispose();
            }

            _player = new MidiPlayer(_outputDevice);
            _player.Play(path);
        }

        public static void stopPlayMidi()
        {
            if (_player != null) {
                _player.Dispose();
            }
        }

        public static void shutdownPlayer()
        {
            _outputDevice.Close();
            if (_player != null) {
                _player.Dispose();
            }
        }
    }
}
