using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NodesGenerator.XmlModel
{
    [Serializable]
    public class SyntaxNodeTags
    {
        public SyntaxNodeTags() { }

        public SyntaxNodeTags(List<Tuple<int, int>> tags)
        {
            tags.ForEach(t =>
            {
                CategoryIndices.Add(t.Item1);
                TagIndices.Add(t.Item2);
            });
        }

        public List<Tuple<int, int>> ToNodeInfoTags()
        {
            
            return CategoryIndices
                .Zip(TagIndices, (category, tag) => new Tuple<int, int>(category, tag))
                .ToList();
        }

        [XmlArrayItem("CategoryIndex")]
        public List<int> CategoryIndices = new List<int>();

        [XmlArrayItem("TagIndex")]
        public List<int> TagIndices = new List<int>();
    }
}
