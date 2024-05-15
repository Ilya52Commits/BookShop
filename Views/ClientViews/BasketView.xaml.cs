using BookShopCore.Model;
using BookShopCore.ViewModels.ClientViewModels;

namespace BookShopCore.Views.ClientViews;

/// <summary>
/// Логика взаимодействия для BasketView.xaml
/// </summary>
public partial class BasketView
{
  private User user;

  public BasketView(User user)
  {
    InitializeComponent();

    this.user = user;

    DataContext = new BasketViewModel(user);
  }
}