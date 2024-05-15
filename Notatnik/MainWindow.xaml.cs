using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notatnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = "";
        private bool save = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            Text.Cut();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Text.Copy();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            Text.Paste();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int selectionStart = Text.SelectionStart;
            Text.Text = Text.Text.Remove(selectionStart, Text.SelectionLength);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }

        private void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt";
            dialog.Multiselect = false;
            dialog.Title = "Otwórz";
            if (dialog.ShowDialog() == true)
            {
                Text.Text = File.ReadAllText(dialog.FileName);
                path = dialog.FileName;
                save = true;
            }
        }

        private void Save()
        {
            if (path == "")
            {
                SaveAs();
            }
            else
            {
                File.WriteAllText(path, Text.Text);
                save = true;
            }
        }

        private bool SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Zapisz jako";
            dialog.Filter = "txt files (*.txt)|*.txt";
            dialog.AddExtension = true;

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, Text.Text);
                path = dialog.FileName;
                save = true;
                return true;
            }
            return false;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void Text_Changed(object sender, TextChangedEventArgs e)
        {
            if (path != "")
            {
                if (File.ReadAllText(path) != Text.Text)
                    save = false;
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (!save)
            {
                MessageBoxResult response = MessageBox.Show("Czy chcesz zapisać zmiany w pliku?", "Notatnik", MessageBoxButton.YesNoCancel);
                if (response == MessageBoxResult.Yes)
                {
                    if (path == "")
                    {
                        if (!SaveAs())
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        Save();
                    }
                }
                else if(response == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            base.OnClosing(e);
        }
        private void ShortCut_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.O)
                {
                    Open();
                    Keyboard.ClearFocus();
                }
                if (e.Key == Key.S)
                {
                    Save();
                    Keyboard.ClearFocus();
                }
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (e.Key == Key.S)
                    {
                        SaveAs();
                        Keyboard.ClearFocus();
                    }
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}