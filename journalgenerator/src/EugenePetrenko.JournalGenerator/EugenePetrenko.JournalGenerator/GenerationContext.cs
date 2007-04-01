using System.Collections.Generic;
using System.Reflection;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public abstract class GenerationContext
  {
    protected readonly LinkManager myManager;
    private readonly string myTemplateName;

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
    }

    public LanguageGenerationContext LanguageContext(Language language)
    {
      Dictionary<string, object> dic = new Dictionary<string, object>();
      AppendLanguageContextInternal(language, dic);

      StringTemplate template = Program.Instance.Templates[language].GetInstanceOf(TemplateName);
      return new LanguageGenerationContext(template, dic);
    }
  }
}