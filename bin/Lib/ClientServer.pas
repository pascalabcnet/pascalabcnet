unit ClientServer;

uses System.Net, System.Net.Sockets, System.IO;

var 
  servername := '127.0.0.1';
  port := 13000;

/// ��������� ������, ����������� �� ������� ������ � ����������� ��������� ��������
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

/// ��������� ������, ����������� �� ������� ������, ����������� ��������� �������� � ������������ ������ � �����
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
    br.Close();
    
    data := OnProcessCommand(data);
    
    var bw := new BinaryWriter(stream);
    bw.Write(data);
    bw.Close();
    
    stream.Close();
    client.Close();
  end;
end;

/// ���������� �������� ��� �������� ������� �������
procedure SendCommand(s: string);
begin
  var client := new TcpClient(servername,port);
  var stream := client.GetStream();
  
  var bw := new BinaryWriter(stream);
  bw.Write(s);
  bw.Close();
  
  stream.Close();
  client.Close();
end;

/// ���������� �������� ��� �������� ������� ������� � ��������� ������
function SendCommandAndReceiveAnswer(s: string): string;
begin
  var client := new TcpClient(servername,port);
  var stream := client.GetStream();
  
  var bw := new BinaryWriter(stream);
  bw.Write(s);
  bw.Close();
  
  var br := new BinaryReader(stream);
  Result := br.ReadString();
  br.Close();
  
  stream.Close();
  client.Close();
end;


begin
end.