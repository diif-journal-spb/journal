using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IAuthor : ILocalizedEntity<IAuthorInfo>
  {
  }

  public class Author : LocalizedEntity<IAuthorInfo>, IAuthor
  {
    public Author(XmlElement element, IXmlDataLoader loader) : base(element)
    {
      foreach (XmlElement node in element.SelectNodes("author"))
      {
        IAuthorInfo info = loader.ParseAuthorInfo(node);
        AddEntity(info.JournalLanguage, info);
      }
    }
  }
}