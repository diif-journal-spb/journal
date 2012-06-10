using System.Windows;

namespace EugenePetrenko.NumberEditor
{
  /// <summary>
  /// Interaction logic for NumberEditor.xaml
  /// </summary>
  public partial class NumberEditor
  {
    public NumberEditor()
    {
      InitializeComponent();
    }

    public string GenerateXml()
    {
      return new ArticleInfoXml
               {
                 Abstract = myAbstract.Text,
                 FirstPage = myPageFrom.Text,
                 LastPage = myPageTo.Text,
                 Lang = myLanguage.LanguageValue,
                 Pdf = myPdf.Text,
                 Title = myTitle.Text
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
