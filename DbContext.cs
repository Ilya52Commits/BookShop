using BookShopCore.Model;
using Microsoft.EntityFrameworkCore;

namespace BookShopCore;

/* Класс взаимодействия с базой данных */
internal class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
  /* Строка подключения к базе данных PostgreSQL */
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=booksDB;Username=postgres;Password=52");

  #region Создание объекта таблиц базы данных
  public DbSet<User> Users { get; set; }  // Объект таблицы пользователей
  public DbSet<Book> Books { get; set; }  // Объект таблицы книг
  #endregion
}
