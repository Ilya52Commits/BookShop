using BookShopCore.ViewModels.ClientViewModels;
using System.Windows.Controls;
using BookShopCore.Model;

namespace BookShopCore.Views.ClientViews;

/// <summary>
/// Логика взаимодействия для ClientProductView.xaml
/// </summary>
public partial class ClientProductView : Page
{
  public ClientProductView(User user)
  {
    InitializeComponent();

    DataContext = new ClientProductViewModel(user);
  }
}