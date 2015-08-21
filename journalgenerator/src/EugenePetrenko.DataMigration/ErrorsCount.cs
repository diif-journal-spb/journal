using System;

namespace EugenePetrenko.DataMigration
{

  public class ErrorsCount
  {
    private int myErrors = 0;

    public int TotalErrors
    {
      get { return myErrors; }
    }

    public void Filter(string name, Action action)
    {
      Catch(name, action, e => e.LogAndThrow(e.Exception.Message));
    }

    public void Catch(string name, Action action, Action<IErrorHelper> onError)
    {
      try
      {
        action();
      }
      catch (Exception e)
      {
        var h = new ErrorHelperImpl(name, e);
        try
        {
          onError(h);
        }
        finally
        {
          if (h.IsError) myErrors++;
        }
      }
    }

    public interface IErrorHelper
    {
      Exception Exception { get; }
      string Message { get; }
      
      IErrorHelper LogAndThrow(string message, params object[] argz);
      IErrorHelper Log(string message, params object[] argz);

      IErrorHelper NotError();
    }

    private class ErrorHelperImpl : IErrorHelper
    {
      private readonly string myContext;
      public bool IsError { get; private set; }
      public Exception Exception { get; private set; }

      public IErrorHelper LogAndThrow(string tmpl, params object[] args)
      {
        var message = string.Format(tmpl, args);
        Console.Out.WriteLine(message);
        throw new Exception(message, Exception);
      }

      public string Message
      {
        get { return Exception.Message;  }
      }

      public IErrorHelper Log(string tmpl, params object[] args)
      {
        var message = string.Format(tmpl, args);
        Console.Out.WriteLine(message);
        return this;
      }

      public IErrorHelper NotError()
      {
        IsError = false;
        return this;
      }

      public ErrorHelperImpl(string context, Exception e)
      {
        Exception = e;
        myContext = context;
        IsError = true;
      }
    }
  }
}
