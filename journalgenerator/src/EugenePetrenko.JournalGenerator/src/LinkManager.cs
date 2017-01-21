using System.IO;
using System.Text;

namespace EugenePetrenko.JournalGenerator
{
  public class LinkManager
  {
    private static readonly char[] SEP = new char[] { '\\', '/' };

    private readonly string myGenerationUrl;
    private readonly string myGeneratePath;

    public string GenerationBaseUrl => myGenerationUrl;

    public string GeneratePath => myGeneratePath;

    public LinkManager(string generationUrl, string generatePath)
    {
      myGenerationUrl = generationUrl;
      myGeneratePath = Path.GetFullPath(generatePath);
    }

    public string Combine(char sep, string s1, string s2)
    {
      return s1.TrimEnd(SEP) + sep + s2.TrimStart(SEP);
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
    
    public static string MakeRelative(string path, string pathBase)
    {
      string[] pathElements = path.Split(SEP);
      string[] pathBaseElements = pathBase.Split(SEP);

      int index = 0;

      for (; 
        index < pathElements.Length 
        && index < pathBaseElements.Length 
        && pathElements[index] == pathBaseElements[index]; index++ )
      {
      }

      if (index == pathElements.Length && index == pathBaseElements.Length)
      {
        return pathBaseElements[pathBaseElements.Length - 1];
      } 

      StringBuilder builder = new StringBuilder();
      for (int i = index; i < pathBaseElements.Length - 1; i++)
      {
        builder.Append("../");
      }

      for (int i = index; i < pathElements.Length; i++)
      {
        builder.Append(pathElements[i]).Append("/");
      }
      return builder.ToString().TrimEnd(SEP);
    }

    public string GetRootLink(Language lang, LinkTemplate page)
    {
      return MakeRelative(RootLink, page.ToLink(lang, null).ToString());
    }

    public string RootLink => myGenerationUrl.Replace('\\', '/').TrimEnd('/');

    public string GetRootLinkForLanguage(Language lang, LinkTemplate paht)
    {
      return MakeRelative(Combine('/', RootLink, lang).TrimEnd('/'), paht.ToLink(lang, null).ToString());
    }
  }
}
