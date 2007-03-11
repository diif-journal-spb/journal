using System;
using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IArticle
  {
    string Id { get; }

    ICollection<JournalLanguage> JournalLanguages { get; }
    IArticleInfo ForLanguage(JournalLanguage journalLanguage);
  }

  public class Article : Entity, IArticle
  {
    private readonly Dictionary<JournalLanguage, IArticleInfo> myArticles = new Dictionary<JournalLanguage, IArticleInfo>();

    public Article(XmlElement el, XmlDataLoader loader) : base(loader.EntityGenerator)
    {
      foreach (XmlElement node in el.SelectNodes("articleInfo"))
      {
        IArticleInfo info = loader.ParseArticleInfo(node);
        myArticles[info.JournalLanguage] = info;
      }
    }

    public ICollection<JournalLanguage> JournalLanguages
    {
      get { return myArticles.Keys; }
    }

    public IArticleInfo ForLanguage(JournalLanguage journalLanguage)
    {
      throw new NotImplementedException();
    }
  }
}