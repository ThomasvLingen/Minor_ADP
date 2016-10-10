using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.States {
    class ChangesEditorState : EditorState {
        public void quit_callback()
        {
            string message_box_text = "YOU SHOULD SAVE YOUR WORK, CHAP";
            string caption = "HELLO";

            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBox.Show(message_box_text, caption, button, icon);

            Console.WriteLine("YOU SHOULD SAVE YOUR WORK, CHAP");
        }
    }
}
