using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;

namespace EugenePetrenko.NumberEditor
{
  public static class HTMLHelpers
  {
    public static string FixWordHTML(string html)
    {
      try
      {
        html = ProcessAsHTMLDocument(html);
        Console.Out.WriteLine("Post processed XML: \n\n" + html);
      }
      catch(Exception e)
      {
        Console.Out.WriteLine("FUCUUUCUCUCK: " + e);
        //NOP
      }

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
      html = Regex.Replace(html, @"</em>\s*<em>", " ");
      html = html.Trim();
      return html;
    }

    private static string ProcessAsHTMLDocument(string html)
    {
      var doc = new HtmlDocument();
      doc.LoadHtml(html);

      doc.DocumentNode.SelectNodes("//comment()").notNull().ForEach(x => x.Remove());
      CleanupRecursive(doc.DocumentNode);

      return doc.SaveToString();
    }

    private static bool IsInvalidAttribute(HtmlAttribute x)
    {
      switch (x.Name)
      {
        case "style":
        case "class":
        case "lang":
          return true;
        default:
          return false;
      }
    }

    private static bool IsProxyNode(HtmlNode x)
    {
      if (x.Name.StartsWith("w:")) return true;
      if (x.Name.StartsWith("o:")) return true;
      if (x.Name.StartsWith("pst")) return true;

      switch (x.Name)
      {
        case "a":
        case "style":
        case "script":
        case "meta":
        case "body":
        case "div":
          return true;
        default:
          return false;
      }      
    }

    private static bool IsInvalidNode(HtmlNode node)
    {
      if (node is HtmlCommentNode)return true;
      
      switch (node.Name)
      {
        case "style":
        case "script":
        case "head":
        case "meta":
          return true;
        default:
          return false;
      }      
    }

    private static void CleanupRecursive(HtmlNode node)
    {
      if (node.Name == "#text") return;

      if (node.Name == "html")
      {
        node.Attributes.RemoveAll();
      }

      if (IsInvalidNode(node))
      {
        node.Remove();
        return;
      }

      var href = node.Attributes["href"];
      if (node.Name == "a" && href != null)
      {
        node.ParentNode.InsertBefore(node.OwnerDocument.CreateTextNode(href.Value), node);
        href.Remove();
      }

      if (IsProxyNode(node))
      {
        node.ChildNodes.notNull()
          .Select(child => child.CloneNode(true))
          .Select(copy => node.ParentNode.InsertBefore(copy, node))
          .ToArray()
          .ForEach(CleanupRecursive);
        
        node.Remove();
        return;
      }

      node.Attributes.notNull().Where(IsInvalidAttribute).ForEach(x=>x.Remove());
      if (node.HasChildNodes)
      {
        node.ChildNodes.notNull().ForEach(CleanupRecursive);
      }
    }
  }
}