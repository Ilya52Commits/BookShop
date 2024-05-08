using BookShopCore.Model;
using System.Windows.Controls;
using BookShopCore.ViewModels.AdminViewModels;

namespace BookShopCore.Views.AdminViews;

/// <summary>
/// Логика взаимодействия для AdminProductView.xaml
/// </summary>
public partial class AdminProductView : Page
{
  public AdminProductView(User user)
  {
    InitializeComponent();

    //DataContext = new AdminProductViewModel(user);
  }
}