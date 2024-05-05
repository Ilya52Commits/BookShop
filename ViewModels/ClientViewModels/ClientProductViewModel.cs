using BookShopCore.Model;
using BookShopCore.Views.ClientViews;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookShopCore.ViewModels;

internal sealed class ClientProductViewModel : BaseViewModel
{
  private readonly DbContext _dbContext;

  private readonly ObservableCollection<Book> _books; 
  public ObservableCollection<Book> Books
  {
    get => _books;
    protected init
    {
      _books = value;
      OnPropertyChanged();
    }
  }

  public RelayCommand BuyCommand { get; }
  public RelayCommand NavigateToBasketCommand { get; }

  public ClientProductViewModel()
  {
    _dbContext = new DbContext();

    var isBookTableNotEmpty = _dbContext.Books.Any();

    if (!isBookTableNotEmpty)
    {
      var firstBook = new Book
      {
        Name = "Гарри Поттер и филосовский камень",
        Author = "Джуан Роулинг",
        Genre = "Фэнтези", 
        Price = 1200
      };

      var secondBook = new Book
      {
        Name = "Воспитание чувств",
        Author = "Гюстав Флобер",
        Genre = "Романтизм",
        Price = 210
      };

      _dbContext.Books.Add(firstBook);
      _dbContext.Books.Add(secondBook);

      _dbContext.SaveChanges();
    }

    Books = new ObservableCollection<Book>(_dbContext.Books); 

    BuyCommand = new RelayCommand(BuyCommandExecute);
    NavigateToBasketCommand = new RelayCommand(NavigateToBasketCommandExecute);
  }

  private static void NavigateToBasketCommandExecute()
  {
    var mainWindow = Application.Current.MainWindow as MainWindow;

    mainWindow?.MainFrame.NavigationService.Navigate(new BasketView());
  }

  private static void BuyCommandExecute()
  {
    


    NavigateToBasketCommandExecute();
  }
}
