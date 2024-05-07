using BookShopCore.Model;
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
    get => _books;          // Ввод значения
    protected init          // Изменение значения 
    {
      _books = value;       // Присваивание нового значения
      OnPropertyChanged();  // Вызов события изменения
    }
  }

  /* Описание команд страницы */
  public RelayCommand<Book> AddToBucketCommand { get; }               // Команда купить
  public RelayCommand<User> NavigateToBasketCommand { get; }  // Переход на страницу корзины

  /* Конструктор по умолчанию */
  public ClientProductViewModel(User user)
  {
    _dbContext = new DbContext();                             // Инициализация консектса базы данных

    _user = user;                                             // Инициализация пользователя 

    // Вызов метода добавления первых товаров в бд
    AddingInitialBookElements();

    Books = new ObservableCollection<Book>(_dbContext.Books); // Инициализация коллекции книг

    AddToBucketCommand = new RelayCommand<Book>(BuyCommandExecute);                           // Инициализация команды покупки
    NavigateToBasketCommand = new RelayCommand<User>(NavigateToBasketCommandExecute); // Инициализация команды перехода в корзину

  }

  /// <summary>
  /// Метод добавления первых книг в бд
  /// </summary>
  private void AddingInitialBookElements()
  {
    var isBookTableNotEmpty = _dbContext.Books.Any(); // Проверка книг на пустоту

    // Проверка коллекции на пустоту
    if (!isBookTableNotEmpty)
    {
      // Создание объекта первой книги
      var firstBook = new Book
      {
        Name = "Гарри Поттер и филосовский камень", // Присваивание название книги
        Author = "Джуан Роулинг",                   // Присваивание автора
        Genre = "Фэнтези",                          // Присваивание жанра
        Price = 1200                                // Присваивание цены
      };

      // Создание обхекта второй книги
      var secondBook = new Book
      {
        Name = "Воспитание чувств",
        Author = "Гюстав Флобер",
        Genre = "Романтизм",
        Price = 210
      };

      /* Добавление книг в базу данных */
      _dbContext.Books.Add(firstBook);
      _dbContext.Books.Add(secondBook);

      // Сохранение изменений
      _dbContext.SaveChanges();
    }
  }

  #region Методы класса
  /// <summary>
  /// Метод перехода на страницу корзины
  /// </summary>
  private void NavigateToBasketCommandExecute(User us)
  {
    var mainWindow = Application.Current.MainWindow as MainWindow;

    mainWindow?.MainFrame.NavigationService.Navigate(new BasketView(us));
  }

  /// <summary>
  /// Метод покупки товара
  /// </summary>
  private void BuyCommandExecute(Book book)
  {
    _dbContext.Users.First(user => user.Id == _user.Id).SelectedBooks.Add(book);

    NavigateToBasketCommandExecute(_user);
  }
  #endregion
}
