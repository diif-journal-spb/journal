using System;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IAuthorInfo : IEntity
  {
    JournalLanguage JournalLanguage { get;}

    string FirstName { get; }
    string MiddleName { get; }
    string LastName { get; }

    string EMail { get;}
    string Address { get; }
  }


  public class AuthorInfo : Entity, IAuthorInfo, IComparable<AuthorInfo>
  {
    private JournalLanguage myJournalLanguage;

    private readonly string myFirstName;
    private readonly string myMiddleName;
    private readonly string myLastName;
    private readonly string myEMail;
    private readonly string myAddress;

    public AuthorInfo(string id, JournalLanguage journalLanguage, string firstName, string middleName, string lastName, string eMail, string address) : base(id)
    {
      myJournalLanguage = journalLanguage;
      myFirstName = firstName;
      myMiddleName = middleName;
      myLastName = lastName;
      myEMail = eMail;
      myAddress = address;
    }

    public AuthorInfo(XmlElement el, IXmlDataLoader loader) : base(loader.EntityGenerator)
    {
      myJournalLanguage = loader.ParseLanguage(el);
      myFirstName = el.SelectSingleNode("FirstName/text()").Value;
      myMiddleName = el.SelectSingleNode("MiddleName/text()").Value;
      myLastName = el.SelectSingleNode("LastName/text()").Value;
      myEMail = el.SelectSingleNode("Email/text()").Value;
      myAddress = el.SelectSingleNode("Address/text()").Value;
    }

    public JournalLanguage JournalLanguage
    {
      get { return myJournalLanguage; }
    }

    public string FirstName
    {
      get { return myFirstName; }
    }

    public string MiddleName
    {
      get { return myMiddleName; }
    }

    public string LastName
    {
      get { return myLastName; }
    }

    public string EMail
    {
      get { return myEMail; }
    }

    public string Address
    {
      get { return myAddress; }
    }

    private string SortKey
    {
      get { return LastName + " " + FirstName + " " + MiddleName; }
    }

    int IComparable<AuthorInfo>.CompareTo(AuthorInfo other)
    {
      return SortKey.CompareTo(other.SortKey);
    }
  }
}