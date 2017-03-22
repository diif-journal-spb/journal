using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EugenePetrenko.NumberEditor
{
  public static class ReferencesParser
  {
    public static IEnumerable<string> ParseReferences(string text)
    {
      HTMLHelpers.MagicLinksEscape e;
      var lines = HTMLHelpers.ExcapeLinksWithMagic(text, out e).Trim().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                      .Select(x => x.Trim())
                      .Where(x => x.Length > 0)
                      .ToArray();


      var result = new List<string>();
      var acc = new StringBuilder();
      var regex = new Regex("^\\d+(\\.|\\)|\\])|^\\[\\d+\\]|^\\\\bibitem{[^}]+}");
      foreach (var line in lines)
      {
        if (regex.IsMatch(line))
        {
          result.Add(acc.ToString());
          acc = new StringBuilder();
          acc.Append(regex.Replace(line, "").Trim());
        }
        else
        {
          if (acc.Length > 0) acc.Append(" ");
          acc.Append(line.Trim());
        }
      }
      result.Add(acc.ToString());

      var seps = new Regex(@"([,;\.!\?]+)\s*");
      return result
        .Select(
          x => seps.Replace(x, "$1 ")
        )
        .Select(
          x => x.Trim().Trim(",.".ToCharArray()).Trim()
        )
        .Select(
          x => Regex.Replace(x, "[-–]+", "-")
        )
        .Select(
          x => Regex.Replace(x, @"</em>\s*<em>", " ")
        )
        .Select(
          x => Regex.Replace(x, @"\s\s+", " ")
        )
        .Where(x => x.Length > 0)
        .Select( 
          x => e.InstertHTMLLinks(x))
        .ToArray();
    } 

  }
}
