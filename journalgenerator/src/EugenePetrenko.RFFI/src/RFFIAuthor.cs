using System.Collections.Generic;
using System.Linq;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
  public class RFFIAuthor
  {
    private readonly IAuthor myAuthor;
    private readonly int myAuthorId;

    public RFFIAuthor(int num, IAuthor author)
    {
      myAuthorId = num;
      myAuthor = author;
    }

    [XmlElementPath("individInfo", Clone = true), XmlForeach]
    public IEnumerable<RFFIAuthorInfo> Authors
    {
      get
      {
        return
          myAuthor.AllLanguages().Select(
            en => new RFFIAuthorInfo(
              en.JournalLanguage.Lang(), 
              en.LastName, 
              en.FirstName + " " + en.MiddleName, 
              en.Address.FilterXml(),
              en.EMail)
              );
      }
    }

    [XmlAttribute("num")]
    public string Id
    {
      get { return myAuthorId.ToString(); }
    }
  }
}