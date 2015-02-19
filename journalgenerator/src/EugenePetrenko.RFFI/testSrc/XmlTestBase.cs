using System;
using System.IO;
using System.Xml;
using NUnit.Framework;

namespace EugenePetrenko.RFFI
{
  public class XmlTestBase
  {
    protected static void DoTest<T>(string gold) where T : new()
    {
      var t = new T();
      XmlDocument build = XmlAttributeProcessor.Build(t);

      var sw = new StringWriter();
      build.Save(new XmlTextWriter(sw));

      try
      {
        Assert.AreEqual(gold.Trim(), sw.ToString().Trim());
      }
      catch (Exception e)
      {
        Console.Out.WriteLine("sw.ToString() = {0}", sw);
        throw new Exception(e.Message, e);
      }
    }
  }
}