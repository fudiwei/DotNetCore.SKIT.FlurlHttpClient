using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers
{
    internal static class XmlHelper
    {
        public static bool TryDeserialize(string xml, Type type, out Exception error)
        {
            error = default!;

            try
            {
                using TextReader xmlReader = new StringReader(xml);
                XmlSerializer xmlSerializer = new XmlSerializer(type, new XmlRootAttribute("xml"));
                xmlSerializer.Deserialize(xmlReader);
            }
            catch (Exception ex)
            {
                error = new Exception($"An unexpected exception was thrown when XML deserialize '{type}'.", ex);
                return false;
            }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                if (xmlDocument.DocumentElement != null)
                {
                    Action<Type, XmlNode> func = default!;
                    func = new Action<Type, XmlNode>((curType, curNode) =>
                    {
                        if (curNode.NodeType == XmlNodeType.Element)
                        {
                            PropertyInfo[] properties = curType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                            XmlElementAttribute? xmlElementAttribute = properties.Select(p => p.GetCustomAttribute<XmlElementAttribute>(inherit: true)).FirstOrDefault(attr => attr?.ElementName == curNode.Name);
                            XmlArrayAttribute? xmlArrayAttribute = properties.Select(p => p.GetCustomAttribute<XmlArrayAttribute>(inherit: true)).FirstOrDefault(attr => attr?.ElementName == curNode.Name);
                            XmlArrayItemAttribute? xmlArrayItemAttribute = properties.Select(p => p.GetCustomAttribute<XmlArrayItemAttribute>(inherit: true)).FirstOrDefault(attr => attr?.ElementName == curNode.Name);
                            if (xmlElementAttribute is null && xmlArrayAttribute is null && xmlArrayItemAttribute is null)
                            {
                                throw new Exception($"Could not find member '{curNode.Name}' on object of type '{curType}'.");
                            }
                        }

                        for (int i = 0; i < curNode.ChildNodes.Count; i++)
                        {
                            Type nextType = curType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Single(p =>
                                {
                                    XmlElementAttribute? xmlElementAttribute = p.GetCustomAttribute<XmlElementAttribute>(inherit: true);
                                    XmlArrayAttribute? xmlArrayAttribute = p.GetCustomAttribute<XmlArrayAttribute>(inherit: true);
                                    XmlArrayItemAttribute? xmlArrayItemAttribute = p.GetCustomAttribute<XmlArrayItemAttribute>(inherit: true);
                                    return curNode.Name == xmlElementAttribute?.ElementName ||
                                           curNode.Name == xmlArrayAttribute?.ElementName ||
                                           curNode.Name == xmlArrayItemAttribute?.ElementName;
                                })
                                .PropertyType;
                            XmlNode nextNode = curNode.ChildNodes[i];

                            if (nextType.IsArray)
                                nextType = nextType.GetElementType();

                            func.Invoke(nextType, nextNode);
                        }
                    });

                    for (int i = 0; i < xmlDocument.DocumentElement.ChildNodes.Count; i++)
                    {
                        func.Invoke(type, xmlDocument.DocumentElement.ChildNodes[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }

            return error is null;
        }
    }
}
