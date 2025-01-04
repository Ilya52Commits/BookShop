using System.ComponentModel;

namespace BookShop.MVVM.ViewModels;

/// <summary>
/// Реализация класса для подключения к INotifyPropertyChanged
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
  /* Событие INotifyPropertyChanged */
  public event PropertyChangedEventHandler? PropertyChanged; 

  /// <summary>
  /// Метод для сообщения о событии изменения
  /// </summary>
  /// <param name="propertyName"></param>
  protected virtual void OnPropertyChanged(string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
