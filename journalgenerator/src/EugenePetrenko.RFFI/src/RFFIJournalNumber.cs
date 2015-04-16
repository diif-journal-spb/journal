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

    [XmlElementPath("operator"), XmlText] public string Operator {get { return "jonnyzzz"; }}
    [XmlElementPath("pid"), XmlText] public string Pid { get { return "42"; } }
    [XmlElementPath("date"), XmlText] public string Date { get { return DateTime.UtcNow.ToString(CultureInfo.InvariantCulture); } }
    [XmlElementPath("cntArticle"), XmlText] public string CntArticle { get { return "" + myCount; } }
    [XmlElementPath("cntNode"), XmlText] public string CntNode { get { return "0\r\n"; } }
    [XmlElementPath("cs"), XmlText] public string CS { get { return "0"; } }
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
    public RFFIOperCard OperCard { get { return new RFFIOperCard(myIssue.Articles.Count()); } }

    [XmlElementPath("titleid"), XmlText]
    public string TitleId {get { return "25189"; }}

    [XmlElementPath("issn"), XmlText]
    public string JournalISSN { get { return "1817-2172";}}

    [XmlElementPath("codeNEB"), XmlText]
    public string codeNEB { get { return "2072909X"; } }
   
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

    [XmlIgnore, XmlElementPath("jrncode"), XmlText, XmlStaticAttribute("jcountry", "ru")]
    public string JournalCode { get { return "123456"; } }

    public int TotalPages
    {
      get { return myIssue.JournalTotalPages; }
    }

/*
    public void RegisterPdfDownload(Action<IArticleInfo, string> myPdfManager)
    {
      foreach (var article in Issue.Articles)
      {
        IArticle art = article.Article;
        myPdfManager(art.ForLanguage(JournalLanguage.RU), article.Article.AllLanguages().Select(x=>x.Pdf));
      }
    }
*/
  }
}