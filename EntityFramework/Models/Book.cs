namespace BookShop.EntityFramework.Models;

/// <summary>
/// Описание таблицы книги
/// </summary>
public class Book
{
  public int Id { get; init; }
  public string? Name { get; set; }
  public string? Author { get; set; }
  public string? Genre { get; set; }
  public int Price { get; set; }
}