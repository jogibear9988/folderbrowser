using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using System.IO;

namespace FolderBrowser
{
    public class BrowserViewModel : ViewModelBase
    {
        private string _selectedFolder;
        private bool _expanding = false;

        public string SelectedFolder
        {
            get
            {
                return _selectedFolder;
            }
            set
            {
                _selectedFolder = value;
                OnPropertyChanged("SelectedFolder");
                OnSelectedFolderChanged();
            }
        }

        public ObservableCollection<FolderViewModel> Folders
        {
            get;
            set;
        }

        public DelegateCommand<object> FolderSelectedCommand
        {
            get
            {
                return new DelegateCommand<object>(it => SelectedFolder = Environment.GetFolderPath((Environment.SpecialFolder)it));
            }
        }
     
        
        public BrowserViewModel()
        {
            Folders = new ObservableCollection<FolderViewModel>();
            Environment.GetLogicalDrives().ToList().ForEach(it => Folders.Add(new FolderViewModel { Root = this, FolderPath = it.TrimEnd('\\'), FolderName = it.TrimEnd('\\'), FolderIcon = "Images\\HardDisk.ico" }));
        }

        private void OnSelectedFolderChanged()
        {
            if (!_expanding)
            {
                try
                {
                    _expanding = true;
                    FolderViewModel child = Expand(Folders, SelectedFolder);
                    child.IsSelected = true;
                }
                finally
                {
                    _expanding = false;
                }
            }
        }

        private FolderViewModel Expand(ObservableCollection<FolderViewModel> childFolders, string path)
        {
            if (String.IsNullOrEmpty(path) || childFolders.Count == 0)
            {
                return null;
            }

            string folderName = path;
            if (path.Contains('/') || path.Contains('\\'))
            {
                int idx = path.IndexOfAny(new char[] { '/', '\\' });
                folderName = path.Substring(0, idx);
                path = path.Substring(idx + 1);
            }
            else
            {
                path = null;
            }

            var results = childFolders.Where<FolderViewModel>(folder => folder.FolderName == folderName);
            if (results != null && results.Count() > 0)
            {
                FolderViewModel fvm = results.First();
                fvm.IsExpanded = true;
                
                var retVal = Expand(fvm.Folders, path);
                if (retVal != null)
                {
                    return retVal;
                }
                else
                {
                    return fvm;
                }
            }

            return null;
        }


    }
}
