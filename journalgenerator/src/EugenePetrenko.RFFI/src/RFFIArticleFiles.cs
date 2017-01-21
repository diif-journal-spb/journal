using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
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
        return $"{BaseURL}/pdf/{x.Pdf.Replace("\\", "/")}";
      }
    }
  }
}
