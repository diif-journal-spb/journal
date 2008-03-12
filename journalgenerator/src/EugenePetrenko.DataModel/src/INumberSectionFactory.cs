using System.Collections.Generic;

namespace EugenePetrenko.DataModel
{
  public interface INumberSectionFactory
  {
    string ElementName { get; }
    INumberSection Create(IArticle[] articles);

    IArticle[] Filter(IEnumerable<INumberSection> section);    
  }

  public class PublicationsNumberFactory : INumberSectionFactory
  {
    public string ElementName
    {
      get { return "article"; }
    }

    public INumberSection Create(IArticle[] articles)
    {
      return new PubSection(false, articles, Pair.Create(JournalLanguage.RU, ""), Pair.Create(JournalLanguage.EN, ""));
    }

    public IArticle[] Filter(IEnumerable<INumberSection> section)
    {
      List<IArticle> art = new List<IArticle>();
      foreach (INumberSection numberSection in section)
      {
        if (numberSection is PubSection)
        {
          art.AddRange(numberSection.Articles);
        }
      }
      return art.ToArray();
    }

    private class PubSection : NumberSectionImpl
    {
      public PubSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section) : base(showTitle, articles, section)
      {
      }
    }
  }
  
  public class BooksNumberFactory : INumberSectionFactory
  {
    public string ElementName
    {
      get { return "book-article"; }
    }

    public INumberSection Create(IArticle[] articles)
    {
      return new BooksSection(true, articles, Pair.Create(JournalLanguage.RU, "Новые книги"), Pair.Create(JournalLanguage.EN, "New Books"));
    }

    public IArticle[] Filter(IEnumerable<INumberSection> section)
    {
      List<IArticle> art = new List<IArticle>();
      foreach (INumberSection numberSection in section)
      {
        if (numberSection is BooksSection)
        {
          art.AddRange(numberSection.Articles);
        }
      }
      return art.ToArray();
    }

    private class BooksSection : NumberSectionImpl
    {
      public BooksSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section)
        : base(showTitle, articles, section)
      {
      }
    }
  }
}