using BookShopCore.Model;

namespace BookShopCore.ViewModels.ManagerViewModel;

class ManagerProductViewModel
{
  private readonly User _user; 

  public ManagerProductViewModel(User user)
  {
    _user = user;
  }
}

