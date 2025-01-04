using BookShop.EntityFramework;
using BookShop.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repository;

public class MsSqlUserRepository : IRepository<User>
{
  private readonly Context _db = new();
  
  private bool _disposed;
  
  public IEnumerable<User> GetObjectList() => _db.Users;
 
  public User GetObject(int id) => _db.Users.Find(id);
 
  public void Create(User user) => _db.Users.Add(user);
 
  public void Update(User user) => _db.Entry(user).State = EntityState.Modified;
 
  public void Delete(int id)
  {
    var user = _db.Users.Find(id);
    if( user != null )
      _db.Users.Remove(user);
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