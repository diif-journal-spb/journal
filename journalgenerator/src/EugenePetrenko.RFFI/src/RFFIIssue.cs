using System;
using System.Collections.Generic;
using System.Linq;
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

    [XmlElementPath("volume"), XmlText] 
    public string Volume {get { return myNumber.Year; }}

    [XmlElementPath("number"), XmlText]
    public string Number { get { return myNumber.Year + "-" + myNumber.Number; } }

    [XmlElementPath("altNumber"), XmlText] 
    public string AltNumber {get { return myNumber.Number; }}

    [XmlElementPath("part"), XmlText] 
    public string Part {get { return myNumber.Number; }}

    [XmlElementPath("dateUni"), XmlText]
    public string JournalDateUni { get { return myNumber.Year + (myNumber.IntNumber*3-2) + "/" + (myNumber.IntNumber*3); } }

    [XmlElementPath("issTitle"), XmlText]
    public string IssnTitle { get { return myNumber.Year + " #" + myNumber.Number; } }

    [XmlElementPath("pages"), XmlText]
    public string JournalPages { get { return "1-" + GetPages(); } }

    [XmlElementPath("articles", "article", CloneData = new []{false, true}), XmlForeach]
    public IEnumerable<RFFIArticle> Articles
    {
      get 
      {
        return Articlez.Select(article => new RFFIArticle(article)).ToArray();
      }
    }

    private IEnumerable<IArticle> Articlez
    {
      get
      {
        return new PublicationsNumberFactory().Filter(myNumber.Sections);
      }
    }
    
    private int GetPages()
    {
      return Articlez.SelectMany(x => x.AllLanguages()).Max(x => x.LastPage);
    }
  }
}