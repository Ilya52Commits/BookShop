using System.Windows.Controls;
using BookShop.EntityFramework.Models;
using ClientProductViewModel = BookShop.MVVM.ViewModels.ClientViewModels.ClientProductViewModel;

namespace BookShop.MVVM.Views.ClientViews;

/// <summary>
/// Логика взаимодействия для ClientProductView.xaml
/// </summary>
public partial class ClientProductView : Page
{
  public ClientProductView(User user)
  {
    InitializeComponent();

    /* Привязка объекта ViewModel к контексту данных */
    DataContext = new ClientProductViewModel(user);
  }
}