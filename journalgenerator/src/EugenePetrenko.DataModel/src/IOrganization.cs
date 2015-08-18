using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IOrganization : ILocalizedEntity<IOrganizationInfo>, IEntity
  {
    
  }

  public class Organization : LocalizedEntity<IOrganizationInfo>, IOrganization
  {
    public Organization(XmlElement element, IXmlDataLoader loader)
      : base(element)
    {
      foreach (XmlElement node in element.SelectNodes("org"))
      {
        var info = loader.ParseOrganizationInfo(node, this);
        AddEntity(info.JournalLanguage, info);
      }
    }
  }
}
