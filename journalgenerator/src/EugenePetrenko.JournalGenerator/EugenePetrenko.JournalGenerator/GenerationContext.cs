using System;
using System.Collections.Generic;
using System.Reflection;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public delegate object ConvertToLanguage(Language language);

  public abstract class GenerationContext<T> : IEquatable<GenerationContext<T>> where T : LanguageGenerationContext
  {
    protected readonly LinkManager myManager;
    private readonly string myTemplateName;
    private readonly Dictionary<string, HtmlDynamicPage> myPages = new Dictionary<string, HtmlDynamicPage>();

    [GenerationHidden]
    public string TemplateName
    {
      get { return myTemplateName; }
    }

    protected GenerationContext(LinkManager manager, string templateName)
    {
      myTemplateName = templateName;
      myManager = manager;
    }

    protected virtual void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      foreach (PropertyInfo info in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
      {
        if (!info.IsDefined(typeof (GenerationHiddenAttribute), true))
          ctx.Add(info.Name, info.GetGetMethod().Invoke(this, new object[0]));
      }

      ctx["ResourceLink"] = myManager.RootLink;
      ctx["LanguageResourceLink"] = myManager.GetRootLink(language);
    }

    public T LanguageContext(Language language)
    {
      Dictionary<string, object> dic = new Dictionary<string, object>();

      Program.Instance.AddPredefinedProperties(dic, language);

      AppendLanguageContextInternal(language, dic);

      SmartLookupDictionary sdic = new SmartLookupDictionary(myTemplateName, dic, language, myPages);

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
  }
}