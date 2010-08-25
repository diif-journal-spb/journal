using System.Collections.Generic;
using System.Linq;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
  public class RFFIAuthor
  {
    private static int myAuthorCount = 1;

    private readonly IAuthor myAuthor;
    private readonly int myAuthorId = myAuthorCount++;

    public RFFIAuthor(IAuthor author)
    {
      myAuthor = author;
    }

    [XmlElementPath("individual", "individInfo", Clone = true), XmlForeach]
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

    [XmlAttribute("authornum")]
    public string Id
    {
      get { return myAuthorId.ToString(); }
    }
  }
}