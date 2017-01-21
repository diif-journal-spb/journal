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

    INumberSection[] Sections { get; }
  }

  public class NumberImpl : Entity, INumber
  {
    private readonly string myYear;
    private readonly string myNumber;

    public NumberImpl(XmlElement element, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myYear = element.GetAttribute("year").Trim();
      myNumber = element.GetAttribute("number").Trim();

      var sections = new List<INumberSection>();
      //todo: Load list of types from XML
      var factories = new INumberSectionFactory[]
                        {
                          new PublicationsNumberFactory(),
                          new BooksNumberFactory(),
                          new PhdNumberFactory(),
                          new MonographNumberFactory(),
                          new ConfNumberFactory(),
                        };
      foreach (var factory in factories)
      {
        var articles = new List<IArticle>();
        // ReSharper disable once PossibleNullReferenceException
        foreach (XmlElement node in element.SelectNodes(factory.ElementName))
        {
          articles.Add(loader.ParseArticle(node));
        }

        if (articles.Count > 0)
        {
          sections.Add(factory.Create(articles.ToArray()));
        }
      }

      Sections = sections.ToArray();
    }

    public string Year => myYear;

    public int IntYear => int.Parse(myYear);

    public string Number => myNumber;

    public int IntNumber => int.Parse(myNumber);

    public INumberSection[] Sections { get; }

    public override string ToString()
    {
      return "Number " + Number + "." + Year;
    }
  }
}