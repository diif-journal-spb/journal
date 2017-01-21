using System.IO;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public class FileLanguageGenerationContext : LanguageGenerationContext
  {
    private readonly string myDestFile;

    public FileLanguageGenerationContext(StringTemplate template, SmartLookupDictionary attributes, string destFile) : base(template, attributes)
    {
      myDestFile = destFile;
    }

    public virtual void GeneratePageToFile()
    {
      string path = Path.GetDirectoryName(DestFile);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      using (TextWriter tw = File.CreateText(DestFile))
        tw.WriteLine(GeneratePage());
    }

    public string DestFile => myDestFile;
  }
}
