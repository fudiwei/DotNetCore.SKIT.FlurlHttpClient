using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers
{
    using SKIT.FlurlHttpClient.Internal;

    public static class XmlHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="type"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool TryDeserialize(string xml, Type type, out Exception error)
        {
            return TryDeserialize(xml, type, "xml", out error);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="type"></param>
        /// <param name="rootName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool TryDeserialize(string xml, Type type, string rootName, out Exception error)
        {
            error = default!;

            try
            {
                _XmlSimpleSerializer.Deserialize(xml, type);
            }
            catch (Exception ex)
            {
                error = new Exception($"An unexpected exception was thrown when XML deserialize '{type}'.", ex);
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(type, new XmlRootAttribute(rootName));
                XmlDeserializationEvents events = new XmlDeserializationEvents
                {
                    OnUnknownAttribute = (_, e) =>
                    {
                        if (IsObsoleteMember(e.ObjectBeingDeserialized, e.Attr.Name, isAttribute: true)) return;
                        throw new XmlException($"Could not find member '{e.Attr.Name}' on object of type '{e.ObjectBeingDeserialized?.GetType()}'. Unknown attribute.");
                    },
                    OnUnknownElement = (_, e) =>
                    {
                        if (IsObsoleteMember(e.ObjectBeingDeserialized, e.Element.Name, isAttribute: false)) return;
                        throw new XmlException($"Could not find member '{e.Element.Name}' on object of type '{e.ObjectBeingDeserialized?.GetType()}'. Unknown element.");
                    },
                    OnUnknownNode = (_, e) =>
                    {
                        switch (e.NodeType)
                        {
                            case XmlNodeType.Element:
                            case XmlNodeType.Attribute:
                            case XmlNodeType.CDATA:
                                return;

                            case XmlNodeType.Text:
                                {
                                    if (string.IsNullOrWhiteSpace(e.Text?.Replace("\u200B", string.Empty)))
                                        return;
                                }
                                break;
                        }

                        throw new XmlException($"Unexpected token type '{e.NodeType}' when deserializing. Line: {e.LineNumber}, Position: {e.LinePosition}, Node '{e.Name}'.");
                    }
                };

                using StringReader textReader = new StringReader(xml);
                using XmlReader xmlReader = XmlReader.Create(textReader);
                serializer.Deserialize(xmlReader, events);
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }

            return error is null;
        }

        static bool IsObsoleteMember(object? obj, string name, bool isAttribute)
        {
            if (obj is null) return false;

            foreach (PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.GetCustomAttribute<ObsoleteAttribute>() is null) continue;

                string? mapped = isAttribute
                    ? prop.GetCustomAttribute<XmlAttributeAttribute>()?.AttributeName
                    : prop.GetCustomAttributes<XmlElementAttribute>().Select(a => a.ElementName).FirstOrDefault(n => n == name)
                      ?? prop.GetCustomAttribute<XmlArrayAttribute>()?.ElementName;

                if ((string.IsNullOrEmpty(mapped) ? prop.Name : mapped) == name) return true;
            }

            return false;
        }
    }
}
