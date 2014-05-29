using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace EugenePetrenko.NumberEditor
{
  public static class Extensions
  {
    public static void ForEach<T>(this IEnumerable<T> enu, Action<T> action)
    {
      foreach (var e in enu)
        action(e);
    }

    public static string SaveToString(this HtmlDocument doc)
    {
      var s = new StringWriter();
      doc.Save(s);
      return s.ToString();
    }

    public static IEnumerable<T> notNull<T>(this IEnumerable<T> t)
    {
      if (t == null) return new T[0];
      return t.ToArray();      
    }
  }
}