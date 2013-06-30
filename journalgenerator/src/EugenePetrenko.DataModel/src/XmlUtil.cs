using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public static class XmlUtil
  {
    public static string ReadText(this XmlNode node, string request)
    {
      var xmlNode = node.SelectSingleNode(request);
      if (xmlNode == null)
        return null;
      return (xmlNode.Value ?? string.Empty).Trim();
    }
    
    public static IEnumerable<XmlElement> Elements(this XmlNodeList nodes)
    {
      if (nodes == null) yield break;
      foreach (XmlElement node in nodes)
      {
        yield return node;
      }
    }
  }
}