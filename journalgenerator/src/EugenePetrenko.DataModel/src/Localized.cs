using System.Collections.Generic;

namespace EugenePetrenko.DataModel
{
  public class Localized<T> : ILocalizedEntity<T>
  {
    private readonly Dictionary<JournalLanguage, T> myEntities = new Dictionary<JournalLanguage, T>();

    public ICollection<JournalLanguage> JournalLanguages
    {
      get { return myEntities.Keys; }
    }

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
      myEntities.Add(lang, t);
    }
  }
}