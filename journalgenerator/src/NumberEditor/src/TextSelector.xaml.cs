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

    public TextSelector()
    {
      InitializeComponent();
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      var dlg = new OpenFileDialog {Multiselect = true, RestoreDirectory = true};
      if (true != dlg.ShowDialog()) return;

      foreach (var file in dlg.FileNames)
      {
        myText.Text += "\r\n\r\n----" + file + "\r\n\r\n";

        if (file.EndsWith(".pdf", StringComparison.CurrentCultureIgnoreCase)) continue;
        try
        {
          using (var rdr = new StreamReader(file, Encoding.GetEncoding("windows-1251")))
          {
            myText.Text += rdr.ReadToEnd().Trim();
          }
        }
        catch (Exception ee)
        {
          myText.Text += ee.ToString();
        }
      }

      myText.Text += string.Join("\r\n", dlg.FileNames);
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
      UpdateXml();
    }

    private void myText_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
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
      var trim = string.Join(" ", Regex.Split(s, @"\s+").Select(RuTitleCase).ToArray()).Trim();
      var pros = new[]
                   {
                     "from", "a", "to", "the", "of", "for", "any", "at", "in", "into", "this", "the", "that", "then",
                     "them", "with", "out", "ower", "by", "an", "to", "into", "across", "below", "abowe", "without", 
                     
                   };
      return pros.Aggregate(trim, (current, pro) => current.Replace(RuTitleCase(pro), pro));
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
    }

    private void AddAuthor()
    {
      var locId = new LocalizedAuthorXml {Id = "n2_2012_"};
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
                new ACommand("Set Article _Abstract " + lang, UIAction(() => { aSelector(lang).Abstract = selection; })).CreateMenuItem
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
      yield return new ACommand("Set pd_f", UIAction(() => article.Items.ForEach(x => x.Pdf = selection))).CreateMenuItem;
    }

    private IEnumerable<Func<object>> AuthorActions(LocalizedAuthorXml author, string selection)
    {
      Func<string, AuthorXml> aSel = lang => lang == "RU" ? author.GetRU() : author.GetEN();
      foreach (var _lang in myLanguages)
      {
        var lang = _lang;
        var commands = new Func<object>[] {
          new ACommand("Set _First Name ", UIAction(() => aSel(lang).FirstName = selection)).CreateMenuItem,
          new ACommand("Set _Middle Name ", UIAction(() => aSel(lang).MiddleName = selection)).CreateMenuItem,
          new ACommand("Set _Last Name ", UIAction(() => { aSel(lang).LastName = selection; author.UpdateId(); })).CreateMenuItem,
          new ACommand("Set _Address ", UIAction(() => aSel(lang).Address = selection)).CreateMenuItem,
        };

        yield return () => new MenuItem
        {
          Header = "_" + lang,
          IsEnabled = true,
          ItemsSource = commands.Select(x => x()).ToArray()
        };
      } 
      yield return () => new Separator();
      yield return new ACommand("Set _Email ", UIAction(() => author.Items.ForEach(x => x.Email = selection))).CreateMenuItem;
      yield return () => new Separator();
      yield return new ACommand("Remove", UIAction(() => myAuthors.Remove(author))).CreateMenuItem;
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
      Clipboard.SetText(myAuthorXml.Text);
    }

    private void CopyArticles_Click(object sender, RoutedEventArgs e)
    {
      Clipboard.SetText(myArticleXml.Text);
    }
  }
}
