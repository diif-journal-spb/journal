using System.Collections.Generic;
using System.Linq;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
  public class RFFIIssue
  {
    private readonly INumber myNumber;
    private readonly IPdfTextManager myPdfManager;

    public RFFIIssue(INumber number, IPdfTextManager pdfManager)
    {
      myNumber = number;
      myPdfManager = pdfManager;
    }

    [XmlIgnore]
    public INumber NumberInternal => myNumber;

    [XmlElementPath("number"), XmlText]
    public string Number => myNumber.Number;

    [XmlElementPath("dateUni"), XmlText]
    public string JournalDateUni {
      get
      {
        switch (myNumber.IntNumber)
        {
          case 1:
            return myNumber.Year + "31/" + myNumber.Year;
          case 2:
            return myNumber.Year + "32/" + myNumber.Year;
          case 3:
            return myNumber.Year + "33/" + myNumber.Year;
          case 4:
            return myNumber.Year + "32/" + myNumber.Year;
          default:
            return myNumber.Year + "/" + myNumber.Year;
        }
      } 
    }

    [XmlElementPath("pages"), XmlText]
    public string JournalPages => "1-" + GetPages();

    [XmlIgnore]
    public int JournalTotalPages => GetPages();

    [XmlElementPath("articles", "article", CloneData = new []{false, true}), XmlForeach]
    public IEnumerable<RFFIArticle> Articles
    {
      get
      {
        var result = new List<RFFIArticle>();
        foreach (var section in myNumber.Sections)
        {
          var type = RFFIArtType.FromSection(section);
          if (type == null) continue;

          foreach (var article in section.Articles)
          {
            result.Add(new RFFIArticle(this, article, myPdfManager, type));
          }
        }
        return result;
      }
    }

    private int GetPages()
    {
      return Articles.Select(x => x.LastPage).Max();
    }
  }
}
