namespace EugenePetrenko.DataModel
{
  public interface IReference
  {
    string Id { get; }
    string Title { get; }    
  }

  public class Reference : IReference
  {
    public Reference(string id, string title)
    {
      Id = id;
      Title = title;
    }

    public string Id { get; private set; }
    public string Title { get; private set; }
  }
}