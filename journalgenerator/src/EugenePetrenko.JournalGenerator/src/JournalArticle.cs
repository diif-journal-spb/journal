using System;
using System.Collections.Generic;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public class JournalArticle : HtmlGenerationContext
  {
    private readonly INumber myNumber;
    private readonly IArticle myArticle;
    private readonly JournalNumberContext myBack;

    public JournalArticle(INumber number, IArticle article, JournalNumberContext back, LinkManager manager) : base(manager, "article")
    {
      myNumber = number;
      myArticle = article;
      myBack = back;
    }

    public IArticle Article => myArticle;


    public override string[] ExtraFiles
    {
      get
      {
        var result = new List<string>();
        foreach (IArticleInfo info in Article.AllLanguages())
        {
          result.AddRange(info.ExtraFiles);
        }
        return result.ToArray();
      }
    }

    private string ArticleFileId()
    {
      int sectionId = 0;
      foreach (var section in myNumber.Sections)
      {
        sectionId++;
        int artcleId = 0;
        foreach (var article in section.Articles)
        {
          artcleId++;
          if (article == myArticle)
          {
            return sectionId + "." + artcleId;
          }
        }
      }
      throw new Exception("Article not found in the number");
    }

    public override LinkTemplate GetLinkTemplate(LinkManager manager)
    {
      string name = $@"numbers\{myNumber.Year}.{myNumber.Number}\article.{ArticleFileId()}.html";
      return new LinkTemplate(manager, name);        
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);

      IArticleInfo info = myArticle.ForLanguage(LanguageUtil.Convert(language));
      ctx.Add("article", info);
      ctx.Add("number", myNumber);
      ctx.Add("backLink", myBack.LinkTemplate.ToLink(language, LinkTemplate));
      ctx.Add("PdfLink", Program.Instance.PdfManager.RegisterPdf(myArticle, language, LinkTemplate));
    }
  }
}
