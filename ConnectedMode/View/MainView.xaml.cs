using System.Windows;
using ConnectedMode.ViewModel;
using MainViewModel = ConnectedMode.ViewModel.MainViewModel;

namespace ConnectedMode.View;

public partial class MainView : Window
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}