using System.Collections;
using System.Collections.Generic;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public class FileHtmlContext : GenerationContext<FileLanguageGenerationContext>
  {
    private readonly HtmlGenerationContext myContent;

    public FileHtmlContext(LinkManager manager, HtmlGenerationContext content)
      : base(manager, "html")
    {
      myContent = content;
    }

    [GenerationHidden]
    public override LinkTemplate LinkTemplate => myContent.LinkTemplate;

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);

      LanguageGenerationContext contentContext = myContent.LanguageContext(language);
      foreach (string key in HtmlGenerationContext.PREDEFINED)
        ctx.Add(key, ((IDictionary) contentContext.Attributes)[key].ToString());

      ctx.Add("content", contentContext.GeneratePage());
      ctx.Add("menu", GeneratePage(language, "menu", contentContext.Attributes));
      ctx.Add("logo", GeneratePage(language, "logo", contentContext.Attributes));
      ctx.Add(CAPTION_KEY, GeneratePage(language, myContent.TemplateName + "_caption", contentContext.Attributes));
    }

    protected override FileLanguageGenerationContext CreateContext(StringTemplate template, SmartLookupDictionary dic,
                                                                   Language language)
    {
      var destFile = myContent.LinkTemplate.ToLink(language, null).DestFile;
      var extraFiles = myContent.ExtraFiles;
      return extraFiles.Length == 0 ? new FileLanguageGenerationContext(template, dic, destFile) : new FileLanguageGenerationContextWithExtraFiles(template, dic, destFile, extraFiles);
    }

    public override string ToString()
    {
      return "FileHtmlContext: " + myContent.LinkTemplate;
    }
  }
}
