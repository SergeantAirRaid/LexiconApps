using System;
using System.Collections.Generic;
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

namespace SystemArchiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private (List<FileInfo>, long) _Files = (new List<FileInfo>(), 0);
        private Dictionary<string, int> _Extensions = new Dictionary<string, int>();

        public MainWindow()
        {
            InitializeComponent();

            //LexiTools.FileIO.DirectoryProcessUpdate += FileIO_DirectoryProcessUpdate;
        }

        private void FileIO_DirectoryProcessUpdate(object sender, EventArgs e)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                txtConsole.Text = sender.ToString() + Environment.NewLine + txtConsole.Text;
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtConsole.Text = "";
                btnProcess.IsEnabled = false;
                barProgress.Visibility = Visibility.Visible;
                string filepath = txtFilepath.Text;
                await Task.Run(() => ProcessFileStructure(filepath));

                txtConsole.Text = "";
                foreach (var ext in _Extensions)
                {
                    txtConsole.Text += ext.Key + ": " + ext.Value + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                btnProcess.IsEnabled = true;
                barProgress.Visibility = Visibility.Hidden;
            }
        }

        private async Task ProcessFileStructure(string path)
        {
            _Files = LexiTools.FileIO.GetAllFiles(path);
        }
    }
}
