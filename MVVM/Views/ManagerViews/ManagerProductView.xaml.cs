using System.Windows.Controls;
using BookShop.EntityFramework.Models;
using ManagerProductViewModel = BookShop.MVVM.ViewModels.ManagerViewModel.ManagerProductViewModel;

namespace BookShop.MVVM.Views.ManagerViews;

/// <summary>
/// Логика взаимодействия для ManagerProductView.xaml
/// </summary>
public partial class ManagerProductView : Page
{
  public ManagerProductView(User user)
  {
    InitializeComponent();

    /* Привязка объекта ViewModel к контексту данных */
    DataContext = new ManagerProductViewModel(user);
  }
}

