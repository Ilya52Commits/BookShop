using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using BookShop.ViewModels;
using BookShop.Views.ClientViews;
using CommunityToolkit.Mvvm.Input;
using ClientProductView = BookShop.MVVM.Views.ClientViews.ClientProductView;

namespace BookShop.MVVM.ViewModels.ClientViewModels;

/* Главный класс ViewModel страницы корзины для клиента */
internal partial class BasketViewModel : BaseViewModel
{
  /* Переменная для взаимодействия с базой данных */
  private readonly Context _context;

  /* Данные пользователя, вошедшему на страницу */
  private readonly User _user;

  /* Коллекция книг для обращения к базе данных */
  private readonly ObservableCollection<Book> _selectBook; 
  public ObservableCollection<Book> SelectBook
  {
    get => _selectBook;     // Вывод значения
    protected init          // Изменение значения
    {
      _selectBook = value;  // Присваивание нового значения
      OnPropertyChanged();  // Вызов события изменения 
    }
  }
  
  /* Конструктор по умолчанию */
  public BasketViewModel(User user)
  {
    _context = new Context();                                       // Инициализация контекста базы данных

    _user = user;                                                       // Инициализация пользователя

    _selectBook = new ObservableCollection<Book>(user.SelectedBooks);   // Инициализация коллекции выбронных книг
  }

  /// <summary>
  /// Метод перехода на страницу продуктов
  /// </summary>
  [RelayCommand]
  private void NavigateToClientProductPage()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new ClientProductView(_context.Users.First(user1 => user1.Id == _user.Id)));
  }

  /// <summary>
  /// Метод удаления товара из корзины
  /// </summary>
  /// <param name="book"></param>
  [RelayCommand]
  private void DeleteProduct(Book book)
  {
    // Поиск пользователя, из списка которого будет удалён товар
    var deletingUser =  _context.Users.First(user => user.Id == _user.Id);

    // Поиск удаляемой книги
    var retrievableBook =  _context.Books.First(book1 => book1.Id == book.Id);

    // Удаление книги
    deletingUser.SelectedBooks.Remove(retrievableBook);
 
    // Удаления из колекции для отображения
    _selectBook.Remove(book);

    // Обновление базы данных
    _context.SaveChanges();
  }

  /// <summary>
  /// Метод покупки
  /// </summary>
  [RelayCommand]
  private void BuyProduct()
  {
    // Поиск пользователя, который осуществляет покупку
    var buyingUser = _context.Users.First(user => user.Id == _user.Id);

    // Проход по списку товаров
    foreach (var book in _selectBook)
    {
      // Поиск книги для покупки
      var shoppableBook = _context.Books.First(book1 => book1.Id == book.Id);
      // Удаление книги из списка выбранных
      buyingUser.SelectedBooks.Remove(shoppableBook);
    }
    
    // Очистка списка для отображения
    _selectBook.Clear();
    
    // Сохранение изменений
    _context.SaveChanges();

    // Вывод сообщения об успешной покупке
    MessageBox.Show("Вы успешно купили товар!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
  }
}
