using System;
using System.Xml;
using JetBrains.Annotations;

namespace EugenePetrenko.RFFI
{
  [MeansImplicitUse]
  public abstract class XmlBaseAttribute : Attribute
  {
    public abstract void Apply(XmlNode node, string data);
  }
}