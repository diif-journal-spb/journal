using NUnit.Framework;

namespace EugenePetrenko.DataModel
{
  [TestFixture]
  public class OrganizaionTest : BaseTest
  {
    public static void LoadMockOrgs(IXmlDataLoader loader)
    {
      DoTest("<org-xml id='111'><org lang='RU'><name>Name</name><address>Address</address></org></org-xml>",
        root => loader.ParseOrganization(root));
    }

    [Test]
    public void Test_01()
    {
      DoTest("<org-xml id='111'><org lang='RU'><name>Name</name><address>Address</address></org></org-xml>",
        root =>
        {
          var org = myLoader.ParseOrganization(root);

          Assert.AreEqual("111", org.Id);
          Assert.AreEqual("Name", org.ForLanguage(JournalLanguage.RU).Name);
          Assert.AreEqual("Address", org.ForLanguage(JournalLanguage.RU).Address);
        });


    }
  }
}
