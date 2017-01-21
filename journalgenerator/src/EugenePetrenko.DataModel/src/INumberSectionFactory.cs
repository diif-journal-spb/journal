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
      var art = new List<IArticle>();
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
      var art = new List<IArticle>();
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

  public class PhdNumberFactory : INumberSectionFactory
  {
    public string ElementName
    {
      get { return "phd-article"; }
    }

    public INumberSection Create(IArticle[] articles)
    {
      return new PhdsSection(
        true,
        articles,
        Pair.Create(JournalLanguage.RU, "Диссертации"),
        Pair.Create(JournalLanguage.EN, "Phds"));
    }

    public IArticle[] Filter(IEnumerable<INumberSection> section)
    {
      var art = new List<IArticle>();
      foreach (INumberSection numberSection in section)
      {
        if (numberSection is PhdsSection)
        {
          art.AddRange(numberSection.Articles);
        }
      }
      return art.ToArray();
    }

    private class PhdsSection : NumberSectionImpl
    {
      public PhdsSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section)
        : base(showTitle, articles, section)
      {
      }
    }
  }

  public class MonographNumberFactory : INumberSectionFactory
  {
    public string ElementName
    {
      get { return "monograph-article"; }
    }

    public INumberSection Create(IArticle[] articles)
    {
      return new MonograpSection(
        true, 
        articles, 
        Pair.Create(JournalLanguage.RU, "Монографии"), 
        Pair.Create(JournalLanguage.EN, "Monographs"));
    }

    public IArticle[] Filter(IEnumerable<INumberSection> section)
    {
      var art = new List<IArticle>();
      foreach (INumberSection numberSection in section)
      {
        if (numberSection is MonograpSection)
        {
          art.AddRange(numberSection.Articles);
        }
      }
      return art.ToArray();
    }

    private class MonograpSection : NumberSectionImpl
    {
      public MonograpSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section)
        : base(showTitle, articles, section)
      {
      }
    }
  }

  public class ConfNumberFactory : INumberSectionFactory
  {
    public string ElementName
    {
      get { return "conf-article"; }
    }

    public INumberSection Create(IArticle[] articles)
    {
      return new ConfSection(
        true, 
        articles, 
        Pair.Create(JournalLanguage.RU, "Материалы Конференций"), 
        Pair.Create(JournalLanguage.EN, "Conference Papers"));
    }

    public IArticle[] Filter(IEnumerable<INumberSection> section)
    {
      var art = new List<IArticle>();
      foreach (INumberSection numberSection in section)
      {
        if (numberSection is ConfSection)
        {
          art.AddRange(numberSection.Articles);
        }
      }
      return art.ToArray();
    }

    private class ConfSection : NumberSectionImpl
    {
      public ConfSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section)
        : base(showTitle, articles, section)
      {
      }
    }
  }


}