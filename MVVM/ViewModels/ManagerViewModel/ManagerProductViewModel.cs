using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework.Models;
using BookShop.MVVM.Views;
using BookShop.Repository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AuthorizationView = BookShop.MVVM.Views.AuthorizationView;

namespace BookShop.MVVM.ViewModels.ManagerViewModel;

/// <summary>
/// Реализация ViewModel менеджера
/// </summary>
public partial class ManagerProductViewModel : ObservableObject
{
  #region Свойства

  /* Переменная для взаимодействия с базой данных */
  private readonly MsSqlBookRepository _contextBook;

  /* Данные пользователя, вошедшему на страницу */
  private readonly User _user;

  /* Коллекция книг для обращения к базе данных */
  [ObservableProperty] private readonly ObservableCollection<Book> _books;

  #endregion

  /* Конструктор по умолчанию */
  public ManagerProductViewModel(User user)
  {
    _contextBook = new MsSqlBookRepository();

    _user = user;

    _books = new ObservableCollection<Book>(_contextBook.GetObjectList());
  }

  #region Реализация команд

  /// <summary>
  /// Метод перехода на страницу пользователей админа
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  [RelayCommand]
  private void NavigateToAuthorization()
  {
    // Сохранение изменений
    _contextBook.Save();

    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Переходит к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
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