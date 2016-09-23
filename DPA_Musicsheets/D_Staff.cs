using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class D_Staff
    {
        public List<D_Bar> bars { get; private set; }
        public List<Tuple<int, Tuple<int, int>>> measures { get; private set; }
        public int tempo { get; set; }

        public D_Staff()
        {
            this.bars = new List<D_Bar>();
            this.measures = new List<Tuple<int, Tuple<int, int>>>();
            this.tempo = -1;
        }

        public void addBar(D_Bar bar)
        {
            this.bars.Add(bar);
        }

        public void removeBar(D_Bar bar)
        {
            this.bars.Remove(bar);
        }

        public void addMeasure(int time, int top, int bottom)
        {
            Tuple<int, int> measure = new Tuple<int, int>(top, bottom);
            Tuple<int, Tuple<int, int>> timed_measure = new Tuple<int, Tuple<int, int>>(time, measure);

            this.measures.Add(timed_measure);
        }

        public Tuple<int, int> getMeasure(int time)
        {
            if(time > measures[measures.Count - 1].Item1) {
                return measures[measures.Count - 1].Item2;
            }
            for(int i = 0; i < measures.Count; i++) {
                if (time < measures[i+1].Item1) {
                    return measures[i].Item2;
                }
            }

            return null;
        }
    }
}
