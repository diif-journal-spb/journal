namespace EugenePetrenko.JournalGenerator
{
  public class AHref<T> where T: GenerationContext
  {
    private readonly T myContext;
    private readonly string myCaption;

    public AHref(T context, string caption)
    {
      myContext = context;
      myCaption = caption;
    }

    public T Context
    {
      get { return myContext; }
    }

    public string Caption
    {
      get { return myCaption; }
    }
  }
}