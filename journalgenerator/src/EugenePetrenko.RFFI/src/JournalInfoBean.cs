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

    private static readonly JournalInfoBean RUS = new JournalInfoBean("rus", "Дифференциальные Уравнения и Процессы Управления", "Санкт-Петербургский государственный технический университет", "Санкт-Петербург", @"Дифференциальные уравнения и процессы управления.""Лаборатория ""Нелинейный анализ и математическое моделирование"", Главный корпус, к. 125, Санкт-Петербургский государственный технический университет, ул. Политехническая, д. 29, 195251, Санкт-Петербург, Россия");
    private static readonly JournalInfoBean ENG = new JournalInfoBean("eng", "Electronic Journal Difference Equations and Control Processes", "Saint-Petersburg State Technical University", "Saint Petersburg", @"State Publishing Committee of the Russian Federation North-western Regional Department, Saint-Petersburg, Registration number: P 23275 on March 7, 1997");

    public static readonly JournalInfoBean[] BEANS = {RUS, ENG};
  }
}