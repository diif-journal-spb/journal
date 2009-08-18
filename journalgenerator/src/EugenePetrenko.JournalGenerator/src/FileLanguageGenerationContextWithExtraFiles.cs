using System.IO;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public class FileLanguageGenerationContextWithExtraFiles : FileLanguageGenerationContext
  {
    private readonly string[] myExtraFiles;

    public FileLanguageGenerationContextWithExtraFiles(StringTemplate template, SmartLookupDictionary attributes, string destFile, params string[] extraFiles) : base(template, attributes, destFile)
    {
      myExtraFiles = extraFiles;
    }

    public override void GeneratePageToFile()
    {
      base.GeneratePageToFile();

      string path = Path.GetDirectoryName(DestFile);
      foreach(string file in myExtraFiles)
      {
        FileUtil.Copy(Path.Combine(Program.Instance.DataDir, file), Path.Combine(path, Path.GetFileName(file)));
      }
    }
  }
}