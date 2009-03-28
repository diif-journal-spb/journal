using System.Xml;

namespace EugenePetrenko.DataModel
{
  internal abstract class LocalizedEntitySequence<T> : LocalizedEntity<T> 
    where T : IEntity
  {
    protected abstract T CreateElement(XmlNode node, IXmlDataLoader loader);
    protected virtual void Constructor(XmlNode node, IXmlDataLoader loader)
    {
    }

    protected LocalizedEntitySequence(XmlNode el, IXmlDataLoader loader)
      : base(loader.EntityGenerator)
    {
      Constructor(el, loader);
      foreach (XmlElement element in el.SelectNodes("data"))
      {
        JournalLanguage lang = loader.ParseLanguage(element);
        AddEntity(lang, CreateElement(element, loader));
      }
    }
  }
}