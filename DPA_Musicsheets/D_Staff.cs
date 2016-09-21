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
        public Tuple<int, int> measure { get; private set; }

        public D_Staff(int top_measure, int bottom_measure)
        {
            this.bars = new List<D_Bar>();
            this.measure = new Tuple<int, int>(top_measure, bottom_measure);
        }

        public void addBar(D_Bar bar)
        {
            this.bars.Add(bar);
        }

        public void removeBar(D_Bar bar)
        {
            this.bars.Remove(bar);
        }

        public void setMeasure(int top_measure, int bottom_measure)
        {
            this.measure = new Tuple<int, int>(top_measure, bottom_measure);
        }
    }
}
