using Microsoft.Win32;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DPA_Musicsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<MidiTrack> MidiTracks { get; private set; }

        D_Staff songData;

        public ObservableCollection<IncipitViewerWPF> StaffViewers { get; set; } = new ObservableCollection<IncipitViewerWPF>();

        public MainWindow()
        {
            this.MidiTracks = new ObservableCollection<MidiTrack>();
            InitializeComponent();
            DataContext = MidiTracks;
        }

        private void updatePSAMWithSongData()
        {
            StaffViewers.Clear();

            IncipitViewerWPF temp_staff = IncipitViewerWPFWrapper.getWPFstaff(this.songData.clef);
            
            int bar_count = 0;
            D_Measure previous_bar_measure = null;
            foreach(D_Bar bar in this.songData.bars) {
                bar_count++;
                if (bar.measure != previous_bar_measure) {
                    temp_staff.AddMusicalSymbol(PSAMAdapter.fromMeasure(bar.measure));
                }

                foreach(D_Note note in bar.notes) {
                    temp_staff.AddMusicalSymbol(PSAMAdapter.fromNote(note));
                }

                if(bar_count == 4) {
                    StaffViewers.Add(temp_staff);
                    temp_staff = IncipitViewerWPFWrapper.getWPFstaff(this.songData.clef);
                    bar_count = 0;
                }

                temp_staff.AddMusicalSymbol(new Barline());
                previous_bar_measure = bar.measure;
            }

            if (bar_count < 4) {
                StaffViewers.Add(temp_staff);
            }

            ListBoxViewer.ItemsSource = StaffViewers;
        }

        private void btnPlayClick(object sender, RoutedEventArgs e)
        {
            MidiPlayerWrapper.playMidi(txt_SongFilePath.Text);
        }

        private void btnStopClick(object sender, RoutedEventArgs e)
        {
            MidiPlayerWrapper.stopPlayMidi();
        }

        private void btnOpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "All Music Files|*.mid;*.ly|Midi Files(.mid)|*.mid|Lilypond files (.ly)|*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                txt_SongFilePath.Text = openFileDialog.FileName;
            }
        }
        
        private void btnShowContentClick(object sender, RoutedEventArgs e)
        {
            string filename = txt_SongFilePath.Text;
            string extension = System.IO.Path.GetExtension(filename);

            switch (extension) {
                case ".mid":
                    showMidiTracks(MidiReader.ReadMidi(filename));
                    this.songData = MidiParser.getInstance().parseFile(filename);
                break;
                case ".ly":
                    this.songData = LilypondParser.getInstance().parseFile(filename);
                break;
                default:
                    Console.WriteLine("I can't parse " + extension);
                break;
            }

            this.updatePSAMWithSongData();
        }

        private void showMidiTracks(IEnumerable<MidiTrack> midiTracks)
        {
            MidiTracks.Clear();
            foreach (var midiTrack in midiTracks)
            {
                MidiTracks.Add(midiTrack);
            }

            tabCtrl_MidiContent.SelectedIndex = 0;
        }

        private void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MidiPlayerWrapper.shutdownPlayer();
        }
    }
}
