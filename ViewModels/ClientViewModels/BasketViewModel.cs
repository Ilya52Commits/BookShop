using BookShopCore.Model;
using BookShopCore.Views.ClientViews;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookShopCore.ViewModels.ClientViewModels;

/* Главный класс ViewModel страницы корзины для клиента */
internal class BasketViewModel : BaseViewModel
{
  /* Переменная для взаимодействия с базой данных */
  private readonly DbContext _dbContext;

  /* Данные пользователя, вошедшему на страницу */
  private readonly User _user;

  /* Коллекция книг для обращения к базе данных */
  private ObservableCollection<Book> _selectBook; 
  public ObservableCollection<Book> SelectBook
  {
    get => _selectBook;     // Вывод значения
    protected init          // Изменение значения
    {
      _selectBook = value;  // Присваивание нового значения
      OnPropertyChanged();  // Вызов события изменения 
    }
  }

  /* Описание команд страницы */
  public RelayCommand NavigateToClientProductPageCommand { get; set; }  // Команда перехода на страницу продуктов
  public RelayCommand<Book> DeleteProductCommand { get; set; }          // Команда удаления продукта из корзины
  public RelayCommand BuyProductCommand { get; set; }                   // Команда купить

  /* Конструктор по умолчанию */
  public BasketViewModel(User user)
  {
    _dbContext = new DbContext();                                     // Инициализация контекста базы данных

    _user = user;                                                     // Инициализация пользователя

    _selectBook = new ObservableCollection<Book>(user.SelectedBooks); // Инициализация коллекции выбронных книг

    NavigateToClientProductPageCommand = new RelayCommand(NavigateToClientProductPageCommandExecute); // Инициализация команды перехода на страницу продуктов
    DeleteProductCommand = new RelayCommand<Book>(DeleteProductCommandExecute);                       // Инициализация команды удаления продукта из корзины
    BuyProductCommand = new RelayCommand(BuyProductCommandExecute);                                   // Инициализация команды купить
  }

  /// <summary>
  /// Метод перехода на страницу продуктов
  /// </summary>
  private void NavigateToClientProductPageCommandExecute()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new ClientProductView(_dbContext.Users.First(user1 => user1.Id == _user.Id)));
  }

  /// <summary>
  /// Мето удаления товара из корзины
  /// </summary>
  /// <param name="book"></param>
  private void DeleteProductCommandExecute(Book book)
  {
    // Удаление товара из списка выбранных товаров
    _dbContext.Users.First(user1 => user1.Id == _user.Id).SelectedBooks.Remove(book);

    // Удаления из колекции для отображения
    _selectBook.Remove(book);

    // Обновление базы данных
    _dbContext.SaveChanges();
  }

  /// <summary>
  /// Метод покупки
  /// </summary>
  private void BuyProductCommandExecute()
  {
    // Поиск списка выбранных элементов пользователя
    var userSelectedBooks = _dbContext.Users.First(user1 => user1.Id == _user.Id).SelectedBooks;

    // Цикл для удаления элементов
    foreach (var book in _user.SelectedBooks)
    {
      // Удаление элементов
      _dbContext.Users.First(user1 => user1.Id == _user.Id).SelectedBooks.Remove(book);
    }

    // Очистка списка для отображения
    _selectBook.Clear();
    
    // Сохранение изменений
    _dbContext.SaveChanges();

    // Вывод сообщения об успешной покупке
    MessageBox.Show("Вы успешно купили товар!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
  }
}
