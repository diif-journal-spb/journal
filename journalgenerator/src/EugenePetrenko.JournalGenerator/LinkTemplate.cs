using System;

namespace EugenePetrenko.JournalGenerator
{
  public class LinkTemplate : IEquatable<LinkTemplate>
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

    public Link ToLink(Language language, LinkTemplate pageLink)
    {
      return ToLink(language, pageLink, language);
    }

    public Link ToLink(Language language, LinkTemplate pageLink, Language currentLang)
    {
      if (pageLink == null)
      {
        return new Link(myLinkManager, language, myPageName);
      }
      else
      {
        return new Link(myLinkManager, language, myPageName, pageLink.ToLink(currentLang, null));
      }
    }

    public bool Equals(LinkTemplate linkTemplate)
    {
      if (linkTemplate == null) return false;
      return Equals(myPageName, linkTemplate.myPageName);
    }

    public override bool Equals(object obj)
    {
      if (this == obj) return true;
      return Equals(obj as LinkTemplate);
    }

    public override int GetHashCode()
    {
      return myPageName.GetHashCode();
    }
  }
}