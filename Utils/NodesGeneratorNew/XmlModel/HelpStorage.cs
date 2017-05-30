using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NodesGenerator.XmlModel
{
    [Serializable]
    public class HelpUnit : IComparable<HelpUnit>
    {
        public HelpUnit() { }

        public HelpUnit(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [XmlAttribute]
        public string Key;

        [XmlAttribute]
        public string Value;

        public int CompareTo(HelpUnit other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Key, other.Key, StringComparison.Ordinal);
        }
    }

    [Serializable]
    public class HelpStorage
    {
        [XmlArrayItem("HelpData")]
        public List<HelpUnit> HelpData = new List<HelpUnit>();
        //public Dictionary<string, string> HelpData = new Dictionary<string, string>();

        public HelpStorage() { }

        public HelpStorage(NodeGenerator.HelpStorage storage)
        {
            foreach (var helpElementKey in storage.HelpData.Keys)
            {
                HelpData.Add(new HelpUnit( 
                    helpElementKey as string,
                    (storage.HelpData[helpElementKey] as NodeGenerator.HelpContext).help_context));
            }
            HelpData.Sort();
        }

        public NodeGenerator.HelpStorage ToHelpStorage()
        {
            NodeGenerator.HelpStorage storage = new NodeGenerator.HelpStorage();
            foreach (var helpElement in HelpData)
                storage.add_context(helpElement.Key, new NodeGenerator.HelpContext() { help_context = helpElement.Value });

            return storage;
        }
    }
}
