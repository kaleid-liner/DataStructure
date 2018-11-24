using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.IO;

namespace HuffmanZip
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ZipClickAsync(object sender, RoutedEventArgs e)
        {
            var fileChooser = new Microsoft.Win32.OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "选择需要压缩的文件",
                InitialDirectory = Environment.CurrentDirectory
            };
            if (fileChooser.ShowDialog() == true)
            {
                string filename = fileChooser.FileName;
                FolderBrowserDialog folderChooser = new FolderBrowserDialog()
                {
                    Description = "选择目标目录",
                    RootFolder = Environment.SpecialFolder.Desktop,
                    SelectedPath = Path.GetDirectoryName(filename)
                };
                if (folderChooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = string.IsNullOrEmpty(folderChooser.SelectedPath)
                        ? Path.GetDirectoryName(filename) : folderChooser.SelectedPath;
                    progressIndicator.Visibility = Visibility.Visible;
                    await Task.Run(() =>
                        DataStructure.HuffmanZip.HuffmanZip.Compress(filename,
                        Path.Combine(path, Path.GetFileName(filename) + ".huff")));
                    progressIndicator.Visibility = Visibility.Hidden;
                    tipsSnackBar.MessageQueue.Enqueue("成功压缩到" + path, "GOT IT", () => { });
                }
            }
        }

        private async void UnzipClickAsync(object sender, RoutedEventArgs e)
        {
            var fileChooser = new Microsoft.Win32.OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "选择需要解压的文件",
                Filter = "Huff files (*.huff)|*.huff|All files|*.*",
                InitialDirectory = Environment.CurrentDirectory
            };
            if (fileChooser.ShowDialog() == true)
            {
                string filename = fileChooser.FileName;
                string destinationFileName = Path.GetFileName(filename);
                if (destinationFileName.EndsWith(".huff"))
                {
                    destinationFileName = destinationFileName.Substring(0, destinationFileName.Length - 5);
                }
                FolderBrowserDialog folderChooser = new FolderBrowserDialog()
                {
                    Description = "选择解压目录",
                    RootFolder = Environment.SpecialFolder.Desktop,
                    SelectedPath = Path.GetDirectoryName(filename)
                };
                folderChooser.ShowDialog();
                string path = string.IsNullOrEmpty(folderChooser.SelectedPath)
                    ? Path.GetDirectoryName(filename) : folderChooser.SelectedPath;
                progressIndicator.Visibility = Visibility.Visible;
                try
                {
                    await Task.Run(() => DataStructure.HuffmanZip.HuffmanZip.Decompress(filename,
                        Path.Combine(path, destinationFileName)));
                    tipsSnackBar.MessageQueue.Enqueue("成功解压到" + path, "GOT IT", () => { });
                }
                catch (FormatException)
                {
                    tipsSnackBar.MessageQueue.Enqueue("压缩文件格式损坏", "GOT IT", () => { });
                }
                finally
                {
                    progressIndicator.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
