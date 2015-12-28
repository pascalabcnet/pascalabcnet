// Использование WebClient
uses System.Net;

begin
  var w := new WebClient();
  w.Encoding := System.Text.Encoding.UTF8;
  var s := w.DownloadString('http://pascalabc.net');
  writeln(s);
  w.DownloadFile('http://pascalabc.net/images/logo/LogoPABCNET2010_Rus.png','LogoPABCNET2010_Rus.png');
end.