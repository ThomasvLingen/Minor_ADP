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

        public ObservableCollection<PSAMWPFControlLibrary.IncipitViewerWPF> StaffViewers { get; set; } = new ObservableCollection<PSAMWPFControlLibrary.IncipitViewerWPF>();

        //private void btn_Show_Click(object sender, RoutedEventArgs e)
        //{
        //    StaffViewers.Clear();
        //    //var staff = PSAMStaff.BuildStaff(song.Tracks[0]);
        //    //StaffViewers.Add(staff);
        //    ListBoxViewer.ItemsSource = StaffViewers;
        //    e.Handled = true;
        //}

        public MainWindow()
        {
            this.MidiTracks = new ObservableCollection<MidiTrack>();
            InitializeComponent();
            DataContext = MidiTracks;
            //notenbalk.LoadFromXmlFile("Resources/example.xml");
        }

        private IncipitViewerWPF getNewStaff()
        {
            IncipitViewerWPF to_return = new IncipitViewerWPF();

            to_return.AddMusicalSymbol(new Clef(ClefType.GClef, 2));
            to_return.Width = 525;
            to_return.Height = 120;
            to_return.HorizontalAlignment = HorizontalAlignment.Left;
            to_return.VerticalAlignment = VerticalAlignment.Center;
            to_return.VerticalContentAlignment = VerticalAlignment.Center;

            return to_return;
        }

        private void updatePSAMWithSongData()
        {
            IncipitViewerWPF temp_staff = getNewStaff();
            
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
                    temp_staff = getNewStaff();
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
            _player.Play(txt_MidiFilePath.Text);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi Files(.mid)|*.mid" };
            if (openFileDialog.ShowDialog() == true)
            {
                txt_MidiFilePath.Text = openFileDialog.FileName;
            }
        }
        
        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (_player != null)
                _player.Dispose();
        }

        private void btn_ShowContent_Click(object sender, RoutedEventArgs e)
        {
            ShowMidiTracks(MidiReader.ReadMidi(txt_MidiFilePath.Text));
            this.songData = MidiParser.parseMidi(txt_MidiFilePath.Text);
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
