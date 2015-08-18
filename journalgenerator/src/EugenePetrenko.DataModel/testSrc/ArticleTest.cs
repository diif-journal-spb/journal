using System.Xml;
using NUnit.Framework;

namespace EugenePetrenko.DataModel
{
  [TestFixture]
  public class ArticleTest : BaseTest
  {
    [Test]
    public void Test_01()
    {
      DoTest(@"<article>" + ArticleInfoTest.TEST_01 + 
                     "<authors>" + AuthorTest.TEST_01 + "</authors>" + 
              "</article>", 
             delegate(XmlElement root)
               {
                 OrganizaionTest.LoadMockOrgs(myLoader);
                 IArticle art = myLoader.ParseArticle(root);

                 Assert.AreEqual(1, art.JournalLanguages.Count);

                 IArticleInfo info = art.ForLanguage(JournalLanguage.RU);
                 ArticleInfoTest.Assert_Test_01(info);
               }); 
    }

    [Test]
    public void Test_02()
    {
      DoTest(@"<article>" + ArticleInfoTest.TEST_01 +
                     "<authors>" + AuthorTest.TEST_01 + "</authors>" +
              "</article>",
             delegate(XmlElement root)
             {
               OrganizaionTest.LoadMockOrgs(myLoader);

               myLoader.LoadAuthors(root.OwnerDocument);

               IArticle art = myLoader.ParseArticle(root);

               Assert.AreEqual(1, art.JournalLanguages.Count);

               IArticleInfo info = art.ForLanguage(JournalLanguage.RU);
               ArticleInfoTest.Assert_Test_01(info);
             });
    }

  }
}