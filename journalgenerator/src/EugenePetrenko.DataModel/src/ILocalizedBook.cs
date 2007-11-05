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
    string[] Authors { get; }
  }

  internal class LocalizedBookImpl : Entity, ILocalizedBook
  {
    private readonly DateTime myDate;
    private readonly string myAbstract;
    private readonly string myTitle;
    private readonly string myImage;
    private readonly string[] myAuthors;

    public LocalizedBookImpl(DateTime date, string image, XmlNode el, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myAbstract = el.SelectSingleNode("abstract/text()").Value.Trim();
      myTitle = el.SelectSingleNode("title/text()").Value.Trim();
      myDate = date;
      myImage = image.Trim();
      List<string> authors = new List<string>();
      foreach (XmlNode node in el.SelectNodes("authors\author\text()"))
      {
        authors.Add(node.Value);
      }
      myAuthors = authors.ToArray();
    }

    public string[] Authors
    {
      get { return myAuthors; }
    }

    public DateTime Date
    {
      get { return myDate; }
    }

    public string Abstract
    {
      get { return myAbstract; }
    }

    public string Title
    {
      get { return myTitle; }
    }

    public string Image
    {
      get { return myImage; }
    }
  }
}