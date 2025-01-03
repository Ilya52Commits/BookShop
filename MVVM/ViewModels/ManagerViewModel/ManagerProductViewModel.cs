using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using BookShop.MVVM.ViewModels;
using BookShop.Views;
using CommunityToolkit.Mvvm.Input;
using AuthorizationView = BookShop.MVVM.Views.AuthorizationView;

namespace BookShop.ViewModels.ManagerViewModel;

public partial class ManagerProductViewModel : BaseViewModel
{
  /* Переменная для взаимодействия с базой данных */
  private readonly Context _context;

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

  /* Конструктор по умолчанию */
  public ManagerProductViewModel(User user)
  {
    _context = new Context();                               // Инициализация контекста базы данных

    _user = user;                                               // Инициализация пользователя

    _books = new ObservableCollection<Book>(_context.Books);  // Инициализация коллекции выбронных книг
  }

  #region Методы класса
  /// <summary>
  /// Метод перехода на страницу пользователей админа
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  [RelayCommand]
  private void NavigateToAuthorization()
  {
    // Сохранение изменений
    _context.SaveChanges();

    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }

  /// <summary>
  /// Метод добавления новой книги
  /// </summary>
  [RelayCommand]
  private void AddNewBook()
  {
    // Создание пустого обхъекта новой книги
    var newBook = new Book
    {
      Name = string.Empty,    // Создание пустого названия книги
      Author = string.Empty,  // Создание пустого имени автора
      Genre = string.Empty,   // Создание пустого названия жанра
      Price = 0,              // Создание нулевой цены
    };

    /* Взаимодействие с базой данных */
    _context.Books.Add(newBook);      // Добавление объекта в базу данных
    _context.SaveChanges();           // Сохранение изменений

    // Добавление в список
    _books.Add(newBook);
  }

  /// <summary>
  /// Метод удаления товара из корзины
  /// </summary>
  /// <param name="book"></param>
  [RelayCommand]
  private void DeleteProduct(Book book)
  {
    // Удаление книги
    _context.Remove(book);
    // Обновление базы данных
    _context.SaveChanges();

    // Удаления из колекции для отображения
    _books.Remove(book);
  }
  #endregion
}

