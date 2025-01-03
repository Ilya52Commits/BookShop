using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using BookShop.MVVM.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientProductView = BookShop.MVVM.Views.ClientViews.ClientProductView;

namespace BookShop.MVVM.ViewModels.ClientViewModels;

internal partial class BasketViewModel(User user) : ObservableObject
{
  /* Переменная для взаимодействия с базой данных */
  private readonly Context _context = new();

  /* Данные пользователя, вошедшему на страницу */

  /* Коллекция книг для обращения к базе данных */
  [ObservableProperty]
  private ObservableCollection<Book> _selectBook = new(user.SelectedBooks); 
  
  /* Конструктор по умолчанию */

  /// <summary>
  /// Метод перехода на страницу продуктов
  /// </summary>
  [RelayCommand]
  private void NavigateToClientProductPage()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new ClientProductView(_context.Users.First(user1 => user1.Id == user.Id)));
  }

  /// <summary>
  /// Метод удаления товара из корзины
  /// </summary>
  /// <param name="book"></param>
  [RelayCommand]
  private void DeleteProduct(Book book)
  {
    // Поиск пользователя, из списка которого будет удалён товар
    var deletingUser =  _context.Users.First(user1 => user1.Id == user.Id);

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
    var buyingUser = _context.Users.First(user1 => user1.Id == user.Id);

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
