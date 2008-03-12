using NUnit.Framework;

namespace EugenePetrenko.JournalGenerator.Test
{
  [TestFixture]
  public class LinkManagerTest
  {
    private static void DoTest(string basePath, string path, string result)
    {
      Assert.AreEqual(result, LinkManager.MakeRelative(path, basePath));      
    }

    [Test]
    public void Test_01()
    {
      DoTest("a/b", "a/b/c/d", "c/d");
    }
    
    [Test]
    public void Test_02()
    {
      DoTest("a/b", "a/b", "b");
    }
    
    [Test]
    public void Test_03()
    {
      DoTest("a/b/c", "a/q", "../q");
    }
    
    [Test]
    public void Test_04()
    {
      DoTest("a/b/c/d", "a/b/e/f", "../e/f");      
    }
  }
}
