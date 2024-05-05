using BookShopCore.Model;
using BookShopCore.Views;
using GalaSoft.MvvmLight.Command;
using System.Text.RegularExpressions;
using System.Windows;

namespace BookShopCore.ViewModels;

/* Главный клас ViewModel регистрации */
internal sealed partial class RegistrationViewModel : BaseViewModel // Наследуем от ViewModel BaseViewModel для INotifyPropertyChanged
{
  #region Поля класса
  /* Переменная модели для взаимодействия с данными */
  private readonly DbContext _dbContext;  

  /* Описания параметров для Login */ 
  private string _login; 
  public string Login
  {
    get => _login;  // Вывод значения
   
    set             // Изменение значения
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

  /* Описание параметров для ConfigPassword */
  private string _confPassword;
  public string ConfigPassword
  {
    get => _confPassword;
    set
    {
      _confPassword = value;
      OnPropertyChanged();
    }
  }

  /* Описание команд ViewModel */
  public RelayCommand RegistrationClientCommand { get; }      // Добавление в базу данных
  public RelayCommand NavigateToAuthorizationCommand { get; }  // Навигация между View
  #endregion

  /* Конструктор класса */
  public RegistrationViewModel()
  {
    _dbContext = new DbContext(); // Создание объекта модели бд

    _login = string.Empty;        // Инициализация _login
    _email = string.Empty;        // Инициализация _email
    _password = string.Empty;     // Инициализация _password
    _confPassword = string.Empty; // Инициализация _confPassword


    AddTheMainAdmin();


    RegistrationClientCommand = new RelayCommand(RegistrationClientCommandExecute);   // Создание объекта команды и присваивание метода регистрации
    NavigateToAuthorizationCommand = new RelayCommand(NavigateToAuthorizationExecute);  // Создание объекта команды и присваивание метода навигации
  }

  private void AddTheMainAdmin()
  {
    var admin = _dbContext.Users.FirstOrDefault(a => a.Login == "Admin" && a.Password == "Admin" && a.Email == "Admin" && a.Type == "Admin" && a.IsValidateAdmin == true);

    if (admin != null) return;
    var mainAdmin = new User
    {
      Login = "Admin",
      Email = "Admin",
      Password = "Admin",
      Type = "Admin",
      IsValidateAdmin = true
    };

    _dbContext.Add(mainAdmin);
    _dbContext.SaveChanges();
  }

  #region Методы класса
  /// <summary>
  /// Метод для навигации между View
  /// </summary>
  private void NavigateToAuthorizationExecute()
  {
    // Сохраняет изменения в базе данных
    _dbContext.SaveChanges();

    // Получение экземпляра главного окна 
    var mainWindow = Application.Current.MainWindow as MainWindow;  

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }

  /// <summary>
  /// Метод для регистрации и передачи параметров в бд
  /// </summary>
  private void RegistrationClientCommandExecute()
  {
    // Проверка условий валидации вводимых данных
    if (!IsLoginValidation(_login) || !IsEmailValidation(_email) || !IsPasswordValidation(_password, _confPassword))
    {
      MessageBox.Show("Некоректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      return; 
    }

    // Создание объекта модели User
    var newUser = new User
    {
      Login = _login,       // Присваивание логина
      Email = _email,       // Присваивание почты
      Password = _password, // Присваивание пароля
      Type = "Client",      // Выставление типа пользователя "по умолчанию"
    };
    
    // Создание нового пользователя в базе
    _dbContext.Users.Add(newUser);

    MessageBox.Show("Регистрация прошла успешно!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

    // Вызов метода для перехода на View авторизации
    NavigateToAuthorizationExecute();
  }

  #region Методя проверки валидности вводимых данных
  /// <summary>
  /// Выполняет проверку валидности логина
  /// </summary>
  /// <param name="login"></param>
  /// <returns></returns>
  private static bool IsLoginValidation(string login)
  {
    if (login.Length < 2)
    {
      MessageBox.Show("Недопустимый логин!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    var containsNumbers = MyRegex().IsMatch(login);
    if (containsNumbers)
    {
      MessageBox.Show("Недопустимые символы логина!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    if (!char.IsLower(login[0])) return true;
    MessageBox.Show("Логин должен быть с большёй буквы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
    return false;

  }

  /// <summary>
  /// Выполняет проверку валидности почты
  /// </summary>
  /// <param name="email"></param>
  /// <returns></returns>
  private static bool IsEmailValidation(string email)
  {
    if (!email.Contains('@'))
    {
      MessageBox.Show("Вы указали некоректную почту!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    var emailSplitArray = email.Split('@');
    if (emailSplitArray.Length >= 2 && emailSplitArray[1].Split('.').Length >= 2
                                    && emailSplitArray[1].Split('.')[0].Length >= 2 &&
                                    emailSplitArray[1].Split('.')[1].Length >= 2) return true;
    MessageBox.Show("Вы указали некоректную почту!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
    return false;

  }

  /// <summary>
  /// Выполняет проверку валиндости пароля
  /// </summary>
  /// <param name="password"></param>
  /// <param name="confPassword"></param>
  /// <returns></returns>
  private static bool IsPasswordValidation(string password, string confPassword)
  {
    if (password.Length < 8)
    {
      MessageBox.Show("Недопустимая длина пароля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }
    
    var isMatch = MyRegex1().IsMatch(password);

    if (isMatch)
    {
      MessageBox.Show("Недопустимый пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    if (password == confPassword) return true;
    MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
    return false;

  }

    [GeneratedRegex(@"[\d\W]")]
    private static partial Regex MyRegex();
    [GeneratedRegex("[А-Яа-яЁё]")]
    private static partial Regex MyRegex1();
    #endregion
    #endregion
}