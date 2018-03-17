using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EugenePetrenko.RFFI
{
  [XmlRoot("operCard")]
  public class RFFIOperCard
  {
    private readonly int myCount;

    public RFFIOperCard(int count)
    {
      myCount = count;
    }

    [XmlElementPath("operator"), XmlText] public string Operator => "jonnyzzz";
    [XmlElementPath("pid"), XmlText] public string Pid => "42";
    [XmlElementPath("date"), XmlText] public string Date => DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
    [XmlElementPath("cntArticle"), XmlText] public string CntArticle => "" + myCount;
    [XmlElementPath("cntNode"), XmlText] public string CntNode => "0\r\n";
    [XmlElementPath("cs"), XmlText] public string CS => "0";
  }

  [XmlRoot("journal")]
  public class RFFIJournalNumber
  {
    private readonly RFFIIssue myIssue;

    public RFFIJournalNumber(RFFIIssue issue)
    {
      myIssue = issue;
    }
    
    [XmlElementPath("operCard"), XmlText]
    public RFFIOperCard OperCard => new RFFIOperCard(myIssue.Articles.Select(x=>x.Articles.Count).Sum());

    [XmlElementPath("titleid"), XmlText]
    public string TitleId => "7282";

    [XmlElementPath("issn"), XmlText]
    public string JournalISSN => "1817-2172";

    [XmlElementPath("codeNEB"), XmlText]
    public string codeNEB => "18172172";

    [XmlElementPath("journalInfo", Clone = true), XmlForeach]
    public IEnumerable<JournalInfoBean> JournalInfo => JournalInfoBean.BEANS;

    [XmlElementPath("issue")]
    public RFFIIssue Issue => myIssue;

    [XmlIgnore, XmlElementPath("jrncode"), XmlText, XmlStaticAttribute("jcountry", "ru")]
    public string JournalCode => "123456";

    public int TotalPages => myIssue.JournalTotalPages;
  }
}
