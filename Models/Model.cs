using System.Text.RegularExpressions;

namespace BookShopCore.Model;

public interface IDataErrorInfo /**********************************************************************************/
{
  string Error { get; }
  string this[string columnName] { get; }
}

#region Модели таблиц
/* Модель таблицы пользователя */
public class User : IDataErrorInfo
{

  public int Id { get; set; }                         // Идентификатор пользователя
  public string Login { get; set; }                   // Логин пользователя
  public string Email { get; set; }                   // Почта пользователя
  public string Password { get; set; }                // Пароль ползователя
  public string Type { get; set; }                    // Тип пользователя
  public bool IsValidateAdmin { get; set; } = false;  // Подтверждение на администратора

  /*********************************************************************************************************/
  public string this[string columnName] 
  {
    get
    {
      string error = String.Empty;
      switch (columnName)
      {
        case "Login":
          if (Login.Length < 2)
          {
            error = "Недопустимый логин!";
          }

          bool containsNumbers = Regex.IsMatch(Login, "[\\d\\W]");
          if (containsNumbers)
          {
            error = "Недопустимые символы логина!";
          }

          if (char.IsLower(Login[0]))
          {
            error = "Логин должен быть с большёй буквы!";
          }
          break;
      }

      return error; 
    }  
  }
  public string Error
  {
    get
    {
      throw new NotImplementedException();
    }
  }
}
/**********************************************************************************************************/

/* Модель таблицы книг */ 
public class Book
{
  public int Id { get; set; }           // Идентификатор книги
  public string Name { get; set; }      // Название книги
  public string Author { get; set; }    // Автор продукта
  public string Genre { get; set; }     // Жанр книги
  public int Price { get; set; }     // Цена книги
}
#endregion