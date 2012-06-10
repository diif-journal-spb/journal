using System;

namespace EugenePetrenko.NumberEditor
{
  /// <summary>
  /// Interaction logic for LongTextField.xaml
  /// </summary>
  public partial class LongTextField
  {
    public LongTextField()
    {
      InitializeComponent();
    }

    public string LabelText
    {
      get { return myLabel.Content as string; }
      set { myLabel.Content = value; }
    }

    public string TextValue
    {
      get { return myText.Text; }
      set { myText.Text = value; }
    }

    public bool MultiLine
    {
      get { return !double.IsNaN(myText.Height); }
      set { myText.Height = value ? 60 : Double.NaN; }
    }
  }
}
