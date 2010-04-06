using System;
using System.Xml;
using JetBrains.Annotations;

namespace EugenePetrenko.RFFI
{
  [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class)]
  [MeansImplicitUse]
  public class XmlElementPathAttribute : Attribute
  {
    private readonly string[] Path;
    private bool myClone = false;
    private bool[] myCloneData;

    public XmlElementPathAttribute(params string[] path)
    {
      Path = path;
    }

    public XmlNode Apply(XmlNode doc)
    {
      for (int i = 0; i < Path.Length; i++)
      {
        string path = Path[i];
        XmlNode node = doc.SelectSingleNode(path);
        if (node == null || Clone || (myCloneData != null && myCloneData.Length > i && myCloneData[i]))
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

    public bool Clone
    {
      get { return myClone; }
      set { myClone = value; }
    }

    public bool[] CloneData
    {
      get { return myCloneData; }
      set { myCloneData = value; }
    }
  }
}