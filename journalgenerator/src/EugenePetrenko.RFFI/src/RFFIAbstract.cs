namespace EugenePetrenko.RFFI
{
  public class RFFIAbstract
  {
    private readonly string myText;
    private readonly string myLang;

    public RFFIAbstract(string text, string lang)
    {
      myText = text;
      myLang = lang;
    }

    [XmlText]
    public string Text
    {
      get { return myText; }
    }

    [XmlAttribute("lang")]
    public string Lang
    {
      get { return myLang; }
    }
  }
}