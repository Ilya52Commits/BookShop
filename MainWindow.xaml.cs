using BookShop.Views;
using RegistrationView = BookShop.MVVM.Views.RegistrationView;

namespace BookShop
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {
      InitializeComponent();
      
      MainFrame.NavigationService.Navigate(new RegistrationView());
    }
  }
}