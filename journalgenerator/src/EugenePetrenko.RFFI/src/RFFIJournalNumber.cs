using System.Collections.Generic;

namespace EugenePetrenko.RFFI
{
  [XmlRoot("journals"), XmlElementPath("journal")]
  public class RFFIJournalNumber
  {
    private readonly RFFIIssue myIssue;

    public RFFIJournalNumber(RFFIIssue issue)
    {
      myIssue = issue;
    }

    [XmlElementPath("journalInfo", Clone = true), XmlForeach]
    public IEnumerable<JournalInfoBean> JournalInfo
    {
      get { return JournalInfoBean.BEANS; }
    }

    [XmlElementPath("issue")]
    public RFFIIssue Issue
    {
      get { return myIssue; }
    }

    [XmlElementPath("jrncode"), XmlText, XmlStaticAttribute("jcountry", "ru")]
    public string JournalCode { get { return "123456"; } }

    [XmlElementPath("issn"), XmlText]
    public string JournalISSN { get { return "1817-2172";}}
  }
}