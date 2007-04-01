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
    private JournalLanguage myJournalLanguage;
    private IAuthorInfo[] myAuthors;
    private int myFirstPage;
    private int myLastPage;
    private string myPdf;
    private string myAbstract;
    private string myTitle;

    public ArticleInfo(Article article, XmlElement el, IXmlDataLoader loader) : base(loader.EntityGenerator)
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

      myFirstPage = int.Parse(el.GetAttribute("FirstPage"));
      myLastPage = int.Parse(el.GetAttribute("LastPage"));

      myPdf = el.SelectSingleNode("pdf/text()").Value;
      myTitle = el.SelectSingleNode("title/text()").Value;
      myAbstract = el.SelectSingleNode("abstract/text()").Value;

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