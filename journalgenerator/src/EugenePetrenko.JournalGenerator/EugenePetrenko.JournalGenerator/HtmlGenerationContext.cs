using System;
using System.Collections.Generic;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public abstract class HtmlGenerationContext : GenerationContext<LanguageGenerationContext>, IEquatable<HtmlGenerationContext>
  {
    private LinkTemplate myLinkTemplate = null;

    public abstract LinkTemplate GetLinkTemplate(LinkManager manager);

    protected HtmlGenerationContext(LinkManager manager, string templateName) : base(manager, templateName)
    {
    }

    [GenerationHidden]
    public LinkTemplate LinkTemplate
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

    public void AppendLinkParams(Language language, Dictionary<string, object> ctx, LinkTemplate linkTemplate)
    {
      Language foreignLanguage = (Language)(-(int)language);
      Link foreignLink = linkTemplate.ToLink(foreignLanguage);
      Link link = linkTemplate.ToLink(language);      

      ctx["Link"] = link;
      ctx["ForeignLink"] = foreignLink;
      ctx["LanguageName"] = LanguageUtil.LanguageToName(language);
      ctx["ForeignLanguageName"] = LanguageUtil.LanguageToName(foreignLanguage);
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      AppendLinkParams(language, ctx, LinkTemplate);
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