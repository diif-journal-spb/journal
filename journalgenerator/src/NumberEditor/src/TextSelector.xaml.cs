using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using MIMER.RFC2045;

namespace EugenePetrenko.NumberEditor
{
  /// <summary>
  /// Interaction logic for TextSelector.xaml
  /// </summary>
  public partial class TextSelector
  {
    private IEnumerable<string> myLanguages = new[] {"EN"};
    private LocalizedArticleXml myArticle = new LocalizedArticleXml();
    private List<LocalizedAuthorXml> myAuthors = new List<LocalizedAuthorXml>();
    private bool myIsInitialized;

    public TextSelector()
    {
      InitializeComponent();
      Dispatcher.Invoke(DispatcherPriority.Loaded, (Action)(() =>
      {
        myIsInitialized = true;
      }));
    }


    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      var dlg = new OpenFileDialog {Multiselect = true, RestoreDirectory = true};
      if (true != dlg.ShowDialog()) return;

      myText.Text += FileLoader.LoadAllFiles(dlg.FileNames.ToList());
    }


    private void MenuItem_Click_1(object sender, RoutedEventArgs e)
    {
      myText.Clear();
      myLanguages = new[] {"EN"};
      myArticle = new LocalizedArticleXml();
      myArticle.InitLanguages(myLanguages);
      myAuthors = new List<LocalizedAuthorXml>();
      AddAuthor();
      //TODO: add more actions here

      UpdateXml();
    }

    private void MenuItem_Click_2(object sender, RoutedEventArgs e)
    {
      myText.Clear();
      myLanguages = new[] { "EN", "RU" };
      myArticle = new LocalizedArticleXml();
      myArticle.InitLanguages(myLanguages);
      myAuthors = new List<LocalizedAuthorXml>();
      AddAuthor();
      UpdateXml();
    }

    private int myUpdateVersion;
    private void UpdateXml()
    {
      if (!myIsInitialized) return;

      //hack-style update's cleaup
      int version = ++myUpdateVersion;
      Action update = () =>
                        {
                          if (version != myUpdateVersion) return;
                          myArticle.SetAuthors(myAuthors); 
                          myArticleXml.Text = myArticle.Serialize();
                          myAuthorXml.Text = string.Join("\r\n\r\n", myAuthors.Select(x => x.Serialize()));
                        };
      Dispatcher.Invoke(DispatcherPriority.Normal, TimeSpan.FromMilliseconds(500), update);
    }

    private void myText_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!myIsInitialized) return;
      UpdateXml();
    }

    private void myText_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      if (!myIsInitialized) return;

      var menu = new ContextMenu {ItemsSource = GetAvailableCommands().Select(x => x()).ToArray()};
      myText.ContextMenu = menu;
      myText.ContextMenu.IsOpen = true;
    }

    private string RuTitleCase(string s)
    {
      s = s.Trim();
      if (s.Length == 0) return "";
      var x = String.Join(" ", Regex.Split(s, @"\s+").Where(_ => _.Length > 0).ToArray()).Trim();
      return Char.ToUpper(x[0]) + (x.Length == 1 ? "" : x.Substring(1).ToLower()).Trim();
    }

    private string EnTitleCase(string s)
    {
      var pros = new[]
                   {
                     "and", "on", "from", "a", "to", "the", "of", "for", "any", "at", "in", "into", "this", "the", "that", "then",
                     "them", "with", "out", "over", "by", "an", "to", "into", "across", "below", "abowe", "without"
                   }.ToLookup(x => x, StringComparer.InvariantCultureIgnoreCase);

      var trim = Regex.Split(s, @"\s+")
        .Select(x => x.Trim())
        .Where(x => x.Length > 0)
        .Select((x,i) => i != 0 && pros.Contains(x) ? x.ToLower() : RuTitleCase(x))
        ;

      return String.Join(" ", trim).Trim().TrimEnd(".,;! \t\r\n".ToCharArray()).Trim();
    }

    private string TitleCase(string lang, string text)
    {
      if (lang == "EN") return EnTitleCase(text);
      if (lang == "RU") return RuTitleCase(text);
      return text;
    }

    private IEnumerable<Func<object>> GetAvailableCommands()
    {
      var selection = (myText.SelectedText ?? "").Trim();

      yield return () => new MenuItem
                           {
                             Header = "_Article",
                             IsEnabled = selection.Length > 0,
                             ItemsSource = ArticleActions(myArticle, selection).Select(x => x()).ToArray()
                           };

      yield return () => new MenuItem
      {
        Header = "_New Author",
        IsEnabled = true,
        ItemsSource = new []
                        {
                          new ACommand("Add _Author", UIAction(AddAuthor)).CreateMenuItem(),
                          new ACommand("Add _Exact Author", UIAction(() => myAuthors.Add(new LocalizedAuthorXml {Id=selection}))).CreateMenuItem()
                        }
      };

      int cnt = 1;
      foreach (var _author in myAuthors)
      {
        var author = _author;
        var e = author.GetEN();
        var name = "_" + cnt++ + "@" + (e.FirstName ?? "") + " " + (e.MiddleName ?? "") + " " + (e.LastName ?? "") + " " + (author.Id ?? "");
        yield return () => new MenuItem
                             {
                               IsEnabled = selection.Length > 0,
                               Header = name.Trim(),
                               ItemsSource = AuthorActions(author, selection).Select(x=>x()).ToArray()
                             };
      }

      yield return () => new Separator();

      yield return new ACommand("Fix Word HTML", UIAction(() =>
      {
        var html = HTMLHelpers.FixWordHTML(selection);

        myText.AppendText("\n\n\n" + html + "\n\n\n");
      })).CreateMenuItem; 
      
      yield return new ACommand("Fix bibitems into HTML", UIAction(() =>
      {
        var html = TEXHelpers.FixTexIntoHTML(selection);

        myText.AppendText("\n\n\n" + html + "\n\n\n");
      })).CreateMenuItem;

    }

    private void AddAuthor()
    {
      var locId = new LocalizedAuthorXml {Id = LocalizedAuthorXml.DefaultIdPrefix};
      locId.InitLanguages(myLanguages);
      myAuthors.Add(locId);
    }

    private IEnumerable<Func<object>> ArticleActions(LocalizedArticleXml article, string selection)
    {
      Func<string, ArticleInfoXml> aSelector = lang => lang == "RU" ? article.GetRU() : article.GetEN();

      foreach (var _lang in myLanguages)
      {
        var lang = _lang;
        var commands
          = new Func<object>[]
              {
                new ACommand("Set Article _Title " + lang, UIAction(() => { aSelector(lang).Title = TitleCase(lang, selection); })).CreateMenuItem,
                new ACommand("Set Article _Abstract " + lang, UIAction(() => { aSelector(lang).Abstract = selection; })).CreateMenuItem,
                () => new Separator(), 
                new ACommand("Set Keywords " + lang, UIAction(() => { aSelector(lang).SetKeywords(KeywordsParser.ParseKeywords(selection)); })).CreateMenuItem, 
                new ACommand("Remove Keywords " + lang, UIAction(() => { aSelector(lang).Keywords = null; })).CreateMenuItem, 
                () => new Separator(),
                new ACommand("Add _references " + lang, UIAction(() => aSelector(lang).AddReferences(ReferencesParser.ParseReferences(selection)))).CreateMenuItem,
                new ACommand("Remove reference " + lang, UIAction(() => aSelector(lang).RemoveReference())).CreateMenuItem
              };

        yield return () => new MenuItem
                             {
                               Header = "_" + lang,
                               IsEnabled = true,
                               ItemsSource = commands.Select(x=>x()).ToArray()
                             };
      }
      yield return () => new Separator();
      yield return new ACommand("Set First-Last _Pages", UIAction(() =>
                                                                   {
                                                                     var nums = selection.Trim().Split("-".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                                                                     if (nums.Length != 2) return;
                                                                     article.Items.ForEach(x => x.FirstPage = int.Parse(nums[0]));
                                                                     article.Items.ForEach(x => x.LastPage = int.Parse(nums[1]));
                                                                   })).CreateMenuItem;
      
      yield return new ACommand("Set First Page", UIAction(() => article.Items.ForEach(x => x.FirstPage = int.Parse(selection)))).CreateMenuItem;
      yield return new ACommand("Set Last Page", UIAction(() => article.Items.ForEach(x => x.LastPage = int.Parse(selection)))).CreateMenuItem;
      
      yield return () => new Separator();
      yield return new ACommand("Set pd_f", UIAction(() => article.Items.ForEach(x => x.Pdf = selection))).CreateMenuItem;
    }

    private IEnumerable<Func<object>> AuthorActions(LocalizedAuthorXml author, string selection)
    {
      foreach (var _lang in myLanguages)
      {
        var lang = _lang;
        
        Func<AuthorXml> RU = author.GetRU;
        Func<AuthorXml> EN = author.GetEN;
        var aSel = _lang == "RU" ? RU : EN;

        yield return () => new MenuItem
        {
          Header = "_" + lang,
          IsEnabled = true,
          ItemsSource = LangAuthorCommands(author, selection, aSel).Select(x => x()).ToArray()
        };
      } 
      yield return () => new Separator();
      yield return new ACommand("Set _Email ", UIAction(() => author.Items.ForEach(x => x.Email = selection))).CreateMenuItem;
      yield return () => new Separator();
      yield return new ACommand("Remove", UIAction(() => myAuthors.Remove(author))).CreateMenuItem;
    }

    private IEnumerable<Func<object>> LangAuthorCommands(LocalizedAuthorXml author, string selection, Func<AuthorXml> aSel)
    {
      yield return new ACommand("Set _First Name ", UIAction(() => aSel().FirstName = selection)).CreateMenuItem;
      yield return new ACommand("Set _Middle Name ", UIAction(() => aSel().MiddleName = selection)).CreateMenuItem;
      yield return new ACommand("Set _Last Name ", UIAction(() => { aSel().LastName = selection; author.UpdateId(); })).CreateMenuItem;
      yield return () => new Separator();

      Func<string[]> split3 = () => Regex.Split(selection, @"[\.\s,]+")
          .Select(x => x.Trim())
          .Where(x => x.Length > 0)
          .Select(x => x.Length <= 2 ? x + "." : x)
          .ToArray();

      yield return new ACommand("Set 'First Middle Las_t' Name", UIAction(() =>
      {
        string[] text = split3();
        if (text.Length == 3)
        {
          aSel().FirstName = text[0];
          aSel().MiddleName = text[1];
          aSel().LastName = text[2];
        }
        author.UpdateId();
      })).CreateMenuItem;
      
      yield return new ACommand("Set 'Last First Middl_e' Name", UIAction(() =>
      {
        string[] text = split3();

        if (text.Length == 3)
        {
          aSel().LastName = text[0];
          aSel().FirstName = text[1];
          aSel().MiddleName = text[2];
        }
        author.UpdateId();
      })).CreateMenuItem;
      yield return () => new Separator();
      yield return new ACommand("Set _Address ", UIAction(() => aSel().Address = selection)).CreateMenuItem;
    }

    private Action UIAction(Action a)
    {
      return () =>
               {
                 try
                 {
                   a();
                 }
                 catch (Exception e)
                 {
                   MessageBox.Show(e.ToString());
                 }
                 UpdateXml();
               };
    }


    private class ACommand : ICommand
    {
      private readonly Action myExecuteAction;
      private readonly string myName;

      public ACommand(string name, Action executeAction)
      {
        myName = name;
        myExecuteAction = executeAction;
      }

      public object CreateMenuItem()
      {
        return new MenuItem {Header = myName, Command = this};
      }

      public void Execute(object parameter)
      {
        myExecuteAction();
      }

      public bool CanExecute(object parameter)
      {
        return true;
      }

      public event EventHandler CanExecuteChanged;
    }

    private void CopyAuthors_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Clipboard.SetText(myAuthorXml.Text);
      }
      catch
      {
        //NOP
      }
    }

    private void CopyArticles_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Clipboard.SetText(myArticleXml.Text);
      }
      catch
      {
        //NOP
      }
    }
  }
}
