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

    public string DestFile
    {
      get { return myDestFile; }
    }
  }

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