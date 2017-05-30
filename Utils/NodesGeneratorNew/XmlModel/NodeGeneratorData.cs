using NodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NodesGenerator;

namespace NodesGenerator.XmlModel
{
    [XmlRoot]
    public class NodeGeneratorData
    {
        public NodeGeneratorData() { }

        public NodeGeneratorData(NodeGenerator.NodeGenerator nodeGenerator)
        {
            foreach (var node in nodeGenerator.all_nodes.Cast<node_info>())
                SyntaxNodes.Add(new SyntaxNode(node));

            Settings = new Settings(nodeGenerator);

            TagCategories = nodeGenerator.tag_cats.Select(x => new FilterCategory(x)).ToList();

            HelpStorage = new HelpStorage(nodeGenerator.help_storage);
        }

        [XmlArrayItem(typeof(SyntaxNode), ElementName = "SyntaxNode")]
        public List<SyntaxNode> SyntaxNodes = new List<SyntaxNode>();

        [XmlElement]
        public Settings Settings;

        [XmlArrayItem("FilterCategory")]
        public List<FilterCategory> TagCategories = new List<FilterCategory>();

        [XmlElement]
        public HelpStorage HelpStorage;

        

        public NodeGenerator.NodeGenerator ToNodeGenerator()
        {
            NodeGenerator.NodeGenerator generator = new NodeGenerator.NodeGenerator();

            var nodeNames = new Dictionary<string, node_info>();

            foreach (var node in SyntaxNodes)
                nodeNames.Add(node.Name, new node_info());

            foreach (var node in SyntaxNodes)
                node.InitNodeInfo(nodeNames, nodeInfo: nodeNames[node.Name]);

            var oldNodes = SyntaxNodes.Select(x => nodeNames[x.Name]).ToArray();

            generator.set_nodes(oldNodes);
            Settings.InitNodeGenerator(generator);
            generator.tag_cats = TagCategories.Select(x => x.ToFilterCategory()).ToList();
            generator.help_storage = HelpStorage.ToHelpStorage();
            return generator;
        }
    }
}
