using System;
using System.IO;
using EugenePetrenko.DataModel;
using JetBrains.Annotations;

namespace EugenePetrenko.RFFI
{
  public class RFFIArtType
  {
    public readonly string Type;

    private RFFIArtType(string type, string description)
    {
      Type = type;
    }

    private static RFFIArtType RAR = new RFFIArtType("RAR", "научная статья");
    private static RFFIArtType EDI = new RFFIArtType("EDI", "редакторская заметка");
    private static RFFIArtType BRV = new RFFIArtType("BRV", "рецензия");
    private static RFFIArtType CNF = new RFFIArtType("CNF", "материалы конференции");
    private static RFFIArtType THS = new RFFIArtType("THS", "тезисы доклада на конференции");
    private static RFFIArtType SCO = new RFFIArtType("SCO", "краткое сообщение");
    private static RFFIArtType REV = new RFFIArtType("REV", "обзорная статья");
    private static RFFIArtType ABS = new RFFIArtType("ABS", "аннотация");
    private static RFFIArtType REP = new RFFIArtType("REP", "научный отчет");
    private static RFFIArtType COR = new RFFIArtType("COR", "переписка");
    private static RFFIArtType PER = new RFFIArtType("PER", "персоналии");
    private static RFFIArtType MIS = new RFFIArtType("MIS", "разное");
    private static RFFIArtType UNK = new RFFIArtType("UNK", "неопределен");

    [CanBeNull]
    public static RFFIArtType FromSection(INumberSection section)
    {
      if (section is PublicationsNumberFactory.PubSection)
      {
        return RAR;
      }

      if (section is ConfNumberFactory.ConfSection)
      {
        return CNF;
      }

      if (section is MonographNumberFactory.MonograpSection)
      {
        return UNK;
      }

      if (section is PhdNumberFactory.PhdsSection)
      {
        return UNK;
      }

      if (section is BooksNumberFactory.BooksSection)
      {
        return UNK;
      }

      throw new Exception("Unknown section: " + section.GetType().Name);
    }
  }
}
