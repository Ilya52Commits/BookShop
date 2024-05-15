using BookShopCore.Model;
using BookShopCore.ViewModels.AdminViewModels;
using System.Windows.Controls;

namespace BookShopCore.Views.AdminViews;

/// <summary>
/// Логика взаимодействия для AdminUserView.xaml
/// </summary>
public partial class AdminUserView : Page
{
  public AdminUserView(User user)
  {
    InitializeComponent();

    /* Привязка объекта ViewModel к контексту данных */
    DataContext = new AdminUserViewModel(user);
  }
}

