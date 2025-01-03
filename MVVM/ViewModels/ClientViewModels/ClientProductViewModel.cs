using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using BookShop.ViewModels;
using BookShop.Views;
using BookShop.Views.ClientViews;
using CommunityToolkit.Mvvm.Input;
using AuthorizationView = BookShop.MVVM.Views.AuthorizationView;
using BasketView = BookShop.MVVM.Views.ClientViews.BasketView;

namespace BookShop.MVVM.ViewModels.ClientViewModels;

internal sealed partial class ClientProductViewModel : BaseViewModel
{
  #region Параметры класса
  /* Переменная модели для взаимодействия с данными */
  private readonly Context _context;

  /* Данные пользователя, вошедшему на страницу */
  private readonly User _user;

  /* Коллекция книг для обращения к базе данных */
  private readonly ObservableCollection<Book>? _books; 
  public ObservableCollection<Book>? Books
  {
    get => _books;          // Вывод значения
    private init          // Изменение значения 
    {
      _books = value;       // Присваивание нового значения
      OnPropertyChanged();  // Вызов события изменения
    }
  }
  #endregion

  /* Конструктор по умолчанию */
  public ClientProductViewModel(User user)
  {
    _context = new Context();

    _user = user;

    Books = new ObservableCollection<Book>(_context.Books); 
  }

  #region Методы класса
  /// <summary>
  /// Метод перехода на страницу авторизации
  /// </summary>
  [RelayCommand]
  private void NavigateToAuthorization()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }

  /// <summary>
  /// Метод перехода на страницу корзины
  /// </summary>
  [RelayCommand]
  private void NavigateToBasket()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainWindow;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new BasketView(_context.Users.First(user1 => user1.Id == _user.Id)));
  }

  /// <summary>
  /// Метод покупки товара
  /// </summary>
  [RelayCommand]
  private void AddToBasket(Book book)
  {
    // Поиск списка выбранных товаров у пользователя
    var userSelectedBooks = _context.Users.First(user => user.Id == _user.Id).SelectedBooks;

    // Поиск элемента товара
    var isAlreadyHave = userSelectedBooks.Contains(book);
    // Если он уже добавлен в корзину, то происходит выход из метода
    if (isAlreadyHave) return;

    // Поиск пользователя
    userSelectedBooks.Add(book);

    // Сохранение изменений
    _context.SaveChanges();
  }
  #endregion
}
