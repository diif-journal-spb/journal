using System.Collections;
using System.Collections.Generic;

namespace EugenePetrenko.JournalGenerator
{
  public class Hashset<T>
  {
    private readonly Dictionary<T,T> myDic = new Dictionary<T, T>();
    public bool Contains(T t)
    {
      return myDic.ContainsKey(t);
    }

    public void Add(T t)
    {
      myDic[t] = t;
    }

    public void Remove(T t)
    {
      myDic.Remove(t);
    }

    public int Count { get { return myDic.Count; } }

    public IEnumerable<T> Values
    {
      get { return myDic.Keys; }
    }
  }
}