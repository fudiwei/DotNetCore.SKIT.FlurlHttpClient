using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace SKIT.FlurlHttpClient.Internal
{
#pragma warning disable IDE1006
    public static class _XmlSimpleSerializer
#pragma warning restore IDE1006
    {
        private const string DEFAULT_ROOT_ELEMENT_NAME = "xml";

        // REF: https://docs.microsoft.com/zh-cn/dotnet/api/system.xml.serialization.xmlserializer#dynamically-generated-assemblies
        private static readonly Hashtable _cache = new Hashtable();

        private static XmlSerializer GetTypedSerializer(Type type, string rootName)
        {
            string key = (type.FullName ?? type.ToString()) + ", XmlRoot=" + rootName;
            XmlSerializer? xmlSerializer = (XmlSerializer?)_cache[key];
            if (xmlSerializer == null)
            {
                xmlSerializer = new XmlSerializer(type, new XmlRootAttribute(rootName));
                _cache[key] = xmlSerializer;
            }

            return xmlSerializer;
        }

        public static string Serialize(object value, Type inputType, string rootName = DEFAULT_ROOT_ELEMENT_NAME)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (inputType is null) throw new ArgumentNullException(nameof(inputType));
            if (rootName is null) throw new ArgumentNullException(nameof(rootName));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            settings.WriteEndDocumentOnClose = false;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;

            using MemoryStream stream = new MemoryStream();
            using XmlWriter writer = XmlWriter.Create(stream, settings);
            XmlSerializer serializer = GetTypedSerializer(inputType, rootName);
            serializer.Serialize(writer, value);
            writer.Flush();

            string xml;
            xml = Encoding.UTF8.GetString(stream.ToArray());
            xml = Regex.Replace(xml, "\\s*<\\w+ xsi:nil=\"true\"[^>]*/>", string.Empty, RegexOptions.IgnoreCase); // 去除空字段节点
            xml = Regex.Replace(xml, "\\s+xmlns:[^\\s=\"]+=\"[^\"]*\"", string.Empty, RegexOptions.IgnoreCase);   // 去除 xmlns:* 属性
            xml = Regex.Replace(xml, "\\s+xsi:[^\\s=\"]+=\"\\w+\"", string.Empty, RegexOptions.IgnoreCase);       // 去除 xsi:* 属性
            xml = Regex.Replace(xml, "^[^<]", string.Empty, RegexOptions.IgnoreCase);                             // 去除 BOM
            return xml;
        }

        public static object Deserialize(string xml, Type returnType, string rootName = DEFAULT_ROOT_ELEMENT_NAME)
        {
            if (xml is null) throw new ArgumentNullException(nameof(xml));
            if (returnType is null) throw new ArgumentNullException(nameof(returnType));
            if (rootName is null) throw new ArgumentNullException(nameof(rootName));

            using StringReader reader = new StringReader(xml);
            XmlSerializer serializer = GetTypedSerializer(returnType, rootName);
            return serializer.Deserialize(reader)!;
        }
    }
}
