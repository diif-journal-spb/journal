using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EugenePetrenko.NumberEditor
{
  public static class HTMLHelpers
  {
    public static string FixWordHTML(string html)
    {
      html = Regex.Replace(html, "[\\r\\n\\s]+", " ");
      html = new List<string>
        {
          @"<!--(w|W)+?-->",
          @"<title>(w|W)+?</title>",
          @"s?class=w+",
          @"s+style='[^']+'",
          @"<(meta|link|/?o:|/?style|/?div|/?std|/?head|/?html|body|/?body|/?span|!\[)[^>]*?>",
          @"</?st1[^>]*>",
          @"(<[^>]+>)+&nbsp;(</w+>)+",
          @"s+v:w+=""[^""]+""",
          @"\s*class=Mso[^s>]*",
          @"\s*style='[^']*'",
          @"(nr){2,}",
          @"</p\s*>"
        }.Aggregate(html, (current, s) => Regex.Replace(current, s, "", RegexOptions.IgnoreCase))
        ;

      html = Regex.Replace(html, @"\s*&nbsp;\s*", " ");
      html = Regex.Replace(html, @"<p\s*>", "\n");
      html = Regex.Replace(html, @"</i>\s*<i>", "");
      html = Regex.Replace(html, @"<i>", "<em>");
      html = Regex.Replace(html, @"</i>", "</em>");
      html = html.Trim();
      return html;
    }    
  }
}