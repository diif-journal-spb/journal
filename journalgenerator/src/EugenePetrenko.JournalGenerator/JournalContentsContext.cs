using System;
using System.Collections.Generic;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public class JournalContentsContext : HtmlGenerationContext
  {
    private readonly IJournal myJournal;
    private readonly Dictionary<INumber, JournalNumberContext> myNumbers = new Dictionary<INumber, JournalNumberContext>();

    public JournalContentsContext(LinkManager manager, IJournal journal) : base(manager, "collection")
    {
      myJournal = journal;
      foreach (INumber number in myJournal.Numbers)
      {
        myNumbers.Add(number, new JournalNumberContext(manager, number));
      }

      AddContextRange(myNumbers.Values);

      Program.Instance.AppendGlobalContext("CollectionLink", this);
    }

    public override LinkTemplate GetLinkTemplate(LinkManager manager)
    {
      return new LinkTemplate(manager, "collection.html");
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

      List<YearToNumbers> data = new List<YearToNumbers>();

      foreach (int year in years)
      {
        List<INumber> list = numbers[year];
        list.Sort(cmdNumber);
        List<NumberToLink> d = new List<NumberToLink>();
        foreach (INumber number in list)
        {
          d.Add(new NumberToLink(number.Number, myNumbers[number].LinkTemplate.ToLink(language, LinkTemplate)));
        }
        data.Add(new YearToNumbers(year.ToString(), d));
      }

      ctx.Add("content", data);

      base.AppendLanguageContextInternal(language, ctx);
    }

    public struct YearToNumbers
    {
      private readonly string myYear;
      private readonly List<NumberToLink> myNumbers;

      public YearToNumbers(string year, List<NumberToLink> numbers)
      {
        myYear = year;
        myNumbers = numbers;
      }

      public string Year
      {
        get { return myYear; }
      }

      public List<NumberToLink> Issues
      {
        get { return myNumbers; }
      }
    }

    public struct NumberToLink
    {
      private readonly string myNumber;
      private readonly Link myLink;

      public NumberToLink(string number, Link link)
      {
        myNumber = number;
        myLink = link;
      }

      public string IssueNumber
      {
        get { return myNumber; }
      }

      public Link IssueLink
      {
        get { return myLink; }
      }
    }
  }
}