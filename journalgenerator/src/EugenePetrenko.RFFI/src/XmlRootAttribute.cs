using System;
using System.Xml;

namespace EugenePetrenko.RFFI
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class XmlRootAttribute : Attribute
  {
    private readonly string RootName;

    public XmlRootAttribute(string rootName)
    {
      RootName = rootName;
    }

    public XmlNode Apply(XmlDocument doc)
    {
      doc.AppendChild(doc.CreateXmlDeclaration("1.0", "windows-1251", "no"));      
      return doc.AppendChild(doc.CreateElement(RootName));
    }
  }
}