using System.Windows;

namespace EugenePetrenko.NumberEditor
{
  /// <summary>
  /// Interaction logic for AuthorsEditor.xaml
  /// </summary>
  public partial class AuthorsEditor
  {
    public AuthorsEditor()
    {
      InitializeComponent();
    }

    public string GenerateXml()
    {
      return new AuthorXml
      {
        Lang = myLanguage.LanguageValue,
        FirstName = myFirstName.TextValue,
        MiddleName = myMiddleName.TextValue,
        LastName = myLastName.TextValue,
        Address = myAddress.TextValue,
        Email = myEmail.TextValue,
      }.Serialize();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      string messageBoxText = GenerateXml();
      Clipboard.SetText(messageBoxText, TextDataFormat.UnicodeText);

      MessageBox.Show(messageBoxText);
    }

  }
}
