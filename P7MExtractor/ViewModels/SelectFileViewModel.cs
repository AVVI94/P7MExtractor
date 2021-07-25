using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;
using P7MExtractor.Models;
using System.IO;
using System.Windows.Input;
using P7MExtractor.Others;
using P7MExtractor.Resources;

namespace P7MExtractor.ViewModels
{
    public class SelectFileViewModel : BaseViewModel
    {
        SignedFileModel _signedFile;        
        Next _next;

        public SelectFileViewModel(SignedFileModel signedFile, Next nextView)
        {
            _signedFile = signedFile;
            _next = nextView;

            SelectFileCommand = new GenericCommand(SelectFile);
        }

        public void SelectFile()
        {
            OpenFileDialog dialog = new();
            dialog.Filter = $"{ResourceManager.FindResource("SelectFileViewModel_SelectFile_FilterText")}|*.p7m";

            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.FileName))
                return;

            _signedFile.FilePath = dialog.FileName;
            _next?.Invoke();
        }

        public void DragAndDropEvent(object sender, DragEventArgs e)
        {
            var file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            if (file.ToLower().EndsWith(".p7m"))
            {
                _signedFile.FilePath = file;
                _next?.Invoke();                
            }            
        }

        public ICommand SelectFileCommand { get; set; }
    }
}
