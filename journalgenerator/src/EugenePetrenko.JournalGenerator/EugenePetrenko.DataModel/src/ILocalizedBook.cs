using System;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface ILocalizedBook : IEntity
  {
    DateTime Date { get; }
    string Abstract { get; }
    string Title { get; }
    string Image { get; }
  }

  internal class LocalizedBookImpl : Entity, ILocalizedBook
  {
    private readonly DateTime myDate;
    private readonly string myAbstract;
    private readonly string myTitle;
    private readonly string myImage;

    public LocalizedBookImpl(DateTime date, string image, XmlNode el, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myAbstract = el.SelectSingleNode("abstract/text()").Value;
      myTitle = el.SelectSingleNode("title/text()").Value;
      myDate = date;
      myImage = image;
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