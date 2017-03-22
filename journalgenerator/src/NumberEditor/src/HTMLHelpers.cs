using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
      }
      catch (Exception e)
      {
        Console.Out.WriteLine("FUCUUUCUCUCK: " + e);
        //NOP
      }

      html = Regex.Replace(html, "[\\r\\n\\s]+", " ");
      while (true)
      {
        var next = FixWordHTMLImpl(html);
        if (next == html) break;
        html = next;
      }

      return html;
    }

    private static string FixWordHTMLImpl(string html)
    {
      
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
          @"</p\s*>",
          @"</li\s*>",
          @"<em\s*>\s*</\s*em\s*>",
          @"<u\s*>\s*</\s*u\s*>",
          @"</?\s*(tr|td|th|table|style)\s*>",
        }.Aggregate(html, (current, s) => Regex.Replace(current, s, "", RegexOptions.IgnoreCase| RegexOptions.Multiline))
        ;

      
      html = Regex.Replace(html, @"\s*&nbsp;\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"\s*\u00a0\s*", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @" +", " ");
      html = Regex.Replace(html, @"<p\s*>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"<br\s*/?\s*>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"<li\s*>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"</i>\s*<i>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"<i>", "<em>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"</i>", "</em>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"<b>\s*</b>", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"</em> *<em>", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = html.Replace("&nbsp;", " ");
      html = html.Replace("\r", "");
      html = Regex.Replace(html, @"\n\n+", "\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"[^\S\n]{2,}", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"\n[^\S\n]+\n", "\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = Regex.Replace(html, @"\n\s*<b>\s*(\[\d+\])\s*</\s*b>", "\n$1 ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      html = html.Trim();

      html =
        string.Join("\n", 
          html
          .Split("\n".ToCharArray(), StringSplitOptions.None)
          .Select(x => Regex.Replace(x, @"\s+", " ")));

      return html;
    }

    public class MagicLinksEscape
    {
      internal readonly Dictionary<string, string> TokenToLink = new Dictionary<string, string>();

      public string InstertHTMLLinks(string text)
      {
        foreach (var e in TokenToLink)
        {
          var t = e.Value;
          var link = " <a href=\"" + t + "\">" + t + "</a> ";
          text = text.Replace(e.Key, link);
        }
        return text;
      }
    }

    public static string ExcapeLinksWithMagic(string text, out MagicLinksEscape e)
    {
      var ml = new MagicLinksEscape();
      e = ml;

      var regex = new Regex(@"[\s]+(?<url>(http://|https://|ftp://)[^,""\s<)]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

      int keyId = 42;
      return regex.Replace(text, match =>
      {
        var key = "LINK_" + keyId++ + "_LINK";
        var t = match.Groups["url"].Value.Trim().TrimEnd('.').Trim();
        ml.TokenToLink[key] = t;
        return key;
      });     
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
        case "dir":
        case "style":
        case "class":
        case "lang":
        case "clear":
          return true;
        default:
          return false;
      }
    }

    private static bool IsProxyNode(HtmlNode x)
    {
      if (x.Name.StartsWith("w:")) return true;
      if (x.Name.StartsWith("o:")) return true;
      if (x.Name.StartsWith("st1:")) return true;
      if (x.Name.StartsWith("pst")) return true;

      switch (x.Name)
      {
        case "a":
        case "style":
        case "script":
        case "meta":
        case "body":
        case "div":
        case "ol":
        case "span":
        case "strong":
        case "table":
        case "tr":
        case "th":
        case "td":
        case "pre":
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

      if (node.Name == "img")
      {
        node.ParentNode.InsertBefore(node.OwnerDocument.CreateTextNode("!!!!ERROR!!! IMAGE IS NOT ALLOWERD!"), node);
        node.Remove(); 
        return;
      }

      var href = node.Attributes["href"];
      if (node.Name == "a" && href != null && node.InnerText.Trim() == href.Value.Trim())
      {
        var url = href.Value;
        if (url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("ftp://"))
        {
          node.ParentNode.InsertBefore(node.OwnerDocument.CreateTextNode(url), node);
        }
        else
        {
          node.ParentNode.InsertBefore(node.OwnerDocument.CreateTextNode("!!!!ERROR!!! INVALID URL: !" + url), node);          
        }
        node.Remove();      
        return;
      }

      if (node.Name == "li")
      {
        node.InsertBefore(node.OwnerDocument.CreateTextNode("  42. "), node.FirstChild);
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
