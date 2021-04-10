using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace CP_dll
{
    public interface IParsable
    {
        T GetConfig<T>();
    }

    public class ConfigProvider
    {
        IParsable parser;
        public ConfigProvider(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File doesnt exist");
            }
            if (Path.GetExtension(filePath) == ".json")
            {
                parser = new ParserJson(filePath);
            }
            if (Path.GetExtension(filePath) == ".xml")
            {
                parser = new ParserXml(filePath);
            }
        }
        public T GetConfig<T>()
        {
            return parser.GetConfig<T>();
        }
    }

    class ParserJson : IParsable
    {
        string filePath;
        public ParserJson(string filePath)
        {
            this.filePath = filePath;
        }
        public T GetConfig<T>()
        {
            string jsonString;
            using (var sr = new StreamReader(filePath))
            {
                jsonString = sr.ReadToEnd();
            }
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }

    class ParserXml : IParsable
    {
        string filePath;
        public ParserXml(string filePath)
        {
            this.filePath = filePath;
        }
        public T GetConfig<T>()
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            var objProps = typeof(T).GetProperties();

            foreach (var prop in objProps)
            {
                var root = xmlDoc.DocumentElement;
                XmlNode node = null;
                FindNode(prop, root, ref node);

                if (node != null)
                {
                    prop.SetValue(obj, Convert.ChangeType(node.InnerText, prop.PropertyType));
                }
            }

            return obj;
        }
        private void FindNode(PropertyInfo prop, XmlNode root, ref XmlNode resNode)
        {
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == prop.Name)
                {
                    resNode = node;
                }
                FindNode(prop, node, ref resNode);
            }
        }
        
    }

}
