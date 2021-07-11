namespace Gamefreak130.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using Sims3.SimIFace;

    [Persistable]
    public abstract class Settings
    {
        public string Export()
        {
            StringBuilder text = new("<?xml version=\"1.0\" encoding=\"utf-8\"?><Settings>");
            WriteObject(text, this);
            return text.Append("</Settings>").ToString();
        }

        private static void WriteObject(StringBuilder text, object val)
        {
            if (val is null)
            {
                throw new ArgumentNullException("Object value");
            }
            Type type = val.GetType();
            if (type == typeof(string) || type == typeof(decimal) || type.IsEnum || type.IsPrimitive)
            {
                text.Append(XmlConvert.EncodeLocalName(val.ToString()));
            }
            else if ((type.IsGenericType || type.IsArray) && val is IList list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    text.Append($"<i_{i}>");
                    WriteObject(text, list[i]);
                    text.Append($"</i_{i}>");
                }
            }
            else if (type.IsGenericType && val is IDictionary dict)
            {
                foreach (DictionaryEntry entry in dict)
                {
                    if (entry.Key is not Enum and not string and not decimal && !entry.Key.GetType().IsPrimitive)
                    {
                        throw new ArgumentException("Dictionary keys must be a primitive type, enum, or string");
                    }
                    string key = XmlConvert.EncodeLocalName(entry.Key.ToString());
                    text.Append($"<{key}>");
                    WriteObject(text, entry.Value);
                    text.Append($"</{key}>");
                }
            }
            else
            {
                foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    string name = XmlConvert.EncodeLocalName(field.Name);
                    text.Append($"<{name}>");
                    WriteObject(text, field.GetValue(val));
                    text.Append($"</{name}>");
                }
            }
        }

        public void Import(XmlDocument xml, bool replaceDictionaries = false) => ReadObject(this, xml.DocumentElement, replaceDictionaries);

        private static object ReadObject(object currentObj, XmlNode objNode, bool replaceDictionaries)
        {
            if (currentObj is null)
            {
                throw new ArgumentNullException("Object value");
            }
            Type type = currentObj.GetType();
            if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal))
            {
                return Convert.ChangeType(XmlConvert.DecodeName(objNode.InnerText), type);
            }
            else if (type.IsEnum)
            {
                try
                {
                    return Enum.Parse(currentObj.GetType(), XmlConvert.DecodeName(objNode.InnerText));
                }
                catch (ArgumentException)
                {
                }
            }
            else if (type.IsGenericType && currentObj is IDictionary currentDict)
            {
                Type[] generics = type.GetGenericArguments();
                if (!generics[0].IsEnum && generics[0] != typeof(string) && generics[0] != typeof(decimal) && !generics[0].IsPrimitive)
                {
                    throw new ArgumentException("Dictionary keys must be a primitive type, enum, or string");
                }
                IDictionary newDict = Activator.CreateInstance(type, true) as IDictionary;
                foreach (XmlNode node in objNode.ChildNodes)
                {
                    object key = null;
                    string name = XmlConvert.DecodeName(node.Name);
                    if (generics[0].IsEnum)
                    {
                        try
                        {
                            key = Enum.Parse(generics[0], name);
                        }
                        catch (ArgumentException)
                        {
                        }
                    }
                    else
                    {
                        key = Convert.ChangeType(name, generics[0]);
                    }

                    object val = generics[1] == typeof(string) ? string.Empty : Activator.CreateInstance(generics[1], true);
                    newDict[key] = ReadObject(val, node, replaceDictionaries);
                }
                if (replaceDictionaries)
                {
                    return newDict;
                }
                foreach (DictionaryEntry entry in newDict)
                {
                    if (currentDict.Contains(entry.Key))
                    {
                        currentDict[entry.Key] = newDict[entry.Key];
                    }
                }
            }
            else if ((type.IsGenericType && currentObj is IList) || type.IsArray)
            {
                Type elementType = type.IsGenericType ? type.GetGenericArguments()[0] : type.GetElementType();
                IList list = type.IsArray ? Array.CreateInstance(elementType, objNode.ChildNodes.Count) : Activator.CreateInstance(type, true) as IList;
                for (int i = 0; i < objNode.ChildNodes.Count; i++)
                {
                    object val = elementType == typeof(string) ? string.Empty : Activator.CreateInstance(elementType, true);
                    val = ReadObject(val, objNode[$"i_{i}"], replaceDictionaries);
                    if (list.IsFixedSize)
                    {
                        list[i] = val;
                    }
                    else
                    {
                        list.Add(val);
                    }
                }
                return list;
            }
            else
            {
                Dictionary<string, FieldInfo> fields = new();
                foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    fields.Add(field.Name, field);
                }
                foreach (XmlNode node in objNode.ChildNodes)
                {
                    if (fields.TryGetValue(XmlConvert.DecodeName(node.Name), out FieldInfo field))
                    {
                        object val = field.GetValue(currentObj);
                        if (val is null)
                        {
                            throw new ArgumentNullException($"{field.Name} in {type.FullName}");
                        }
                        field.SetValue(currentObj, ReadObject(val, node, replaceDictionaries));
                    }
                }
            }
            return currentObj;
        }
    }
}
