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
  public RelayCommand<User> UserDeletionCommand { get; set; }         // Команда удаления пользователя
  public RelayCommand<User> UserEditCommand { get; set; }             // Команда редактирования пользователя
  #endregion

  /* Конструктор класса */
  public AdminUserViewModel(User user)
  {
    _dbContext = new DbContext();                              // Инициализация контекста базы данных

    _user = user;                                              // Инициализация админа

    _users = new ObservableCollection<User>(_dbContext.Users); // Инициализация коллекции пользователей

    NavigateToAuthorizationCommand = new RelayCommand(NavigateToAuthorizationCommandExecute);       // Инициализация команды перехода на страницу авторизации
    NavigateToAdminProductPageCommand = new RelayCommand(NavigateToAdminProductPageCommandExecute); // Инициализация команды перехода на страницу продуктов админа
    UserDeletionCommand = new RelayCommand<User>(UserDeletionCommadExecute);                        // Инициализация команды удаления пользователя
  }

  #region Методы класса
  /// <summary>
  /// Метод перехода на страницу продуктов админа
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  private void NavigateToAdminProductPageCommandExecute()
  {
    _dbContext.SaveChanges();

    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AdminProductView(_dbContext.Users.First(user => user.Id == _user.Id)));
  }

  /// <summary>
  /// Метод для перехода на страницу авторизации
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  private void NavigateToAuthorizationCommandExecute()
  {
    _dbContext.SaveChanges();

    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }

  /// <summary>
  /// Метод удаления пользователя
  /// </summary>
  private void UserDeletionCommadExecute(User deletionUser)
  {
    // Если пользователь удаляет себя
    if (deletionUser.Id == _user.Id) 
    {
      // Появляется сообщение о невозможности удаления
      MessageBox.Show("Невозможно удалить себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      return;
    }

    // Если пользователь удаляет админа
    if (deletionUser.Role == "Admin")
    {
      // Появляется сообщение о невозможности удаления
      MessageBox.Show("Невозможно удалить админа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      return; 
    }

    // Удаление пользователя из базы данных
    _dbContext.Users.Remove(deletionUser);

    // Сохранение изменений
    _dbContext.SaveChanges();

    // Удаление пользователя из списка
    _users.Remove(deletionUser);
  }
  #endregion
}
