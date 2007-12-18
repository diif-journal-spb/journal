namespace EugenePetrenko.RFFI
{
  public class RFFIArticleTitle
  {
    private readonly string myTitle;
    private readonly string myLang;
    
    public RFFIArticleTitle(string title, string lang)
    {
      myTitle = title;
      myLang = lang;
    }

    [XmlText]
    public string Title
    {
      get { return myTitle; }
    }

    [XmlAttribute("lang")]
    [XmlAttribute("fieldlang")]
    public string Lang
    {
      get { return myLang; }
    }
  }
}