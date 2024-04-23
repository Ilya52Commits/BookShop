using System.ComponentModel;

namespace BookShopCore.ViewModels;

/* Реализация класса для подключения к INotifyPropertyChanged */ 
public class BaseViewModel : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler? PropertyChanged;  // Создание объекта собития класса PropertyChanged

  // Метод для сообщения о событии изменения
  protected virtual void OnPropertyChanged(string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
