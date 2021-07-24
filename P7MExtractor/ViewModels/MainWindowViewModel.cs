using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using P7MExtractor.Models;
using P7MExtractor.Views;

namespace P7MExtractor.ViewModels
{
    public delegate void Next();
    class MainWindowViewModel : BaseViewModel
    {
        public SignedFileModel file { get; set; } = new();
        
        private UserControl currentView;
        public UserControl CurrentView { get => currentView; set { currentView = value; OnPropertyChanged(nameof(CurrentView)); } }
                
        int _currentViewInt;
        public MainWindowViewModel()
        {
            SelectFileViewModel vm = new(file, Next);
            CurrentView = new SelectFileView(vm) { DataContext = vm };
            _currentViewInt = 0;
        }

        public void Next()
        {
            switch (_currentViewInt)
            {
                case 0:
                    CurrentView = new ExtractorView() { DataContext = new ExtractorViewModel(file, Next) };
                    _currentViewInt++;
                    break;
                case 1:
                    SelectFileViewModel vm = new(file, Next);
                    CurrentView = new SelectFileView(vm) { DataContext = vm };
                    _currentViewInt--;
                    break;
                default:
                    break;
            }
        }
    }
}
