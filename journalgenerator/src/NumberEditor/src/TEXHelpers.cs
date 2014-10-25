using System.Text.RegularExpressions;

namespace EugenePetrenko.NumberEditor
{
  public static class TEXHelpers
  {
    public static string FixTexIntoHTML(string tex)
    {
      tex = Regex.Replace(tex, "[\\r\\n\\s]+", " ");
      tex = tex.Replace("\\newblock", ""); 
      tex = Regex.Replace(tex, @"\s+", " "); 
      tex = tex.Replace("~", "");
      tex = tex.Replace("\\&", "&");
      tex = Regex.Replace(tex, @"(\\bibitem\{[^\}]*\})\s*", "\n\n$1");
      tex = Regex.Replace(tex, @"\{\s*\\em\s*([^\}]*)\s*\}", "<em>$1</em>");
      tex = Regex.Replace(tex, @"\{\s*\\it\s*([^\}]*)\s*\}", "<em>$1</em>");
      tex = Regex.Replace(tex, @"\\uppercase\{([^\}]+)\}", match => match.Groups[1].Value.ToUpper());
      tex = Regex.Replace(tex, @"\\""\{\s*a\s*\}", "ä");
      tex = Regex.Replace(tex, @"\\""\{\s*A\s*\}", "Ä");
      tex = Regex.Replace(tex, @"\\""\{\s*O\s*\}", "Ö");
      tex = Regex.Replace(tex, @"\\""\{\s*o\s*\}", "ö");
      tex = Regex.Replace(tex, @"\\""\{\s*u\s*\}", "ü");
      tex = Regex.Replace(tex, @"\\""\{\s*U\s*\}", "Ü");
      tex = Regex.Replace(tex, @"\\r\{\s*a\s*\}", "å");
      tex = Regex.Replace(tex, @"\\r\{\s*A\s*\}", "Å");
      tex = Regex.Replace(tex, @"\\u\{\s*o\s*\}", "ŏ");
      tex = Regex.Replace(tex, @"\\u\{\s*O\s*\}", "Ŏ");
      tex = Regex.Replace(tex, @"\\u\{\s*O\s*\}", "Ŏ");
      tex = Regex.Replace(tex, @"\\u\{\s*Z\s*\}", "Ž");
      tex = Regex.Replace(tex, @"\\u\{\s*z\s*\}", "ž");
      tex = Regex.Replace(tex, @"\\SS", "ß");
      tex = Regex.Replace(tex, @"[-–]+", "-");

      //fallback for unknown
      tex = Regex.Replace(tex, @"\\[cdbvutH]\{\s*(.?)\s*\}", "$1");

      return tex.Trim();
    }
  }
}