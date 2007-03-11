using System;
using System.Collections.Generic;
using System.Reflection;

namespace EugenePetrenko.JournalGenerator
{
  public class LanguageGenerationContext
  {
    private readonly Dictionary<string, object> myAttributes;
    private readonly string myDestFile;

    public LanguageGenerationContext(Dictionary<string, object> attributes, string destFile)
    {
      myAttributes = attributes;
      myDestFile = destFile;
    }

    public Dictionary<string, object> Attributes
    {
      get { return myAttributes; }
    }

    public string DestFile
    {
      get { return myDestFile; }
    }
  }

  [AttributeUsage(AttributeTargets.Property)]
  public class GenerationHiddenAttribute : Attribute
  {    
  }

  public class GenerationContext
  {
    private readonly LinkTemplate myLinkTemplate;
    private readonly string myTemplateName;

    [GenerationHidden]
    public string TemplateName
    {
      get { return myTemplateName; }
    }

    [GenerationHidden]
    public LinkTemplate LinkTemplate
    {
      get { return myLinkTemplate; }
    }

    public GenerationContext(LinkTemplate url, string templateName)
    {
      myLinkTemplate = url;
      myTemplateName = templateName;
    }

    public LanguageGenerationContext LanguageContext(Language language)
    {
      Dictionary<string, object> dic = new Dictionary<string, object>();

      Language foreignLanguage = (Language)(-(int)language);
      Link link = myLinkTemplate.ToLink(foreignLanguage);

      dic["Link"] = myLinkTemplate.ToLink(language);      
      dic["ForeignLink"] = link;
      dic["LanguageName"] = LanguageToName(language);
      dic["ForeignLanguageName"] = LanguageToName(foreignLanguage);

      AppendLanguageContextInternal(language, dic);

      return new LanguageGenerationContext(dic, link.DestFile);
    }

    private static string LanguageToName(Language l)
    {
      switch (l)
      {
        case Language.EN:
          return "English";
        case Language.RU:
          return "Русский";
      }
      throw new ArgumentException("Unexpected language");
    }

    protected virtual void AppendLanguageContextInternal(Language language, Dictionary<string, object> ctx)
    {
      foreach (PropertyInfo info in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
      {
        if (!info.IsDefined(typeof(GenerationHiddenAttribute), true))
          ctx.Add(info.Name, info.GetGetMethod().Invoke(this, new object[0]));
      }
    }
  }
}