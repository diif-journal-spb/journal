using System;
using NUnit.Framework;

namespace EugenePetrenko.NumberEditor
{
  [TestFixture]
  public class SerializationTest
  {
    
    [Test]
    public void articleFull_empty()
    {
      var art = new LocalizedArticleXml();
      Console.Out.WriteLine("art.Serialize() = {0}", art.Serialize());
    }

    [Test]
    public void articleFull_refs()
    {
      var art = new LocalizedArticleXml();
      art.AddAuthor(new AuthorRef{Ref = "n20_44", SortKey = "1"});
      Console.Out.WriteLine("art.Serialize() = {0}", art.Serialize());
    }

    [Test]
    public void articleFull_num()
    {
      var art = new LocalizedArticleXml
                  {
                    RU = new ArticleInfoXml
                           {
                             Abstract = "aaa",
                             FirstPage = 1,
                             LastPage = 43,
                             Lang = "RU",
                             Pdf = "pdf",
                             Title = "title"
                           }
                  };

      Console.Out.WriteLine("art.Serialize() = {0}", art.Serialize());
    }

    [Test]
    public void articleFull_num2()
    {
      var art = new LocalizedArticleXml
                  {
                    RU = new ArticleInfoXml
                           {
                             Abstract = "aaa",
                             FirstPage = 1,
                             LastPage = 43,
                             Lang = "RU",
                             Pdf = "pdf",
                             Title = "title"
                           },
                           EN = new ArticleInfoXml
                           {
                             Abstract = "affaa",
                             FirstPage = 1,
                             LastPage = 43,
                             Lang = "EN",
                             Pdf = "pd3f",
                             Title = "ti3tle"
                           }
                  };

      Console.Out.WriteLine("art.Serialize() = {0}", art.Serialize());
    }

    [Test]
    public void autrhorFull()
    {
      var art = new LocalizedAuthorXml();

      Console.Out.WriteLine("art.Serialize() = {0}", art.Serialize());
    }

    [Test]
    public void autrhorFull_EN()
    {
      var art = new LocalizedAuthorXml
                  {
                    EN = new AuthorXml
                           {
                             FirstName = "aq",
                             MiddleName = "B",
                             LastName = "Q!",
                             Address = "wer",
                             Email = "eee",
                             Lang = "EN",
                           }
                  };

      Console.Out.WriteLine("art.Serialize() = {0}", art.Serialize());
    }
  }
}