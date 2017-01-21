namespace EugenePetrenko.RFFI
{
  public class RFFIArtType
  {
    public readonly string Type;

    public RFFIArtType(string type)
    {
      Type = type;
    }

    public static RFFIArtType RAR = new RFFIArtType("RAR");
    public static RFFIArtType EDI = new RFFIArtType("EDI");
    public static RFFIArtType BRV = new RFFIArtType("BRV");
    public static RFFIArtType CNF = new RFFIArtType("CNF");
    public static RFFIArtType SCO = new RFFIArtType("SCO");
    public static RFFIArtType REV = new RFFIArtType("REV");
    public static RFFIArtType ABS = new RFFIArtType("ABS");
    public static RFFIArtType REP = new RFFIArtType("REP");
    public static RFFIArtType COR = new RFFIArtType("COR");
    public static RFFIArtType PER = new RFFIArtType("PER");
    public static RFFIArtType MIS = new RFFIArtType("MIS");
    public static RFFIArtType UNK = new RFFIArtType("UNK");
  }
}
