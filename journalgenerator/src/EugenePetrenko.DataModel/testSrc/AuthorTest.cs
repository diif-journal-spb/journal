using System.Xml;
using NUnit.Framework;

namespace EugenePetrenko.DataModel
{
  [TestFixture]
  public class AuthorTest : BaseTest
  {
    public const string TEST_01 = @"
          <authors org='111'>
             " + AuthorInfoTest.TEST_01 + @"
          </authors>
";

    [Test]
    public void Test_01()
    {
      DoTest(
        TEST_01,
        delegate(XmlElement root)
          {
            OrganizaionTest.LoadMockOrgs(myLoader);
            IAuthor list = myLoader.ParseAuthor(root);
            Assert_Test_01(list);
          });
    }

    public static void Assert_Test_01(IAuthor list)
    {
      Assert.AreEqual(1, list.JournalLanguages.Count);
      AuthorInfoTest.AssertXml_Test_01(list.ForLanguage(JournalLanguage.EN));
    }
  }
}