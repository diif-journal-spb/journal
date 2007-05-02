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

    public Article(XmlElement el, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      List<IAuthor> authors = new List<IAuthor>();
      foreach (XmlAttribute attribute in el.SelectSingleNode("authors").SelectNodes("author/@ref"))
      {
        authors.Add(loader.LookupAuthor(attribute.Value));
      }
      myAuthors = authors.ToArray();

      foreach (XmlElement node in el.SelectNodes("articleInfo"))
      {
        IArticleInfo info = loader.ParseArticleInfo(this, node);
        AddEntity(info.JournalLanguage, info);
      }
    }

    public IAuthor[] Authors
    {
      get { return myAuthors; }
    }
  }
}