using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NodesGenerator.XmlModel
{
    [Serializable]
    public class FilterTag
    {
        public FilterTag() { }

        public FilterTag(NodeGenerator.FilterTag tag)
        {
            Name = tag.name;
            ReferenceCount = tag.ref_count;
        }

        [XmlAttribute]
        public string Name = "";

        [XmlAttribute]
        public int ReferenceCount = 0;

        public override string ToString()
        {
            return Name;
        }
    }

    [Serializable]
    public class FilterCategory
    {
        public FilterCategory() { }

        public FilterCategory(NodeGenerator.FilterCategory filter)
        {
            Name = filter.name;
            Tags = filter.tags.Select(tag => new FilterTag(tag)).ToList();
        }

        public NodeGenerator.FilterCategory ToFilterCategory()
        {
            var result = new NodeGenerator.FilterCategory();
            result.name = Name;
            result.tags = Tags.Select(x => new NodeGenerator.FilterTag() { name = x.Name, ref_count = x.ReferenceCount }).ToList();
            return result;
        }

        [XmlAttribute]
        public string Name = "";

        [XmlArrayItem("Tag")]
        public List<FilterTag> Tags = new List<FilterTag>();
    }
}
