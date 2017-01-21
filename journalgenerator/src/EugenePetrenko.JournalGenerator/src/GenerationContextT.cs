using System;
using System.Collections.Generic;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public abstract class GenerationContext<T> : GenerationContext, IEquatable<GenerationContext<T>> 
    where T : LanguageGenerationContext
  {        
    private readonly string myTemplateName;
    
    [GenerationHidden]
    public string TemplateName => myTemplateName;

    protected GenerationContext(LinkManager manager, string templateName) : base(manager)
    {
      myTemplateName = templateName;    
    }

    public abstract LinkTemplate LinkTemplate { get; }

    protected virtual void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      this.AppendProprerties(ctx);

      ctx["ResourceLink"] = myManager.GetRootLink(language, LinkTemplate);
      ctx["LanguageResourceLink"] = myManager.GetRootLinkForLanguage(language, LinkTemplate);
    }

    public T LanguageContext(Language language)
    {
      var dic = new Dictionary<string, object>();

      Program.Instance.AddPredefinedProperties(dic, language, LinkTemplate);

      AppendLanguageContextInternal(language, dic);

      var sdic = new SmartLookupDictionary(myTemplateName, dic, language, LinkTemplate);

      sdic.LookupTemplate += delegate(string item)
                               {
                                 var page = new HtmlDynamicPage(myManager, item);
                                 AddContext(page);
                                 return page;
                               };

      StringTemplate template = Program.Instance.Templates[language].GetInstanceOf(TemplateName);
      return CreateContext(template, sdic, language);
    }

    protected abstract T CreateContext(StringTemplate template, SmartLookupDictionary dic, Language language);

    protected void AddContext(HtmlGenerationContext ctx)
    {
      Program.Instance.AddPage(ctx);
    }

    protected void AddContextRange<TT>(IEnumerable<TT> ctx) 
      where TT : HtmlGenerationContext
    {
      foreach (var t in ctx)
      {
        AddContext(t);
      }
    }

    public bool Equals(GenerationContext<T> generationContext)
    {
      if (generationContext == null) return false;
      return Equals(myTemplateName, generationContext.myTemplateName);
    }

    public override bool Equals(object obj)
    {
      if (this == obj) return true;
      return Equals(obj as GenerationContext<T>);
    }

    public override int GetHashCode()
    {
      return myTemplateName.GetHashCode();
    }

    protected string GeneratePage(Language lang, string template, SmartLookupDictionary context)
    {
      return GeneratePage(myManager, lang, template, context);
    }

    private string GeneratePage(LinkManager links, Language lang, string template, SmartLookupDictionary dic)
    {
      return new SimplePageContext(dic, links, template, LinkTemplate).LanguageContext(lang).GeneratePage();
    }
  }
}
