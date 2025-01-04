using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using BookShop.MVVM.Views;
using BookShop.Repository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AuthorizationView = BookShop.MVVM.Views.AuthorizationView;
using BasketView = BookShop.MVVM.Views.ClientViews.BasketView;

namespace BookShop.MVVM.ViewModels.ClientViewModels;

internal sealed partial class ClientProductViewModel : ObservableObject
{
  #region Параметры класса
  /* Контекст для взаимодействия с моделью книг */
  private readonly MsSqlBookRepository _contextBook;
  
  /* Контекст для взаимодействия с моделью книги */
  private readonly MsSqlUserRepository _contextUser;

  /* Данные пользователя, вошедшему на страницу */
  private readonly User _user;

  /* Коллекция книг для обращения к базе данных */
  [ObservableProperty]
  private readonly ObservableCollection<Book>? _books; 
  #endregion

  /* Конструктор по умолчанию */
  public ClientProductViewModel(User user)
  {
    _contextBook = new MsSqlBookRepository();

    _user = user;

    Books = new ObservableCollection<Book>(_contextBook.GetObjectList()); 
  }

  #region Методы класса
  /// <summary>
  /// Метод перехода на страницу авторизации
  /// </summary>
  [RelayCommand]
  private static void NavigateToAuthorization()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Переходит к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }

  /// <summary>
  /// Метод перехода на страницу корзины
  /// </summary>
  [RelayCommand]
  private void NavigateToBasket()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Переходит к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new BasketView(_contextUser.GetObject(_user.Id)));
  }

  /// <summary>
  /// Метод покупки товара
  /// </summary>
  [RelayCommand]
  private void AddToBasket(Book book)
  {
    // Поиск списка выбранных товаров у пользователя
    var userSelectedBooks = _contextUser.GetObject(_user.Id).SelectedBooks;

    // Поиск элемента товара
    var isAlreadyHave = userSelectedBooks.Contains(book);
    // Если он уже добавлен в корзину, то происходит выход из метода
    if (isAlreadyHave) return;

    // Поиск пользователя
    userSelectedBooks.Add(book);

    // Сохранение изменений
    _contextUser.Save();
  }
  #endregion
}
