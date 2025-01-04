using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework.Models;
using BookShop.MVVM.Views;
using BookShop.Repository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientProductView = BookShop.MVVM.Views.ClientViews.ClientProductView;

namespace BookShop.MVVM.ViewModels.ClientViewModels;

/// <summary>
/// Реализация ViewModel корзины
/// </summary>
/// <param name="user"></param>
internal partial class BasketViewModel(User user) : ObservableObject
{
  #region Свойства

  /* Контекст для взаимодействия с моделью пользователя */
  private readonly MsSqlUserRepository _contextUser = new();

  /* Контекст для взаимодействия с моделью книги */
  private readonly MsSqlBookRepository _contextBook = new();

  /* Коллекция книг для обращения к базе данных */
  [ObservableProperty] private ObservableCollection<Book> _selectBook = new(user.SelectedBooks);

  #endregion

  #region Реализация команд

  /// <summary>
  /// Метод перехода на страницу продуктов
  /// </summary>
  [RelayCommand]
  private void NavigateToClientProductPage()
  {
    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new ClientProductView(_contextUser.GetObject(user.Id)));
  }

  /// <summary>
  /// Метод удаления товара из корзины
  /// </summary>
  /// <param name="book"></param>
  [RelayCommand]
  private void DeleteProduct(Book book)
  {
    // Поиск пользователя, из списка которого будет удалён товар
    var deletingUser = _contextUser.GetObject(user.Id);

    // Поиск удаляемой книги
    var retrievableBook = _contextBook.GetObject(book.Id);

    // Удаление книги
    deletingUser.SelectedBooks.Remove(retrievableBook);

    // Удаления из колекции для отображения
    SelectBook.Remove(book);

    // Обновление базы данных
    _contextUser.Save();
  }

  /// <summary>
  /// Метод покупки
  /// </summary>
  [RelayCommand]
  private void BuyProduct()
  {
    // Поиск пользователя, который осуществляет покупку
    var buyingUser = _contextUser.GetObject(user.Id);

    // Проход по списку товаров
    foreach (var book in SelectBook)
    {
      // Поиск книги для покупки
      var shoppableBook = _contextBook.GetObject(book.Id);
      // Удаление книги из списка выбранных
      buyingUser.SelectedBooks.Remove(shoppableBook);
    }

    // Очистка списка для отображения
    SelectBook.Clear();

    // Сохранение изменений
    _contextUser.Save();

    // Вывод сообщения об успешной покупке
    MessageBox.Show("Вы успешно купили товар!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
  }

  #endregion
}