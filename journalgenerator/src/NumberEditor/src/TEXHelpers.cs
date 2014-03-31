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
      tex = Regex.Replace(tex, @"\\bibitem\{[^\}]*\}", "\n");
      tex = Regex.Replace(tex, @"\{\s*\\em\s*([^\}]*)\s*\}", "<em>$1</em>");
      return tex.Trim();
    }
  }
}