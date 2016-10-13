using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using System.Windows.Controls;

namespace DPA_Musicsheets {
    public class StaffView {
        private ObservableCollection<IncipitViewerWPF> StaffViewers { get; set; } = new ObservableCollection<IncipitViewerWPF>();
        private ListBox view;

        public StaffView(ListBox view)
        {
            this.view = view;
        }

        public void updateView(D_Staff staff)
        {
            if (staff == null) {
                return;
            }

            StaffViewers.Clear();

            IncipitViewerWPF temp_staff = IncipitViewerWPFWrapper.getWPFstaff(staff.clef);

            int bar_count = 0;
            D_Measure previous_bar_measure = null;
            foreach (D_Bar bar in staff.bars) {
                bar_count++;
                if (bar.measure != previous_bar_measure) {
                    temp_staff.AddMusicalSymbol(PSAMAdapter.fromMeasure(bar.measure));
                }

                foreach (D_Note note in bar.notes) {
                    temp_staff.AddMusicalSymbol(PSAMAdapter.fromNote(note));
                }

                if (bar_count == 4) {
                    StaffViewers.Add(temp_staff);
                    temp_staff = IncipitViewerWPFWrapper.getWPFstaff(staff.clef);
                    bar_count = 0;
                }

                temp_staff.AddMusicalSymbol(new Barline());
                previous_bar_measure = bar.measure;
            }

            if (bar_count < 4) {
                StaffViewers.Add(temp_staff);
            }

            this.view.ItemsSource = StaffViewers;
        }
    }
}
