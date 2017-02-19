using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NodesGenerator.XmlModel
{
    [Serializable]
    public class Method
    {
        public Method() { }

        public Method(string text)
        {
            Text = text;
        }

        public NodeGenerator.method_info ToMethodInfo()
        {
            return new NodeGenerator.method_info(Text);
        }

        [XmlElement]
        public string Text;
    }
}
