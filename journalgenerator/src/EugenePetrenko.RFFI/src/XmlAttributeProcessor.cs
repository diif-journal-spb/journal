using System.Collections;
using System.Reflection;
using System.Xml;

namespace EugenePetrenko.RFFI
{
  public static class XmlAttributeProcessor
  {
    private static void Apply(XmlNode node, object o)
    {
      foreach (PropertyInfo info in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        MethodInfo getter = info.GetGetMethod();
        object getterValue2 = getter.Invoke(o, new object[0]);
        foreach (object getterValue in Values(info, getterValue2))
        {
          bool hasPath = false;
          foreach (XmlElementPathAttribute path in info.GetCustomAttributes(typeof (XmlElementPathAttribute), true))
          {
            hasPath = true;
            XmlNode el = path.Apply(node);
            if (getterValue is string)
            {
              processXmlActions(info, el, getterValue);
            }
            else
            {
              Apply(el, getterValue);
            }
          }
          if (!hasPath)
          {
            processXmlActions(info, node, getterValue);
          }
        }
      }
    }

    private static IEnumerable Values(ICustomAttributeProvider info, object getterValue)
    {
      if (info.IsDefined(typeof (XmlForeachAttribute), true))
      {
        foreach (object value in (IEnumerable) getterValue)
        {
          yield return value;
        }
      }
      else
      {
        yield return getterValue; 
      }
    }

    private static void processXmlActions(ICustomAttributeProvider info, XmlNode el, object getterValue)
    {
      foreach (XmlBaseAttribute action in info.GetCustomAttributes(typeof (XmlBaseAttribute), true))
      {
        action.Apply(el, getterValue.ToString());
      }
    }

    public static XmlDocument Build(object o)
    {
      object[] attr = o.GetType().GetCustomAttributes(typeof (XmlRootAttribute), true);
      XmlDocument doc = new XmlDocument();
      XmlNode root = ((XmlRootAttribute) attr[0]).Apply(doc);

      foreach (
        XmlElementPathAttribute attribute in o.GetType().GetCustomAttributes(typeof (XmlElementPathAttribute), true))
      {
        root = attribute.Apply(root);
      }

      Apply(root, o);
      return doc;
    }
  }
}