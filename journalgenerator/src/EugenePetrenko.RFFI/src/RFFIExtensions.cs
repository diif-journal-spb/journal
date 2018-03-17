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
      str = Regex.Replace(str, "<br[^/>]*/?>", "\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "<p>\\s*", "\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "</p>", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "</?em[^/>]*/?>", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*<sup(er)?>\\s*", "^", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</sup(er)?>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*<sub(script)?>\\s*", "_", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</sub(script)?>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*<li>\\s*", "\n* ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</li>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</?ul>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</?ol>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</?u>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</?b>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</?i>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</?strong>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</?subscripn>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</?nobr>\\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "\\s*</?center>\\s*", "\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      
      //NBSP;
      str = Regex.Replace(str, "\u00A0", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      str = Regex.Replace(str, "[ ]+", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      return str;
    }
  }
}
