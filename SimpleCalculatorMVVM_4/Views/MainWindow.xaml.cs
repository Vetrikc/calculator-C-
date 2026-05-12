using System.Windows;
using SimpleCalculatorMVVM_4.ViewModels;

namespace SimpleCalculatorMVVM_4.Views
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
