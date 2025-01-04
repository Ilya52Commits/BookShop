using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using BookShop.MVVM.Views;
using BookShop.Repository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminUserView = BookShop.MVVM.Views.AdminViews.AdminUserView;

namespace BookShop.MVVM.ViewModels.AdminViewModels;

/// <summary>
/// Реализация ViewModel администратора и продукта
/// </summary>
internal partial class AdminProductViewModel : ObservableObject
{
  #region Свойства

  /* Контекст для взаимодействия с моделью книги */
  private readonly MsSqlBookRepository _contextBook;

  /* Контекст для взаимодействия с моделью пользователя */
  private readonly MsSqlUserRepository _contextUser;
  
  private readonly User _user;

  [ObservableProperty] private readonly ObservableCollection<Book> _books;

  /* Конструктор по умолчанию */
  public AdminProductViewModel(User user)
  {
    _contextBook = new MsSqlBookRepository();
    _contextUser = new MsSqlUserRepository();
    
    _books = new ObservableCollection<Book>(_contextBook.GetObjectList());
    
    _user = user;
  }

  #endregion

  #region Реализация команд

  /// <summary>
  /// Метод перехода на страницу пользователей админа
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  [RelayCommand]
  private void NavigateToAdminUserPage()
  {
    // Сохранение изменений
    _contextBook.Save();

    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Переходит к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(
      new AdminUserView(_contextUser.GetObject(_user.Id)));
  }

  /// <summary>
  /// Метод добавления новой книги
  /// </summary>
  [RelayCommand]
  private void AddNewBook()
  {
    /* Взаимодействие с базой данных */
    _contextBook.Create(new Book()); // Добавление объекта в базу данных
    _contextBook.Save(); // Сохранение изменений

    // Добавление в список
    Books.Add(new Book());
  }

  /// <summary>
  /// Метод удаления товара из корзины
  /// </summary>
  /// <param name="book"></param>
  [RelayCommand]
  private void DeleteProduct(Book book)
  {
    // Удаление книги
    _contextBook.Delete(book.Id);
    // Обновление базы данных
    _contextBook.Save();

    // Удаления из коллекции для отображения
    Books.Remove(book);
  }

  #endregion
}