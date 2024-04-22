using BookShopCore.ViewModels;
using System.Windows.Controls;

namespace BookShopCore.Views;
/// <summary>
/// Логика взаимодействия для RegistrationView.xaml
/// </summary>
public partial class RegistrationView : Page
{
  public RegistrationView()
  {
    InitializeComponent();

    DataContext = new RegistrationViewModel();
  }
}