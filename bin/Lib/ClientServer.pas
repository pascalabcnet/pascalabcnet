// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit ClientServer;

uses System.Net, System.Net.Sockets, System.IO;

var 
  servername := '127.0.0.1';
  port := 13000;

/// Запускает сервер, принимающий от клиента строку и выполняющий указанное действие
procedure RunServer(OnProcessCommand: string -> ());
begin
  var server := new TcpListener(IPAddress.Any, port);
  server.Start();

  while (true) do
  begin
    var client := server.AcceptTcpClient();
    var stream := client.GetStream();

    var br := new BinaryReader(stream);
    var data := br.ReadString();
    br.Close();
    
    stream.Close();
    client.Close();

    OnProcessCommand(data);
  end;
end;

/// Запускает сервер, принимающий от клиента строку, выполняющий указанное действие и отправляющий строку в ответ
procedure RunServer(OnProcessCommand: string -> string);
begin
  var server := new TcpListener(IPAddress.Any, port);
  server.Start();

  while (true) do
  begin
    var client := server.AcceptTcpClient();
    var stream := client.GetStream();

    var br := new BinaryReader(stream);
    var data := br.ReadString();
    
    data := OnProcessCommand(data);
    
    var bw := new BinaryWriter(stream);
    bw.Write(data);
    bw.Flush();
    
    stream.Close();
    client.Close();
  end;
end;

/// Вызывается Клиентом для отправки команды Серверу
procedure SendCommand(s: string);
begin
  var client := new TcpClient(servername,port);
  var stream := client.GetStream();
  
  var bw := new BinaryWriter(stream);
  bw.Write(s);
  bw.Flush();

  stream.Close();
  client.Close();
end;

/// Вызывается Клиентом для отправки команды Серверу и получения ответа
function SendCommandAndReceiveAnswer(s: string): string;
begin
  var client := new TcpClient(servername,port);
  var stream := client.GetStream();
  
  var bw := new BinaryWriter(stream);
  bw.Write(s);
  bw.Flush();
  
  var br := new BinaryReader(stream);
  Result := br.ReadString();
  
  stream.Close();
  client.Close();
end;


begin
end.