using System.Collections;
using System.Collections.Generic;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public class HtmlContext : GenerationContext<FileLanguageGenerationContext>
  {
    private readonly HtmlGenerationContext myContent;

    public HtmlContext(LinkManager manager, HtmlGenerationContext content)
      : base(manager, "html")
    {
      myContent = content;
    }

    private string GeneratePage(Language lang, string template, LanguageGenerationContext parentContext)
    {
      return new SimplePageContext(parentContext.Attributes, myManager, template).LanguageContext(lang).GeneratePage();      
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);
      
      LanguageGenerationContext contentContext = myContent.LanguageContext(language);
      foreach (string key in HtmlGenerationContext.PREDEFINED)
        ctx.Add(key, ((IDictionary) contentContext.Attributes)[key].ToString());

      ctx.Add("content", contentContext.GeneratePage());
      ctx.Add("menu", GeneratePage(language, "menu", contentContext));
      ctx.Add("logo", GeneratePage(language, "logo", contentContext));
      ctx.Add("caption", GeneratePage(language, myContent.TemplateName + "_caption", contentContext));
    }


    protected override FileLanguageGenerationContext CreateContext(StringTemplate template, SmartLookupDictionary dic,
                                                                   Language language)
    {
      string destFile = myContent.LinkTemplate.ToLink(language).DestFile;
      return new FileLanguageGenerationContext(template, dic, destFile);
    }

    public override string ToString()
    {
      return "HtmlContext: " + myContent.LinkTemplate;
    }
  }
}