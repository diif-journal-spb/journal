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

    public IArticle Article
    {
      get { return myArticle; }
    }

    public override LinkTemplate GetLinkTemplate(LinkManager manager)
    {
      string name = string.Format(@"numbers\{0}.{1}\article.{2}.html", myNumber.Year, myNumber.Number, myArticle.FileId);
      return new LinkTemplate(manager, name);        
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);

      IArticleInfo info = myArticle.ForLanguage(LanguageUtil.Convert(language));
      ctx.Add("article", info);
      ctx.Add("number", myNumber);
      ctx.Add("backLink", myBack.LinkTemplate.ToLink(language));
      ctx.Add("PdfLink", new PdfLink(myManager, info));
    }
  }
}