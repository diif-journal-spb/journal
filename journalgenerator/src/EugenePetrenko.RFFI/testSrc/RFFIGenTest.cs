using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using EugenePetrenko.DataModel;
using NUnit.Framework;

namespace EugenePetrenko.RFFI
{
  [TestFixture]
  public class RFFIGenTest : XmlTestBase, IPdfTextManager
  {
    public INumber[] Numbers
    {
      get
      {
        return XmlDataLoader.Parse(DataDirectory).Numbers;
      }
    }

    [Test, TestCaseSource("Numbers")]
    public void ShouldGenerateValidRFFIXML(INumber number)
    {
      DoRFFI(number);
    }

    public string PdfText(IArticleInfo article)
    {
      return "mock <pdf> text " + article.Id;
    }

    private string Path(XmlNode node)
    {
      if (node is XmlElement)
      {
        return Path(node.ParentNode) + "/" + node.Name;
      }

      if (node == null) return "";
      return Path(node.ParentNode);
    }

    private void DoRFFI(INumber number)
    {
      var doc = GenerateRFFI(number);

      Console.Out.WriteLine(SaveXML(doc));

      RFFISchemaValidator.EnsureRFFIValid(doc);

      //check no XML in text
      foreach (var text in doc.SelectNodes("//text()").Cast<XmlNode>())
      {
        if (Path(text).EndsWith("article/text")) continue;
        Assert.IsFalse(Regex.Matches(text.Value, "<\\w+/?>").Count > 0, "<TAG> contained in " + text.Value);
      }

      //check names trimmed
      foreach (var text in doc.SelectNodes("//surname/text()|//initials/text()").Cast<XmlText>())
      {
        Assert.AreEqual(text.Value.Trim(), text.Value, "Incorrect name: " + text.Value);
      }
    }

    private XmlDocument GenerateRFFI(INumber number)
    {
      var num = new RFFIJournalNumber(new RFFIIssue(number, this));
      XmlDocument doc = XmlAttributeProcessor.Build(num);
      return doc;
    }

    private XmlDocument MockDocument
    {
      get
      {
        var num = XmlDataLoader.Parse(MockData).Numbers.Single();
        return GenerateRFFI(num);
      }
    }

    [Test]
    public void MockNumberShouldBeValid()
    {
      Console.Out.WriteLine(SaveXML(MockDocument));

      RFFISchemaValidator.EnsureRFFIValid(MockDocument);
    }

    [Test]
    public void MockNumberShouldHaveValidDeclaration()
    {
      AssertXMLNode(MockDocument, "/*", @"<journal xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""JournalArticulus.xsd"" />" );
    }

  }
}