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
    public class SyntaxNode
    {
        public SyntaxNode() { }

        public SyntaxNode(node_info node)
        {
            Name = node.node_name;
            BaseName = node?.base_class?.node_name;
            foreach (var field in node.subnodes)
                Fields.Add(field.XmlField());
            Methods.AddRange(
                node.methods.Select(
                    method => new Method(method.method_text)));

            Tags = new SyntaxNodeTags(node.tags);
        }

        public node_info ToNodeInfo(Dictionary<string, node_info> nodeNames)
        {
            var result = new node_info();
            InitNodeInfo(nodeNames, result);
            return result;
        }

        public void InitNodeInfo(Dictionary<string, node_info> nodeNames, node_info nodeInfo)
        {
            nodeInfo.node_name = Name;
            nodeInfo.base_class = BaseName == null ? null : nodeNames[BaseName];
            foreach (var field in Fields)
                nodeInfo.add_subnode(field.ToNodeFieldInfo(nodeNames));
            nodeInfo.set_methods(Methods.Select(m => m.ToMethodInfo()).ToArray());
            nodeInfo.tags = Tags.ToNodeInfoTags();
        }

        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string BaseName;
        
        [XmlArrayItem(typeof(SyntaxField), ElementName = "SyntaxField")]
        [XmlArrayItem(typeof(SimpleField), ElementName = "SimpleField")]
        [XmlArrayItem(typeof(ExtendedField), ElementName = "ExtendedField")]
        public List<SyntaxField> Fields = new List<SyntaxField>();
        
        [XmlArrayItem(typeof(Method), ElementName = "Method")]
        public List<Method> Methods = new List<Method>();

        [XmlElement]
        public SyntaxNodeTags Tags;
    }
}
