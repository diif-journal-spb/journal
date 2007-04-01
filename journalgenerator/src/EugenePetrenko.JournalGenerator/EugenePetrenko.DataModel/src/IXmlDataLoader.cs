using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IXmlDataLoader
  {
    IAuthorInfo ParseAuthorInfo(XmlElement element);
    IAuthor ParseAuthor(XmlElement element);
    List<IAuthor> ParseAuthors(XmlElement element);

    IArticleInfo ParseArticleInfo(IArticle article, XmlElement element);
    INumber ParseNumber(XmlElement element);
    IArticle ParseArticle(XmlElement element);
    JournalLanguage ParseLanguage(XmlElement element);

    EntityGenerator EntityGenerator { get; }

    IAuthor LookupAuthor(string id);
    void LoadAuthors(XmlDocument doc);
  }
}