namespace EugenePetrenko.DataModel
{
  public interface INumberSection : ILocalizedEntity<ILocalizedNumberSection>
  {
    bool ShowTitle { get;}
    IArticle[] Articles {get; }    
  }

  public interface ILocalizedNumberSection
  {
    string Name { get; }
  }

  public class LocalizedNumberSection : ILocalizedNumberSection
  {
    private readonly string myName;

    public LocalizedNumberSection(string name)
    {
      myName = name;
    }

    public string Name
    {
      get { return myName; }
    }    
  }


  public class NumberSectionImpl : Localized<ILocalizedNumberSection>, INumberSection
  {        
    private readonly bool myShowTitle;    
    private readonly IArticle[] myArticles;

    public NumberSectionImpl(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section)
    {
      myShowTitle = showTitle;
      myArticles = articles;

      foreach (Pair<JournalLanguage, string> pair in section)
      {
        AddEntity(pair.A, new LocalizedNumberSection(pair.B));
      }
    }

    public bool ShowTitle
    {
      get { return myShowTitle; }
    }

    public IArticle[] Articles
    {
      get { return myArticles; }
    }
  }  
}