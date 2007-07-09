using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Antlr.StringTemplate;
using EugenePetrenko.DataModel;
using ICSharpCode.SharpZipLib.Zip;

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
    private readonly PdfManager myPdfManager;

    private readonly Hashset<HtmlGenerationContext> myProcessedPages = new Hashset<HtmlGenerationContext>();
    private readonly Queue<HtmlGenerationContext> myQueue = new Queue<HtmlGenerationContext>();

    private readonly Dictionary<string, ConvertToLanguage> myGlobalContext = new Dictionary<string, ConvertToLanguage>();

    public PdfManager PdfManager
    {
      get { return myPdfManager; }
    }

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
      
      string templates = GetType().Assembly.CodeBase.Substring("file:///".Length);
      templates = Path.GetFullPath(Path.Combine(templates, "../../../../../templates"));

      if (!Directory.Exists(templates))
      {
        Console.Out.WriteLine("Failed to find templates dir");
        return;
      }
      string data = Path.Combine(Path.GetDirectoryName(templates), "data");
      string version = DateTime.Now.ToString("yyyy-MM-dd");
      
      BackUp(data, Path.Combine(destFile, "backup\\data-" + version + ".zip"));
      BackUp(templates, Path.Combine(destFile, "backup\\templates-" + version + ".zip"));

      myJournal = XmlDataLoader.Parse(data);
      myPdfManager = new PdfManager(myLinkManager, Path.Combine(Path.GetDirectoryName(templates), "pdf"));

      Console.Out.WriteLine("Template path = {0}, exists = [{1}]", templates, Directory.Exists(templates));

      myTemplates = new Dictionary<Language, StringTemplateGroup>();

      FileUtil.Copy(Path.Combine(templates, "shared"), destFile);

      myLanguages = new List<Language>();
      foreach (Language lang in Enum.GetValues(typeof(Language)))
      {
        myLanguages.Add(lang);
        string tpath = Path.Combine(templates, lang.ToString());

        Console.Out.WriteLine("Loading template for lang {0} from {1}", lang, tpath);

        myTemplates[lang] = new StringTemplateGroup("journal_" + lang, tpath);
      }
    }

    private static void BackUp(string folder, string output)
    {
      string dir = Path.GetDirectoryName(output);
      if (!Directory.Exists(dir))
        Directory.CreateDirectory(dir);

      FastZip zip = new FastZip();
      zip.CreateEmptyDirectories = false;
      zip.CreateZip(output, folder, true, null);
    }


    public void GeneratePage(HtmlGenerationContext ctxp)
    {
      FileHtmlContext ctx = new FileHtmlContext(myLinkManager, ctxp);
      Console.Out.WriteLine("\r\nGeneratign page {1}: {0}...", ctx, ctx.TemplateName);

      foreach (Language language in myLanguages)
      {
        FileLanguageGenerationContext context = ctx.LanguageContext(language);
        context.GeneratePageToFile();
      }
      
      Console.Out.WriteLine("Done\r\n");
    }

    public void AddPredefinedProperties(IDictionary dic, Language lang, LinkTemplate page)
    {
      foreach (KeyValuePair<string, ConvertToLanguage> pair in myGlobalContext)
      {
        if (!dic.Contains(pair.Key))
          dic.Add(pair.Key, pair.Value(lang, page));
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

      myPdfManager.CopyFiles();
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
//      string[] args = new string[] { @"/url=http://diff.neva.ru/j/", string.Format(@"/dest=E:\Projects\journalGenerator\release\{0}", DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss")) };
      string[] args = new string[] {@"/url=file:\\\c:\tmp\", @"/dest=c:\tmp\"};
      Program program = new Program(new CommandLineParser(args));
      program.BuildPages();
    }
  }
}
