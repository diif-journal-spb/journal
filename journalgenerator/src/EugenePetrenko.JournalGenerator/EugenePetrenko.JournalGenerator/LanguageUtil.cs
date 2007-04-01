using System;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public static class LanguageUtil
  {
    public static JournalLanguage Convert(Language lang)
    {
      switch (lang)
      {
        case Language.EN:
          return JournalLanguage.EN;
        case Language.RU:
          return JournalLanguage.RU;
      }
      throw new ArgumentException("Unexpected language");
    }

    public static string LanguageToName(Language l)
    {
      switch (l)
      {
        case Language.EN:
          return "English";
        case Language.RU:
          return "Русский";
      }
      throw new ArgumentException("Unexpected language");
    }
  }
}