namespace EugenePetrenko.JournalGenerator
{
  public sealed class PdfLink : LinkTemplate
  {
    private readonly Link myPageLink;

    public PdfLink(LinkManager linkManager, string file, Link pageLink)
      : base(linkManager, file)
    {
      myPageLink = pageLink;
    }

    public string DestFile => myLinkManager.Combine('\\', myLinkManager.GeneratePath, "pdf", myPageName);

    public override string ToString()
    {
      return LinkManager.MakeRelative(myLinkManager.Combine('/', myLinkManager.GenerationBaseUrl, "pdf", myPageName), myPageLink.ToString());
    }
  }
}
