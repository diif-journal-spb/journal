using System;
using System.Xml;
using JetBrains.Annotations;

namespace EugenePetrenko.RFFI
{
  [MeansImplicitUse]
  [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class)]
  public class XmlIgnoreAttribute : Attribute {}
  
  [MeansImplicitUse]
  [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class)]
  public class XmlElementPathAttribute : Attribute
  {
    private readonly string[] Path;
    public bool Clone { get; set; }
    public bool[] CloneData { get; set; }

    public XmlElementPathAttribute(params string[] path)
    {
      Clone = false;
      Path = path;
    }

    public XmlNode Apply(XmlNode doc)
    {
      for (int i = 0; i < Path.Length; i++)
      {
        string path = Path[i];
        XmlNode node = doc.SelectSingleNode(path);
        if (node == null || Clone || (CloneData != null && CloneData.Length > i && CloneData[i]))
        {
          doc = doc.AppendChild(doc.OwnerDocument.CreateElement(path));
        }
        else
        {
          doc = node;
        }
      }
      return doc;
    }
  }
}