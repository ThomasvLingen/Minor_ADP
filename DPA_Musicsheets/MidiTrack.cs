using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class MidiTrack
    {
        public string TrackName { get; set; }
        public ObservableCollection<string> Messages { get; private set; }

        public MidiTrack()
        {
            this.Messages = new ObservableCollection<string>();
        }
    }
}