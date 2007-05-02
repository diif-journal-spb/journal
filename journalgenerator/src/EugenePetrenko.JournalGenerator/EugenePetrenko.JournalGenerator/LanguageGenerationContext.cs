using System;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public class LanguageGenerationContext : IStringTemplateErrorListener
  {
    private readonly SmartLookupDictionary myAttributes;
    private readonly StringTemplate myTemplate;

    public LanguageGenerationContext(StringTemplate template, SmartLookupDictionary attributes)
    {
      myAttributes = attributes;
      myTemplate = template;
    }

    public string GeneratePage()
    {
      myTemplate.Attributes = myAttributes;
      return myTemplate.ToString();
    }

    public SmartLookupDictionary Attributes
    {
      get { return myAttributes; }
    }

    void IStringTemplateErrorListener.Error(string msg, Exception e)
    {
      Console.Out.WriteLine("[{0}] Error = {1},{2}", myTemplate.Name, msg, e);
    }

    void IStringTemplateErrorListener.Warning(string msg)
    {
      Console.Out.WriteLine("[{0}] Error = {1}", myTemplate.Name, msg);
    }
  }
}