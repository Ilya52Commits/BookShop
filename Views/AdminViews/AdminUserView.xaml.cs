using BookShopCore.Model;
using BookShopCore.ViewModels.AdminViewModels;
using System.Windows.Controls;

namespace BookShopCore.Views.AdminViews;

/// <summary>
/// Логика взаимодействия для AdminUserView.xaml
/// </summary>
public partial class AdminUserView : Page
{
  private User user;

  public AdminUserView(User user)
  {
    InitializeComponent();

    DataContext = new AdminUserViewModel(user);
  }
}

