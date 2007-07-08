using System.Xml;
using NUnit.Framework;

namespace EugenePetrenko.DataModel
{
  [TestFixture]
  public class AuthorInfoTest : BaseTest
  {
    public const string TEST_01 =
      @"
         <author lang=""EN"">
            <FirstName>AAA</FirstName>
            <MiddleName>BBB</MiddleName>
            <LastName>CCC</LastName>
            <Email>eeee</Email>
            <Address>Address</Address>
         </author>";

    [Test]
    public void Test_01()
    {
      DoTest(
        TEST_01,
        delegate(XmlElement root)
          {
            IAuthorInfo info = myLoader.ParseAuthorInfo(root);

            AssertXml_Test_01(info);
          }
        );
    }

    public static void AssertXml_Test_01(IAuthorInfo info)
    {
      Assert.AreEqual("AAA", info.FirstName);
      Assert.AreEqual("BBB", info.MiddleName);
      Assert.AreEqual("CCC", info.LastName);
      Assert.AreEqual("eeee", info.EMail);
      Assert.AreEqual("Address", info.Address);
      Assert.AreEqual(JournalLanguage.EN, info.JournalLanguage);
    }
  }
}