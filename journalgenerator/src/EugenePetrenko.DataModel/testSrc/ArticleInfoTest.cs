using System.Xml;
using NUnit.Framework;

namespace EugenePetrenko.DataModel
{
  [TestFixture]
  public class ArticleInfoTest : BaseTest
  {
    public const string TEST_01 = @"
               <articleInfo lang=""RU"" FirstPage=""10"" LastPage=""20"">
                 <pdf>ddd.pdf</pdf>
                 <title>title123</title>
                 <abstract>This is a mega cool article</abstract>
               </articleInfo>";

    [Test]
    public void Test_01()
    {
      DoTest(
        TEST_01,
        delegate(XmlElement root)
        {
          IArticleInfo info = myLoader.ParseArticleInfo(null, root);
          Assert_Test_01(info);
        }); 
    }

    public static void Assert_Test_01(IArticleInfo info)
    {
      Assert.AreEqual(0, info.Authors.Length);
      Assert.AreEqual("ddd.pdf", info.Pdf);
      Assert.AreEqual("This is a mega cool article", info.Abstract);
      Assert.AreEqual("title123", info.Title);
      Assert.AreEqual(10, info.FirstPage);
      Assert.AreEqual(20, info.LastPage);
      Assert.AreEqual(JournalLanguage.RU, info.JournalLanguage);      
    }
  }
}