using BookShopCore.Views;
using System.Windows;

namespace BookShopCore
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      MainFrame.NavigationService.Navigate(new RegistrationView());
    }
  }
}