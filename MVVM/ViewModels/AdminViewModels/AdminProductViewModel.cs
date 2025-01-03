using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using BookShop.MVVM.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminUserView = BookShop.MVVM.Views.AdminViews.AdminUserView;

namespace BookShop.MVVM.ViewModels.AdminViewModels;

internal partial class AdminProductViewModel : ObservableObject
{
  private readonly Context _context;
  
  private readonly User _user;
  
  [ObservableProperty]
  private readonly ObservableCollection<Book> _books;
  
  /* Конструктор по умолчанию */
  public AdminProductViewModel(User user)
  {
    _context = new Context();
    _user = user;
    _books = new ObservableCollection<Book>(_context.Books);
  }

  #region Методы класса
  /// <summary>
  /// Метод перехода на страницу пользователей админа
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  [RelayCommand]
  private void NavigateToAdminUserPage()
  {
    // Сохранение изменений
    _context.SaveChanges();

    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AdminUserView(_context.Users.First(user1 => user1.Id == _user.Id)));
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
  /// Мето удаления товара из корзины
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