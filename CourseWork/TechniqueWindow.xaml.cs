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
    /// Interaction logic for TechniqueWindow.xaml
    /// </summary>
    public partial class TechniqueWindow : Window
    {
        public TechniqueWindow(TechniqueViewModel viewModel)
        {
            InitializeComponent();

            viewModel.CloseWindowAction = Close;
            DataContext = viewModel;
        }
    }
}
