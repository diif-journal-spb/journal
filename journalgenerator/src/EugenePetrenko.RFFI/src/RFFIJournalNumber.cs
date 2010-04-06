using System;
using System.Collections.Generic;
using EugenePetrenko.DataModel;

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

    public void RegisterPdfDownload(Action<IArticleInfo, string> myPdfManager)
    {
      foreach (var article in Issue.Articles)
      {
        IArticle art = article.Article;
        myPdfManager(art.ForLanguage(JournalLanguage.RU), article.Pdf);
      }
    }
  }
}