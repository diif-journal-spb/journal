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