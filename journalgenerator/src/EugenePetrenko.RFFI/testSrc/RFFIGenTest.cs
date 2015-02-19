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
        var journal = XmlDataLoader.Parse(DataDirectory);
        return journal.Numbers;
      }
    }

    [Test, TestCaseSource("Numbers")]
    public void ShouldGenerateValidRFFIXML(INumber number)
    {
      var num = new RFFIJournalNumber(new RFFIIssue(number));
      XmlDocument doc = XmlAttributeProcessor.Build(num);

      RFFISchemaValidator.EnsureRFFIValid(doc);
    }
  }
}