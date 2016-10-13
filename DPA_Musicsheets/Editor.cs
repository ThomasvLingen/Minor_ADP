﻿using System;
using System.Collections.Generic;
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
        Timer tmr = new Timer();
        String file_path = ".temp";
        Action idle_callback;

        public Editor(TextBox textbox, Action idle_callback)
        {
            this.view = textbox;
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

        public void saveFile()
        {
            this.view.Dispatcher.Invoke(() => {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Lilypond music file|*.ly";
                dialog.Title = "Save music";

                dialog.ShowDialog();

                if (dialog.FileName != "") {
                    System.IO.File.WriteAllText(dialog.FileName, this.view.Text);
                }
            });
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
