using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public class LocalizedEntity<T> : Entity, ILocalizedEntity<T> where T : IEntity
  {
    private readonly Dictionary<JournalLanguage, T> myArticles = new Dictionary<JournalLanguage, T>();

    public LocalizedEntity(EntityGenerator gen) : base(gen)
    {
    }

    public LocalizedEntity(XmlElement element) : base(element)
    {
    }

    public ICollection<JournalLanguage> JournalLanguages
    {
      get { return myArticles.Keys; }
    }

    public T ForLanguage(JournalLanguage journalLanguage)
    {
      T t;
      return myArticles.TryGetValue(journalLanguage, out t) ? t : myArticles[JournalLanguage.EN];
    }

    public IEnumerable<T> AllLanguages()
    {
      return myArticles.Values;
    }

    protected void AddEntity(JournalLanguage lang, T t)
    {
      myArticles.Add(lang, t);
    }
  }
}