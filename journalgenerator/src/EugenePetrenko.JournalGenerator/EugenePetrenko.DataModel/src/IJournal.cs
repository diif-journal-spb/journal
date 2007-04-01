using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IJournal : IEntity
  {
    INumber[] Numbers { get; }
    IAuthor[] Authors { get; }
  }

  public class Journal : Entity, IJournal
  {
    private readonly INumber[] myNumbers;
    private readonly IAuthor[] myAuthors;


    public Journal(EntityGenerator gen, INumber[] numbers, IAuthor[] authors) : base(gen)
    {
      myNumbers = numbers;
      myAuthors = authors;
    }

    public INumber[] Numbers
    {
      get { return myNumbers; }
    }

    public IAuthor[] Authors
    {
      get { return myAuthors; }
    }
  }
}