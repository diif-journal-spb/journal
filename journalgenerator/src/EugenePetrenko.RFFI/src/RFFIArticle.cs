using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EugenePetrenko.DataModel;
using JetBrains.Annotations;

namespace EugenePetrenko.RFFI
{
  public class RFFIArticle
  {
    private readonly RFFIIssue myRffiIssue;
    private readonly IArticle myArticle;
    private readonly IPdfTextManager myPdfManager;

    public RFFIArticle(RFFIIssue rffiIssue, IArticle article, IPdfTextManager pdfManager)
    {
      myRffiIssue = rffiIssue;
      myArticle = article;
      myPdfManager = pdfManager;
    }

    [XmlElementPath("pages"), XmlText]
    public string Pages { get { return FirstPage + "-" + LastPage; }}

    private int FirstPage { get { return myArticle.AllLanguages().Min(info => info.FirstPage); } }
    private int LastPage { get { return myArticle.AllLanguages().Max(info => info.LastPage); } }

    [XmlElementPath("artType"), XmlText]
    public string ArtType { get { return "RAR"; } }

    [XmlElementPath("authors", "author", CloneData = new []{false, true}), XmlForeach]
    public RFFIAuthor[] Authors
    {
      get
      {
        int i = 1;
        return myArticle.Authors.Select(author => new RFFIAuthor(i++, author)).ToArray();
      }
    }

    [XmlElementPath("artTitles", "artTitle", CloneData = new []{false, true}), XmlForeach]
    public IEnumerable<RFFIArticleTitle> Titles
    {
      get
      {
        return myArticle.AllLanguages().Select(x => new RFFIArticleTitle(x.Title, x.JournalLanguage.Lang()));
      }
    }

    [XmlElementPath("abstracts", "abstract", CloneData = new []{false, true}), XmlForeach]
    public IEnumerable<RFFIAbstract> Abstracts
    {
      get
      {
        return myArticle.AllLanguages().Select(x => new RFFIAbstract(x.Abstract, x.JournalLanguage.Lang()));
      }
    }

    [XmlElementPath("text")]
    public RFFIArticleText PdfText
    {
      get { return new RFFIArticleText(myPdfManager, myArticle); }
    }

    [XmlElementPath("references", "reference", CloneData = new[] { false, true }), XmlForeach]
    public IEnumerable<RFFIReference> Refererences
    {
      get { return myArticle.References.Select(x => new RFFIReference(x)); }
    }

    [XmlElementPath("files")]
    public RFFIArticleFiles Files
    {
      get { return new RFFIArticleFiles(myRffiIssue, myArticle);}
    }
  }

  public class RFFIArticleText
  {
    private readonly IPdfTextManager myPdfText;
    private readonly IArticle myArticle;

    public RFFIArticleText(IPdfTextManager pdfText, IArticle article)
    {
      myPdfText = pdfText;
      myArticle = article;
    }

    [XmlAttribute("lang")]
    public string Lang
    {
      get
      {
        return myArticle.AllLanguages().Select(x => x.JournalLanguage).Contains(JournalLanguage.RU)
          ? JournalLanguage.RU.Lang()
          : JournalLanguage.EN.Lang();
      }
    }

    [XmlText]
    public string Text
    {
      get
      {
        return myPdfText.PdfText(myArticle.AllLanguages().First());
      }
    }
  }

  public class RFFIArticleFiles
  {
    private const string BaseURL = "http://www.math.spbu.ru/diffjournal";

    private readonly RFFIIssue myRffiIssue;
    private readonly IArticle myArticle;

    public RFFIArticleFiles(RFFIIssue rffiIssue, IArticle article)
    {
      myRffiIssue = rffiIssue;
      myArticle = article;
    }
    
    [XmlElementPath("furl", CloneData = new [] {true}), XmlText]
    public string PdfURLs
    {
      get
      {
        var x = myArticle.ForLanguage(JournalLanguage.RU);
        return string.Format("{0}/pdf/{1}", BaseURL, x.Pdf.Replace("\\", "/"));
      }
    }

    private static class ArticleLinkExtensions
    {
      private static string getArticleLink(INumber number, IArticle article)
      {
        return string.Format(@"numbers\{0}.{1}\article.{2}.html", number.Year, number.Number, ArticleFileId(number, article));
      }

      public static string getArticleURL(string baseURL, INumber number, IArticle article)
      {
        return Regex.Replace(baseURL + "/" + getArticleLink(number, article), @"[\\/]+", "/");
      }

      private static string ArticleFileId(INumber myNumber, IArticle myArticle)
      {
        int sectionId = 0;
        foreach (var section in myNumber.Sections)
        {
          sectionId++;
          int artcleId = 0;
          foreach (var article in section.Articles)
          {
            artcleId++;
            if (article == myArticle)
            {
              return sectionId + "." + artcleId;
            }
          }
        }
        throw new Exception("Article not found in the number");
      }
    }
  }
}
