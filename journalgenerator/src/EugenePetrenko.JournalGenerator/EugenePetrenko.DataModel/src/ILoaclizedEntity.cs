using System.Collections.Generic;

namespace EugenePetrenko.DataModel
{
  public interface ILoaclizedEntity<T> : IEntity where T : IEntity
  {
    ICollection<JournalLanguage> JournalLanguages { get; }
    T ForLanguage(JournalLanguage journalLanguage);    
  }
}