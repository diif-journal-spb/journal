using System.IO;

namespace EugenePetrenko.JournalGenerator
{
  public class LinkManager
  {
    private readonly string myGenerationUrl;
    private readonly string myGeneratePath;

    public string GenerationBaseUrl { get { return myGenerationUrl; } }

    public string GeneratePath
    {
      get { return myGeneratePath; }
    }

    public LinkManager(string generationUrl, string generatePath)
    {
      myGenerationUrl = generationUrl;
      myGeneratePath = Path.GetFullPath(generatePath);
    }
  }
}