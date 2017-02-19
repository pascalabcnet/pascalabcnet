using NodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NodesGenerator.XmlModel
{
    public class SimpleField : SyntaxField
    {
        public SimpleField() { }

        public SimpleField(simple_element field)
            : base(field)
        {
            Type = field.val_field_type_name;
        }

        public override node_field_info ToNodeFieldInfo(Dictionary<string, node_info> nodeNames)
        {
            var result = new simple_element();
            InitFieldInfo(result, nodeNames);
            return result;
        }

        protected void InitFieldInfo(simple_element nodeField, Dictionary<string, node_info> nodeNames)
        {
            var simpleField = nodeField as node_field_info;
            base.InitFieldInfo(simpleField, nodeNames);
            nodeField.val_field_type_name = Type;
        }

        [XmlAttribute]
        public string Type;
    }
}
