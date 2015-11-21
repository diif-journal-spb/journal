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
      string name = string.Format(@"numbers\{0}.{1}\issue.html", myNumber.Year, myNumber.Number);
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
      public List<ArticleInfoContext> Articles
      {
        get { return myArticles; }
      }

      [UsedByStringTemplate]
      public bool ShowTitle
      {
        get { return myShowTitle; }
      }

      [UsedByStringTemplate]
      public string TitleName
      {
        get { return myTitleName; }
      }
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
      public IArticleInfo Info
      {
        get { return myInfo; }
      }

      [UsedByStringTemplate]
      public Link Link
      {
        get { return myLink; }
      }

      [UsedByStringTemplate]
      public PdfLink Pdf
      {
        get { return myPdf; }
      }

      [UsedByStringTemplate]
      public bool HasReferences
      {
        get { return myInfo.HasReferences; }
      }

      [UsedByStringTemplate]
      public bool HasKeywords
      {
        get { return myInfo.HasKeywords; }
      }
    }
  }
}