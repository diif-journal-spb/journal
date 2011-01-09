using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace EugenePetrenko.NumberEditor
{
  /// <summary>
  /// Interaction logic for NumberEditor.xaml
  /// </summary>
  public partial class NumberEditor : UserControl
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
                 Lang = myLanguageSelector.Text,
                 Pdf = myPdf.Text,
                 Title = myTitle.Text
               }.Serialize();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      MessageBox.Show(GenerateXml());
    }
  }

  [Serializable]
  public class ArticleInfoXml
  {
    [XmlAttribute("lang")]
    public string Lang { get; set; }

    [XmlAttribute("FirstPage")]
    public string FirstPage { get; set; }

    [XmlAttribute("LastPage")]
    public string LastPage { get; set; }

    [XmlElement("pdf")]
    public string Pdf { get; set; }

    [XmlElement("Abstract")]
    public string Abstract { get; set; }

    [XmlElement("Title")]
    public string Title { get; set; }
  }

}
