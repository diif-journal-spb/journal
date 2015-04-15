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

    [XmlIgnore]
    public INumber NumberInternal { get { return myNumber; }}

    [XmlElementPath("volume"), XmlText] 
    public string Volume {get { return myNumber.Year; }}

    [XmlElementPath("number"), XmlText]
    public string Number { get { return myNumber.Number; } }

    [XmlElementPath("altNumber"), XmlText] 
    public string AltNumber {get { return myNumber.Number; }}

    [XmlElementPath("part"), XmlText] 
    public string Part {get { return myNumber.Number; }}

    [XmlElementPath("dateUni"), XmlText]
    public string JournalDateUni {
      get
      {
        switch (myNumber.IntNumber)
        {
          case 1:
            return myNumber.Year + "31/" + myNumber.Year;
          case 2:
            return myNumber.Year + "32/" + myNumber.Year;
          case 3:
            return myNumber.Year + "33/" + myNumber.Year;
          case 4:
            return myNumber.Year + "32/" + myNumber.Year;
          default:
            return myNumber.Year + "/" + myNumber.Year;
        }
      } 
    }

    [XmlElementPath("issTitle"), XmlText]
    public string IssnTitle { get { return myNumber.Year + "." + myNumber.Number; } }

    [XmlElementPath("pages"), XmlText]
    public string JournalPages { get { return "1-" + GetPages(); } }

    [XmlIgnore]
    public int JournalTotalPages { get { return GetPages(); }}

    [XmlElementPath("articles", "article", CloneData = new []{false, true}), XmlForeach]
    public IEnumerable<RFFIArticle> Articles
    {
      get 
      {
        return Articlez.Select(article => new RFFIArticle(this, article)).ToArray();
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