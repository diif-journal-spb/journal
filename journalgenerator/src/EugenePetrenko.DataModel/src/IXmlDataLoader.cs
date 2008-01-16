using System;
using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IXmlDataLoader
  {
    IAuthorInfo ParseAuthorInfo(XmlElement element, IAuthor host);
    IAuthor ParseAuthor(XmlElement element);
    List<IAuthor> ParseAuthors(XmlElement element);

    IArticleInfo ParseArticleInfo(IArticle article, XmlElement element);
    INumber ParseNumber(XmlElement element);
    IArticle ParseArticle(XmlElement element);
    JournalLanguage ParseLanguage(XmlElement element);
    DateTime ParseDateTime(string text);

    EntityGenerator EntityGenerator { get; }

    IAuthor LookupAuthor(string id);
    void LoadAuthors(XmlDocument doc);
  }
}