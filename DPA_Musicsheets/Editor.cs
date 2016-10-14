using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows;
using DPA_Musicsheets.Memento;
using Microsoft.Win32;

namespace DPA_Musicsheets {
    public class Editor {

        TextBox view;
        TextBox open_file_view;
        Timer tmr = new Timer();
        String file_path = ".temp";
        Action idle_callback;

        public Editor(TextBox editor_textbox, TextBox open_file_textbox, Action idle_callback)
        {
            this.view = editor_textbox;
            this.open_file_view = open_file_textbox;
            this.view.IsEnabled = false;
            this.view.AcceptsReturn = true;
            this.idle_callback = idle_callback;
            tmr.Interval = 2000;
            tmr.Elapsed += new ElapsedEventHandler(timerCallBack);
        }

        public void setText(string text)
        {
            this.view.Dispatcher.Invoke(() => {
                this.view.Text = text;
            });
        }

        public string getText()
        {
            return this.view.Dispatcher.Invoke(() => {
                return this.view.Text;
            });
        }

        public string getCurrentSelection()
        {
            return this.view.Dispatcher.Invoke(() => {
                return this.view.SelectedText;
            });
        }

        public void setCurrentIndex(int index)
        {
            this.view.Dispatcher.Invoke(() => {
                this.view.SelectionStart = index;
            });
        }

        public int getCurrentIndex()
        {
            return this.view.Dispatcher.Invoke(() => {
                return this.view.SelectionStart;
            });
        }

        public void setFileName(String file_path)
        {
            this.file_path = file_path;
        }

        public void readFile()
        {
            this.view.Text = System.IO.File.ReadAllText(this.file_path);
        }

        public void openFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "All Music Files|*.mid;*.ly|Midi Files(.mid)|*.mid|Lilypond files (.ly)|*.ly" };
            if (openFileDialog.ShowDialog() == true) {
                this.open_file_view.Dispatcher.Invoke(() => {
                    this.open_file_view.Text = openFileDialog.FileName;
                });
            }
        }

        public void saveFile()
        {
            this.view.Dispatcher.Invoke(() => {
                string filename = this.getSaveFilenameFromDialog("Lilypond music file|*.ly", "Save music");

                if (filename != "") {
                    this.saveFile(filename);
                }
            });
        }

        public string getSaveFilenameFromDialog(string filter, string title)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = filter;
            dialog.Title = title;

            dialog.ShowDialog();

            return dialog.FileName;
        }

        public void saveFile(string filename)
        {
            this.view.Dispatcher.Invoke(() => {
                if (filename != "") {
                    System.IO.File.WriteAllText(filename, this.view.Text);
                }
            });
        }

        // This function assumes that `lilypond` is accessible from the command line
        public void saveToPDF()
        {
            string tmp_source_file = @"lily_tmp";
            this.saveFile(tmp_source_file + ".ly");
            string lilypond_location = @"lilypond";
            string destination_file = this.getSaveFilenameFromDialog("PDF file|*.PDF", "Save music in PDF");

            if (destination_file != "") {
                var process = new Process {
                    StartInfo = {
                        WorkingDirectory = Directory.GetCurrentDirectory(),
                        WindowStyle = ProcessWindowStyle.Normal,
                        Arguments = String.Format("--pdf \"{0}\"", "./" + tmp_source_file + ".ly"),
                        FileName = lilypond_location
                    }
                };

                process.Start();
                while (!process.HasExited) {}
                File.Copy("./" + tmp_source_file + ".pdf", destination_file);
            }
        }

        private void setEnable(bool state)
        {
            this.view.IsEnabled = state;
        }

        public void enable()
        {
            this.setEnable(true);
        }

        public void disable()
        {
            this.setEnable(false);
        }

        public void newChange(object sender, EventArgs e)
        {
            tmr.Stop();
            tmr.Start();
        }

        public void timerCallBack(object sender, EventArgs e)
        {
            tmr.Stop();
            this.idle_callback();
        }

        public void restoreFromMemento(EditorMemento memento)
        {
            this.setText(memento.editor_contents);
        }
    }
}
