using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IOrganizationInfo : IEntity
  {
    IOrganization Organization { get; }
    JournalLanguage JournalLanguage { get; }

    string Name { get; }
    string Address { get; }
  }

  public class OrganizationInfo : Entity, IOrganizationInfo
  {
    private readonly JournalLanguage myJournalLanguage;
    private readonly IOrganization myOrganization;
    private readonly string myName;
    private readonly string myAddress;

    public OrganizationInfo(XmlElement el, IXmlDataLoader loader, IOrganization organization)
      : base(loader.EntityGenerator)
    {
      myJournalLanguage = loader.ParseLanguage(el);
      myOrganization = organization;
      myName = SafeRead(el.SelectSingleNode("name/text()"));
      myAddress = SafeRead(el.SelectSingleNode("address/text()"));
    }

    private static string SafeRead(XmlNode node)
    {
      return node == null ? string.Empty : node.Value;
    }

    public JournalLanguage JournalLanguage => myJournalLanguage;

    public IOrganization Organization => myOrganization;

    public string Address => myAddress;

    public string Name => myName;

    public override string ToString()
    {
      return "POrganizationInfp{Id=" + Id + ", Name=" + Name + "}";
    }
  }
}
