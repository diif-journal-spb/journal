using System;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface INewsItem : ILocalizedEntityWithDate<ILocalizedNewsItem>
  {
  }

  internal class NewsItemImpl : LocalizedEntitySequenceWithDate<ILocalizedNewsItem>, INewsItem
  {
    public NewsItemImpl(XmlNode el, IXmlDataLoader loader) : base(el, loader)
    {
    }

    protected override ILocalizedNewsItem CreateElement(XmlNode node, IXmlDataLoader loader)
    {
      return new LocalizedNewsItem(Date, node, loader.EntityGenerator);
    }
  }
}