using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface INumber : IEntity
  {
    string Year { get; }
    string Number { get; }

    IArticle[] Articles { get; }
  }

  public class Number : Entity, INumber
  {
    private string myYear;
    private string myNumber;
    private IArticle[] myArticles;

    public Number(XmlElement element, XmlDataLoader loader) : base(loader.EntityGenerator)
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

    string INumber.Year
    {
      get { return myYear; }
    }

    string INumber.Number
    {
      get { return myNumber; }
    }

    IArticle[] INumber.Articles
    {
      get { return myArticles; }
    }
  }
}