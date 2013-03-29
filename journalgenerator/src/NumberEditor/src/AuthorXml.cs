using System;
using System.Xml.Serialization;

namespace EugenePetrenko.NumberEditor
{
  [Serializable, XmlRoot("author")]
  public class AuthorXml : Localizable
  {
    [XmlAttribute("lang")]
    public string Lang { get; set; }

    [XmlElement]
    public string FirstName { get; set; }
    [XmlElement]
    public string MiddleName { get; set; }
    [XmlElement]
    public string LastName { get; set; }
    [XmlElement]
    public string Address { get; set; }
    [XmlElement]
    public string Email { get; set; }
  }

  [Serializable, XmlRoot("author")]
  public class LocalizedAuthorXml : LocalizableHolder<AuthorXml>
  {
    protected override AuthorXml NewT()
    {
      return new AuthorXml();
    }

    [XmlElement("author")]
    public AuthorXml[] Items
    {
      get { return ItemsIntenal; }
      set { ItemsIntenal = value; }
    }

    [XmlAttribute("id")]
    public string Id { get; set; }


    public void UpdateId()
    {
      Id = "n_2013_" + (GetEN().LastName ?? "LAST_NAME").ToLower();
    }
  }
}