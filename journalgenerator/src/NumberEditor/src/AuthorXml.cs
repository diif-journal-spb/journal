using System;
using System.Xml.Serialization;

namespace EugenePetrenko.NumberEditor
{
  [Serializable, XmlRoot("author")]
  public class AuthorXml
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
}