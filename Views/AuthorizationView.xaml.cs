using BookShopCore.ViewModels;
using System.Windows.Controls;

namespace BookShopCore.Views;

/// <summary>
/// Логика взаимодействия для AuthorizationView.xaml
/// </summary>
public partial class AuthorizationView : Page
{
  public AuthorizationView()
  {
    InitializeComponent();

    DataContext = new AuthorizationViewModel();
  }
}