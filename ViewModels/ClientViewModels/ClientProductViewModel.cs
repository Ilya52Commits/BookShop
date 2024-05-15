using BookShopCore.Model;
using BookShopCore.Views;
using BookShopCore.Views.ClientViews;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookShopCore.ViewModels.ClientViewModels;

/* Главный класс ViewModel страницы продуктов для клиента */
internal sealed class ClientProductViewModel : BaseViewModel
{
  /* Переменная модели для взаимодействия с данными */
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
  public RelayCommand<Book> AddToBascketCommand { get; set; }       // Команда купить
  public RelayCommand NavigateToBasketCommand { get; set; }         // Переход на страницу корзины
  public RelayCommand NavigateToAuthorizationCommand { get; set; }  // Переход на страницу авторизации

  /* Конструктор по умолчанию */
  public ClientProductViewModel(User user)
  {
    _dbContext = new DbContext();                             // Инициализация консектса базы данных

    _user = user;                                             // Инициализация пользователя 

    Books = new ObservableCollection<Book>(_dbContext.Books); // Инициализация коллекции книг

    AddToBascketCommand = new RelayCommand<Book>(AddToBascketCommandExecute);                 // Инициализация команды покупки
    NavigateToBasketCommand = new RelayCommand(NavigateToBasketCommandExecute);               // Инициализация команды перехода в корзину
    NavigateToAuthorizationCommand = new RelayCommand(NavigateToAuthorizationCommandExecute); // Инициалзиация команды перехода на авторизацию
  }

  /// <summary>
  /// Метод перехода на страницу авторизации
  /// </summary>
  private void NavigateToAuthorizationCommandExecute()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }

  #region Методы класса
  /// <summary>
  /// Метод перехода на страницу корзины
  /// </summary>
  private void NavigateToBasketCommandExecute()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new BasketView(_dbContext.Users.First(user1 => user1.Id == _user.Id)));
  }

  /// <summary>
  /// Метод покупки товара
  /// </summary>
  private void AddToBascketCommandExecute(Book book)
  {
    // Поиск списка выбранных товаров у пользователя
    var userSelectedBooks = _dbContext.Users.First(user => user.Id == _user.Id).SelectedBooks;

    // Поиск элемента товара
    var isAlreadyHave = userSelectedBooks.Contains(book);
    // Если он уже добавлен в корзину, то происходит выход из метода
    if (isAlreadyHave) return;

    // Поиск пользователя
    userSelectedBooks.Add(book);

    // Сохранение изменений
    _dbContext.SaveChanges();
  }
  #endregion
}
