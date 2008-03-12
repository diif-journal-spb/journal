using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public class LocalizedEntity<T> : Entity, ILocalizedEntity<T> where T : IEntity
  {
    private readonly Localized<T> myLocalized = new Localized<T>();
   
    public LocalizedEntity(EntityGenerator gen) : base(gen)
    {
    }

    public LocalizedEntity(XmlElement element) : base(element)
    {
    }

    public T ForLanguage(JournalLanguage journalLanguage)
    {
      return myLocalized.ForLanguage(journalLanguage);
    }

    public IEnumerable<T> AllLanguages()
    {
      return myLocalized.AllLanguages();
    }

    public void AddEntity(JournalLanguage lang, T t)
    {
      myLocalized.AddEntity(lang, t);
    }

    public ICollection<JournalLanguage> JournalLanguages
    {
      get { return myLocalized.JournalLanguages; }
    }
  }
}