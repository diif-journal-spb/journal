using System;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IAuthor : ILocalizedEntity<IAuthorInfo>, IEntity
  {
    IOrganization Organization { get; }
  }

  public class Author : LocalizedEntity<IAuthorInfo>, IAuthor
  {
    private readonly IOrganization myOrganization;

    public Author(XmlElement element, IXmlDataLoader loader) : base(element)
    {
      var organizationId = element.GetAttribute("org");
      if (organizationId == null) throw new ArgumentException("Author " + Id + " does not have Organization");

      myOrganization = loader.LookupOrganization(organizationId);

      foreach (XmlElement node in element.SelectNodes("author"))
      {
        IAuthorInfo info = loader.ParseAuthorInfo(node, this);
        AddEntity(info.JournalLanguage, info);
      }
    }

    public IOrganization Organization => myOrganization;
  }
}
