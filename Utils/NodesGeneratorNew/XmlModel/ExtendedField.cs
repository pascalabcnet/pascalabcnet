using NodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NodesGenerator.XmlModel
{
    public class ExtendedField : SimpleField
    {
        public ExtendedField() { }

        public ExtendedField(extended_simple_element field)
            : base(field)
        {
            CreateVariable = field.create_var;
            DeleteVariable = field.delete_var;
        }

        public override node_field_info ToNodeFieldInfo(Dictionary<string, node_info> nodeNames)
        {
            var result = new extended_simple_element();
            InitFieldInfo(result, nodeNames);
            return result;
        }

        protected void InitFieldInfo(extended_simple_element nodeField, Dictionary<string, node_info> nodeNames)
        {
            var extendedField = nodeField as simple_element;
            base.InitFieldInfo(extendedField, nodeNames);
            nodeField.create_var = CreateVariable;
            nodeField.delete_var = DeleteVariable;
        }

        [XmlAttribute]
        public bool CreateVariable;

        [XmlAttribute]
        public bool DeleteVariable;
    }
}
