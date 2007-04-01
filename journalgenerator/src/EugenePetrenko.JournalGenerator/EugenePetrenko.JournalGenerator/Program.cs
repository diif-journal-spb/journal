using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Antlr.StringTemplate;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public class Program
  {
    private static Program myInstance = null;
    public static Program Instance
    {
      get { return myInstance; }
    }

    private readonly List<Language> myLanguages;
    private readonly CommandLineParser myCommandLine;
    private readonly LinkManager myLinkManager;
    private readonly Dictionary<Language, StringTemplateGroup> myTemplates;
    private readonly IJournal myJournal;

    private static void Copy(string fromDir, string toDir)
    {
      if (!Directory.Exists(toDir))
        Directory.CreateDirectory(toDir);

      foreach (string file in Directory.GetFiles(fromDir))
      {
        File.Copy(file, Path.Combine(toDir, Path.GetFileName(file)));        
      }

      foreach (string dir in Directory.GetDirectories(fromDir))
      {
        Copy(dir, Path.Combine(toDir, Path.GetFileName(dir)));
      }
    }

    public Program(CommandLineParser commandLineParser)
    {
      myInstance = this;
      myCommandLine = commandLineParser;
      string destFile = myCommandLine.GetValue("dest");

      try
      {
        if (Directory.Exists(destFile))
          Directory.Delete(destFile, true);
      } catch
      {
        Console.Out.WriteLine("Umable to clean up output folder {0}", destFile);
      }

      myLinkManager = new LinkManager(myCommandLine.GetValue("url"), destFile);

      string path = GetType().Assembly.CodeBase.Substring("file:///".Length);
      path = Path.GetFullPath(Path.Combine(path, "../../../../../../templates"));

      string data = Path.Combine(Path.GetDirectoryName(path), "data");
      myJournal = XmlDataLoader.Parse(data);

      Console.Out.WriteLine("Template path = {0}, exists = [{1}]", path, Directory.Exists(path));

      myTemplates = new Dictionary<Language, StringTemplateGroup>();

      Copy(Path.Combine(path, "shared"), destFile);

      myLanguages = new List<Language>();
      foreach (FieldInfo info in typeof(Language).GetFields(BindingFlags.Public | BindingFlags.Static))
      {
        Language lang = (Language) info.GetValue(null);
        myLanguages.Add(lang);
        string tpath = Path.Combine(path, lang.ToString());

        Console.Out.WriteLine("Loading template for lang {0} from {1}", lang, tpath);

        myTemplates[lang] = new StringTemplateGroup("journal_" + lang, tpath);
      }
    }

    public void GeneratePage(HtmlGenerationContext ctxp)
    {
      HtmlContext ctx = new HtmlContext(myLinkManager, ctxp);
      Console.Out.WriteLine("Generatign page {1}: {0}...", ctx, ctx.TemplateName);

      foreach (Language language in myLanguages)
      {
        FileLanguageGenerationContext context = ctx.LanguageContext(language);
        context.GeneratePageToFile();
      }
      
      Console.Out.WriteLine("Done");
    }

    public void Test()
    {
      Queue<HtmlGenerationContext> data = new Queue<HtmlGenerationContext>();
      data.Enqueue(new JournalContentsContext(myLinkManager, myJournal));

      while(data.Count > 0 )
      {
        HtmlGenerationContext ctx = data.Dequeue();
        GeneratePage(ctx);
        foreach (HtmlGenerationContext page in ctx.ExtraPages)
        {
          data.Enqueue(page);
        }
      }
//      GenerationContext ctx = new GenerationContext(new Link(myLinkManager, Language.RU, "index.html"), "index");
//      GeneratePage(ctx);
    }

    public List<Language> Languages
    {
      get { return myLanguages; }
    }

    public Dictionary<Language, StringTemplateGroup> Templates
    {
      get { return myTemplates; }
    }

    static void Main(string[] _args)
    {
      string[] args = new string[] {@"/url=file:\\\c:\tmp\", @"/dest=c:\tmp\"};
      Program program = new Program(new CommandLineParser(args));
      program.Test();
    }
  }
}
