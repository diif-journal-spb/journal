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
                 Lang = myLanguageEn.IsChecked == true ? "EN" : myLanguageRu.IsChecked == true ? "RU" : "???",
                 Pdf = myPdf.Text,
                 Title = myTitle.Text
               }.Serialize();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      MessageBox.Show(GenerateXml());
    }
  }
}
