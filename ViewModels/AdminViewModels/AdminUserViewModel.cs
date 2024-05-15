using BookShopCore.Model;
using BookShopCore.Views;
using BookShopCore.Views.AdminViews;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookShopCore.ViewModels.AdminViewModels;

/* Главный класс ViewModel страницы пользователей для админа */
internal class AdminUserViewModel : BaseViewModel
{
  #region Параметры класса
  /* Переменная модели для взаимодействия с данными */
  private readonly DbContext _dbContext;

  /* Переменная админа, вошедшего на страницу */
  private readonly User _user;

  /* Коллекция пользователей для обращения к базе данных */
  private readonly ObservableCollection<User> _users;
  public ObservableCollection<User> Users
  {
    get => _users;          // Вызов значения
    protected init          // Изменение значения
    {
      _users = value;       // Присваивание нового значения
      OnPropertyChanged();  // Вызов события изменения
    }
  }

  /* Описание команд страницы */
  public RelayCommand NavigateToAuthorizationCommand { get; set; }    // Команда перехода на страницу авторизации
  public RelayCommand NavigateToAdminProductPageCommand { get; set; } // Команда перехода на страницу продуктов админа
  #endregion

  /* Конструктор класса */
  public AdminUserViewModel(User user)
  {
    _dbContext = new DbContext();                              // Инициализация контекста базы данных

    _user = user;                                              // Инициализация админа

    _users = new ObservableCollection<User>(_dbContext.Users); // Инициализация коллекции пользователей

    NavigateToAuthorizationCommand = new RelayCommand(NavigateToAuthorizationCommandExecute);       // Инициализация команды перехода на страницу авторизации
    NavigateToAdminProductPageCommand = new RelayCommand(NavigateToAdminProductPageCommandExecute); // Инициализация команды перехода на страницу продуктов админа
  }

  /// <summary>
  /// Метод перехода на страницу продуктов админа
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  private void NavigateToAdminProductPageCommandExecute()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AdminProductView(_dbContext.Users.First(user1 => user1.Id == _user.Id)));
  }

  /// <summary>
  /// Метод для перехода на страницу авторизации
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  private void NavigateToAuthorizationCommandExecute()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }
}
