using System.Collections.Generic;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public abstract class CollectionGenerationContext<T, Q> : HtmlGenerationContext     
    where T : ILocalizedEntity<Q>
    where Q : IEntity
  {
    private readonly IEnumerable<T> myItems;
    private readonly string myKey;

    public CollectionGenerationContext(string key, IEnumerable<T> news, LinkManager manager, string template)
      : base(manager, template)
    {
      myKey = key;
      myItems = news;
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);

      ctx[myKey] = new List<Q>(CollectionForLanguage(myItems, LanguageUtil.Convert(language)));
    }

    private static IEnumerable<Q> CollectionForLanguage(IEnumerable<T> data, JournalLanguage lang)
    {
      foreach (T entity in data)
      {
        yield return entity.ForLanguage(lang);
      }
    }
  }
}