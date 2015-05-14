{$reference 'AODL.dll'}
uses System, AODL.Document, AODL.Document.TextDocuments, AODL.Document.Collections, AODL.Document.Content.Text;
var doc : TextDocument;
    pCollection : ParagraphCollection;
begin
doc := new TextDocument();
doc.New();
pCollection := ParagraphBuilder.CreateParagraphCollection(doc,'Privet world!',true,ParagraphBuilder.ParagraphSeperator);
foreach p : Paragraph in pCollection do
doc.Content.Add(p);
doc.SaveTo('Letter.odt');
end.