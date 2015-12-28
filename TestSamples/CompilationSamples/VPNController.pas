uses CRT, System.Net.NetworkInformation, System.Diagnostics;

const PingTimeOut = 3000;

procedure StartAndWait(Command:string; args: string);
var Pr:Process;
begin
  Pr := new Process;
  //Pr.StartInfo.CreateNoWindow := true;
  Pr.StartInfo.UseShellExecute := false;
  Pr.StartInfo.FileName := Command;
  Pr.StartInfo.Arguments := args;
  Pr.StartInfo.RedirectStandardOutput := true;
  Writeln(Command + ' ' + args);
  Pr.Start;
  try
    //Pr.BeginOutputReadLine();
    Writeln(Pr.StandardOutput.ReadToEnd);
    //Pr.WaitForExit();
  except
    Writeln('Ошибка: Не могу выполнить ' + Command);
  end;            
end;

function PingAdress(IP: string): boolean;
var IPPing: Ping;
    res: PingReply;
begin
  IPPing := new Ping;
  Result := true;
  try
    ClearLine;
    Write('Ping ' + IP + '...');
    res := IPPing.Send(IP, PingTimeOut);
  except
    Result := false;
  end;
  if Result then    
    Result := res.Status = IPStatus.Success;
  ClearLine;
  if Result then 
    Write('Ping ' + IP + '... OK')
  else
    Write('Ping ' + IP + '... ERROR');    
  Sleep(1000);
end;

procedure SleepSEC(sec: integer; msg: string);
begin
  for var i:=1 to sec do begin
    ClearLine;
    Write(String.Format(msg,sec-i));
    Sleep(1000);
  end;
end;

procedure WritelnWithTime(s: string);
begin
  Writeln(String.Format('[{0}] {1}', System.DateTime.Now.ToLongTimeString, s));
end;

procedure WriteWithTime(s: string);
begin
  Write(String.Format('[{0}] {1}', System.DateTime.Now.ToLongTimeString, s));
end;

var VPNIP, IP, ServiceName: string;
    CheckPeriod, VPNRestartWait: integer;

begin
  Writeln('VPNController v0.1 (c) DarkStar 2007');
  try
    ServiceName := CommandLineArgs[0];
    IP := CommandLineArgs[1];
    VPNIP := CommandLineArgs[2];
    CheckPeriod := System.Convert.ToInt32(CommandLineArgs[3]);
    VPNRestartWait := System.Convert.ToInt32(CommandLineArgs[4]);
  except
    Writeln('Comman Line Args: ServiceName IP VPNIP CheckPeriod VPNRestartWait');    
    exit;
  end;
  while true do begin
    SleepSEC(CheckPeriod,'Следующая проверка через {0} секунд');
    if not PingAdress(VPNIP) then begin
      if not PingAdress(IP) then begin
        Writeln;
        WritelnWithTime('Невозможно восстановить связть т.к. отуствует доступ к удаленному узлу '+ IP);  
      end else begin
        Writeln;
        WritelnWithTime('Нет связи с узлом ' + VPNIP);
        WritelnWithTime('Перзапускаю '+ServiceName+'...');
        StartAndWait('NET','STOP '+ServiceName);
        SleepSEC(VPNRestartWait,'Ожидаю запуска '+ServiceName+'... {0}');
        Writeln;
        StartAndWait('NET','START '+ServiceName);
      end;
    end else begin
      WriteWithTime('Все в норме');
    end;
  end;
end.