using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets {
    public class D_Measure {
        // Top number
        public int beats_per_bar = -1;
        // Bottom number
        public int beat_length = -1;

        public int start_beat = 0;
        public int end_beat = 0;
    
        public D_Measure(int beats_per_bar, int beat_length, int start_beat, int end_beat)
        {
            this.beats_per_bar = beats_per_bar;
            this.beat_length =  beat_length;
            this.start_beat = start_beat;
            this.end_beat =  end_beat;
        }

        public D_Measure(int beats_per_bar, int beat_length, int start_beat)
        {
            this.beats_per_bar = beats_per_bar;
            this.beat_length = beat_length;
            this.start_beat = start_beat;
        }
    }
}
