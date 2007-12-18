using System;
using System.Xml;

namespace EugenePetrenko.RFFI
{
  [AttributeUsage(AttributeTargets.Property)]
  public class XmlStaticAttributeAttribute : XmlAttributeAttribute
  {
    private readonly string myValue;

    public XmlStaticAttributeAttribute(string key, string value) : base(key)
    {
      myValue = value;
    }

    public override void Apply(XmlNode node, string data)
    {
      base.Apply(node, myValue);
    }
  }
}