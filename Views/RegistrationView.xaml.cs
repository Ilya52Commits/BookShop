using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookShopCore.Views
{
  /// <summary>
  /// Логика взаимодействия для RegistrationView.xaml
  /// </summary>
  public partial class RegistrationView : Page
  {
    public RegistrationView()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var loginText = Login.Text.Trim();
      var emailText = Email.Text.Trim();
      var passwordText = Password.Password.Trim();
      var confPasswordText = ConfPassword.Password.Trim();

      
    }

    private static bool IsNumberAlreadyRegistred(string str)
    {
      

      return false;
    }

    public static bool IsPhoneCorrect(string str)
    {
      if (str.Length != 11 || str.Length != 12)
        return false;

      return true;
    }

    // function of chek login and password 
    private static void CheckLoginPassword()
    {
      
    }

    private static bool CheckEmail(string email)
    {
      if (!email.Contains("@"))
        return false;

      string[] delimiter = email.Split('@');
      if (delimiter.Length != 2)
        return false;

      string login = delimiter[0];
      string domain = delimiter[1];

      if (login.Length == 0 || domain.Length == 0)
        return false;

      char firstSymbol = login[0];
      if (Regex.IsMatch(firstSymbol.ToString(), @"[A-Z][a-z]"))
        return false;

      string[] arrDobain = domain.Split('.');
      if (arrDobain.Length != 2)
        return false;

      return true;
    }

  }
}
