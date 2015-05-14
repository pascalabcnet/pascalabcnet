#reference 'System.Xml.dll'

uses System.Xml;

begin
var settings := new XmlWriterSettings();
settings.Indent := true;
settings.NewLineOnAttributes := true;
settings.OmitXmlDeclaration := true;
var xmlw := XmlWriter.Create('out.xml',settings);
xmlw.WriteStartElement('books');
xmlw.WriteStartElement('book');
xmlw.WriteAttributeString('genre','IT');
xmlw.WriteElementString('title','C# and OOP');
xmlw.WriteElementString('author','Vasja Pupkin');
xmlw.WriteElementString('year','2006');
xmlw.WriteEndElement();
xmlw.WriteEndElement();
xmlw.Close();

var xmlr := XmlReader.Create('out.xml');
xmlr.Read();
xmlr.ReadStartElement('books');
xmlr.ReadStartElement('book');
writeln('book:');
xmlr.ReadStartElement('title');
writeln('title: ',xmlr.ReadString());
xmlr.ReadEndElement();
xmlr.ReadStartElement('author');
writeln('author: ',xmlr.ReadString());
xmlr.ReadEndElement();
xmlr.ReadStartElement('year');
writeln('year: ',xmlr.ReadContentAsInt());
xmlr.ReadEndElement();//year
xmlr.ReadEndElement();//book
xmlr.ReadEndElement();//books
xmlr.Close();
end.