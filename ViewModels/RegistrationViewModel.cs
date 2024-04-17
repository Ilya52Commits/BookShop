using BookShopCore.Model;
using GalaSoft.MvvmLight.Command;
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
  public string confPassword
  {
    get => _confPassword;
    set
    {
      _confPassword = value;
      OnPropertyChanged();  
    }
  }

  /* *************************************************** */
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
  }

  public void RegistrationClientCommandExecute()
  {
    var newUser = new User
    {
      Login = _login,
      Email = _email,
      Password = _password,
    };                        // продумать вариант с type
    
    _dbContext.Users.Add(newUser);

    MessageBox.Show("Регистрация прошла успешно");
  }
}
