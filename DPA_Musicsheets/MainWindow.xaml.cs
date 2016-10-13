﻿using Microsoft.Win32;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.States;
using DPA_Musicsheets.Memento;
using DPA_Musicsheets.Command;

namespace DPA_Musicsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<MidiTrack> MidiTracks { get; private set; }

        D_Staff songData;
        Editor editor;
        EditorStateManager manager = new EditorStateManager();
        EditorHistoryCaretaker editor_history;
        EditorCommandList editor_commands;
        StaffView note_viewer;

        private List<System.Windows.Input.Key> keys_down = new List<System.Windows.Input.Key>();

        bool undo_redo_pressed = false;

        public MainWindow()
        {
            InitializeComponent();

            this.MidiTracks = new ObservableCollection<MidiTrack>();
            this.DataContext = MidiTracks;

            this.editor = new Editor(lilypondEditor, editorCallback);
            this.editor_history = new EditorHistoryCaretaker();
            this.editor_commands = new EditorCommandList(this.editor);
            this.lilypondEditor.TextChanged += new System.Windows.Controls.TextChangedEventHandler(editor.newChange);

            this.note_viewer = new StaffView(this.ListBoxViewer);

            this.manager.state = new NoChangesEditorState();

            this.updateHistoryButtons();
        }

        private void keydown_event(object sender, KeyEventArgs e)
        {
            this.keys_down.Add(e.Key);

            if (this.keys_down.ContainsSameItems(new List<System.Windows.Input.Key>() { System.Windows.Input.Key.LeftCtrl })) {
                this.editor_commands.executeCommand("add_treble");
            }
        }

        private void keyup_event(object sender, KeyEventArgs e)
        {
            this.keys_down.Remove(e.Key);
        }

        public void editorCallback()
        {
            this.evaluate_editor_contents(this.undo_redo_pressed);

            if (this.undo_redo_pressed) {
                this.undo_redo_pressed = false;
            }

            this.manager.state = new ChangesEditorState();
        }

        public void evaluate_editor_contents(bool undo_redo_pressed)
        {
            try {
                this.songData = LilypondParser.getInstance().parseText(editor.getText());
                this.updateNoteViewer(); 

                if (!undo_redo_pressed) {
                    this.editor_history.save(new EditorMemento(this.editor));
                }

                this.updateHistoryButtons();
            } catch (Exception e) {
                Console.WriteLine("NON VALID LILYPOND YOU LOSER");
            }
        }

        private void updateNoteViewer()
        {
            Dispatcher.Invoke(() => {
                this.note_viewer.updateView(this.songData);
            });
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
                    this.editor.enable();
                    this.editor.setFileName(filename);
                    this.editor.readFile();
                    //this.songData = LilypondParser.getInstance().parseFile(filename);
                break;
                default:
                    Console.WriteLine("I can't parse " + extension);
                break;
            }

            this.updateNoteViewer();
        }

        private void showMidiTracks(IEnumerable<MidiTrack> midiTracks)
        {
            MidiTracks.Clear();

            foreach (var midiTrack in midiTracks) {
                MidiTracks.Add(midiTrack);
            }

            tabCtrl_MidiContent.SelectedIndex = 0;
        }

        private void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MidiPlayerWrapper.shutdownPlayer();
            this.manager.quit_callback();
        }

        private void updateHistoryButtons()
        {
            this.btnRedo.Dispatcher.Invoke(() => {
                this.btnRedo.IsEnabled = this.editor_history.canRedo();
            });

            this.btnUndo.Dispatcher.Invoke(() => {
                this.btnUndo.IsEnabled = this.editor_history.canUndo();
            });
        }

        private void btnRedoClick(object s, RoutedEventArgs e)
        {
            this.undo_redo_pressed = true;
            var redo_memento = this.editor_history.redo();

            if (redo_memento != null) {
                this.editor.restoreFromMemento(redo_memento);
            }

            this.updateHistoryButtons();
        }

        private void btnUndoClick(object s, RoutedEventArgs e)
        {
            this.undo_redo_pressed = true;
            var undo_memento = this.editor_history.undo();

            if (undo_memento != null) {
                this.editor.restoreFromMemento(undo_memento);
            }

            this.updateHistoryButtons();
        }
    }
}
