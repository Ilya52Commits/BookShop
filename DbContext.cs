using BookShopCore.Model;
using Microsoft.EntityFrameworkCore;

namespace BookShopCore;
internal class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=booksDB;Username=postgres;Password=52");

  public DbSet<User> Users { get; set; }
  public DbSet<Product> Products { get; set; }
}
