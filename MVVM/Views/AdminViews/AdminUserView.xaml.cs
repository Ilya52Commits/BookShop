using System.Windows.Controls;
using BookShop.EntityFramework.Models;
using AdminUserViewModel = BookShop.MVVM.ViewModels.AdminViewModels.AdminUserViewModel;

namespace BookShop.MVVM.Views.AdminViews;

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

