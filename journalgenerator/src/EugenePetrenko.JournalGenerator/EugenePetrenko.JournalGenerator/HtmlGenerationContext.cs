using System.Collections.Generic;

namespace EugenePetrenko.JournalGenerator
{
  public abstract class HtmlGenerationContext : GenerationContext
  {
    private readonly HtmlPageTitle myTitle;
    private LinkTemplate myLinkTemplate = null;
    private readonly List<GenerationContext> myExtraPages = new List<GenerationContext>();

    public abstract LinkTemplate GetLinkTemplate(LinkManager manager);


    protected HtmlGenerationContext(LinkManager manager, string templateName) : base(manager, templateName)
    {
      myTitle = new HtmlPageTitle(manager, templateName + "_title");
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

    public static void AppendLinkParams(Language language, Dictionary<string, object> ctx, LinkTemplate linkTemplate)
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

    [GenerationHidden]
    public GenerationContext TitlePage
    {
      get { return myTitle; }
    }

    [GenerationHidden]
    public List<GenerationContext> ExtraPages
    {
      get { return myExtraPages; }
    }

    protected void AddContext(GenerationContext ctx)
    {
      myExtraPages.Add(ctx);
    }

    protected void AddContextRange<T>(ICollection<T> ctx) where T : GenerationContext
    {
      foreach (T t in ctx)
      {
        AddContext(t);
      }
    }

  }
}