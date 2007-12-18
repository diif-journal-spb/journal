using System;
using System.Xml;

namespace EugenePetrenko.RFFI
{
  [AttributeUsage(AttributeTargets.Property)]
  public class XmlTextAttribute : XmlBaseAttribute
  {
    public override void Apply(XmlNode node, string data)
    {
      node.AppendChild(node.OwnerDocument.CreateTextNode(data));
    }
  }
  
  [AttributeUsage(AttributeTargets.Property)]
  public class XmlCDataAttribute : XmlBaseAttribute
  {
    public override void Apply(XmlNode node, string data)
    {
      node.AppendChild(node.OwnerDocument.CreateCDataSection(data));
    }
  }
}