using System.Windows;
using SimpleCalculatorMVVM.ViewModels;

namespace SimpleCalculatorMVVM.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CalculatorViewModel();
        }
    }
}
