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

    public ArticleInfo(XmlElement el, XmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myJournalLanguage = loader.ParseLanguage(el);
      List<IAuthorInfo> authors = new List<IAuthorInfo>();
      foreach (XmlElement author in el.SelectNodes("authors/author"))
      {
        authors.Add(loader.ParseAuthorInfo(author));
      }
      myAuthors = authors.ToArray();

      myFirstPage = int.Parse(el.GetAttribute("FirstPage"));
      myLastPage = int.Parse(el.GetAttribute("LastPage"));

      myPdf = el.SelectSingleNode("pdf/text()").Value;
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

    public string Abstract
    {
      get { return myAbstract; }
    }
  }
}