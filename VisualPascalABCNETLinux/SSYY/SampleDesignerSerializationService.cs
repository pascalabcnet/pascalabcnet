using System;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

namespace SampleDesignerHost
{
    /// <summary>
    /// Implementation de IDesignerSerializationService
    /// </summary>
    public class SampleDesignerSerializationService : IDesignerSerializationService
    {
        private IDesignerHost host;

        public SampleDesignerSerializationService(IDesignerHost host)
        {
            this.host = host;
        }

        public object Serialize(ICollection objects)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.AppendChild(xDoc.CreateElement("COPY"));

            object[] list = new object[objects.Count];
            objects.CopyTo(list, 0);
            ((SampleDesignerHost)host).WriteComponentCollection(xDoc, list, xDoc.DocumentElement);

            return xDoc.OuterXml;
        }

        public ICollection Deserialize(object serializationData)
        {
            ArrayList errors = new ArrayList();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(serializationData.ToString());
            ICollection objects = ((SampleDesignerHost)host).CreateComponents(xDoc, errors);

            foreach (string s in errors)
            {
                System.Diagnostics.Debug.WriteLine(s);
            }
            return objects;
        }

    }
}