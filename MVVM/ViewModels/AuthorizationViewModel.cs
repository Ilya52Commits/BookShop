using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using BookShop.EntityFramework;
using BookShop.MVVM.Views;
using CommunityToolkit.Mvvm.Input;
using AdminUserView = BookShop.MVVM.Views.AdminViews.AdminUserView;
using ClientProductView = BookShop.MVVM.Views.ClientViews.ClientProductView;
using ManagerProductView = BookShop.MVVM.Views.ManagerViews.ManagerProductView;
using RegistrationView = BookShop.MVVM.Views.RegistrationView;

namespace BookShop.MVVM.ViewModels;

/* Главный клас ViewModel авторизации */
internal sealed partial class AuthorizationViewModel : BaseViewModel, INotifyDataErrorInfo
{
  #region Параметры валидации
  /* Словарь для хранения ошибок валидации.
   Ключ - имя свойства; 
   Значение - список сообщений об ошибках */
  private readonly Dictionary<string, List<string>> _errors = new();

  /* Это свойство, которое возвращает true, если есть ошибки валидации, и false, если ошибок нет */
  public bool HasErrors => _errors.Count > 0;

  /* Событие, которое вызывается при изменении ошибок валидации */
  public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

  /// <summary>
  /// Метод, который возвращает список ошибок валидации для указанного свойства
  /// </summary>
  /// <param name="propertyName"></param>
  /// <returns></returns>
  public IEnumerable GetErrors(string? propertyName)
  {
    // Если propertyName не равно null и словарь _errors содержит ключ propertyName
    if (propertyName != null && _errors.TryGetValue(propertyName, out var value))
      // то метод возвращает список ошибок, связанных с указанным свойством
      return value;

    // Метод возвращает пустой список
    return Enumerable.Empty<string>();
  }

  /// <summary>
  /// Метод, который выполняет валидацию указанного свойства
  /// </summary>
  /// <param name="propertyName"></param>
  /// <param name="propertyValue"></param>
  private void Validate(string propertyName, object propertyValue)
  {
    var results = new List<ValidationResult>(); // Список результата валидации

    // Выполняется валидация значения propertyValue для указанного свойства propertyName
    Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);

    // Если список результатов не пустой
    if (results.Count != 0)
    {
      // Ошибки валидации добавляются в словарь _errors с ключом, соответствующим имени свойства propertyName
      _errors.Add(propertyName, results.Select(r => r.ErrorMessage).ToList()!);
      // Вызывается событие ErrorsChanged, чтобы уведомить об изменении ошибок валидации для данного свойства
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
    else
    {
      // Удаление запись об ошибке валидации для указанного свойства из словаря _errors
      _errors.Remove(propertyName);
      // Вызывается событие ErrorsChanged, чтобы уведомить об изменении ошибок валидации для данного свойства
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
  }
  #endregion

  /* Переменная модели для взаимодействия с данными */
  private readonly Context _context = new(); // Создание объекта модели бд

  /* Описание параметров для Login */
  private string _login = string.Empty; // Инициализация переменной логина

  // Присвоение сообщения
  [Required(ErrorMessage = "Недопустимый логин")]
  public string Login                 // Вывод значения
  {
    get => _login;                    // Изменение значения
    set
    {
      _login = value;                 // Присваивание нового значения
      Validate(nameof(Login), value); // Применение метода валидации
      OnPropertyChanged();            // Вызов события изминения
    }
  }

  /* Описание параметров для Email */
  private string _email = string.Empty; // Инициализация переменной почты

  [Required(ErrorMessage = "Недопустимая почта")]
  public string Email
  {
    get => _email;
    set
    {
      _email = value;
      Validate(nameof(Login), value);
      OnPropertyChanged();
    }
  }

  /* Описание параметров для Password */
  private string _password = string.Empty; // Инициализация переменной пароля

  public string Password
  {
    get => _password;
    set
    {
      _password = value;
      OnPropertyChanged();
    }
  }
  /* Конструктор по умолчанию */

  #region Методы класса
  /// <summary>
  /// Метод перехода на страницу регистрации
  /// </summary>
  private static void NavigateToRegistrationCommandExecute()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Навигирует к View регистрации
    mainWindow?.MainFrame.NavigationService.Navigate(new RegistrationView());
  }

  /// <summary>
  /// Метод обработки вводимых данных
  /// </summary>
  [RelayCommand]
  private void AuthorizationClient()
  {
    // Если вводятся данные главного админа 
    if (_login == "Admin" && _password == "Admin")
    {
      // Выполнение запроса к бд для проверки на главного админа
      var mainAdmin = _context.Users.FirstOrDefault(u => u.Login == _login && u.Password == _password
                                                  && u.Role == "Admin");

      // Если пользователь является админом
      if (mainAdmin != null)
      {
        // Выводится сообщение приветствие админа
        MessageBox.Show("Здравсвтуйте, создатель!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

        // Получение экземпляра главного окна 
        var mainWindow = Application.Current.MainWindow as MainView;

        // Навигирует к View авторизации
        mainWindow?.MainFrame.NavigationService.Navigate(
          new AdminUserView(_context.Users.First(user => user.Login == "Admin" && user.Password == "Admin" && user.Role == "Admin")));

        return; 
      }
      // Иначе выводится сообщение, что пользователь не админ
      else
        MessageBox.Show("Вы не создатель!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    var admin = _context.Users.FirstOrDefault(admin => admin.Login == _login && admin.Password == _password && admin.Role == "Admin");
    if (admin !=  null)
    {
      // Выводится сообщение об успешном входе в систему
      MessageBox.Show("Добрый день, админ!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

      // Получение экземпляра главного окна 
      var mainWindow = Application.Current.MainWindow as MainView;

      // Переход на страницу продуктов менеджера
      mainWindow?.MainFrame.NavigationService.Navigate(
        new AdminUserView(_context.Users.First(admin => admin.Login == _login && admin.Password == _password && admin.Role == "Admin")));

      return; 
    }

    var manager = _context.Users.FirstOrDefault(manager => manager.Login == _login && manager.Password == _password && manager.Role == "Manager");
    if (manager != null)
    {
      // Выводится сообщение об успешном входе в систему
      MessageBox.Show("Добрый день, менеджер!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

      // Получение экземпляра главного окна 
      var mainWindow = Application.Current.MainWindow as MainView;

      // Переход на страницу продуктов менеджера
      mainWindow?.MainFrame.NavigationService.Navigate(
        new ManagerProductView(_context.Users.First(manager => manager.Login == _login && manager.Password == _password && manager.Role == "Manager")));

      return; 
    }

    // Выполнение запроса к базе данных PostgresSQL для проверки почты и пароля
    var user = _context.Users.FirstOrDefault(u => u.Login == _login && u.Password == _password);
    // Если польователь нашёлся
    if (user != null)
    {
      // Выводится сообщение об успешном входе в систему
      MessageBox.Show("Вы успешно вошли в систему!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

      // Получение экземпляра главного окна 
      var mainWindow = Application.Current.MainWindow as MainView;

      // Навигирует к View авторизации
      mainWindow?.MainFrame.NavigationService.Navigate(
        new ClientProductView(_context.Users.First(user => user.Login == _login && user.Password == _password)));

      return;
    }
    // Выводится сообщение об ошибке
    else
      MessageBox.Show("Почта или пароль не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
  }
  #endregion
}
