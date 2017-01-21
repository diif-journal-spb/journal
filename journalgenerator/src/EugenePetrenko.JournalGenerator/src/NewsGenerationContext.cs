using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public class NewsGenerationContext : CollectionGenerationContext<INewsItem, ILocalizedNewsItem>
  {
    public NewsGenerationContext(IJournal journal, LinkManager manager) : base("news", journal.News, manager, "news")
    {
      Program.Instance.AppendGlobalContext("NewsLink", this);
    }

    public override LinkTemplate GetLinkTemplate(LinkManager manager)
    {
      return new LinkTemplate(myManager, "news.html");
    }
  }
}
