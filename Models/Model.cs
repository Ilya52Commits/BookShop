namespace BookShopCore.Model;

public class User
{
  public int Id { get; set; }
  public string Login { get; set; }
  public string Password { get; set; }
  public string Type { get; set; }
}

public class Product
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Author { get; set; }

  public string Genre { get; set; }

  public string Price { get; set; }
}