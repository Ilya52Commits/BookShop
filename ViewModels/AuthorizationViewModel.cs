using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookShopCore.Views;
using BookShopCore.Views.AdminViews;
using BookShopCore.Views.ClientViews;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace BookShopCore.ViewModels;

internal sealed class AuthorizationViewModel : BaseViewModel, INotifyDataErrorInfo
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
    Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName}, results);

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
  private readonly DbContext _dbContext;

  /* Описание параметров для Login */
  private string _login;
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
  private string _email;
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
  public RelayCommand NavigateToRegistrationCommand { get; }  // Переход на страницу регистрации

  /* Конструктор по умолчанию */
  public AuthorizationViewModel()
  {
    _dbContext = new DbContext(); // Создание объекта модели бд

    _login = string.Empty;        // Инициализация переменной логина
    _email = string.Empty;        // Инициализация переменной почты
    _password = string.Empty;     // Инициализация переменной пароля

    AuthorizationClientCommand = new RelayCommand(AuthorizationClientCommandExecute);       // Инициализация команды обработки вводимых данных
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
  /// Метод обработки вводимых данных
  /// </summary>
  private void AuthorizationClientCommandExecute()
  {
    // Если вводятся данные главного админа 
    if (_login == "Admin" && _password == "Admin")
    {
      // Выполнение запроса к бд для проверки на главного админа
      var admin = _dbContext.Users.FirstOrDefault(u => u.Login == _login && u.Password == _password
                                                  && u.Role == "Admin" && u.IsValidateAdmin == true);

      // Если пользователь является админом
      if (admin != null)
      {
        // Выводится сообщение приветствие админа
        MessageBox.Show("Здравсвтуйте, создатель!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

        /* Переход на страницу продуктов для админа */
        // Получение экземпляра главного окна 
        var mainWindow = Application.Current.MainWindow as MainWindow;

        // Навигирует к View авторизации
        mainWindow?.MainFrame.NavigationService.Navigate(new AdminProductView(_dbContext.Users.First(user => user.Login == "Admin" && user.Password == "Admin" 
                                                                                                     && user.Role == "Admin" && user.IsValidateAdmin == true)));
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

        /* Переход на страницу продуктов для клиента */
        // Получение экземпляра главного окна 
        var mainWindow = Application.Current.MainWindow as MainWindow;

        // Навигирует к View авторизации
        mainWindow?.MainFrame.NavigationService.Navigate(
          new ClientProductView(_dbContext.Users.First(user1 => user1.Login == _login && user1.Password == _password)));
      }
      // Выводится сообщение об ошибке
      else
        MessageBox.Show("Почта или пароль не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }
  #endregion
}
