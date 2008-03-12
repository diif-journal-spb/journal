using System;
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

    [Obsolete]
    IArticle[] Articles { get; }

    INumberSection[] Sections { get; }
  }

  public class NumberImpl : Entity, INumber
  {
    private readonly string myYear;
    private readonly string myNumber;
    private readonly INumberSection[] mySections;

    public NumberImpl(XmlElement element, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myYear = element.GetAttribute("year").Trim();
      myNumber = element.GetAttribute("number").Trim();

      List<INumberSection> sections = new List<INumberSection>();
      //todo: Load list of types from XML
      foreach (INumberSectionFactory factory in new INumberSectionFactory[] { new PublicationsNumberFactory(), new BooksNumberFactory()})
      {
        List<IArticle> articles = new List<IArticle>();
        foreach (XmlElement node in element.SelectNodes(factory.ElementName))
        {
          articles.Add(loader.ParseArticle(node));
        }

        if (articles.Count > 0)
        {
          sections.Add(factory.Create(articles.ToArray()));
        }
      }
      
      mySections = sections.ToArray();
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
      get { return new PublicationsNumberFactory().Filter(Sections); }
    }

    public INumberSection[] Sections
    {
      get { return mySections; }
    }
  }
}