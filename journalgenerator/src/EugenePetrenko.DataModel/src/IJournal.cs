using System.Collections.Generic;
using System.Linq;

namespace EugenePetrenko.DataModel
{
  public interface IJournal : IEntity
  {
    INumber[] Numbers { get; }
    IAuthor[] Authors { get; }

    INewsItem[] News { get; }
    IBook[] Books { get; }

    IEnumerable<ILocalizedNewsItem> NewsForLanguage(JournalLanguage lang);
    IEnumerable<ILocalizedBook> BooksForLanguage(JournalLanguage lang);
  }

  public class Journal : Entity, IJournal
  {
    private readonly INumber[] myNumbers;
    private readonly IAuthor[] myAuthors;
    private readonly INewsItem[] myNews;
    private readonly IBook[] myBooks;

    public Journal(EntityGenerator gen, INumber[] numbers, IAuthor[] authors, INewsItem[] news, IBook[] books) : base(gen)
    {
      myNumbers = numbers;
      myAuthors = authors;
      myNews = news;
      myBooks = books;
    }

    public INumber[] Numbers => myNumbers;

    public IAuthor[] Authors => myAuthors;

    public INewsItem[] News => myNews;

    public IBook[] Books => myBooks;

    private static IEnumerable<T> CollectionForLanguage<T>(IEnumerable<ILocalizedEntity<T>> data, JournalLanguage lang)
      where T : IEntity
    {
      return data.Select(entity => entity.ForLanguage(lang));
    }

    public IEnumerable<ILocalizedNewsItem> NewsForLanguage(JournalLanguage lang)
    {
      return CollectionForLanguage(News, lang);
    }

    public IEnumerable<ILocalizedBook> BooksForLanguage(JournalLanguage lang)
    {
      return CollectionForLanguage(Books, lang);
    }
  }
}