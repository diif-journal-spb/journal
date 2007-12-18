using System;
using System.Collections.Generic;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
  public class RFFIIssue
  {
    private readonly INumber myNumber;

    public RFFIIssue(INumber number)
    {
      myNumber = number;
    }

    [XmlElementPath("jyear"), XmlText]
    public string JournalYear { get { return myNumber.Year; } }

    [XmlElementPath("jdateUni"), XmlText]
    public string JournalDateUni { get { return myNumber.Year + (myNumber.IntNumber*3-2) + "/" + (myNumber.IntNumber*3); } }

    [XmlElementPath("pages"), XmlText]
    public string JournalPages { get { return "1-" + GetPages(); } }

    [XmlElementPath("article", Clone = true), XmlForeach]
    public IEnumerable<RFFIArticle> Articles
    {
      get
      {
        foreach (IArticle article in myNumber.Articles)
        {
          yield return new RFFIArticle(article);
        }
      }
    }
    
    private int GetPages()
    {
      int sum = 0;
      foreach (IArticle article in myNumber.Articles)
      {        
        foreach (IArticleInfo info in article.AllLanguages())
        {
          sum = Math.Max(sum, info.LastPage);
        }        
      }
      return sum;
    }
  }
}