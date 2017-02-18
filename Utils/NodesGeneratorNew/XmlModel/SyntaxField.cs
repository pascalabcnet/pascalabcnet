using NodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NodesGenerator.XmlModel
{
    [Serializable]
    public class SyntaxField
    {
        public SyntaxField() { }

        public SyntaxField(node_field_info field)
        {
            Name = field.field_name;
            SyntaxType = field.field_type?.node_name;
        }

        public virtual node_field_info ToNodeFieldInfo(Dictionary<string, node_info> nodeNames)
        {
            var nodeField = new node_field_info();
            InitFieldInfo(nodeField, nodeNames);
            return nodeField;
        }

        protected void InitFieldInfo(node_field_info nodeField, Dictionary<string, node_info> nodeNames)
        {
            nodeField.field_name = Name;
            nodeField.field_type = SyntaxType == null ? null : nodeNames[SyntaxType];
        }

        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string SyntaxType;
    }
}
