using System;
using System.Xml;
using JetBrains.Annotations;

namespace EugenePetrenko.RFFI
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  [MeansImplicitUse]
  public class XmlAttributeAttribute : XmlBaseAttribute
  {
    private readonly string Key;

    public XmlAttributeAttribute(string key)
    {
      Key = key;
    }

    public override void Apply(XmlNode node, string data)
    {
      XmlAttribute attr = node.OwnerDocument.CreateAttribute(Key);
      attr.Value = data;
      node.Attributes.Append(attr);
    }
  }
}