using System.Collections.Generic;
using System.Linq;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
  public class RFFIArticle
  {
    private readonly IArticle myArticle;

    public RFFIArticle(IArticle article)
    {
      myArticle = article;
    }

    [XmlElementPath("fpageart"), XmlText]
    public string FirstPage { get { return getFirstPage().ToString(); } }

    [XmlElementPath("lpageart"), XmlText]
    public string LastPage { get { return getLastPage().ToString(); } }

    [XmlElementPath("arttitles", "arttitle", CloneData = new bool[]{false, true}), XmlForeach]
    public IEnumerable<RFFIArticleTitle> Titles
    {
      get
      {
        IArticleInfo en = myArticle.ForLanguage(JournalLanguage.EN);
        yield return new RFFIArticleTitle(en.Title, "eng");
        IArticleInfo ru = myArticle.ForLanguage(JournalLanguage.RU);
        yield return new RFFIArticleTitle(ru.Title, "rus");
      }
    }

    [XmlElementPath("abstracts", "abstract", CloneData = new bool[] { false, true }), XmlForeach]
    public IEnumerable<RFFIAbstract> Abstracts
    {
      get
      {
        IArticleInfo en = myArticle.ForLanguage(JournalLanguage.EN);
        yield return new RFFIAbstract(en.Abstract, "eng");
        IArticleInfo ru = myArticle.ForLanguage(JournalLanguage.RU);
        yield return new RFFIAbstract(ru.Abstract, "rus");
      }
    }

    [XmlElementPath("authors", "author", CloneData = new bool[]{false, true}), XmlForeach]
    public IEnumerable<RFFIAuthor> Authors
    {
      get
      {
        return myArticle.Authors.Select(author => new RFFIAuthor(author)).ToArray();
      }
    }

    [XmlElementPath("nokeywords"), XmlText]
    public string NoKeywords { get { return ""; } }

    [XmlElementPath("nobiblist"), XmlText]
    public string NoBiblist { get { return ""; } }
    
    [XmlElementPath("fpdf"), XmlText]
    public string Pdf { get { return myArticle.ForLanguage(JournalLanguage.EN).Pdf; } }

    private int getFirstPage()
    {
      return myArticle.AllLanguages().Select(info => info.FirstPage).FirstOrDefault();
    }

    private int getLastPage()
    {
      return myArticle.AllLanguages().Select(info => info.LastPage).FirstOrDefault();
    }

    public IArticle Article
    {
      get { return myArticle; }
    }
  }
}