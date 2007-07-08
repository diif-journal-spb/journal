using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IArticleInfo : IEntity
  {
    JournalLanguage JournalLanguage { get;}

    IAuthorInfo[] Authors { get; }

    int FirstPage { get; }
    int LastPage { get; }

    string Pdf { get; }
    string Title { get; }
    string Abstract { get; }    
  }

  public class ArticleInfo : Entity, IArticleInfo
  {
    private readonly JournalLanguage myJournalLanguage;
    private readonly IAuthorInfo[] myAuthors;
    private readonly int myFirstPage;
    private readonly int myLastPage;
    private readonly string myPdf;
    private readonly string myAbstract;
    private readonly string myTitle;

    public ArticleInfo(IArticle article, XmlElement el, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myJournalLanguage = loader.ParseLanguage(el);
      List<IAuthorInfo> authors = new List<IAuthorInfo>();
      if (article != null && article.Authors != null)
      {
        foreach (IAuthor author in article.Authors)
        {
          authors.Add(author.ForLanguage(JournalLanguage));
        }
      }
      authors.Sort();
      myAuthors = authors.ToArray();

      myFirstPage = 0;
      myLastPage = 0;
      int.TryParse(el.GetAttribute("LastPage"), out myLastPage);
      int.TryParse(el.GetAttribute("FirstPage"), out myFirstPage);

      myPdf = el.SelectSingleNode("pdf/text()").Value.Trim();
      myTitle = el.SelectSingleNode("title/text()").Value.Trim();
      myAbstract = el.SelectSingleNode("abstract/text()").Value.Trim();

    }

    public JournalLanguage JournalLanguage
    {
      get { return myJournalLanguage; }
    }

    public IAuthorInfo[] Authors
    {
      get { return myAuthors; }
    }

    public int FirstPage
    {
      get { return myFirstPage; }
    }

    public int LastPage
    {
      get { return myLastPage; }
    }

    public string Pdf
    {
      get { return myPdf; }
    }

    public string Title
    {
      get { return myTitle; }
    }

    public string Abstract
    {
      get { return myAbstract; }
    }
  }
}