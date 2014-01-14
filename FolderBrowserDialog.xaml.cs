using System.Windows;
using System.Windows.Input;

namespace FolderBrowser
{
    /// <summary>
    /// Interaction logic for FolderBrowserDialog.xaml
    /// </summary>
    public partial class FolderBrowserDialog : Window
    {
        private BrowserViewModel _viewModel;

        public BrowserViewModel ViewModel
        {
            get
            { 
                return _viewModel = _viewModel ?? new BrowserViewModel();
            }
        }

        public string SelectedFolder
        {
            get { return ViewModel.SelectedFolder; }
            set { ViewModel.SelectedFolder = value; }
        }

        public FolderBrowserDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
            {
                // close the dialog on a double-click of a folder
                DialogResult = true;
            }
        }
    }
}
