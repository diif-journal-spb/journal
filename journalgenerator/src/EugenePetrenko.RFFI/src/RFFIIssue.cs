using System;
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

      if (number.Sections.Length > 1)
      {
        Console.WriteLine("  Number contains extra sections: " + string.Join(", ", number.Sections.Select(x=>x.GetType().Name).Distinct().OrderBy(x=>x).ToList()));
      }
    }

    [XmlIgnore]
    public INumber NumberInternal => myNumber;

    [XmlElementPath("number"), XmlText]
    public string Number => myNumber.Number;

    [XmlElementPath("dateUni"), XmlText]
    // A change proposed by Сергей Дурнев on Apr 6
    public string JournalDateUni => myNumber.Year;

    [XmlElementPath("pages"), XmlText]
    public string JournalPages => "1-" + GetPages();

    [XmlIgnore]
    public int JournalTotalPages => GetPages();

    [XmlElementPath("articles"), XmlForeach]
    public IEnumerable<RFFIArticles> Articles
    {
      get
      {
        int sectionOffset = 0;
        foreach (var section in myNumber.Sections)
        {
          var type = RFFIArtType.FromSection(section);
          if (type == null) continue;

          var rffiArticles = section.Articles
            .Select(x => new RFFIArticle(this, x, myPdfManager, type, sectionOffset))
            .ToList();

          sectionOffset = (999 + rffiArticles.Max(x => x.LastPage)) / 1000 * 1000;   
          
          yield return
            section.ShowTitle
              ? new RFFISectionArticles(rffiArticles, new RFFISection(section))
              : new RFFIArticles(rffiArticles);
        }
      }
    }

    private int GetPages()
    {
      return Articles.Sum(a => a.Articles.Select(x => x.LastPage).Max());
    }
  }

  public class RFFISection
  {
    private readonly INumberSection mySection;

    public RFFISection(INumberSection section)
    {
      mySection = section;
    }

    [XmlElementPath("secTitle", Clone = true), XmlForeach]
    public IEnumerable<RFFISectionTitle> SecTitle => mySection.JournalLanguages.Select(l => new RFFISectionTitle(l, mySection.ForLanguage(l)));
  }

  public class RFFISectionTitle
  {
    private readonly JournalLanguage myLang;
    private readonly ILocalizedNumberSection mySection;

    public RFFISectionTitle(JournalLanguage lang, ILocalizedNumberSection section)
    {
      myLang = lang;
      mySection = section;
    }

    [XmlText]
    public string SecTitle => mySection.Name;

    [XmlAttribute("lang")]
    public string SetLang => myLang.Lang();
  }

  public class RFFIArticles
  {
    public RFFIArticles(IList<RFFIArticle> articles)
    {
      Articles = articles;
    }

    [XmlElementPath("article", Clone = true), XmlForeach]
    public IList<RFFIArticle> Articles { get; }
  }

  public class RFFISectionArticles : RFFIArticles
  {
    public RFFISectionArticles(IList<RFFIArticle> articles, RFFISection section) : base(articles)
    {
      Section = section;
    }

    [XmlElementPath("section")]
    public RFFISection Section { get; }
  }
}
