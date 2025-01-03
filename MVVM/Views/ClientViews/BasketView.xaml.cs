using BookShop.EntityFramework.Models;
using BasketViewModel = BookShop.MVVM.ViewModels.ClientViewModels.BasketViewModel;

namespace BookShop.MVVM.Views.ClientViews;

/// <summary>
/// Логика взаимодействия для BasketView.xaml
/// </summary>
public partial class BasketView
{
  public BasketView(User user)
  {
    InitializeComponent();

    /* Привязка объекта ViewModel к контексту данных */
    DataContext = new BasketViewModel(user);
  }
}