namespace EugenePetrenko.RFFI
{
  public class JournalInfoBean
  {
    private readonly string myLang;
    private readonly string myTitle;
    private readonly string myPublisher;
    private readonly string myPublisherPlace;
    private readonly string myPublisherLocation;

    private JournalInfoBean(string lang, string title, string publisher, string publisherPlace, string publisherLocation)
    {
      myLang = lang;
      myTitle = title;
      myPublisher = publisher;
      myPublisherPlace = publisherPlace;
      myPublisherLocation = publisherLocation;
    }

    [XmlAttribute("lang")]
    [XmlAttribute("fieldlang")]
    public string Lang
    {
      get { return myLang; }
    }

    [XmlElementPath("jrntitle"), XmlText]
    public string JournalTitle
    {
      get { return myTitle; }
    }
    
    [XmlElementPath("publ"), XmlText]
    public string Publisher
    {
      get { return myPublisher; }
    }

    [XmlElementPath("placepubl"), XmlText]
    public string PublishingPlace
    {
      get { return myPublisherPlace; }
    }

    [XmlElementPath("loc"), XmlText]
    public string PlublisherLocation
    {
      get { return myPublisherLocation; }
    }

    private static readonly JournalInfoBean RUS = new JournalInfoBean(
      "rus", 
      "Дифференциальные Уравнения и Процессы Управления", 
      "Санкт-Петербургский государственный университет", 
      "Санкт-Петербург", 
      "198504, Россия, Санкт-Петербург, Старый Петергоф, Университетский пр., дом 28. тел: (812) 428-4210, факс: (812) 428-6944"
      );

    private static readonly JournalInfoBean ENG = new JournalInfoBean(
      "eng", 
      "Electronic Journal Difference Equations and Control Processes", 
      "Saint-Petersburg State University",
      "Saint Petersburg", 
      "Universitetsky prospekt, 28, 198504, Peterhof, St. Petersburg, Russia. Phone: +7 (812) 428-4210, Fax: +7 (812) 428-6944");

    public static readonly JournalInfoBean[] BEANS = {RUS, ENG};
  }
}