using System;
using System.Text.RegularExpressions;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
  public static class RFFIExtensions
  {
    public static string Lang(this JournalLanguage lang)
    {
      switch (lang)
      {
        case JournalLanguage.EN:
          return "ENG";
        case JournalLanguage.RU:
          return "RUS";
        default:
          throw new NotImplementedException("Language " + lang + " is not supported");
      }
    }

    public static string FilterXml(this string str)
    {
      return Regex.Replace(str, "<br[^/]*/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
    }
  }
}