namespace BookShop.EntityFramework.Models;

/// <summary>
/// Описание таблицы пользователя
/// </summary>
public class User
{
  public int Id { get; set; }
  public string? Login { get; init; }
  public string? Email { get; init; }
  public string? Password { get; init; }
  public string? Role { get; init; }
  public virtual ICollection<Book> SelectedBooks { get; } = []; 
}