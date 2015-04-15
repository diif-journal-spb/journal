using System;
using System.Collections.Generic;
using System.Linq;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
  public class RFFIArticle
  {
    private readonly RFFIIssue myRffiIssue;
    private readonly IArticle myArticle;

    public RFFIArticle(RFFIIssue rffiIssue, IArticle article)
    {
      myRffiIssue = rffiIssue;
      myArticle = article;
    }

    [XmlElementPath("pages"), XmlText]
    public string Pages { get { return FirstPage + "-" + LastPage; }}

    private int FirstPage { get { return myArticle.AllLanguages().Min(info => info.FirstPage); } }
    private int LastPage { get { return myArticle.AllLanguages().Max(info => info.LastPage); } }

    [XmlElementPath("artType"), XmlText]
    public string ArtType { get { return "UNK"; } }

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

    [XmlElementPath("fhtml"), XmlText]
    public string HTML
    {
      get
      {
        return String.Format("{2}/RU/numbers/{0}.{1}/issue.html", myRffiIssue.NumberInternal.IntYear, myRffiIssue.NumberInternal.IntNumber, BaseURL);
      }
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

  }
}
