// Скачивание файла
uses System.Net;

const 
  address = 'www.yandex.ru';
  filename = 'LogoPABCNET2010_Rus.png';

begin
  var w := new WebClient();
  w.DownloadFile('http://pascalabc.net/images/logo/'+filename,filename);
  Exec(filename);
end.