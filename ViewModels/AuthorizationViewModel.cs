using BookShopCore.Views.ClientViews;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace BookShopCore.ViewModels;

internal sealed class AuthorizationViewModel : BaseViewModel
{
  /* Переменная модели для взаимодействия с данными */
  private readonly DbContext _dbContext;

  /* Описание параметров для Login */
  private string _login;
  public string Login
  {
    get => _login;
    set
    {
      _login = value;
      OnPropertyChanged();
    }
  }

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
  public RelayCommand NavigateToAuthorizationCommand { get; }

  public AuthorizationViewModel()
  {
    _dbContext = new DbContext(); // Создание объекта модели бд

    _login = string.Empty;
    _email = string.Empty;
    _password = string.Empty;

    AuthorizationClientCommand = new RelayCommand(AuthorizationClientCommandExecute);
    NavigateToAuthorizationCommand = new RelayCommand(NavigateToAuthorizationCommandExecute);
  }

  private static void NavigateToAuthorizationCommandExecute()
  {

    // Получение экземпляра главного окна 
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new ClientProductView());
  }

  private void AuthorizationClientCommandExecute()
  {
    // Если вводятся данные главного админа 
    if (_login == "Admin" && _password == "Admin")
    {
      // Выполнение запроса к бд для проверки на главного админа
      var admin = _dbContext.Users.FirstOrDefault(u => u.Login == _login && u.Password == _password && u.Type == "Admin" && u.IsValidateAdmin == true);

      if (admin != null)
      {
        MessageBox.Show("Здравсвтуйте, создатель!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        NavigateToAuthorizationCommandExecute();
      }
      else
      {
        MessageBox.Show("Вы не создатель!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
    // Иначе проводится поиск обычного пользователя
    else
    {
      // Выполнение запроса к базе данных PostgresSQL для проверки почты и пароля
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == _email && u.Password == _password);

      if (user != null)
      {
        MessageBox.Show("Вы успешно вошли в систему!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        NavigateToAuthorizationCommandExecute();
      }
      else
      {
        MessageBox.Show("Почта или пароль не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }
}
