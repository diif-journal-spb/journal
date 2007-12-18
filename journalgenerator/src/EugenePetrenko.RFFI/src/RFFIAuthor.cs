using System.Collections.Generic;
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

    [XmlElementPath("individual", "individInfo", CloneData = new bool[]{false, true}), XmlForeach]
    public IEnumerable<RFFIAuthorInfo> Authors
    {
      get
      {
        IAuthorInfo en = myAuthor.ForLanguage(JournalLanguage.EN);
        yield return new RFFIAuthorInfo("eng", en.LastName, en.FirstName + " " + en.MiddleName, en.Address, en.EMail);
        IAuthorInfo ru = myAuthor.ForLanguage(JournalLanguage.RU);
        yield return new RFFIAuthorInfo("eng", ru.LastName, ru.FirstName + " " + ru.MiddleName, ru.Address, ru.EMail);
      }
    }

    [XmlAttribute("authornum")]
    public string Id
    {
      get { return myAuthorId.ToString(); }
    }
    
  }
}