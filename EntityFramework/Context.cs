using BookShop.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.EntityFramework;

internal sealed class Context : DbContext
{
  #region Создание объекта таблиц базы данных

  public DbSet<User> Users { get; set; } // Объект таблицы пользователей
  public DbSet<Book> Books { get; set; } // Объект таблицы книг

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
}