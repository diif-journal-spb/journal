using System.Collections.Generic;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public class JournalNumberContext : HtmlGenerationContext
  {
    private readonly INumber myNumber;
    private readonly List<JournalArticle> myArticles = new List<JournalArticle>();

    public JournalNumberContext(LinkManager manager, INumber number) : base(manager, "issue")
    {
      myNumber = number;
      foreach (IArticle article in myNumber.Articles)
      {
        JournalArticle art = new JournalArticle(myNumber, article, this, manager);
        myArticles.Add(art);
        AddContext(art);
      }
    }

    public override LinkTemplate GetLinkTemplate(LinkManager manager)
    {
      string name = string.Format(@"numbers\{0}.{1}\issue.html", myNumber.Year, myNumber.Number);
      return new LinkTemplate(manager, name);        
    }

    private List<ArticleInfoContext> BuildInfos(Language lang)
    {
      List<ArticleInfoContext> infos = new List<ArticleInfoContext>();
      foreach (JournalArticle article in myArticles)
      {
        IArticleInfo info = article.Article.ForLanguage(LanguageUtil.Convert(lang));
        infos.Add(new ArticleInfoContext(info, article.LinkTemplate.ToLink(lang), 
          Program.Instance.PdfManager.RegisterPdf(article.Article, lang)));
      }
      return infos;
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);

      List<ArticleInfoContext> list = BuildInfos(language);

      ctx.Add("issue", myNumber);      
      ctx.Add("articles", list);
    }

    public class ArticleInfoContext
    {
      private readonly IArticleInfo myInfo;
      private readonly Link myLink;
      private readonly PdfLink myPdf;


      public ArticleInfoContext(IArticleInfo info, Link link, PdfLink pdf)
      {
        myInfo = info;
        myLink = link;
        myPdf = pdf;
      }

      public IArticleInfo Info
      {
        get { return myInfo; }
      }

      public Link Link
      {
        get { return myLink; }
      }

      public PdfLink Pdf
      {
        get { return myPdf; }
      }
    }
  }
}