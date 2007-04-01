using System.Text;

namespace EugenePetrenko.JournalGenerator
{
  public class LinkTemplate
  {
    protected readonly LinkManager myLinkManager;
    protected readonly string myPageName;

    public LinkTemplate(LinkManager linkManager, string pageName)
    {
      myLinkManager = linkManager;
      myPageName = pageName;
    }

    public override string ToString()
    {
      return string.Format("LinkTemplate: page: {0}", myPageName);
    }

    public Link ToLink(Language language)
    {
      return new Link(myLinkManager, language, myPageName);
    }
  }
}