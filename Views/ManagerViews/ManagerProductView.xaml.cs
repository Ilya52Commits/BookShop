using BookShopCore.ViewModels.ManagerViewModel;
using System.Windows.Controls;
using BookShopCore.Model;

namespace BookShopCore.Views.ManagerViews;

/// <summary>
/// Логика взаимодействия для ManagerProductView.xaml
/// </summary>
public partial class ManagerProductView : Page
{

  public ManagerProductView(User user)
  {
    InitializeComponent();

    DataContext = new ManagerProductViewModel(user);
  }
}

