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
      return new PubSection(articles);
    }

    public class PubSection : NumberSectionImpl
    {
      public PubSection(IArticle[] articles)
        : base(false, articles,
          Pair.Create(JournalLanguage.RU, ""),
          Pair.Create(JournalLanguage.EN, ""))
      {
      }
    }
  }
  
  public class BooksNumberFactory : INumberSectionFactory
  {
    public string ElementName => "book-article";

    public INumberSection Create(IArticle[] articles)
    {
      return new BooksSection(articles);
    }

    public class BooksSection : NumberSectionImpl
    {
      public BooksSection(IArticle[] articles)
        : base(true, articles,
          Pair.Create(JournalLanguage.RU, "Новые книги"),
          Pair.Create(JournalLanguage.EN, "New Books"))
      {
      }
    }
  }

  public class PhdNumberFactory : INumberSectionFactory
  {
    public string ElementName => "phd-article";

    public INumberSection Create(IArticle[] articles)
    {
      return new PhdsSection(articles);
    }

    public class PhdsSection : NumberSectionImpl
    {
      public PhdsSection(IArticle[] articles)
        : base(true, articles,
          Pair.Create(JournalLanguage.RU, "Диссертации"),
          Pair.Create(JournalLanguage.EN, "Phds"))
      {
      }
    }
  }

  public class MonographNumberFactory : INumberSectionFactory
  {
    public string ElementName => "monograph-article";

    public INumberSection Create(IArticle[] articles)
    {
      return new MonograpSection(articles);
    }

    public class MonograpSection : NumberSectionImpl
    {
      public MonograpSection(IArticle[] articles)
        : base(true, articles,
          Pair.Create(JournalLanguage.RU, "Монографии"),
          Pair.Create(JournalLanguage.EN, "Monographs"))
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
        articles);
    }

    public class ConfSection : NumberSectionImpl
    {
      public ConfSection(IArticle[] articles)
        : base(true, articles,
          Pair.Create(JournalLanguage.RU, "Материалы Конференций"),
          Pair.Create(JournalLanguage.EN, "Conference Papers"))
      {
      }
    }
  }
}
