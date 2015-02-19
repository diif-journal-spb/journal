using System.Collections.Generic;
using NUnit.Framework;

namespace EugenePetrenko.RFFI
{
  [TestFixture]
  public class XmlProcessorTest : XmlTestBase
  {
    [Test]
    public void Test_01()
    {
      DoTest<Class_Test_01>("<?xml version=\"1.0\" encoding=\"utf-16\"?><aaaa />");
    }
    
    [Test]
    public void Test_02()
    {
      DoTest<Class_Test_02>("<?xml version=\"1.0\" encoding=\"utf-16\"?><aaaa Bar=\"Foo\">Foo</aaaa>");
    }
    
    [Test]
    public void Test_03()
    {
      DoTest<Class_Test_03>("<?xml version=\"1.0\" encoding=\"utf-16\"?><aaaa><a><b><c Bar=\"Foo\">Foo</c></b></a></aaaa>");
    }    
    
    [Test]
    public void Test_04()
    {
      DoTest<Class_Test_04>("<?xml version=\"1.0\" encoding=\"utf-16\"?><aaaa><x><y><z><a><b><c Bar=\"Foo\">Foo</c></b></a></z></y></x></aaaa>");
    }
    
    [Test]
    public void Test_05()
    {
      DoTest<Class_Test_05>("<?xml version=\"1.0\" encoding=\"utf-16\"?><aaaa><z Bar=\"Foo\">Foo</z><z Bar=\"Foo2\">Foo2</z></aaaa>");
    }

    [XmlRoot("aaaa")]
    private class Class_Test_01
    {
    }
    
    [XmlRoot("aaaa")]
    private class Class_Test_02
    {
      [XmlAttribute("Bar"), XmlText]
      public string Foo { get { return "Foo"; } }
    }

    [XmlRoot("aaaa")]
    private class Class_Test_03
    {
      [XmlElementPath("a","b","c"), XmlAttribute("Bar"), XmlText]
      public string Foo { get { return "Foo"; } }
    }
    
    [XmlRoot("aaaa"), XmlElementPath("x","y","z")]
    private class Class_Test_04
    {
      [XmlElementPath("a","b","c"), XmlAttribute("Bar"), XmlText]
      public string Foo { get { return "Foo"; } }
    }

    [XmlRoot("aaaa")]
    private class Class_Test_05
    {
      [XmlAttribute("Bar"), XmlForeach, XmlText, XmlElementPath("z", Clone = true)]
      public IEnumerable<string> Foo { get { return new [] { "Foo", "Foo2" }; } }
    }    
  }
}