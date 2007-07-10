using System;
using System.Collections;
using System.Collections.Generic;

namespace EugenePetrenko.JournalGenerator
{
  public delegate HtmlDynamicPage LookupItem(string item);

  public class SmartLookupDictionary : IDictionary
  {
    private readonly Dictionary<string, HtmlDynamicPage> myPages = new Dictionary<string, HtmlDynamicPage>();
    private readonly IDictionary myMap;
    private readonly List<LookupItem> myEvents = new List<LookupItem>();
    private readonly Language myLanguage;
    private readonly string myTemplateName;
    private readonly LinkTemplate myCurrentPageLink;

    public event LookupItem LookupTemplate
    {
      add { myEvents.Add(value);}
      remove { myEvents.Remove(value);}
    }

    public IDictionary StaticDictionary
    {
      get { return myMap; }
    }
    
    public SmartLookupDictionary(string templateName, IDictionary predefined, Language language, LinkTemplate currentPageLink)
    {
      myTemplateName = templateName;
      myCurrentPageLink = currentPageLink;
      myMap = predefined;      
      myLanguage = language;
    }

    private HtmlDynamicPage DoLookupTemplate(string key)
    {
      key = key.ToLower();
      HtmlDynamicPage page;
      if (myPages.TryGetValue(key, out page))
        return page;

      foreach (LookupItem item in myEvents)
      {
        page = item(key);
        if (page != null)
        {
          myPages[key] = page;
          return page;
        }
      }
      return null;
    }

    private object DoLookup(string item)
    {
      const string TEMPLATE = "Template";
      const string LINK = "Link";

      if (item.StartsWith(TEMPLATE) && item.EndsWith(LINK))
      {
        string templateName =
          item.Substring(TEMPLATE.Length, item.Length - TEMPLATE.Length - LINK.Length);
        HtmlDynamicPage page = DoLookupTemplate(templateName);

        Link link = page.LinkTemplate.ToLink(myLanguage, myCurrentPageLink);
        myMap[item] = link;
        return link;
      }
      return null;
    }
    bool IDictionary.Contains(object key)
    {
      return myMap.Contains(key) && DoLookup((string) key) != null;
    }

    void IDictionary.Add(object key, object value)
    {
      throw new NotImplementedException();
    }

    void IDictionary.Clear()
    {
      throw new NotImplementedException();
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
      throw new NotImplementedException();
    }

    void IDictionary.Remove(object key)
    {
      throw new NotImplementedException();
    }

    object IDictionary.this[object key]
    {
      get
      {
        object r;
        if (myMap.Contains(key))
        {
          r = myMap[key];
        } else
        {
          r = DoLookup((string) key);
        }
        if (r == null)
        {
          Console.Out.WriteLine("[{1}] Error: lookup failed '{0}'", key, myTemplateName);
        }
        return r;
      }
      set { throw new NotImplementedException(); }
    }

    ICollection IDictionary.Keys
    {
      get { throw new NotImplementedException(); }
    }

    ICollection IDictionary.Values
    {
      get { throw new NotImplementedException(); }
    }

    bool IDictionary.IsReadOnly
    {
      get { return true; }
    }

    bool IDictionary.IsFixedSize
    {
      get { return true; }
    }

    void ICollection.CopyTo(Array array, int index)
    {
      throw new NotImplementedException();
    }

    int ICollection.Count
    {
      get { throw new NotImplementedException(); }
    }

    object ICollection.SyncRoot
    {
      get { return this; }
    }

    bool ICollection.IsSynchronized
    {
      get { return false; }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }
  }
}