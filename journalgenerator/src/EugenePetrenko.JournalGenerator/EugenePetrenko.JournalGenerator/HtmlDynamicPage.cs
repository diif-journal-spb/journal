namespace EugenePetrenko.JournalGenerator
{
  public class HtmlDynamicPage : HtmlGenerationContext
  {
    public HtmlDynamicPage(LinkManager manager, string templateName) : base(manager, templateName)
    {
    }

    public override LinkTemplate GetLinkTemplate(LinkManager manager)
    {
      return new LinkTemplate(manager, TemplateName + ".html");
    }
  }
}