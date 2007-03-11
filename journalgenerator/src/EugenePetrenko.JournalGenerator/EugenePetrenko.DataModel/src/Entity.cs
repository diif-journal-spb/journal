using System;
using System.Collections.Generic;

namespace EugenePetrenko.DataModel
{
  public class Entity : IEntity
  {
    private readonly string myId;

    public Entity(string id)
    {
      myId = GetType().Name + ":" + id;
    }

    public Entity(EntityGenerator gen)
    {
      myId = GetType().Name + ":" + gen.NextId(GetType());
    }

    public string Id
    {
      get { return myId; }
    }
  }

  public class EntityGenerator
  {
    private Dictionary<Type, int> myIds = new Dictionary<Type, int>();

    public string NextId(Type type)
    {
      int id = 0;
      myIds.TryGetValue(type, out id);
      myIds[type] = ++id;

      return id.ToString();
    }
  }
}