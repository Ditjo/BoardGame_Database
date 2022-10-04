using Brætspils_Database.Data;
using Brætspils_Database.ViewModel;
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

namespace Brætspils_Database
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel(
                new EditViewModel(new GameDataDbProvider()),
                new ListViewModel(),
                new FrontpageViewModel());
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        //Calls to load data into the viewModel and awaits until finished
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
