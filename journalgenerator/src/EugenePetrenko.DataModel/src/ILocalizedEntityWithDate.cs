using System;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface ILocalizedEntityWithDate<T> : ILocalizedEntity<T> where T : IEntity
  {
    DateTime Date { get; }
  }

  internal abstract class LocalizedEntitySequenceWithDate<T> : LocalizedEntitySequence<T> where T : IEntity
  {
    private DateTime myDate;


    protected override void Constructor(XmlNode node, IXmlDataLoader loader)
    {
      base.Constructor(node, loader);
      myDate = loader.ParseDateTime(node.SelectSingleNode("@date").Value);
    }

    protected LocalizedEntitySequenceWithDate(XmlNode el, IXmlDataLoader loader)
      : base(el, loader)
    {      
    }
      
    public DateTime Date
    {
      get { return myDate; }
    }

  }
}