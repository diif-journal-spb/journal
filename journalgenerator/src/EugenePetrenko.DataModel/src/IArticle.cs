using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IArticle : ILocalizedEntity<IArticleInfo>
  {
    IAuthor[] Authors{ get; }
  }

  public class Article : LocalizedEntity<IArticleInfo>, IArticle
  {
    private readonly IAuthor[] myAuthors;
    private readonly Dictionary<IAuthor, string> mySortKeys = new Dictionary<IAuthor, string>();

    public Article(XmlNode el, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      List<IAuthor> authors = new List<IAuthor>();
      foreach (XmlElement attribute in el.SelectSingleNode("authors").SelectNodes("author[@ref]"))
      {
        IAuthor author = loader.LookupAuthor(attribute.GetAttribute("ref"));
        authors.Add(author);
        mySortKeys[author] = attribute.GetAttribute("sort-key");
      }
      myAuthors = authors.ToArray();

      foreach (XmlElement node in el.SelectNodes("articleInfo"))
      {
        IArticleInfo info = loader.ParseArticleInfo(this, node);
        AddEntity(info.JournalLanguage, info);
      }
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