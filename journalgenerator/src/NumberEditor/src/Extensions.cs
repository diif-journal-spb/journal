using System;
using System.Collections.Generic;

namespace EugenePetrenko.NumberEditor
{
  public static class Extensions
  {
    public static void ForEach<T>(this IEnumerable<T> enu, Action<T> action)
    {
      foreach (var e in enu)
        action(e);
    }
  }
}