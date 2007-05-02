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

    public string Combine(char sep, string s1, string s2)
    {
      return s1.TrimEnd('/', '\\') + sep + s2.TrimStart('/', '\\');
    }

    public string Combine(char sep, params object[] ss)
    {
      string result = ss[0].ToString();
      for (int i = 1; i < ss.Length; i++)
      {
        result = Combine(sep, result, ss[i].ToString());
      }
      return result;
    }

    public string RootLink
    {
      get { return myGenerationUrl.Replace('\\', '/').TrimEnd('/'); }
    }

    public string GetRootLink(Language lang)
    {
      return Combine('/', RootLink, lang).TrimEnd('/');
    }
  }
}