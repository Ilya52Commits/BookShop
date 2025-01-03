using System.Windows.Controls;
using BookShop.EntityFramework.Models;
using AdminProductViewModel = BookShop.MVVM.ViewModels.AdminViewModels.AdminProductViewModel;

namespace BookShop.MVVM.Views.AdminViews;

/// <summary>
/// Логика взаимодействия для AdminProductView.xaml
/// </summary>
public partial class AdminProductView : Page
{
  public AdminProductView(User user)
  {
    InitializeComponent();

    /* Привязка объекта ViewModel к контексту данных */
    DataContext = new AdminProductViewModel(user);
  }
}