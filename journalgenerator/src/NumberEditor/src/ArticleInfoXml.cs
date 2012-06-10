using System;
using System.Xml.Serialization;

namespace EugenePetrenko.NumberEditor
{
  [Serializable]
  public class ArticleInfoXml
  {
    [XmlAttribute("lang")]
    public string Lang { get; set; }

    [XmlAttribute("FirstPage")]
    public string FirstPage { get; set; }

    [XmlAttribute("LastPage")]
    public string LastPage { get; set; }

    [XmlElement("pdf")]
    public string Pdf { get; set; }

    [XmlElement("Abstract")]
    public string Abstract { get; set; }

    [XmlElement("Title")]
    public string Title { get; set; }
  }
}