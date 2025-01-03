using System.Collections.ObjectModel;
using System.Windows;
using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using BookShop.MVVM.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminProductView = BookShop.MVVM.Views.AdminViews.AdminProductView;
using AuthorizationView = BookShop.MVVM.Views.AuthorizationView;

namespace BookShop.MVVM.ViewModels.AdminViewModels;

internal partial class AdminUserViewModel : ObservableObject
{
  #region Параметры класса
  /* Переменная модели для взаимодействия с данными */
  private readonly Context _context;

  /* Переменная админа, вошедшего на страницу */
  private readonly User _user;

  /* Коллекция пользователей для обращения к базе данных */
  [ObservableProperty]
  private readonly ObservableCollection<User> _users;

  #endregion

  /* Конструктор класса */
  public AdminUserViewModel(User user)
  {
    _context = new Context();

    _user = user;

    _users = new ObservableCollection<User>(_context.Users);
  }

  #region Команды
  /// <summary>
  /// Метод перехода на страницу продуктов админа
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  [RelayCommand]
  private void NavigateToAdminProductPage()
  {
    _context.SaveChanges();

    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Переходит к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AdminProductView(_context.Users.First(user => user.Id == _user.Id)));
  }

  /// <summary>
  /// Метод для перехода на страницу авторизации
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  [RelayCommand]
  private void NavigateToAuthorization()
  {
    _context.SaveChanges();

    // Получение экземпляра главного окна
    var mainWindow = Application.Current.MainWindow as MainView;

    // Навигирует к View авторизации
    mainWindow?.MainFrame.NavigationService.Navigate(new AuthorizationView());
  }

  /// <summary>
  /// Метод удаления пользователя
  /// </summary>
  [RelayCommand]
  private void UserDeletion(User deletionUser)
  {
    // Если пользователь удаляет себя
    if (deletionUser.Id == _user.Id) 
    {
      // Появляется сообщение о невозможности удаления
      MessageBox.Show("Невозможно удалить себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      return;
    }

    // Если пользователь удаляет админа
    if (deletionUser.Role == "Admin")
    {
      // Появляется сообщение о невозможности удаления
      MessageBox.Show("Невозможно удалить админа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
      return; 
    }

    // Удаление пользователя из базы данных
    _context.Users.Remove(deletionUser);

    // Сохранение изменений
    _context.SaveChanges();

    // Удаление пользователя из списка
    _users.Remove(deletionUser);
  }
  #endregion
}
