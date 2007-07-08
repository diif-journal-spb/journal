namespace EugenePetrenko.JournalGenerator
{
  public sealed class Link : LinkTemplate
  {
    private readonly Language myLanguage;

    public Link(LinkManager linkManager, Language language, string pageName) : base(linkManager, pageName.Replace('\\', '/'))
    {
      myLanguage = language;
    }

    public Language Language
    {
      get { return myLanguage; }
    }

    public Link ForeignLink(Language l)
    {
      return new Link(myLinkManager, l, myPageName);
    }

    public string DestFile
    {
      get { return myLinkManager.Combine('\\', myLinkManager.GeneratePath, myLanguage, myPageName); }
    }

    public override string ToString()
    {
      return myLinkManager.Combine('/', myLinkManager.GenerationBaseUrl, myLanguage, myPageName);
    }
  }
}