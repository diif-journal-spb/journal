namespace EugenePetrenko.JournalGenerator
{
  public sealed class Link : LinkTemplate
  {
    private readonly Language myLanguage;
    private readonly Link myPageLink;
    private readonly string myServerPath;

    public Link(LinkManager linkManager, Language language, string pageName) : this(linkManager, language, pageName, null)
    {      
    }

    public Link(LinkManager linkManager, Language language, string pageName, Link pageLink) : base(linkManager, pageName.Replace('\\', '/'))
    {
      myLanguage = language;
      myPageLink = pageLink;

      myServerPath = myLinkManager.Combine('/', myLinkManager.GenerationBaseUrl, myLanguage, myPageName);
      if (myPageLink != null)
      {
        myServerPath = LinkManager.MakeRelative(myServerPath, pageLink.ToString());
      }
    }

    public Language Language
    {
      get { return myLanguage; }
    }

    public Link ForeignLink(Language l)
    {
      return new Link(myLinkManager, l, myPageName, myPageLink);
    }

    public string DestFile
    {
      get { return myLinkManager.Combine('\\', myLinkManager.GeneratePath, myLanguage, myPageName); }
    }

    public override string ToString()
    {
      return myServerPath;
    }
  }
}