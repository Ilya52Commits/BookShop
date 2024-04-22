using BookShopCore.Views;
using BookShopCore.Model;
using GalaSoft.MvvmLight.Command;
using System.Text.RegularExpressions;
using System.Windows;

namespace BookShopCore.ViewModels;
sealed class RegistrationViewModel : BaseViewModel // sealed означает, что его нельзя наследовать дальше
{
  private readonly DbContext _dbContext; // для взаимодействия с данными

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

  public RelayCommand RegistrationClientCommand { get; }
  public RelayCommand NavigateToAutorizationCommand { get; }

  public RegistrationViewModel()
  {
    _dbContext = new DbContext();


    _login = string.Empty;
    _email = string.Empty;
    _password = string.Empty;
    _confPassword = string.Empty; 

    RegistrationClientCommand = new RelayCommand(RegistrationClientCommandExecute);
    NavigateToAutorizationCommand = new RelayCommand(NavigateToAutorizationExecute);
  }

  public void NavigateToAutorizationExecute()
  {
    _dbContext.SaveChanges();

    var mainWindow = Application.Current.MainWindow as MainWindow;

    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }

  public void RegistrationClientCommandExecute()
  {
    if (!IsLoginValidation(_login) || !IsEmailValidation(_email) || !IsPasswordValidation(_password, _confPassword))
    {
      MessageBox.Show("Некоректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      return;
    }

    var newUser = new User
    {
      Login = _login,
      Email = _email,
      Password = _password,
      Type = "Client",
    };
    
    _dbContext.Users.Add(newUser);

    MessageBox.Show("Регистрация прошла успешно!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

    NavigateToAutorizationExecute();
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

    bool containsNumbers = Regex.IsMatch(login, "[\\d\\W]");
    if (containsNumbers)
    {
      MessageBox.Show("Недопустимые символы логина!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    if (char.IsLower(login[0]))
    {
      MessageBox.Show("Логин должен быть с большёй буквы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    return true;
  }

  /// <summary>
  /// Выполняет проверку валидности почты
  /// </summary>
  /// <param name="email"></param>
  /// <returns></returns>
  private static bool IsEmailValidation(string email)
  {
    if (!email.Contains("@"))
    {
      MessageBox.Show("Вы указали некоректную почту!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    string[] emailSplitArray = email.Split('@');
    if (emailSplitArray.Length < 2 || emailSplitArray[1].Split('.').Length < 2 
      || emailSplitArray[1].Split('.')[0].Length < 2 || emailSplitArray[1].Split('.')[1].Length < 2)
    {
      MessageBox.Show("Вы указали некоректную почту!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    return true;
  }


  /// <summary>
  /// Выполняет проверку валиндости пароля
  /// </summary>
  /// <param name="password"></param>
  /// <returns></returns>
  private static bool IsPasswordValidation(string password, string confPassword)
  {
    if (password.Length < 8)
    {
      MessageBox.Show("Недопустимая длина пароля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    bool isMatch = Regex.IsMatch(password, "[А-Яа-яЁё]");

    if (isMatch)
    {
      MessageBox.Show("Недопустимый пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    if (password != confPassword)
    {
      MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }

    return true; 
  }
  #endregion
}