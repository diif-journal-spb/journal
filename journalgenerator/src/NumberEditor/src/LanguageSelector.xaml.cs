using System.Windows.Controls;

namespace EugenePetrenko.NumberEditor
{
  /// <summary>
  /// Interaction logic for LanguageSelector.xaml
  /// </summary>
  public partial class LanguageSelector
  {
    public LanguageSelector()
    {
      InitializeComponent();
    }

    public string LanguageValue
    {
      get
      {
        return myLanguageEn.IsChecked == true ? "EN" : myLanguageRu.IsChecked == true ? "RU" : "";
      }
      set
      {
        switch (value)
        {
            case "EN":
            myLanguageEn.IsChecked = true;
            return;
            case "RU":
            myLanguageRu.IsChecked = true;
            return;
          default:
            myLanguageAny.IsChecked = true;
            return;
        }
      }
    }
  }
}
