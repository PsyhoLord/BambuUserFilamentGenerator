using MvvmCross.Platforms.Wpf.Views;
using BambuConfigGenerator.Core.ViewModels;
using Microsoft.Win32;
using System.Windows;

namespace BambuConfigGenerator.Views
{
    /// <summary>
    /// Interaction logic for FileGeneratorView.xaml
    /// </summary>
    public partial class FileGeneratorView : MvxWpfView<FileGeneratorViewModel>
    {
        public FileGeneratorView()
        {
            InitializeComponent();
        }

        private void SelectFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a file",
                Filter = "JSON files (*.json)|*.json",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;
                MessageBox.Show($"Selected file: {selectedFile}");
            }

        }
    }
}
