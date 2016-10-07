using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class D_NoteFactory
    {

        public static D_Note create_note(NoteLevel level, NoteAlteration alteration, int octave, int beats)
        {
            return new D_Note(
                level,
                beats,
                octave,
                alteration,
                false
            );
        }

        public static D_Note create_note(D_Note other, int new_length)
        {
            return new D_Note(other, new_length);
        }

        public static D_Note create_rest(int beats)
        {
            return new D_Note(
                NoteLevel.rest,
                beats,
                0,
                NoteAlteration.none,
                true
            );
        }
    }
}