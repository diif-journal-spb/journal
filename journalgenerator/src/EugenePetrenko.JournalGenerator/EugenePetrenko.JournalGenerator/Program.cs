using System;
using System.Collections;
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

    private readonly Hashset<HtmlGenerationContext> myProcessedPages = new Hashset<HtmlGenerationContext>();
    private readonly Queue<HtmlGenerationContext> myQueue = new Queue<HtmlGenerationContext>();

    private readonly Dictionary<string, ConvertToLanguage> myGlobalContext = new Dictionary<string, ConvertToLanguage>();

    public Program(CommandLineParser commandLineParser)
    {
      myInstance = this;
      myCommandLine = commandLineParser;
      string destFile = myCommandLine.GetValue("dest");

      try
      {
        FileUtil.SmartDelete(destFile);
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

      FileUtil.Copy(Path.Combine(path, "shared"), destFile);

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
      Console.Out.WriteLine("\r\nGeneratign page {1}: {0}...", ctx, ctx.TemplateName);

      foreach (Language language in myLanguages)
      {
        FileLanguageGenerationContext context = ctx.LanguageContext(language);
        context.GeneratePageToFile();
      }
      
      Console.Out.WriteLine("Done\r\n");
    }

    public void AddPredefinedProperties(IDictionary dic, Language lang)
    {
      foreach (KeyValuePair<string, ConvertToLanguage> pair in myGlobalContext)
      {
        if (!dic.Contains(pair.Key))
          dic.Add(pair.Key, pair.Value(lang));
      }
    }

    public void BuildPages()
    {
      myQueue.Enqueue(new JournalContentsContext(myLinkManager, myJournal));
      myQueue.Enqueue(new NewsGenerationContext(myJournal, myLinkManager));
      myQueue.Enqueue(new BooksGenerationContext(myJournal, myLinkManager));

      while(myQueue.Count > 0 )
      {
        HtmlGenerationContext ctx = myQueue.Dequeue();
        if (myProcessedPages.Contains(ctx))
          continue;

        GeneratePage(ctx);

        myProcessedPages.Add(ctx);
      }
    }

    public void AddPage(HtmlGenerationContext page)
    {
      myQueue.Enqueue(page);
    }

    public void AppendGlobalContext(string name, HtmlGenerationContext ctx)
    {
      myGlobalContext.Add(name, ctx.LinkTemplate.ToLink);
    }

    public Dictionary<Language, StringTemplateGroup> Templates
    {
      get { return myTemplates; }
    }

    static void Main(string[] _args)
    {
      string[] args = new string[] {@"/url=file:\\\c:\tmp\", @"/dest=c:\tmp\"};
      Program program = new Program(new CommandLineParser(args));
      program.BuildPages();
    }
  }
}
