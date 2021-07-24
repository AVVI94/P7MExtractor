using System;
using System.Collections.Generic;
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
using P7MExtractor.ViewModels;

namespace P7MExtractor.Views
{
    /// <summary>
    /// Interaction logic for SelectFileView.xaml
    /// </summary>
    public partial class SelectFileView : UserControl
    {
        SelectFileViewModel _vm;
        public SelectFileView(SelectFileViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            _vm.DragAndDropEvent(sender, e);
        }
    }
}
