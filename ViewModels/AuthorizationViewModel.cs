using BookShopCore.Views.ClientViews;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace BookShopCore.ViewModels;
sealed class AuthorizationViewModel : BaseViewModel
{
  /* Переменная модели для взаимодействия с данными */
  private readonly DbContext _dbContext;

  /* Описание параметров для Email */
  private string _email;
  public string Email
  {
    get => _email;
    set
    {
      _email = value;
      OnPropertyChanged();
    }
  }

  /* Описание параметров для Password */
  private string _password;
  public string Password
  {
    get => _password;
    set
    {
      _password = value;
      OnPropertyChanged();
    }
  }

  public RelayCommand AuthorizationClientCommand { get; }
  public RelayCommand NavigateToAutorizationCommand { get; }

  public AuthorizationViewModel()
  {
    _dbContext = new DbContext(); // Создание объекта модели бд

    _email = string.Empty;
    _password = string.Empty;

    AuthorizationClientCommand = new RelayCommand(AuthorizationClientCommandExecute);
    NavigateToAutorizationCommand = new RelayCommand(NavigateToAutorizationCommandExecute);
  }

  private void NavigateToAutorizationCommandExecute()
  {

    // Получение экземпляра главного окна 
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new ClientProductView());
  }

  private void AuthorizationClientCommandExecute()
  {
    // Выполнение запроса к базе данных PostgreSQL для проверки почты и пароля
    var user = _dbContext.Users.FirstOrDefault(u => u.Email == _email && u.Password == _password);

    if (user != null)
    {
      MessageBox.Show("Вы успешно вошли в систему!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
      NavigateToAutorizationCommandExecute();
    }
    else
    {
      MessageBox.Show("Почта или пароль не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      return; 
    }
  }
}
