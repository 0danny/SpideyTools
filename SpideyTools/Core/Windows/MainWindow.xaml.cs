using MahApps.Metro.Controls;
using SpideyTools.Core;
using SpideyTools.Core.Helper;
using SpideyTools.Core.ViewModels;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpideyTools
{
    public partial class MainWindow : MetroWindow
    {
        public WindowVM viewModel
        {
            get;
        }
   
        public MainWindow(WindowVM _viewModel)
        {
            viewModel = _viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}