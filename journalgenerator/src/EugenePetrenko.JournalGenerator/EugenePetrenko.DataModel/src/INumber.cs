using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface INumber : IEntity
  {
    string Year { get; }
    int IntYear { get; }
    string Number { get; }
    int IntNumber { get; }

    IArticle[] Articles { get; }
  }

  public class NumberImpl : Entity, INumber
  {
    private string myYear;
    private string myNumber;
    private IArticle[] myArticles;

    public NumberImpl(XmlElement element, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myYear = element.GetAttribute("year");
      myNumber = element.GetAttribute("number");
      List<IArticle> articles = new List<IArticle>();
      foreach (XmlElement node in element.SelectNodes("article"))
      {
        articles.Add(loader.ParseArticle(node));
      }
      myArticles = articles.ToArray();
    }

    public string Year
    {
      get { return myYear; }
    }

    public int IntYear
    {
      get { return int.Parse(myYear); }
    }

    public string Number
    {
      get { return myNumber; }
    }

    public int IntNumber
    {
      get { return int.Parse(myNumber); }
    }

    public IArticle[] Articles
    {
      get { return myArticles; }
    }
  }
}