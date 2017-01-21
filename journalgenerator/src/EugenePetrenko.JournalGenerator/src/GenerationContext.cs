namespace EugenePetrenko.JournalGenerator
{
  public delegate object ConvertToLanguage(Language language, LinkTemplate page);

  public abstract class GenerationContext
  {
    public const string CAPTION_KEY = "caption"; 

    protected readonly LinkManager myManager;

    protected GenerationContext(LinkManager manager)
    {
      myManager = manager;
    }
  }
}
