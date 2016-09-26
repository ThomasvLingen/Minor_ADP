﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class D_Staff
    {
        public List<D_Bar> bars { get; private set; }
        public List<D_Measure> measures { get; private set; }
        public int tempo { get; set; }
        public int num_of_beats { get; set; }

        public D_Staff()
        {
            this.bars = new List<D_Bar>();
            this.measures = new List<D_Measure>();
            this.tempo = -1;
        }

        public void fillBarsWithNotes(List<D_Note> notes)
        {
            // Warning: this does not take notes into account that span multiple bars
            int current_bar_index = 0;

            foreach (D_Note note in notes) {
                this.bars[current_bar_index].addNote(note);

                if (this.bars[current_bar_index].isFull()) {
                    current_bar_index++;
                }
            }

            if (!this.bars[this.bars.Count - 1].isFull()) {
                throw new Exception("Last bar is not full!");
            }
        }

        //public static List<D_Bar> getBarsFromNotes(List<D_Note> notes)
        //{
        //    List<D_Bar> bars = new List<D_Bar>();

        //    D_Bar bar_to_add = new D_Bar();
        //    foreach (D_Note note in notes) {
        //        bar_to_add.addNote(note);

        //        if (bar_to_add.isFull()) {
        //            bars.Add(bar_to_add);
        //            bar_to_add = new D_Bar();
        //        }
        //    }

        //    if (!bar_to_add.isFull()) {
        //        throw new Exception("Last bar is not full!");
        //    }

        //    return bars;
        //}


        public void addBar(D_Bar bar)
        {
            this.bars.Add(bar);
        }

        public void removeBar(D_Bar bar)
        {
            this.bars.Remove(bar);
        }

        public void addMeasure(int top, int bottom, int start_beat, int end_beat)
        {
            this.measures.Add(new D_Measure(top, bottom, start_beat, end_beat));
        }

        public void addMeasure(int top, int bottom, int start_beat)
        {
            this.measures.Add(new D_Measure(top, bottom, start_beat));
        }

        public D_Measure getMeasure(int time)
        {
            foreach(D_Measure measure in this.measures) {
                if (time >= measure.start_beat && time < measure.end_beat) {
                    return measure;
                }
            }

            return null;
        }

        public void setMeasureEndTimes()
        {
            int measure_index = 0;
            int measure_end_index = this.measures.Count - 1;

            while (measure_index != measure_end_index) {
                if (measure_index != measure_end_index) {
                    this.measures[measure_index].end_beat = this.measures[measure_index + 1].start_beat;
                }
                measure_index++;
            }

            this.measures[measure_end_index].end_beat = this.num_of_beats + 1;
        }
    }
}
