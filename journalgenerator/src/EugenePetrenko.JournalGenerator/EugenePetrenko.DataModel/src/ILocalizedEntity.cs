using System.Collections.Generic;

namespace EugenePetrenko.DataModel
{
  public interface ILocalizedEntity<T> : IEntity where T : IEntity
  {
    ICollection<JournalLanguage> JournalLanguages { get; }
    T ForLanguage(JournalLanguage journalLanguage);
    IEnumerable<T> AllLanguages();
  }
}