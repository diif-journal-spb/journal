using System.Linq;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
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
    public string Text => myPdfText.PdfText(myArticle.AllLanguages().First());
  }
}
