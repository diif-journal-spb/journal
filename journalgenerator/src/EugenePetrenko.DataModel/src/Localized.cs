using System;
using System.Collections.Generic;

namespace EugenePetrenko.DataModel
{
  public class Localized<T> : ILocalizedEntity<T>
  {
    private readonly Dictionary<JournalLanguage, T> myEntities = new Dictionary<JournalLanguage, T>();

    public ICollection<JournalLanguage> JournalLanguages => myEntities.Keys;

    public T ForLanguage(JournalLanguage journalLanguage)
    {
      T t;
      return myEntities.TryGetValue(journalLanguage, out t) ? t : myEntities[JournalLanguage.EN];
    }

    public IEnumerable<T> AllLanguages()
    {
      return myEntities.Values;
    }

    public void AddEntity(JournalLanguage lang, T t)
    {
      if (myEntities.ContainsKey(lang)) throw new Exception("Duplicate language: " + lang + " for " + t ); 
      myEntities.Add(lang, t);
    }
  }
}
