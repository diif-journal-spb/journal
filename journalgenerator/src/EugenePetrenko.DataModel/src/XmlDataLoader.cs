using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public class XmlDataLoader : IXmlDataLoader
  {
    private readonly string myPath;
    private readonly EntityGenerator myEntityGenerator = new EntityGenerator();
    private readonly List<IAuthor> myAuthors = new List<IAuthor>();
    protected readonly IXmlDataLoader myLoader;
    private readonly IJournal myJournal;

    IAuthorInfo IXmlDataLoader.ParseAuthorInfo(XmlElement element, IAuthor host)
    {
      return new AuthorInfo(element, this, host);
    }

    IAuthor IXmlDataLoader.ParseAuthor(XmlElement element)
    {
      return new Author(element, this);
    }

    List<IAuthor> IXmlDataLoader.ParseAuthors(XmlElement element)
    {
      var result = new List<IAuthor>();
      foreach (XmlElement node in element.SelectNodes("author"))
      {
        result.Add(myLoader.ParseAuthor(node));
      }
      return result;
    }

    IArticleInfo IXmlDataLoader.ParseArticleInfo(IArticle article, XmlElement element)
    {
      return new ArticleInfo(article, element, this);
    }

    INumber IXmlDataLoader.ParseNumber(XmlElement element)
    {
      return new NumberImpl(element, this);
    }

    IArticle IXmlDataLoader.ParseArticle(XmlElement element)
    {
      return new Article(element, this);
    }

    JournalLanguage IXmlDataLoader.ParseLanguage(XmlElement element)
    {
      switch(element.GetAttribute("lang"))
      {
        case "RU":
          return JournalLanguage.RU;
        case "EN":
          return JournalLanguage.EN;
      }
      throw new ArgumentException("Unexpected language");
    }

    public DateTime ParseDateTime(string text)
    {
      var regex = new Regex(@"(\d{4})\.(\d{1,2})\.(\d{1,2})");
      Match match = regex.Match(text);
      int year = int.Parse(match.Groups[1].Value);
      int month = int.Parse(match.Groups[2].Value);
      int day = int.Parse(match.Groups[3].Value);

      return new DateTime(year, month, day);
    }

    EntityGenerator IXmlDataLoader.EntityGenerator
    {
      get { return myEntityGenerator; }
    }

    IAuthor IXmlDataLoader.LookupAuthor(string id)
    {
      foreach (IAuthor author in myAuthors)
      {
        if (author.Id == id)
          return author;
      }
      throw new ArgumentException("Author was not found: '" + id + "'");
    }

    protected XmlDataLoader()
    {
      myLoader = this;
    }

    private delegate void DForeachXml(XmlDocument doc);
    private void ForeachXml(string mask, DForeachXml del)
    {
      foreach (string file in Directory.GetFiles(myPath, mask))
      {
        try {
          var doc = new XmlDocument();
          doc.Load(file);
          del(doc);
        } catch(Exception e) {
          throw new Exception("File " + file + ", " + e.Message, e);
        }
      }
    }

    public XmlDataLoader(string path) : this()
    {    
      var myNumbers = new List<INumber>();

      myPath = path;
      ForeachXml("*.authors",
                 doc => myAuthors.AddRange(myLoader.ParseAuthors(doc.DocumentElement)));

      ForeachXml("*.number",
                 delegate(XmlDocument doc)
                   {
                     foreach (XmlElement node in doc.SelectNodes("//authors-xml"))
                     {
                       myAuthors.AddRange(myLoader.ParseAuthors(node));
                     }                     
                   });

      ForeachXml("*.number",
                 doc => myNumbers.Add(myLoader.ParseNumber(doc.DocumentElement)));     

      var news = new List<INewsItem>();
      ForeachXml("*.news", delegate(XmlDocument doc)
                             {
                               foreach (XmlElement element in doc.SelectNodes("news/item"))
                               {
                                 news.Add(new NewsItemImpl(element, this));
                               }
                             });
      news.Sort((x, y) => x.Date.CompareTo(y.Date));
      
      var books = new List<IBook>();
      ForeachXml("*.books", delegate(XmlDocument doc)
                              {
                                foreach (XmlElement element in doc.SelectNodes("books/book"))
                                {
                                  books.Add(new BookImpl(element, this));
                                }
                              });
      books.Sort((x, y) => x.Date.CompareTo(y.Date));

      Func<INumber, string> cms = x => x.Year + "|" + x.Number;
      myNumbers.Sort((a,b) => cms(a).CompareTo(cms(b)));

      myJournal = new Journal(myEntityGenerator, myNumbers.ToArray(), myAuthors.ToArray(), news.ToArray(), books.ToArray());
    }

    public IJournal Journal
    {
      get { return myJournal; }
    }

    void IXmlDataLoader.LoadAuthors(XmlDocument doc)
    {
      myAuthors.AddRange(myLoader.ParseAuthors(doc.DocumentElement));
    }

    public static IJournal Parse(string path)
    {
        var loader = new XmlDataLoader(path);
      return loader.Journal;
    }
  }
}