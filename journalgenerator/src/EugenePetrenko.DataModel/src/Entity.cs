using System;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public class Entity : IEntity
  {
    private readonly string myRawId;
    private readonly string myId;

    public Entity(string id)
    {
      myRawId = id;
      myId = GetType().Name + ":" + id;
    }

    public Entity(EntityGenerator gen)
    {
      myRawId = gen.NextId(GetType());
      myId = GetType().Name + ":" + myRawId;
    }

    public Entity(XmlElement element)
    {
      myId = element.GetAttribute("id");
    }

    public string Id
    {
      get { return myId; }
    }

    public string FileId
    {
      get { return myRawId; }
    }
  }
}