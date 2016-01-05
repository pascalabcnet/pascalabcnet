unit ClientServer;

uses System.Net, System.Net.Sockets, System.IO;

var 
  hostname := '127.0.0.1';
  port := 13000;

procedure RunServer(OnProcessCommand: string ->());
begin
  var server := new TcpListener(IPAddress.Any, 13000);
  server.Start();

  while (true) do
  begin
    var client := server.AcceptTcpClient();

    var stream := client.GetStream();

    var br := new BinaryReader(stream);
    var data := br.ReadString();
    
    if OnProcessCommand<>nil then 
    begin
      OnProcessCommand(data);
    end;  
    br.Close();

    stream.Close();
    client.Close();
  end;
end;

procedure SendCommand(s: string);
begin
  var client := new TcpClient(hostname,port);
  var stream := client.GetStream();
  
  var bw := new BinaryWriter(stream);
  bw.Write(s);
  bw.Close();
  
  stream.Close();
  client.Close();
end;

begin
end.