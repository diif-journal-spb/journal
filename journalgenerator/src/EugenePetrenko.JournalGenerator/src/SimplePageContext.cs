using System.Collections.Generic;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public sealed class SimplePageContext : GenerationContext<LanguageGenerationContext>
  {
    private readonly SmartLookupDictionary myDic;
    private readonly LinkTemplate myPageLink;

    public SimplePageContext(SmartLookupDictionary dic, LinkManager manager, string templateName, LinkTemplate pageLink) : base(manager, templateName)
    {
      myDic = dic;
      myPageLink = pageLink;
    }

    public override LinkTemplate LinkTemplate
    {
      get { return myPageLink; }
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);
      foreach (string key in myDic.StaticDictionary.Keys)
      {
        if (!ctx.ContainsKey(key))
          ctx.Add(key, myDic.StaticDictionary[key]);
      }
    }


    protected override LanguageGenerationContext CreateContext(StringTemplate template, SmartLookupDictionary dic,
                                                               Language language)
    {
      return new LanguageGenerationContext(template, dic);
    }
  }
}