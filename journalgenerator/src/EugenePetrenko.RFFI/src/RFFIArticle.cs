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

    //TODO: generate fhtml as a link to journal web site
    [XmlElementPath("files", "fpdf"), XmlText]
    public string Pdf { get { return myArticle.AllLanguages().Select(x=>x.Pdf).First(); } }

    public IArticle Article
    {
      get { return myArticle; }
    }

  }
}