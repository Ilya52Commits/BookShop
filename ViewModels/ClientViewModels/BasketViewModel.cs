using BookShopCore.Model;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace BookShopCore.ViewModels.ClientViewModels;
class BasketViewModel : BaseViewModel
{
  private readonly DbContext _dbContext;

  private readonly User _user;

  private ObservableCollection<Book> _selectBook; 
  public ObservableCollection<Book> SelectBook
  {
    get => _selectBook;
    set
    {
      _selectBook = value;
      OnPropertyChanged();
    }
  }

  public RelayCommand NavigateToClientProductPageCommand { get; set; }
  public RelayCommand DeleteProductCommand { get; set; } 
  public RelayCommand BuyProductCommand { get; set; }

  public BasketViewModel(User user)
  {
    _dbContext = new DbContext();

    _user = user;

    _selectBook = new ObservableCollection<Book>(_user.SelectedBooks);


    NavigateToClientProductPageCommand = new RelayCommand(NavigateToClientProductPageCommandExecute);
    DeleteProductCommand = new RelayCommand(DeleteProductCommandExecute);
    BuyProductCommand = new RelayCommand(BuyProductCommandExecute);
  }
  private void NavigateToClientProductPageCommandExecute()
  {
    throw new NotImplementedException();
  }
  private void DeleteProductCommandExecute()
  {
    throw new NotImplementedException();
  }

  private void BuyProductCommandExecute()
  {
    throw new NotImplementedException();
  }
}
