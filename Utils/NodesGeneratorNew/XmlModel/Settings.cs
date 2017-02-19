using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NodesGenerator.XmlModel
{
    [Serializable]
    public class Settings
    {
        public Settings() { }

        public Settings(NodeGenerator.NodeGenerator generator)
        {
            FileName = generator.file_name;
            NamespaceName = generator.namespace_name;
            FactoryName = generator.factory_name;
            VisitorInterfaceFileName = generator.visitor_interface_file_name;
            PcuWriterName = generator.pcu_writer_name;
            PcuWriterFileName = generator.pcu_writer_file_name;
            PcuReaderName = generator.pcu_reader_name;
            PcuReaderFileName = generator.pcu_reader_file_name;
            PcuReaderFileNameHeader = generator.pcu_reader_file_name_h;
            PcuReaderFileNameCpp = generator.pcu_reader_file_name_cpp;
        }

        public void InitNodeGenerator(NodeGenerator.NodeGenerator generator)
        {
            generator.file_name = FileName;
            generator.namespace_name = NamespaceName;
            generator.factory_name = FactoryName;
            generator.visitor_interface_file_name = VisitorInterfaceFileName;
            generator.pcu_writer_name = PcuWriterName;
            generator.pcu_writer_file_name = PcuWriterFileName;
            generator.pcu_reader_name = PcuReaderName;
            generator.pcu_reader_file_name = PcuReaderFileName;
            generator.pcu_reader_file_name_h = PcuReaderFileNameHeader;
            generator.pcu_reader_file_name_cpp = PcuReaderFileNameCpp;
        }

        [XmlElement]
        public string FileName;

        [XmlElement]
        public string NamespaceName;
        
        [XmlElement]
        public string FactoryName;

        [XmlElement]
        public string VisitorInterfaceFileName;

        [XmlElement]
        public string PcuWriterName;
        
        [XmlElement]
        public string PcuWriterFileName;

        [XmlElement]
        public string PcuReaderName;

        [XmlElement]
        public string PcuReaderFileName;

        [XmlElement]
        public string PcuReaderFileNameHeader;

        [XmlElement]
        public string PcuReaderFileNameCpp;
    }
}
