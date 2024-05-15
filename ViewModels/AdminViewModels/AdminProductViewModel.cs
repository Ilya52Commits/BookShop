using BookShopCore.Model;
using BookShopCore.Views;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookShopCore.ViewModels.AdminViewModels;

public class AdminProductViewModel : BaseViewModel
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
  public RelayCommand NavigateToAuthorizationCommand { get; set; } // Команда перехода на страницу авторизации

  /* Конструктор по умолчанию */
  public AdminProductViewModel(User user)
  {
    _dbContext = new DbContext();                               // Инициализация контекста базы данных

    _user = user;                                               // Инициализация пользователя

    _books = new ObservableCollection<Book>(_dbContext.Books);  // Инициализация коллекции выбронных книг

    NavigateToAuthorizationCommand = new RelayCommand(NavigateToAuthorizationCommandExecute); // Инициализация команды перехода на страницу авторизации
  }

  /// <summary>
  /// Метод перехода на страницу авторизации
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