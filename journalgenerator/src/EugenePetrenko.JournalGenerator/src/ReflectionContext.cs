using System.Collections.Generic;
using System.Reflection;

namespace EugenePetrenko.JournalGenerator
{
  public static class ReflectionContext
  {
    public static Dictionary<string, object> GetProperties(this object o)
    {
      var ps = new Dictionary<string, object>();
      o.AppendProprerties(ps);
      return ps;
    }

    public static void AppendProprerties(this object o, Dictionary<string, object> ctx)
    {
      foreach (PropertyInfo info in o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
      {
        if (!info.IsDefined(typeof(GenerationHiddenAttribute), true))
          ctx.Add(info.Name, info.GetGetMethod().Invoke(o, new object[0]));
      }
    }
  }
}
