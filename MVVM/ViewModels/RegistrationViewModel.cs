using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows;
using BookShop.EntityFramework.Models;
using BookShop.MVVM.Views;
using BookShop.Repository;
using CommunityToolkit.Mvvm.Input;
using AuthorizationView = BookShop.MVVM.Views.AuthorizationView;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace BookShop.MVVM.ViewModels;

/// <summary>
/// Реализация ViewModel регистрации
/// </summary>
internal sealed partial class
  RegistrationViewModel : BaseViewModel,
  INotifyDataErrorInfo // Наследуем от ViewModel BaseViewModel для INotifyPropertyChanged
{
  #region Параметры валидации

  private readonly Dictionary<string, List<string>> _errors = new();

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

  #region Поля класса

  /* Переменная модели для взаимодействия с данными */
  private readonly MsSqlUserRepository _contextUser = new();

  /* Описания параметров для Login */
  private string _login = string.Empty;

  [Required(ErrorMessage = "Недопустимый логин")]
  public string Login
  {
    get => _login;

    set
    {
      _login = value;
      Validate(nameof(Login), value);
      OnPropertyChanged(nameof(Login));
    }
  }

  /* Описание параметров для Email */
  private string _email = string.Empty;

  [Required(ErrorMessage = "Недопустимая почта")]
  public string Email
  {
    get => _email;
    set
    {
      _email = value;
      Validate(nameof(Email), value);
      OnPropertyChanged(nameof(Email));
    }
  }

  /* Описание параметров для Password */
  private string _password = string.Empty; // Инициализация _password

  [Required(ErrorMessage = "Недопустимый пароль")]
  public string Password
  {
    get => _password;
    set
    {
      _password = value;
      Validate(nameof(Password), value);
      OnPropertyChanged(nameof(Password));
    }
  }

  /* Описание параметров для ConfigPassword */
  private string _confPassword = string.Empty; // Инициализация _confPassword

  [Required(ErrorMessage = "Пароль не совпадает")]
  public string ConfigPassword
  {
    get => _confPassword;
    set
    {
      _confPassword = value;
      Validate(nameof(ConfigPassword), value);
      OnPropertyChanged(nameof(ConfigPassword));
    }
  }

  #endregion

  #region Методы класса

  /// <summary>
  /// Метод для навигации между View
  /// </summary>
  [RelayCommand]
  private void NavigateToAuthorization()
  {
    // Сохраняет изменения в базе данных
    _contextUser.Save();

    // Получение экземпляра главного окна 
    var mainWindow = Application.Current.MainWindow as MainView;

    // Переходит к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }

  /// <summary>
  /// Метод для регистрации и передачи параметров в бд
  /// </summary>
  [RelayCommand]
  private void RegistrationClient()
  {
    // Проверка условий валидации вводимых данных
    if (!IsLoginValidation(_login) || !IsEmailValidation(_email) || !IsPasswordValidation(_password, _confPassword))
    {
      // Вывод сообщения об ошибке
      MessageBox.Show("Некорректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      return;
    }

    // Создание объекта модели User
    var newUser = new User
    {
      Login = _login,
      Email = _email,
      Password = _password,
      Role = "Client"
    };

    // Создание нового пользователя в базе
    _contextUser.Create(newUser);

    // Вывод сообщения об успешной регистрации
    MessageBox.Show("Регистрация прошла успешно!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

    // Вызов метода для перехода на View авторизации
    NavigateToAuthorization();
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
    MessageBox.Show("Логин должен быть с большой буквы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
      MessageBox.Show("Вы указали некорректную почту!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    var emailSplitArray = email.Split('@');
    if (emailSplitArray.Length >= 2 && emailSplitArray[1].Split('.').Length >= 2
                                    && emailSplitArray[1].Split('.')[0].Length >= 2 &&
                                    emailSplitArray[1].Split('.')[1].Length >= 2) return true;
    MessageBox.Show("Вы указали некорректную почту!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
    return false;
  }

  /// <summary>
  /// Выполняет проверку валидности пароля
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