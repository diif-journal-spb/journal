namespace EugenePetrenko.RFFI
{
  public class RFFIArtType
  {
    public readonly string Type;

    private RFFIArtType(string type, string description)
    {
      Type = type;
    }

    public static RFFIArtType RAR = new RFFIArtType("RAR", "научная статья");
    public static RFFIArtType EDI = new RFFIArtType("EDI", "редакторская заметка");
    public static RFFIArtType BRV = new RFFIArtType("BRV", "рецензия");
    public static RFFIArtType CNF = new RFFIArtType("CNF", "материалы конференции");
    public static RFFIArtType THS = new RFFIArtType("THS", "тезисы доклада на конференции");
    public static RFFIArtType SCO = new RFFIArtType("SCO", "краткое сообщение");
    public static RFFIArtType REV = new RFFIArtType("REV", "обзорная статья");
    public static RFFIArtType ABS = new RFFIArtType("ABS", "аннотация");
    public static RFFIArtType REP = new RFFIArtType("REP", "научный отчет");
    public static RFFIArtType COR = new RFFIArtType("COR", "переписка");
    public static RFFIArtType PER = new RFFIArtType("PER", "персоналии");
    public static RFFIArtType MIS = new RFFIArtType("MIS", "разное");
    public static RFFIArtType UNK = new RFFIArtType("UNK", "неопределен");
  }
}
