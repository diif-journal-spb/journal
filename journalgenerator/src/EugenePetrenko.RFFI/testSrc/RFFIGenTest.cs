using System.CodeDom;
using System.IO;
using System.Linq;
using System.Xml;
using EugenePetrenko.DataModel;
using NUnit.Framework;

namespace EugenePetrenko.RFFI
{
  [TestFixture]
  public class RFFIGenTest : XmlTestBase
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

    private static XmlDocument DoRFFI(INumber number)
    {
      var doc = GenerateRFFI(number);

      RFFISchemaValidator.EnsureRFFIValid(doc);

      return doc;
    }

    private static XmlDocument GenerateRFFI(INumber number)
    {
      var num = new RFFIJournalNumber(new RFFIIssue(number));
      XmlDocument doc = XmlAttributeProcessor.Build(num);
      return doc;
    }

    [Test]
    public void MockNumberShouldWork()
    {
      var num = XmlDataLoader.Parse(MockData).Numbers.Single();
      var doc = GenerateRFFI(num);

      var sq = new StringWriter();
      doc.Save(sq);
      sq.Close();

      var text = sq.ToString();

      DoRFFI(num);
    }
  }
}