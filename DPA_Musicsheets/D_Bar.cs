using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class D_Bar
    {
        public List<D_Note> notes { get; private set; }

        public D_Bar()
        {
            notes = new List<D_Note>();
        }

        public void addNote(D_Note note)
        {
            this.notes.Add(note);
        }

        public void removeNote(D_Note note)
        {
            this.notes.Remove(note);
        }

    }
}
