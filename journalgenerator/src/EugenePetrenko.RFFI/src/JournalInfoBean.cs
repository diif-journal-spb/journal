namespace EugenePetrenko.RFFI
{
  public class JournalInfoBean
  {
    private readonly string myLang;
    private readonly string myTitle;
    private readonly string myPublisherPlace;
    private readonly string myPublisherLocation;

    private JournalInfoBean(string lang, string title, string publisher, string publisherPlace, string publisherLocation)
    {
      myLang = lang;
      myTitle = title;
      Publisher = publisher;
      myPublisherPlace = publisherPlace;
      myPublisherLocation = publisherLocation;
    }

    [XmlAttribute("lang")]
    public string Lang => myLang;

    [XmlElementPath("title"), XmlText]
    public string JournalTitle => myTitle;

    [XmlIgnore, XmlElementPath("publ"), XmlText]
    public string Publisher { get; }

    [XmlIgnore, XmlElementPath("placepubl"), XmlText]
    public string PublishingPlace => myPublisherPlace;

    [XmlIgnore, XmlElementPath("loc"), XmlText]
    public string PlublisherLocation => myPublisherLocation;

    private static readonly JournalInfoBean RUS = new JournalInfoBean(
      "RUS", 
      "Дифференциальные Уравнения и Процессы Управления", 
      "Санкт-Петербургский государственный университет", 
      "Санкт-Петербург", 
      "198504, Россия, Санкт-Петербург, Старый Петергоф, Университетский пр., дом 28. тел: (812) 428-4210, факс: (812) 428-6944"
      );

    private static readonly JournalInfoBean ENG = new JournalInfoBean(
      "ENG", 
      "Differential Equations and Control Processes", 
      "Saint-Petersburg State University",
      "Saint Petersburg", 
      "Universitetsky prospekt, 28, 198504, Peterhof, St. Petersburg, Russia. Phone: +7 (812) 428-4210, Fax: +7 (812) 428-6944");

    public static readonly JournalInfoBean[] BEANS = {RUS, ENG};
  }
}
