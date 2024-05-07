using BookShopCore.Model;
using Microsoft.EntityFrameworkCore;

namespace BookShopCore;
internal class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=booksdb;Username=postgres;Password=52");
// 5432 booksDB
  public DbSet<User> Users { get; set; }
  public DbSet<Book> Books { get; set; }
}
