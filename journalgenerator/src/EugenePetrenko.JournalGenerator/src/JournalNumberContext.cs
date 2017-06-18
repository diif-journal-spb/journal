using System.Collections.Generic;
using EugenePetrenko.DataModel;
using EugenePetrenko.JournalGenerator.Util;

namespace EugenePetrenko.JournalGenerator
{
  public class JournalNumberContext : HtmlGenerationContext
  {
    private readonly INumber myNumber;

    public JournalNumberContext(LinkManager manager, INumber number) : base(manager, "issue")
    {
      myNumber = number;      
    }

    public override LinkTemplate GetLinkTemplate(LinkManager manager)
    {
      string name = $@"numbers\{myNumber.Year}.{myNumber.Number}\issue.html";
      return new LinkTemplate(manager, name);        
    }
    
    private List<NumberSectionInfo> BuildSectionInfos(IEnumerable<INumberSection> sections, Language lang)
    {
      List<NumberSectionInfo> infos = new List<NumberSectionInfo>();

      foreach (INumberSection section in sections)
      {
        NumberSectionInfo inf = new NumberSectionInfo(
          BuildInfos(section.Articles, lang), 
          section.ShowTitle, 
          section.ForLanguage(LanguageUtil.Convert(lang)).Name
          );

        infos.Add(inf);
      }
      return infos;      
    }

    private List<ArticleInfoContext> BuildInfos(IEnumerable<IArticle> articles, Language lang)
    {
      List<ArticleInfoContext> infos = new List<ArticleInfoContext>();      
      foreach (IArticle art in articles)
      {
        JournalArticle article = new JournalArticle(myNumber, art, this, myManager);
        AddContext(article);
        IArticleInfo info = article.Article.ForLanguage(LanguageUtil.Convert(lang));
        infos.Add(new ArticleInfoContext(info, article.LinkTemplate.ToLink(lang, LinkTemplate), 
                                         Program.Instance.PdfManager.RegisterPdf(article.Article, lang, LinkTemplate)));
      }
      return infos;
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      base.AppendLanguageContextInternal(language, ctx);

      ctx.Add("issue", myNumber);      
      ctx.Add("sections", BuildSectionInfos(myNumber.Sections, language));
    }

    public class NumberSectionInfo
    {
      private readonly List<ArticleInfoContext> myArticles;
      private readonly bool myShowTitle;
      private readonly string myTitleName;


      public NumberSectionInfo(List<ArticleInfoContext> articles, bool showTitle, string titleName)
      {
        myArticles = articles;
        myShowTitle = showTitle;
        myTitleName = titleName;
      }

      [UsedByStringTemplate]
      public List<ArticleInfoContext> Articles => myArticles;

      [UsedByStringTemplate]
      public bool ShowTitle => myShowTitle;

      [UsedByStringTemplate]
      public string TitleName => myTitleName;
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

      [UsedByStringTemplate]
      public IArticleInfo Info => myInfo;

      [UsedByStringTemplate]
      public Link Link => myLink;

      [UsedByStringTemplate]
      public PdfLink Pdf => myPdf;

      [UsedByStringTemplate]
      public bool HasReferences => myInfo.HasReferences;

      [UsedByStringTemplate]
      public bool HasAbstracts => myInfo.HasAbstracts;

      [UsedByStringTemplate]
      public bool HasKeywords => myInfo.HasKeywords;
    }
  }
}
