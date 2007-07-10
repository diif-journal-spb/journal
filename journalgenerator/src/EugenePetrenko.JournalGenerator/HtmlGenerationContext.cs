using System;
using System.Collections.Generic;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public abstract class HtmlGenerationContext : GenerationContext<LanguageGenerationContext>, IEquatable<HtmlGenerationContext>
  {
    public static readonly string[] PREDEFINED = { "Link", "ForeignLink", "LanguageName", "ForeignLanguageName" };

    private LinkTemplate myLinkTemplate = null;

    public abstract LinkTemplate GetLinkTemplate(LinkManager manager);

    protected HtmlGenerationContext(LinkManager manager, string templateName) : base(manager, templateName)
    {
    }

    [GenerationHidden]
    public override LinkTemplate LinkTemplate
    {
      get
      {
        if (myLinkTemplate == null)
        {
          myLinkTemplate = GetLinkTemplate(myManager);
        }
        return myLinkTemplate;
      }
    }    

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);

      Language foreignLanguage = (Language)(-(int)language);
      
      Link foreignLink = LinkTemplate.ToLink(foreignLanguage, LinkTemplate, language);
      Link link = LinkTemplate.ToLink(language, null);

      ctx["Link"] = link;
      ctx["ForeignLink"] = foreignLink;
      ctx["LanguageName"] = LanguageUtil.LanguageToName(language);
      ctx["ForeignLanguageName"] = LanguageUtil.LanguageToName(foreignLanguage);
    }

    protected override LanguageGenerationContext CreateContext(StringTemplate template, SmartLookupDictionary dic, Language language)
    {
      return new LanguageGenerationContext(template, dic);
    }

    public bool Equals(HtmlGenerationContext htmlGenerationContext)
    {
      if (htmlGenerationContext == null) return false;
      if (!base.Equals(htmlGenerationContext)) return false;
      return Equals(LinkTemplate, htmlGenerationContext.LinkTemplate);
    }

    public override bool Equals(object obj)
    {
      if (this == obj) return true;
      return Equals(obj as HtmlGenerationContext);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() + 29*LinkTemplate.GetHashCode();
    }
  }
}