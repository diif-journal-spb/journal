using System;
using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface ILocalizedBook : IEntity
  {
    DateTime Date { get; }
    string Abstract { get; }
    string Title { get; }
    string Image { get; }
    string Pdf { get; }
    string[] Authors { get; }
    string ISBN { get;}
  }

  internal class LocalizedBookImpl : Entity, ILocalizedBook
  {
    private readonly DateTime myDate;
    private readonly IBook myBook;
    private readonly string myAbstract;
    private readonly string myTitle;
    private readonly string[] myAuthors;
    private readonly string myISBN;

    public LocalizedBookImpl(DateTime date, IBook book, XmlNode el, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myAbstract = el.ReadText("abstract/text()");
      myTitle = el.ReadText("title/text()");
      myISBN = el.ReadText("@isbn");
      myDate = date;
      myBook = book;

      var authors = new List<string>();
      foreach (XmlNode node in el.SelectNodes("authors/author/text()"))
      {
        authors.Add(node.Value);
      }
      myAuthors = authors.ToArray();
    }

    public string[] Authors => myAuthors;

    public DateTime Date => myDate;

    public string Abstract => myAbstract;

    public string Title => myTitle;

    public string Image => myBook.Image;

    public string Pdf => myBook.Pdf;

    public string ISBN => myISBN;
  }
}
