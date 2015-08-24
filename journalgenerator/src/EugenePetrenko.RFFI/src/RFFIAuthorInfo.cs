namespace EugenePetrenko.RFFI
{
  public class RFFIAuthorInfo
  {
    private readonly string myLang;
    private readonly string mySurname;
    private readonly string myFName;
    private readonly string myauwork;
    private readonly string myAuworkaddr;
    private readonly string myauemail;


    public RFFIAuthorInfo(string lang, string surname, string fName, string auwork, string auworkaddr, string auemail)
    {
      myLang = lang;
      mySurname = surname;
      myFName = fName;
      myauwork = auwork;
      myAuworkaddr = auworkaddr;
      myauemail = auemail;
    }

    [XmlAttribute("lang")]
    public string Lang
    {
      get { return myLang; }
    }

    [XmlElementPath("surname"), XmlText]
    public string Surname
    {
      get { return mySurname.Trim(); }
    }

    [XmlElementPath("initials"), XmlText]
    public string FName
    {
      get { return myFName.Trim(); }
    }

    [XmlElementPath("orgName"), XmlText]
    public string Auwork
    {
      get { return myauwork; }
    }

    [XmlElementPath("email"), XmlText]
    public string Auemail
    {
      get { return myauemail; }
    }

    [XmlElementPath("address"), XmlText]
    public string AuworkAddr
    {
      get { return myAuworkaddr; }
    }
  }
}
