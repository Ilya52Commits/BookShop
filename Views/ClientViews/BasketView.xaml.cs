using BookShopCore.Model;
using BookShopCore.ViewModels.ClientViewModels;

namespace BookShopCore.Views.ClientViews;

/// <summary>
/// Логика взаимодействия для BasketView.xaml
/// </summary>
public partial class BasketView
{
  public BasketView(User user)
  {
    InitializeComponent();

    DataContext = new BasketViewModel(user);
  }
}