using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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

    public static T ExecuteUnderSTA<T>(Func<T> action)
    {
      if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
      {
        return action();
      }

      T result = default(T);
      Exception error = null;

      Thread thread = new Thread(() =>
      {
        try
        {
          result = action();
        }
        catch (Exception e)
        {
          error = e;
        }
      });
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
      thread.Join();

      if (error != null)
      {
        throw new Exception("Failed. " + error.Message, error);
      }
      return result;
    }
  }
}