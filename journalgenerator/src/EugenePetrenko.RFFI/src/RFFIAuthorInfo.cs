namespace EugenePetrenko.RFFI
{
  public class RFFIAuthorInfo
  {
    private readonly string mySurname;
    private readonly string myFName;
    private readonly string myauwork;
    private readonly string myAuworkaddr;
    private readonly string myauemail;


    public RFFIAuthorInfo(string lang, string surname, string fName, string auwork, string auworkaddr, string auemail)
    {
      Lang = lang;
      mySurname = surname;
      myFName = fName;
      myauwork = auwork;
      myAuworkaddr = auworkaddr;
      myauemail = auemail;
    }

    [XmlAttribute("lang")]
    public string Lang { get; }

    [XmlElementPath("surname"), XmlText]
    public string Surname => mySurname.Trim();

    [XmlElementPath("initials"), XmlText]
    public string FName => myFName.Trim();

    [XmlElementPath("orgName"), XmlText]
    public string Auwork => myauwork;

    [XmlElementPath("email"), XmlText]
    public string Auemail => myauemail;

    [XmlElementPath("address"), XmlText]
    public string AuworkAddr => myAuworkaddr;
  }
}
