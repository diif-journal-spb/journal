using System;
using System.Collections.Generic;

namespace EugenePetrenko.DataModel
{
  public class EntityGenerator
  {
    private Dictionary<Type, int> myIds = new Dictionary<Type, int>();

    public string NextId(Type type)
    {
      int id;
      myIds.TryGetValue(type, out id);
      myIds[type] = ++id;

      return id.ToString();
    }
  }
}