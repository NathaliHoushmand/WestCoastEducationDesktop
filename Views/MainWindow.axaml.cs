using Avalonia.Controls;
using Avalonia.Interactivity;
using WestCoastEducationDesktop.ViewModels;

namespace WestCoastEducationDesktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.Courses.LoadCommand.Execute(null);
            vm.Students.LoadCommand.Execute(null);
            vm.Teachers.LoadCommand.Execute(null);
        }
    }
}
