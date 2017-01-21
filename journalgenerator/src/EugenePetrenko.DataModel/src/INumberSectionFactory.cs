using System.Collections.Generic;

namespace EugenePetrenko.DataModel
{
  public interface INumberSectionFactory
  {
    string ElementName { get; }
    INumberSection Create(IArticle[] articles);
  }

  public class PublicationsNumberFactory : INumberSectionFactory
  {
    public string ElementName => "article";

    public INumberSection Create(IArticle[] articles)
    {
      return new PubSection(false, articles, Pair.Create(JournalLanguage.RU, ""), Pair.Create(JournalLanguage.EN, ""));
    }

    public class PubSection : NumberSectionImpl
    {
      public PubSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section) : base(showTitle, articles, section)
      {
      }
    }
  }
  
  public class BooksNumberFactory : INumberSectionFactory
  {
    public string ElementName => "book-article";

    public INumberSection Create(IArticle[] articles)
    {
      return new BooksSection(true, articles, Pair.Create(JournalLanguage.RU, "Новые книги"), Pair.Create(JournalLanguage.EN, "New Books"));
    }

    public class BooksSection : NumberSectionImpl
    {
      public BooksSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section)
        : base(showTitle, articles, section)
      {
      }
    }
  }

  public class PhdNumberFactory : INumberSectionFactory
  {
    public string ElementName => "phd-article";

    public INumberSection Create(IArticle[] articles)
    {
      return new PhdsSection(
        true,
        articles,
        Pair.Create(JournalLanguage.RU, "Диссертации"),
        Pair.Create(JournalLanguage.EN, "Phds"));
    }

    public class PhdsSection : NumberSectionImpl
    {
      public PhdsSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section)
        : base(showTitle, articles, section)
      {
      }
    }
  }

  public class MonographNumberFactory : INumberSectionFactory
  {
    public string ElementName => "monograph-article";

    public INumberSection Create(IArticle[] articles)
    {
      return new MonograpSection(
        true, 
        articles, 
        Pair.Create(JournalLanguage.RU, "Монографии"), 
        Pair.Create(JournalLanguage.EN, "Monographs"));
    }

    public class MonograpSection : NumberSectionImpl
    {
      public MonograpSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section)
        : base(showTitle, articles, section)
      {
      }
    }
  }

  public class ConfNumberFactory : INumberSectionFactory
  {
    public string ElementName => "conf-article";

    public INumberSection Create(IArticle[] articles)
    {
      return new ConfSection(
        true, 
        articles, 
        Pair.Create(JournalLanguage.RU, "Материалы Конференций"), 
        Pair.Create(JournalLanguage.EN, "Conference Papers"));
    }

    public class ConfSection : NumberSectionImpl
    {
      public ConfSection(bool showTitle, IArticle[] articles, params Pair<JournalLanguage, string>[] section)
        : base(showTitle, articles, section)
      {
      }
    }
  }
}
