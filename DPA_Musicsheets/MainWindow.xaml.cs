using Microsoft.Win32;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DPA_Musicsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MidiPlayer _player;
        public ObservableCollection<MidiTrack> MidiTracks { get; private set; }

        // De OutputDevice is een midi device of het midikanaal van je PC.
        // Hierop gaan we audio streamen.
        // DeviceID 0 is je audio van je PC zelf.
        private OutputDevice _outputDevice = new OutputDevice(0);
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

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(_player != null)
            {
                _player.Dispose();
            }

            _player = new MidiPlayer(_outputDevice);
            _player.Play(txt_SongFilePath.Text);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "All Music Files|*.mid;*.ly|Midi Files(.mid)|*.mid|Lilypond files (.ly)|*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                txt_SongFilePath.Text = openFileDialog.FileName;
            }
        }
        
        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (_player != null)
                _player.Dispose();
        }

        private void btn_ShowContent_Click(object sender, RoutedEventArgs e)
        {
            string filename = txt_SongFilePath.Text;
            string extension = System.IO.Path.GetExtension(filename);

            switch (extension) {
                case ".mid":
                    ShowMidiTracks(MidiReader.ReadMidi(filename));
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

        private void ShowMidiTracks(IEnumerable<MidiTrack> midiTracks)
        {
            MidiTracks.Clear();
            foreach (var midiTrack in midiTracks)
            {
                MidiTracks.Add(midiTrack);
            }

            tabCtrl_MidiContent.SelectedIndex = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _outputDevice.Close();
            if (_player != null)
            {
                _player.Dispose();
            }
        }
    }
}
