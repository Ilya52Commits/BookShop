using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repository;

public sealed class MsSqlBookRepository : IRepository<Book>
{
  private readonly Context _db = new();

  private bool _disposed;

  public IEnumerable<Book> GetObjectList() => _db.Books;

  public Book GetObject(int id) => _db.Books.Find(id);

  public void Create(Book book) => _db.Books.Add(book);

  public void Update(Book book) => _db.Entry(book).State = EntityState.Modified;

  public void Delete(int id)
  {
    var book = _db.Books.Find(id);
    if (book != null)
      _db.Books.Remove(book);
  }

  public void Save() => _db.SaveChanges();

  private void Dispose(bool disposing)
  {
    if (!_disposed)
      if (disposing)
        _db.Dispose();

    _disposed = true;
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}