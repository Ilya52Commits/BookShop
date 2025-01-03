namespace BookShop.MVVM.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainView
  {
    public MainView()
    {
      InitializeComponent();
      
      MainFrame.NavigationService.Navigate(new RegistrationView());
    }
  }
}