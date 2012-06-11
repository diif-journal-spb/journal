using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
      var dlg = new OpenFileDialog {Multiselect = true};
      if (true != dlg.ShowDialog()) return;

      foreach (var file in dlg.FileNames)
      {
        myText.Text += "\r\n\r\n----" + file + "\r\n\r\n";
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

      var x = String.Join(" ", (string[]) Regex.Split(s, @"\s+").Where(_ => _.Length > 0));
      x = Char.ToUpper(x[0]) + x.Substring(1).ToLower();
      return x;
    }

    private string EnTitleCase(string s)
    {
      return string.Join(" ", Regex.Split(s, @"\s+").Select(RuTitleCase));
    }

    private IEnumerable<Func<object>> GetAvailableCommands()
    {
      var selection = (myText.SelectedText ?? "").Trim();

      yield return new ACommand("To Title Case: RU", UIAction(() =>
                                                                {
                                                                  var text = RuTitleCase(selection);
                                                                  myText.Text += "\r\n" + text;
                                                                  myText.SelectedText = text;
                                                                })).CreateMenuItem;
      yield return new ACommand("To Title Case: EN", UIAction(() =>
                                                                {
                                                                  var text = EnTitleCase(selection);
                                                                  myText.Text += "\r\n" + text;
                                                                  myText.SelectedText = text;
                                                                })).CreateMenuItem;

      yield return () => new MenuItem
                           {
                             Header = "Article",
                             IsEnabled = selection.Length > 0,
                             ItemsSource = ArticleActions(myArticle, selection).Select(x => x()).ToArray()
                           };
    
      yield return new ACommand("Add Author", UIAction(AddAuthor)).CreateMenuItem;
      yield return new ACommand("Add Exact Author", UIAction(() => myAuthors.Add(new LocalizedAuthorXml {Id=selection}))).CreateMenuItem;

      foreach (var _author in myAuthors)
      {
        var author = _author;
        yield return () =>
                       {
                         var e = author.GetEN();
                         var name = "@" + (e.FirstName ?? "") + " " + (e.MiddleName ?? "") + " " + (e.LastName ?? "");
                         return new MenuItem
                                  {
                                    IsEnabled = selection.Length > 0,
                                    Header = name,
                                    ItemsSource = AuthorActions(author, selection).Select(x=>x()).ToArray()
                                  };
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
        yield return new ACommand("Set Article Title " + lang, UIAction(() => { aSelector(lang).Title = selection; })).CreateMenuItem;
        yield return new ACommand("Set Article Abstract " + lang, UIAction(() => { aSelector(lang).Abstract = selection; })).CreateMenuItem;

        yield return () => new Separator();
      }
      yield return new ACommand("Set First Page", UIAction(() => article.Items.ForEach(x => x.FirstPage = int.Parse(selection)))).CreateMenuItem;
      yield return new ACommand("Set Last Page", UIAction(() => article.Items.ForEach(x => x.LastPage = int.Parse(selection)))).CreateMenuItem;
      yield return new ACommand("Set pdf", UIAction(() => article.Items.ForEach(x => x.Pdf = selection))).CreateMenuItem;
    }

    private IEnumerable<Func<object>> AuthorActions(LocalizedAuthorXml author, string selection)
    {
      Func<string, AuthorXml> aSel = lang => lang == "RU" ? author.GetRU() : author.GetEN();
      foreach (var _lang in myLanguages)
      {
        var lang = _lang;
        yield return new ACommand("Set First Name " + lang, UIAction(() => aSel(lang).FirstName = selection)).CreateMenuItem;
        yield return new ACommand("Set Middle Name " + lang, UIAction(() => aSel(lang).MiddleName = selection)).CreateMenuItem;
        yield return new ACommand("Set Last Name " + lang, UIAction(() => { aSel(lang).LastName = selection; author.UpdateId(); })).CreateMenuItem;
        yield return new ACommand("Set Address " + lang, UIAction(() => aSel(lang).Address = selection)).CreateMenuItem;
        yield return () => new Separator();
      }
      yield return new ACommand("Set Email ", UIAction(() => author.Items.ForEach(x => x.Email = selection))).CreateMenuItem;
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
