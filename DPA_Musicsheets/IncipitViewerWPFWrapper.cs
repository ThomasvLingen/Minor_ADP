using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;

namespace DPA_Musicsheets {
    class IncipitViewerWPFWrapper {

        private static Dictionary<clef, ClefType> clef_types = new Dictionary<clef, ClefType>() {
            { clef.f_key, ClefType.FClef },
            { clef.g_key, ClefType.GClef }
        };

        public static IncipitViewerWPF getWPFstaff(clef clef_type)
        {
            IncipitViewerWPF to_return = new IncipitViewerWPF();

            to_return.AddMusicalSymbol(new Clef(clef_types[clef_type], 2));
            to_return.Width = 525;

            return to_return;
        }
    }
}
