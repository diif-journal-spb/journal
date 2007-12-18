using System;
using System.Xml;

namespace EugenePetrenko.RFFI
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  public class XmlAttributeAttribute : XmlBaseAttribute
  {
    public readonly string Key;

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