using System;
using System.Collections.Generic;
using System.Reflection;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public abstract class GenerationContext<T> : GenerationContext, IEquatable<GenerationContext<T>> where T : LanguageGenerationContext
  {        
    private readonly string myTemplateName;
    
    
    [GenerationHidden]
    public string TemplateName
    {
      get { return myTemplateName; }
    }

    protected GenerationContext(LinkManager manager, string templateName) : base(manager)
    {
      myTemplateName = templateName;    
    }

    public abstract LinkTemplate LinkTemplate { get; }

    protected virtual void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      foreach (PropertyInfo info in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
      {
        if (!info.IsDefined(typeof (GenerationHiddenAttribute), true))
          ctx.Add(info.Name, info.GetGetMethod().Invoke(this, new object[0]));
      }

      ctx["ResourceLink"] = myManager.GetRootLink(language, LinkTemplate);
      ctx["LanguageResourceLink"] = myManager.GetRootLinkForLanguage(language, LinkTemplate);
    }

    public T LanguageContext(Language language)
    {
      Dictionary<string, object> dic = new Dictionary<string, object>();

      Program.Instance.AddPredefinedProperties(dic, language, LinkTemplate);

      AppendLanguageContextInternal(language, dic);

      SmartLookupDictionary sdic = new SmartLookupDictionary(myTemplateName, dic, language, LinkTemplate);

      sdic.LookupTemplate += delegate(string item)
                               {
                                 HtmlDynamicPage page = new HtmlDynamicPage(myManager, item);
                                 AddContext(page);
                                 return page;
                               };

      StringTemplate template = Program.Instance.Templates[language].GetInstanceOf(TemplateName);
      return CreateContext(template, sdic, language);
    }

    protected abstract T CreateContext(StringTemplate template, SmartLookupDictionary dic, Language language);

    public virtual void AddContext(HtmlGenerationContext ctx)
    {
      Program.Instance.AddPage(ctx);
    }

    public void AddContextRange<TT>(ICollection<TT> ctx) where TT : HtmlGenerationContext
    {
      foreach (TT t in ctx)
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

    public string GeneratePage(LinkManager links, Language lang, string template, SmartLookupDictionary dic)
    {
      return new SimplePageContext(dic, links, template, LinkTemplate).LanguageContext(lang).GeneratePage();
    }
  }
}