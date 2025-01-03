using BookShop.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.EntityFramework;

internal sealed class Context : DbContext
{
  #region Создание объекта таблиц базы данных

  public DbSet<User> Users { get; set; }
  public DbSet<Book> Books { get; set; }

  #endregion

  /// <summary>
  /// Конструктор по умолчанию
  /// </summary>
  public Context() => Database.EnsureCreated();

  /// <summary>
  /// Строка подключения к базе данных
  /// </summary>
  /// <param name="optionsBuilder"></param>
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseLazyLoadingProxies()
      .UseSqlServer(
        @"Server=DESKTOP-34SGMAN\LOCALDB;Database=BookShopDb;Trusted_Connection=True;TrustServerCertificate=True;");

  /// <summary>
  /// Метод для наполнения бд начальными данными
  /// </summary>
  /// <param name="modelBuilder"></param>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    /* Инициализация объекта для таблицы Client */
    var mainAdmin = new User { Id = 1, Login = "Admin", Email = "Admin", Password = "Admin", Role = "Admin" };

    #region Инициализация объектов для таблицы Book

    var firstBook = new Book
      { Id = 1, Name = "Гарри Поттер и филосовский камень", Author = "Джуан Роулинг", Genre = "Фэнтези", Price = 1200 };
    var secondBook = new Book
      { Id = 2, Name = "Воспитание чувств", Author = "Гюстав Флобер", Genre = "Романтизм", Price = 210 };

    #endregion

    #region Добавление данных в бд

    modelBuilder.Entity<User>().HasData(mainAdmin);
    modelBuilder.Entity<Book>().HasData(firstBook, secondBook);

    #endregion
  }
}