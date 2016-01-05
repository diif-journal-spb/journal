using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EugenePetrenko.NumberEditor
{
  public static class KeywordsParser
  {
    public static IEnumerable<string> ParseKeywords(string text)
    {
      var lines = text.Trim().Split("\r\n,;.".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Regex.Replace(x, @"\s+", " "))
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .Distinct()
                .OrderBy(x=>x)
                .ToArray();

      return lines;
    }    
  }
}
