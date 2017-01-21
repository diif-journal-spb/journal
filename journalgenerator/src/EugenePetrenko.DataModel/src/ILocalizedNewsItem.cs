using System;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface ILocalizedNewsItem : IEntity
  {
    DateTime Date { get; }
    string Text { get;}
  }

  public class LocalizedNewsItem : Entity, ILocalizedNewsItem
  {
    private readonly string myText;
    private readonly DateTime myDate;

    public LocalizedNewsItem(DateTime date, XmlNode el, EntityGenerator gen) : base(gen)
    {
      myDate = date;
      myText = el.SelectSingleNode("text/text()").Value.Trim();
    }

    public string Text => myText;

    public DateTime Date => myDate;
  }
}
