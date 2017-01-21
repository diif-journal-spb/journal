using System.Collections.Generic;
using System.Linq;
using EugenePetrenko.DataModel;

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
    public string Pages => FirstPage + "-" + LastPage;

    private int FirstPage => myArticle.AllLanguages().Min(info => info.FirstPage);
    private int LastPage => myArticle.AllLanguages().Max(info => info.LastPage);

    [XmlElementPath("artType"), XmlText]
    public string ArtType => "RAR";

    [XmlElementPath("authors", "author", CloneData = new []{false, true}), XmlForeach]
    public RFFIAuthor[] Authors
    {
      get
      {
        var i = 1;
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
    public RFFIArticleText PdfText => new RFFIArticleText(myPdfManager, myArticle);

    [XmlElementPath("references", "reference", CloneData = new[] { false, true }), XmlForeach]
    public IEnumerable<RFFIReference> Refererences
    {
      get { return myArticle.ForLanguage(JournalLanguage.EN).References.Select(x => new RFFIReference(x)); }
    }

    [XmlElementPath("files")]
    public RFFIArticleFiles Files => new RFFIArticleFiles(myRffiIssue, myArticle);
  }
}
