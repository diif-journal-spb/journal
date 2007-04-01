using System.Xml;
using NUnit.Framework;

namespace EugenePetrenko.DataModel
{
  [TestFixture]
  public class JournalLanguageTest : BaseTest
  {
    [Test]
    public void Test_01()
    {
      DoTest("<a lang=\"EN\"/>",
             delegate(XmlElement root) { Assert.AreEqual(JournalLanguage.EN, myLoader.ParseLanguage(root)); });
    }

    [Test]
    public void Test_02()
    {
      DoTest("<a lang=\"RU\"/>",
             delegate(XmlElement root) { Assert.AreEqual(JournalLanguage.RU, myLoader.ParseLanguage(root)); });
    }
  }
}