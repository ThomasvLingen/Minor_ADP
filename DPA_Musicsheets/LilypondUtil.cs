using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets {
    public class LilypondUtil {

        private static Dictionary<NoteLevel, int> noteLevelInt = new Dictionary<NoteLevel, int>() {
                { NoteLevel.C, 1 },
                { NoteLevel.D, 2 },
                { NoteLevel.E, 3 },
                { NoteLevel.F, 4 },
                { NoteLevel.G, 5 },
                { NoteLevel.A, 6 },
                { NoteLevel.B, 7 }
        };

        private static int max_index = 7;
        private static int min_index = 1;

        public static int getClosestNotePositionInSteps(D_Note start, D_Note goal)
        {
            int step;
            int i_old = noteLevelInt[start.level];
            int i_new = noteLevelInt[goal.level];

            if (i_old > i_new) {
                step = max_index - i_old + i_new;
            }
            else {
                step = i_new - i_old;
            }

            if (step > 3) {
                step = step - max_index;
            }

            return step;
        }

        public static bool newNoteOctaveChange(D_Note start, int step)
        {
            int i_old = noteLevelInt[start.level];

            if (step < 0 && i_old + step < min_index) {
                return true;
            }
            else if (step > 0 && i_old + step > max_index) {
                return true;
            }
            return false;
        }
    }
}
