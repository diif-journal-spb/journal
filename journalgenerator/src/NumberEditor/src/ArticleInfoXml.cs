using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace EugenePetrenko.NumberEditor
{
  [Serializable]
  public class ArticleInfoXml : Localizable
  {
    [XmlAttribute("lang")]
    public string Lang { get; set; }

    [XmlAttribute("FirstPage")]
    public int FirstPage { get; set; }

    [XmlAttribute("LastPage")]
    public int LastPage { get; set; }

    [XmlElement("pdf")]
    public string Pdf { get; set; }

    [XmlElement("abstract")]
    public string Abstract { get; set; }

    [XmlElement("title")]
    public string Title { get; set; }
  }


  [Serializable, XmlRoot("article")]
  public class LocalizedArticleXml : LocalizableHolder<ArticleInfoXml>
  {
    protected override ArticleInfoXml NewT()
    {
      return new ArticleInfoXml();
    }

    [XmlArray("references")]
    [XmlArrayItem("reference")]
    public string[] References { get; set; }

    public void AddReferences(IEnumerable<string> x)
    {
      var refs = new List<string>();
      if (References != null) refs.AddRange(References);
      refs.AddRange(x);
      References = refs.ToArray();
    }

    public void RemoveReference()
    {
      if (References == null) return;
      if (References.Length <= 1)
      {
        References = null;
        return;
      }

      var refs = new List<string>();
      if (References != null) refs.AddRange(References);
      refs.RemoveAt(refs.Count-1);
      
      References = refs.ToArray();
    }

    [XmlElement("articleInfo")]
    public ArticleInfoXml[] Items
    {
      get { return ItemsIntenal; }
      set { ItemsIntenal = value; }
    }

    [XmlArray("authors"), XmlArrayItem("author")]
    public AuthorRef[] Authors { get; set; }

    public void AddAuthor(string id)
    {
      AddAuthor(new AuthorRef{Ref = id, SortKey = (GetAuthorsCopy().Count + 1).ToString(CultureInfo.InvariantCulture)});
    }

    public void SetAuthors(IEnumerable<LocalizedAuthorXml> authors)
    {
      Authors = authors.Select((author,i) => new AuthorRef {Ref = author.Id, SortKey = (i+1).ToString(CultureInfo.InvariantCulture)}).ToArray();
    }

    public void AddAuthor(AuthorRef author)
    {
      var authorRefs = GetAuthorsCopy();
      authorRefs.Add(author);
      Authors = authorRefs.ToArray();
    }

    public List<AuthorRef> GetAuthorsCopy()
    {
      return new List<AuthorRef>(Authors ?? new AuthorRef[0]);
    }
  }

  [Serializable]
  public class AuthorRef
  {
    [XmlAttribute("ref")]
    public string Ref { get; set; }
    
    [XmlAttribute("sort-key")]
    public string SortKey { get; set; }
  }
}