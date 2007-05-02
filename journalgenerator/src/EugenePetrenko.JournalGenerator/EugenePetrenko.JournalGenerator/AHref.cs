namespace EugenePetrenko.JournalGenerator
{
  public class AHref<T> where T : HtmlGenerationContext
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