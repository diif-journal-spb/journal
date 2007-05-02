using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public class BooksGenerationContext : CollectionGenerationContext<IBook, ILocalizedBook>
  {
    public BooksGenerationContext(IJournal journal, LinkManager manager) : base( "books", journal.Books, manager, "books")
    {
      Program.Instance.AppendGlobalContext("BooksLink", this);
    }

    public override LinkTemplate GetLinkTemplate(LinkManager manager)
    {
      return new LinkTemplate(myManager, "books.html");
    }
  }
}