using BookShopCore.Model;
using BookShopCore.Views.AdminViews;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookShopCore.ViewModels.AdminViewModels;

/* Главный класс ViewModel страницы продуктов для админа */
internal class AdminProductViewModel : BaseViewModel
{
  /* Переменная для взаимодействия с базой данных */
  private readonly DbContext _dbContext;

  /* Данные пользователя, вошедшему на страницу */
  private readonly User _user;

  /* Коллекция книг для обращения к базе данных */
  private readonly ObservableCollection<Book> _books;
  public ObservableCollection<Book> Books
  {
    get => _books;          // Вывод значения
    protected init          // Изменение значения 
    {
      _books = value;       // Присваивание нового значения
      OnPropertyChanged();  // Вызов события изменения
    }
  }

  /* Описание команд страницы */
  public RelayCommand NavigateToAdminUserPageCommand { get; set; } // Команда перехода на страницу авторизации

  /* Конструктор по умолчанию */
  public AdminProductViewModel(User user)
  {
    _dbContext = new DbContext();                               // Инициализация контекста базы данных

    _user = user;                                               // Инициализация пользователя

    _books = new ObservableCollection<Book>(_dbContext.Books);  // Инициализация коллекции выбронных книг

    NavigateToAdminUserPageCommand = new RelayCommand(NavigateToAdminUserPageCommandExecute); // Инициализация команды перехода на страницу авторизации
  }

  /// <summary>
  /// Метод перехода на страницу пользователей админа
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  private void NavigateToAdminUserPageCommandExecute()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AdminUserView(_dbContext.Users.First(user1 => user1.Id == _user.Id)));
  }
}