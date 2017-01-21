using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IArticleInfo : IEntity
  {
    JournalLanguage JournalLanguage { get;}

    IAuthorInfo[] Authors { get; }
    
    IReference[] References { get; }
    
    bool HasReferences { get; }

    int FirstPage { get; }
    int LastPage { get; }

    string Pdf { get; }
    string Title { get; }
    string Abstract { get; }

    string[] ExtraFiles { get; }

    bool HasKeywords { get;  }
    string[] Keywords { get;  }
  }

  public class ArticleInfo : Entity, IArticleInfo
  {
    private readonly JournalLanguage myJournalLanguage;
    private readonly Article myArticle;
    private readonly IAuthorInfo[] myAuthors;
    private readonly int myFirstPage;
    private readonly int myLastPage;
    private readonly string myPdf;
    private readonly string myAbstract;
    private readonly string myTitle;
    private readonly string[] myExtraFiles;
    private readonly string[] myKeywords;
    private readonly List<IReference> myReferences = new List<IReference>();

    public ArticleInfo(IArticle article, XmlElement el, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myArticle = (Article) article;
      myJournalLanguage = loader.ParseLanguage(el);
      var authors = new List<IAuthorInfo>();
      if (article != null && article.Authors != null)
      {
        foreach (IAuthor author in article.Authors)
        {
          authors.Add(author.ForLanguage(JournalLanguage));
        }
      }
      authors.Sort(delegate(IAuthorInfo x, IAuthorInfo y)
                     {
                       string kX = myArticle.CompareKey(x);
                       string kY = myArticle.CompareKey(y);

                       int v = kX.CompareTo(kY);
                       if (v != 0)
                         return v;

                       return x.CompareTo(y);
                     });
      myAuthors = authors.ToArray();

      myFirstPage = 0;
      myLastPage = 0;
      int.TryParse(el.GetAttribute("LastPage"), out myLastPage);
      int.TryParse(el.GetAttribute("FirstPage"), out myFirstPage);

      myPdf = el.SelectSingleNode("pdf/text()").Value.Trim();
      myTitle = el.SelectSingleNode("title/text()").Value.Trim();
      myAbstract = el.SelectSingleNode("abstract/text()").Value.Trim();

      myKeywords =
        el.SelectNodes("keywords/word/text()")
          .Cast<XmlText>()
          .Select(x => x.Value)
          .SelectMany(x => Regex.Split(x, @"[\r\n,;.]+"))
          .Select(x => Regex.Replace(x, @"\s+", " "))
          .Select(x => x.Trim())
          .Where(x=>x.Length > 0)
          .Distinct()
          .OrderBy(x=>x.ToLower())
          .ToArray();

      var extraFiles = new List<string>();
      foreach(XmlText extra in el.SelectNodes("extra-files/file/text()"))
      {
        string file = extra.Value.Trim();
        extraFiles.Add(file);
      }
      myExtraFiles = extraFiles.ToArray();

      int counter = 1;
      foreach (XmlElement element in el.SelectNodes("references/reference").Elements())
      {
        string id = "" + counter++;
        string title = element.InnerText.Trim();

        myReferences.Add(new Reference(id, title));
      }
    }

    public IReference[] References
    {
      get
      {
        if (JournalLanguage == JournalLanguage.RU && myReferences.Count == 0)
        {
          return myArticle.ForLanguage(JournalLanguage.EN).References;
        }
        else
        {
          return myReferences.ToArray();
        }
      }
    }

    public bool HasKeywords => Keywords.Length > 0;
    public string[] Keywords => myKeywords;

    public bool HasReferences => References.Length > 0;

    public string[] ExtraFiles => myExtraFiles;

    public JournalLanguage JournalLanguage => myJournalLanguage;

    public IAuthorInfo[] Authors => myAuthors;

    public int FirstPage => myFirstPage;

    public int LastPage => myLastPage;

    public string Pdf => myPdf;

    public string Title => myTitle;

    public string Abstract => myAbstract;
  }
}
