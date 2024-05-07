using BookShopCore.Model;
using BookShopCore.ViewModels.ClientViewModels;
using System.Windows.Controls;

namespace BookShopCore.Views.ClientViews;

/// <summary>
/// Логика взаимодействия для BasketView.xaml
/// </summary>
public partial class BasketView : Page
{
  public BasketView(User user)
  {
    InitializeComponent();

    DataContext = new BasketViewModel(user);
  }
}