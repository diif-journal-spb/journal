using System;
using System.Xml;
using JetBrains.Annotations;

namespace EugenePetrenko.RFFI
{
  [MeansImplicitUse]
  [AttributeUsage(AttributeTargets.Property)]
  public class XmlTextAttribute : XmlBaseAttribute
  {
    public override void Apply(XmlNode node, string data)
    {
      node.AppendChild(node.OwnerDocument.CreateTextNode(data));
    }
  }

  [MeansImplicitUse]
  [AttributeUsage(AttributeTargets.Property)]  
  public class XmlCDataAttribute : XmlBaseAttribute
  {
    public override void Apply(XmlNode node, string data)
    {
      node.AppendChild(node.OwnerDocument.CreateCDataSection(data));
    }
  }
}