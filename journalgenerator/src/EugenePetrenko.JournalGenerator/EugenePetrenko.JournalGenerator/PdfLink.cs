using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public sealed class PdfLink : LinkTemplate
  {
    public PdfLink(LinkManager linkManager, IArticleInfo info)
      : base(linkManager, info.Pdf.Replace('\\', '/'))
    {
    }

    public override string ToString()
    {
      return myLinkManager.Combine('/', myLinkManager.GenerationBaseUrl, "pdf", myPageName);
    }
  }
}