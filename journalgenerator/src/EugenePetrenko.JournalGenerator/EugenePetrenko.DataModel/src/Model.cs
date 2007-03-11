/*
 * Created by: 
 * Created: 10 марта 2007 г.
 */

using System;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public class XmlDataLoader
  {
    private readonly string myPath;
    private EntityGenerator myEntityGenerator;

    public IAuthorInfo ParseAuthorInfo(XmlElement element)
    {
      return new AuthorInfo(element, this);
    }

    public IArticleInfo ParseArticleInfo(XmlElement element)
    {
      return new ArticleInfo(element, this);
    }

    public INumber ParseNumber(XmlElement element)
    {
      return new Number(element, this);
    }

    public IArticle ParseArticle(XmlElement element)
    {
      return new Article(element, this);
    }

    public JournalLanguage ParseLanguage(XmlElement element)
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

    public EntityGenerator EntityGenerator
    {
      get { return myEntityGenerator; }
    }
  }
}