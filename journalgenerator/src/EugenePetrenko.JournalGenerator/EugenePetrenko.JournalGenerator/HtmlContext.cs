using System.Collections.Generic;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public class HtmlContext : GenerationContext
  {
    private readonly HtmlGenerationContext myContent;

    public HtmlContext(LinkManager manager, HtmlGenerationContext content)
      : base(manager, "html")
    {
      myContent = content;
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);

      HtmlGenerationContext.AppendLinkParams(language, ctx, myContent.LinkTemplate);

      ctx.Add("title", myContent.TitlePage.LanguageContext(language).GeneratePage());
      ctx.Add("content", myContent.LanguageContext(language).GeneratePage());
      ctx.Add("style", myManager.GenerationBaseUrl + "/style/style.css");
    }

    public new FileLanguageGenerationContext LanguageContext(Language language)
    {
      Dictionary<string, object> dic = new Dictionary<string, object>();
      AppendLanguageContextInternal(language, dic);

      StringTemplate template = Program.Instance.Templates[language].GetInstanceOf(TemplateName);
      string destFile = myContent.LinkTemplate.ToLink(language).DestFile;
      return new FileLanguageGenerationContext(template, dic, destFile);
    }


    public override string ToString()
    {
      return "HtmlContext: " + myContent.LinkTemplate;
    }
  }
}