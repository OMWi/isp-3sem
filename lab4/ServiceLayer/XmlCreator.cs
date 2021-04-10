using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.IO;

namespace ServiceLayer
{
    public class XmlCreator
    {
        public void CreateXmlFile(XmlModel obj, string targetPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlModel));
            using (var fs = new FileStream(targetPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                serializer.Serialize(fs, obj);
            }
            XmlReader reader = XmlReader.Create(targetPath);
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchemaInference schema = new XmlSchemaInference();
            schemaSet = schema.InferSchema(reader);
            targetPath = Path.ChangeExtension(targetPath, ".xsd");
            foreach (XmlSchema s in schemaSet.Schemas())
            {
                using (var sw = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sw))
                    {
                        s.Write(writer);
                    }
                    File.WriteAllText(targetPath, sw.ToString());
                }
            }
        }
    }
}
