using BookShopCore.Views;
using BookShopCore.Views.AdminViews;
using BookShopCore.Views.ClientViews;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace BookShopCore.ViewModels;

internal sealed class AuthorizationViewModel : BaseViewModel
{
  /* Переменная модели для взаимодействия с данными */
  private readonly DbContext _dbContext;

  private static bool _isAdmin; 

  /* Описание параметров для Login */
  private string _login;
  public string Login       // Вывод значения
  {
    get => _login;          // Изменение значения
    set
    {
      _login = value;       // Присваивание нового значения
      OnPropertyChanged();  // Вызов события изминения
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

  /* Описание команд страницы */
  public RelayCommand AuthorizationClientCommand { get; }     // Проверка вводимых данных
  public RelayCommand NavigateToProductPageCommand { get; }   // Переход на страницу продукта
  public RelayCommand NavigateToRegistrationCommand { get; }  // Переход на страницу регистрации

  /* Конструктор по умолчанию */
  public AuthorizationViewModel()
  {
    _dbContext = new DbContext(); // Создание объекта модели бд

    _isAdmin = false; 

    _login = string.Empty;    // Инициализация переменной логина
    _email = string.Empty;    // Инициализация переменной почты
    _password = string.Empty; // Инициализация переменной пароля

    AuthorizationClientCommand = new RelayCommand(AuthorizationClientCommandExecute);       // Инициализация команды обработки вводимых данных
    NavigateToProductPageCommand = new RelayCommand(NavigateToProductPageCommandExecute);   // Инициализация команды перехода на страницу продуктов
    NavigateToRegistrationCommand = new RelayCommand(NavigateToRegistrationCommandExecute); // Инициализация команды перехода на страницу регистрации
  }

  #region Методы
  /// <summary>
  /// Метод перехода на страницу регистрации
  /// </summary>
  private static void NavigateToRegistrationCommandExecute()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View регистрации
    mainWindow?.MainFrame.NavigationService.Navigate(new RegistrationView());
  }

  /// <summary>
  /// Метод перехода на страницу продуктов
  /// </summary>
  private static void NavigateToProductPageCommandExecute()
  {
    if (_isAdmin)
    {
      // Получение экземпляра главного окна 
      var mainWindow = Application.Current.MainWindow as MainWindow;

      // Навигирует к View авторизации
      mainWindow?.MainFrame.NavigationService.Navigate(new AdminProductView());
    }
    else
    {
      // Получение экземпляра главного окна 
      var mainWindow = Application.Current.MainWindow as MainWindow;

      // Навигирует к View авторизации
      mainWindow?.MainFrame.NavigationService.Navigate(new ClientProductView());
    }
  }

  /// <summary>
  /// Метод обработки вводимых данных
  /// </summary>
  private void AuthorizationClientCommandExecute()
  {
    // Если вводятся данные главного админа 
    if (_login == "Admin" && _password == "Admin")
    {
      // Выполнение запроса к бд для проверки на главного админа
      var admin = _dbContext.Users.FirstOrDefault(u => u.Login == _login && u.Password == _password && u.Type == "Admin" && u.IsValidateAdmin == true);

      // Если пользователь является админом
      if (admin != null)
      {
        // Присваивание флага проверки на админа
        _isAdmin = true; 
        // Выводится сообщение приветствие админа
        MessageBox.Show("Здравсвтуйте, создатель!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        // Переход на страницу продуктов для админа
        NavigateToProductPageCommandExecute();
      }
      // Иначе выводится сообщение, что пользователь не админ
      else
        MessageBox.Show("Вы не создатель!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    // Иначе проводится поиск обычного пользователя
    else
    {
      // Выполнение запроса к базе данных PostgresSQL для проверки почты и пароля
      var user = _dbContext.Users.FirstOrDefault(u => u.Login == _login && u.Password == _password);

      // Если польователь нашёлся
      if (user != null)
      {
        // Выводится сообщение об успешном входе в систему
        MessageBox.Show("Вы успешно вошли в систему!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        // Переход на страницу продуктов для клиента
        NavigateToProductPageCommandExecute();
      }
      // Выводится сообщение об ошибке
      else
        MessageBox.Show("Почта или пароль не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }
  #endregion
}
