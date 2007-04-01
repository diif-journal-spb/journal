using System.Collections.Generic;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public class LanguageGenerationContext
  {
    private readonly Dictionary<string, object> myAttributes;
    private readonly StringTemplate myTemplate;

    public LanguageGenerationContext(StringTemplate template, Dictionary<string, object> attributes)
    {
      myAttributes = attributes;
      myTemplate = template;
    }

    public Dictionary<string, object> Attributes
    {
      get { return myAttributes; }
    }

    public string GeneratePage()
    {
      foreach (KeyValuePair<string, object> pair in Attributes)
      {
        myTemplate.SetAttribute(pair.Key, pair.Value);
      }

      return myTemplate.ToString();
    }
  }
}