using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IAuthorInfo : IEntity, IComparable<IAuthorInfo>
  {
    IAuthor Author { get;}
    IOrganizationInfo Origanization { get; }
    JournalLanguage JournalLanguage { get; }

    string FirstName { get; }
    string MiddleName { get; }
    string LastName { get; }

    string EMail { get; }
    string Address { get; }
  }


  public class AuthorInfo : Entity, IAuthorInfo, IComparable<AuthorInfo>
  {
    private readonly JournalLanguage myJournalLanguage;
    private readonly IAuthor myAuthor;
    private readonly string myFirstName;
    private readonly string myMiddleName;
    private readonly string myLastName;
    private readonly string myEMail;
    private readonly string myAddress;
    private readonly string mySortKey;

    private static string PatchAddress(string addr)
    {
      string next = Regex.Replace(addr, "<br[^/]*/>", "\n", RegexOptions.IgnoreCase|RegexOptions.Multiline);
      next = Regex.Replace(next, @"\n(\s*\n)+", "\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      return next.Replace("\r", "").Replace("\n", "<br />");
    }

    public AuthorInfo(XmlElement el, IXmlDataLoader loader, IAuthor author) : base(loader.EntityGenerator)
    {
      myJournalLanguage = loader.ParseLanguage(el);
      myFirstName = SafeRead(el.SelectSingleNode("FirstName/text()")).Trim();
      myMiddleName = SafeRead(el.SelectSingleNode("MiddleName/text()")).Trim();
      myLastName = SafeRead(el.SelectSingleNode("LastName/text()")).Trim();
      myEMail = SafeRead(el.SelectSingleNode("Email/text()")).Trim();
      myAddress = PatchAddress(SafeRead(el.SelectSingleNode("Address/text()")).Trim());
      mySortKey = LastName + " " + FirstName + " " + MiddleName;      
      myAuthor = author;

      if (el.SelectSingleNode("@sort-key") != null)
        throw new ArgumentException("sortkey is not supported for " + this);      
    }

    private static string SafeRead(XmlNode node)
    {
      return node == null ? string.Empty : node.Value;
    }

    public IOrganizationInfo Origanization
    {
      get { 
        return Author.Organization.AllLanguages()
          .FirstOrDefault(x=>x.JournalLanguage == JournalLanguage) 
          ?? Author.Organization.AllLanguages().First(); 
      }
    }

    public IAuthor Author => myAuthor;

    public JournalLanguage JournalLanguage => myJournalLanguage;

    public string FirstName => myFirstName;

    public string MiddleName => myMiddleName;

    public string LastName => myLastName;

    public string EMail => myEMail;

    public string Address => myAddress;

    private string SortKey => mySortKey;

    int IComparable<AuthorInfo>.CompareTo(AuthorInfo other)
    {
      return String.Compare(SortKey, other.SortKey, StringComparison.Ordinal);
    }

    int IComparable<IAuthorInfo>.CompareTo(IAuthorInfo other)
    {
      return String.Compare(SortKey, ((AuthorInfo) other).SortKey, StringComparison.Ordinal);
    }
  }
}
