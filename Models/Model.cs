﻿namespace BookShopCore.Model;

#region Модели таблиц
/* Модель таблицы пользователя */
public class User
{
  public int Id { get; set; }           // Идентификатор пользователя
  public string Login { get; set; }     // Логин пользователя
  public string Email { get; set; }     // Почта пользователя
  public string Password { get; set; }  // Пароль ползователя
  public string Type { get; set; }      // Тип пользователя
}

/* Модель таблицы книг */ 
public class Book
{
  public int Id { get; set; }           // Идентификатор книги
  public string Name { get; set; }      // Название книги
  public string Author { get; set; }    // Автор продукта
  public string Genre { get; set; }     // Жанр книги
  public string Price { get; set; }     // Цена книги
}
#endregion