using BookShopCore.Model;
using System.Windows;

namespace BookShopCore;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
  /* Переменная для взаимодействия с базой данных */
  private readonly DbContext _dbContext = new();

  #region Методы для добавления начальных данных
  /// <summary>
  /// Метод для создания админа
  /// </summary>
  private void AddTheMainAdmin()
  {
    // Поиск главного админа в базе данных
    var admin = _dbContext.Users.FirstOrDefault(a => a.Login == "Admin" && a.Password == "Admin" && a.Email == "Admin" && a.Role == "Admin" && a.IsValidateAdmin == true);
    // Если он присутствует, то происходит выход из метода
    if (admin != null) return;

    // Создание объекта модели User для главного админа
    var mainAdmin = new User
    {
      Login = "Admin",        // Присвоение логина
      Email = "Admin",        // Присвоение почты
      Password = "Admin",     // Присвоение пароля
      Role = "Admin",         // Присвоение типа
      IsValidateAdmin = true  // Присвоение валидации
    };

    // Добавление и сохранение базы данных
    _dbContext.Add(mainAdmin);
    _dbContext.SaveChanges();
  }

  /// <summary>
  /// Метод добавления первых книг в бд
  /// </summary>
  private void AddingInitialBookElements()
  {
    var isBookTableNotEmpty = _dbContext.Books.Any(); // Проверка книг на пустоту

    // Проверка коллекции на пустоту
    if (isBookTableNotEmpty) return;
    // Создание объекта первой книги
    var firstBook = new Book
    {
      Name = "Гарри Поттер и филосовский камень", // Присваивание название книги
      Author = "Джуан Роулинг",                   // Присваивание автора
      Genre = "Фэнтези",                          // Присваивание жанра
      Price = 1200                                // Присваивание цены
    };

    // Создание обхекта второй книги
    var secondBook = new Book
    {
      Name = "Воспитание чувств",
      Author = "Гюстав Флобер",
      Genre = "Романтизм",
      Price = 210
    };

    /* Добавление книг в базу данных */
    _dbContext.Books.Add(firstBook);
    _dbContext.Books.Add(secondBook);

    // Сохранение изменений
    _dbContext.SaveChanges();
  }
  #endregion

  /// <summary>
  /// Виртуальный метод для логики перед запуском программы
  /// </summary>
  /// <param name="e"></param>
  protected override void OnStartup(StartupEventArgs e)
  {
    /* Вызов методов для начального добавления данных */
    AddTheMainAdmin();            // Добавление главного админа
    AddingInitialBookElements();  // Добавление первых книг в магазин

    /* Вызов логики виртульного класса */
    base.OnStartup(e);
  }
}