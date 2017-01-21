using System;
using System.IO;
using System.Reflection;
using System.Xml;
using NUnit.Framework;

namespace EugenePetrenko.RFFI
{
  public class XmlTestBase
  {
    private static string AssemblyDirectory
    {
      get
      {
        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        var uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
      }
    }

    protected static string DataDirectory
    {
      get
      {
        var location = AssemblyDirectory;
        var path = Path.Combine(location, "..\\data");
        return Path.GetFullPath(path);
      }
    }

    protected static string MockData
    {
      get
      {
        var location = AssemblyDirectory;
        var path = Path.Combine(location, @"..\src\EugenePetrenko.RFFI\testSrc\SampleData");
        return Path.GetFullPath(path);
      }
    }

    protected static void DoTest<T>(string gold) where T : new()
    {
      var t = new T();
      XmlDocument build = XmlAttributeProcessor.Build(t);

      var sw = new StringWriter();
      build.Save(new XmlTextWriter(sw));
      var actual = sw.ToString();
      actual = actual
        .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "")
        .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
        .Replace(" xsi:noNamespaceSchemaLocation=\"JournalArticulus.xsd\"", "");

      try
      {
        Assert.AreEqual(gold.Trim(), actual.Trim());
      }
      catch (Exception e)
      {
        Console.Out.WriteLine("sw.ToString() = {0}", sw);
        throw new Exception(e.Message, e);
      }
    }

    protected void AssertXML(XmlDocument doc, String gold)
    {
      var actual = SaveXML(doc);
      var expected = FormatXML(gold);

      Assert.AreEqual(expected, actual);
    }

    protected void AssertXMLNode(XmlDocument doc, string xpath, string gold)
    {
      var node = doc.SelectSingleNode(xpath);
      Assert.NotNull(node);

      var tmp = new XmlDocument();
      var copy = tmp.ImportNode(node, false);

      var outerXml = copy.OuterXml;
      Console.Out.WriteLine("OuterXML: " + outerXml);
      Assert.AreEqual(outerXml, gold);
    }

    protected string SaveXML(XmlDocument doc)
    {
      var sw = new StringWriter();
      doc.Save(sw);
      return sw.ToString();
    }

    private string FormatXML(string xml)
    {
      var doc = new XmlDocument();
      doc.LoadXml(xml);
      return SaveXML(doc);
    }
  }
}