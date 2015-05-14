#reference 'System.Web.dll'

uses System, System.IO, System.Web.UI;

var htmlw : Html32TextWriter;
    tw : StreamWriter;
    
begin
tw := new StreamWriter('test.html');
htmlw := new Html32TextWriter(tw);
htmlw.WriteFullBeginTag('HTML'); 
htmlw.WriteFullBeginTag('BODY'); 
htmlw.WriteFullBeginTag('P'); 
htmlw.WriteLine('Hello from HTML');
htmlw.WriteEndTag('P');
htmlw.WriteEndTag('BODY');
htmlw.WriteEndTag('HTML');
htmlw.Close;
end.