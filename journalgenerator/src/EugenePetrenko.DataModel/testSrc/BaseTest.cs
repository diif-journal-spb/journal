/*
 * Created by: 
 * Created: 14 ����� 2007 �.
 */

using System.Xml;

namespace EugenePetrenko.DataModel
{
  public class BaseTest : XmlDataLoader
  {
    protected delegate void DTest(XmlElement root);

    protected static void DoTest(string xml, DTest test)
    {
      var doc = new XmlDocument();
      doc.LoadXml(xml);

      test(doc.DocumentElement);
    }
  }
}