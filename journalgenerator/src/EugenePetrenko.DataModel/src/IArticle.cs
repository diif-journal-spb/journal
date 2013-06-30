using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IArticle : ILocalizedEntity<IArticleInfo>, IEntity
  {
    IAuthor[] Authors{ get; }

    IReference[] References { get; }
  }

  public class Article : LocalizedEntity<IArticleInfo>, IArticle
  {
    private readonly IAuthor[] myAuthors;
    private readonly Dictionary<IAuthor, string> mySortKeys = new Dictionary<IAuthor, string>();
    private readonly List<IReference> myReferences = new List<IReference>();

    public Article(XmlNode el, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      var authors = new List<IAuthor>();

      int counter = 1;
      foreach (XmlElement element in el.SelectNodes("references/reference").Elements())
      {
        string id = "" + counter++;
        string title = element.InnerText.Trim();

        myReferences.Add(new Reference(id, title));
      }

      foreach (XmlElement attribute in el.SelectNodes("authors/author[@ref]").Elements())
      {
        IAuthor author = loader.LookupAuthor(attribute.GetAttribute("ref"));
        authors.Add(author);
        mySortKeys[author] = attribute.GetAttribute("sort-key");
      }
      myAuthors = authors.ToArray();

      foreach (XmlElement node in el.SelectNodes("articleInfo").Elements())
      {
        var info = loader.ParseArticleInfo(this, node);
        AddEntity(info.JournalLanguage, info);
      }
    }

    public IReference[] References
    {
      get { return myReferences.ToArray(); }
    }

    public string CompareKey(IAuthorInfo author)
    {
      string key;
      return mySortKeys.TryGetValue(author.Author, out key) ? key : string.Empty;
    }

    public IAuthor[] Authors
    {
      get { return myAuthors; }
    }
  }
}