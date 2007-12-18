using System;
using System.Xml;

namespace EugenePetrenko.RFFI
{
  public abstract class XmlBaseAttribute : Attribute
  {
    public abstract void Apply(XmlNode node, string data);
  }
}