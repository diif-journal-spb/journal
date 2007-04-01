using System;
using System.Collections.Generic;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public class JournalContentsContext : HtmlGenerationContext
  {
    private readonly IJournal myJournal;
    private readonly Dictionary<INumber, JournalNumberContext> myNumbers = new Dictionary<INumber, JournalNumberContext>();

    public JournalContentsContext(LinkManager manager, IJournal journal) : base(manager, "journal_content")
    {
      myJournal = journal;
      foreach (INumber number in myJournal.Numbers)
      {
        myNumbers.Add(number, new JournalNumberContext(manager, number));
      }

      AddContextRange(myNumbers.Values);
    }

    public override LinkTemplate GetLinkTemplate(LinkManager manager)
    {
      return new LinkTemplate(manager, "journal.contents.html");
    }

    protected override void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      Dictionary<int, List<INumber>> numbers = new Dictionary<int, List<INumber>>();
      foreach (INumber number in myJournal.Numbers)
      {
        List<INumber> ns;
        if (!numbers.TryGetValue(number.IntYear, out ns))
        {
          ns = new List<INumber>();
        } 
        ns.Add(number);
        numbers[number.IntYear] = ns;
      }

      Comparison<INumber> cmdNumber = delegate(INumber n1, INumber n2)
                                {
                                  return n1.IntNumber - n2.IntNumber;
                                };

      List<int> years = new List<int>(numbers.Keys);
      years.Sort(delegate(int i1, int i2) { return i2 - i1; });

      List<Pair<string, List<Pair<string, Link>>>> data = new List<Pair<string, List<Pair<string, Link>>>>();

      foreach (int year in years)
      {
        List<INumber> list = numbers[year];
        list.Sort(cmdNumber);
        List<Pair<string, Link>> d = new List<Pair<string, Link>>();
        foreach (INumber number in list)
        {
          d.Add(new Pair<string, Link>(number.Number, myNumbers[number].LinkTemplate.ToLink(language)));
        }
        data.Add(new Pair<string, List<Pair<string, Link>>>(year.ToString(), d));
      }

      ctx.Add("content", data);

      base.AppendLanguageContextInternal(language, ctx);
    }
  }
}