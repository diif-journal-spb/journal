using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Antlr.StringTemplate;

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


    public Program(CommandLineParser commandLineParser)
    {
      myInstance = this;
      myCommandLine = commandLineParser;
      myLinkManager = new LinkManager(myCommandLine.GetValue("url"), myCommandLine.GetValue("dest"));

      string path = GetType().Assembly.CodeBase.Substring("file:///".Length);
      path = Path.GetFullPath(Path.Combine(path, "../../../../../../templates"));

      Console.Out.WriteLine("Template path = {0}, exists = [{1}]", path, Directory.Exists(path));

      myTemplates = new Dictionary<Language, StringTemplateGroup>();

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

    public void GenerateWebSite(IList<GenerationContext> ctxs)
    {
      foreach (GenerationContext ctx in ctxs)
      {
        GeneratePage(ctx);
      }
    }

    public void GeneratePage(GenerationContext ctx)
    {
      Console.Out.WriteLine("Generatign page {1}: {0}...", ctx.LinkTemplate, ctx.TemplateName);

      foreach (Language language in myLanguages)
      {
        StringTemplate template = myTemplates[language].GetInstanceOf(ctx.TemplateName);

        LanguageGenerationContext context = ctx.LanguageContext(language);
        foreach (KeyValuePair<string, object> pair in context.Attributes)
        {
          template.SetAttribute(pair.Key, pair.Value);
        }

        string path = Path.GetDirectoryName(context.DestFile);
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);

        using (TextWriter tw = File.CreateText(context.DestFile))
        {
          tw.WriteLine(template);
        }        
      }
      
      Console.Out.WriteLine("Done");
    }

    public void Test()
    {
      GenerationContext ctx = new GenerationContext(new Link(myLinkManager, Language.RU, "index.html"), "index");
      GeneratePage(ctx);
    }

    static void Main(string[] _args)
    {
      string[] args = new string[] {"/url=http://test", @"/dest=c:\tmp\"};
      Program program = new Program(new CommandLineParser(args));
      program.Test();
    }
  }
}
