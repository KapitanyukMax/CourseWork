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
using System.Windows.Shapes;

namespace CourseWork
{
    /// <summary>
    /// Interaction logic for GoodWindow.xaml
    /// </summary>
    public partial class GoodWindow : Window
    {
        public GoodWindow(GoodViewModel viewModel)
        {
            InitializeComponent();

            viewModel.CloseWindowAction = Close;
            DataContext = viewModel;
        }
    }
}
