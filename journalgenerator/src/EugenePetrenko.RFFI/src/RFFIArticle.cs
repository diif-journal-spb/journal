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
    public string FirstPage { get { return myArticle.AllLanguages().Min(info => info.FirstPage).ToString(); } }

    [XmlElementPath("lpageart"), XmlText]
    public string LastPage { get { return myArticle.AllLanguages().Max(info => info.LastPage).ToString(); } }


    [XmlElementPath("arttitles", "arttitle", Clone = true), XmlForeach]
    public IEnumerable<RFFIArticleTitle> Titles
    {
      get
      {
        return myArticle.AllLanguages().Select(x => new RFFIArticleTitle(x.Title, x.JournalLanguage.Lang()));
      }
    }

    [XmlElementPath("abstracts", "abstract", Clone = true), XmlForeach]
    public IEnumerable<RFFIAbstract> Abstracts
    {
      get
      {
        return myArticle.AllLanguages().Select(x => new RFFIAbstract(x.Abstract, x.JournalLanguage.Lang()));
      }
    }

    [XmlElementPath("authors", "author", Clone = true), XmlForeach]
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
    public string Pdf { get { return myArticle.AllLanguages().Select(x=>x.Pdf).First(); } }

    public IArticle Article
    {
      get { return myArticle; }
    }
  }
}