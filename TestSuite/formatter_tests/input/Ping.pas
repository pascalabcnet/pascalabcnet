// Использование Ping
uses System.Net.NetworkInformation;

const address = 'www.yandex.ru';

begin
  var p := new Ping();
  try
    var res := p.Send(address);
    writeln('IP адрес сервера: ',res.Address);
    writeln('Время отклика: ',res.RoundtripTime,' мс');
  except
    on e: Exception do
      write(e.Message);
  end;    
end.