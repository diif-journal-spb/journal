using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;

namespace EugenePetrenko.DataModel
{
  [TestFixture]
  public class AuthorsTest : BaseTest
  {
    [Test]
    public void Test_01()
    {
      DoTest(
        @"
          <authors>
             <author org='111'>
             " + AuthorInfoTest.TEST_01 + @"
             </author>
          </authors>
",
        delegate(XmlElement root)
          {
            OrganizaionTest.LoadMockOrgs(myLoader);

            List<IAuthor> list = myLoader.ParseAuthors(root);
            Assert.AreEqual(1, list.Count);

            AuthorTest.Assert_Test_01(list[0]);
          });
    }
  }
}
