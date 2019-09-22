//  BBC Micro:bit communication module

unit BBCMicrobit;

interface

///  Исключение для информирования об отсутствии подключенного устройства
type
  NoUBSComPortsException = class(Exception) end;
  NoBBCBoardFoundException = class(Exception) end;

///  Класс для взаимодействия с устройством BBC Micro:bit
type
  Microbit = class(System.IDisposable)
  
  protected 
    ///  Порт для связи с устройством
    Port: System.IO.Ports.SerialPort;
    
    ///  Обработчик данных от платы. Принимает только строки
    procedure DataReceivedHandler(sender: object; e: System.IO.Ports.SerialDataReceivedEventArgs);


  
  public  
    
    ///  Копирование файла прошивки в BBC:Microbit
    class procedure CopyFile(filename : string);

  public 
    ///  Обработчик события распознавания
    OnReadString: procedure(Message: string);
    
    ///  Список USB COM-портов с идентификаторами Microbit
    class function GetUsbPortNames: List<string>;
    begin
      //  Вот такие параметры у каждой платы (по крайней мере сейчас)
      var pattern := '^VID_0D28.PID_0204';
      var _rx := new System.Text.RegularExpressions.Regex(pattern, RegexOptions.IgnoreCase);
      Result := new List<string>;
      var {Microsoft.Win32.RegistryKey} rk1 := Microsoft.Win32.Registry.LocalMachine;
      var {Microsoft.Win32.RegistryKey} rk2 := rk1.OpenSubKey('SYSTEM\\CurrentControlSet\\Enum');
      
      foreach s3: string in rk2.GetSubKeyNames do
      begin
        var rk3 := rk2.OpenSubKey(s3);
        foreach s: string in rk3.GetSubKeyNames do
          if _rx.Match(s).Success then
          begin
            var {RegistryKey} rk4 := rk3.OpenSubKey(s);
            foreach s2: string in rk4.GetSubKeyNames do
            begin
              var {RegistryKey} rk5 := rk4.OpenSubKey(s2);
              var {RegistryKey} rk6 := rk5.OpenSubKey('Device Parameters');
              var {string} portName := string(rk6.GetValue('PortName'));
              if not String.IsNullOrEmpty(portName) and System.IO.Ports.SerialPort.GetPortNames.Contains(portName) then
              begin
                Result.Add(string(rk6.GetValue('PortName')));
              end;
            end;
          end;
      end;
    end;
    
    /// <summary>
    /// Конструктор объекта для взаимодействия с Microbit
    /// </summary>
    /// <param name="Handler">Процедура-обработчик, вызываемая для каждой принятой строки</param>
    /// <param name="PortName">Имя COM-порта, можно не указывать</param>
    constructor Create(Handler: procedure(s: string) := nil; PortName: string := '');
    begin
      OnReadString := Handler;
      //  Инициализируем виртуальный COM-порт, который связан с USB-устройством.
      Port := new System.IO.Ports.SerialPort;
      
      if PortName.Length > 0 then 
          //  Порт указан явно при создании объекта класса, значит его и используем
        Port.PortName := PortName
      else
      begin
        //  Порт явно не указан, приходится искать самостоятельно
        //  Получаем список всех похожих COM-портов (по VID и PID)
        var portNames := GetUsbPortNames;
        //  Если подходящих портов нет - выбрасываем исключение
        if portNames.Count = 0 then
          raise new NoUBSComPortsException('Не найдено устройство Microbit')
        else
          //  Берём первый попавшийся. Если устройств несколько, то надо явно указывать
          Port.PortName := portNames[0];
      end;
      
      //  Эти параметры зашиты явно, рекомендованы руководством по Microbit
      Port.BaudRate := 115200;    //Указываем скорость.
      Port.DataBits := 8; 
      Port.Parity := System.IO.Ports.Parity.None;
      Port.StopBits := System.IO.Ports.StopBits.One;
      Port.DataReceived += DataReceivedHandler;
      Port.Open;              //Открываем порт
    end;
    
    ///  Отправка строки на устройство Microbit
    procedure Send(message: string) := Port.WriteLine(message);
    
    ///  Деструктор - закрывает COM-порт 
    destructor Destroy;
    begin
      if Port <> nil then 
      begin
        Port.Close;
        Port := nil;
      end;
    end;
    
    procedure Dispose;
    begin
      Destroy;
    end;
    
    procedure Finalize; override;
    begin
      Destroy
    end;
  end;

implementation

///  Иногда строки приходят "склеенные" – нужно разбить по служебным символам
///  Эта функция возвращает первую группу символов из строки, ограниченную
///  служебными #10 или #13, сама подстрока из строки удаляется.
function ParseStr(var s : string) : string;
begin
  var iStart := 1;
  var len := s.Length;
  
  while (iStart<=len) and ((s[iStart] = #10) or (s[iStart] = #13)) do
    iStart += 1;
  
  var iEnd := iStart + 1;
  
  while (iEnd<=len) and (s[iEnd] <> #10) and (s[iEnd] <> #13) do
    iEnd += 1;
  
  //  Копируем фрагмент, но с проверкой
  if iEnd-iStart > 0 then
    Result := copy(s, iStart, iEnd-iStart)
  else
    Result := '';
  
  while (iEnd<len) and ((s[iEnd+1] = #10) or (s[iEnd+1] = #13)) do
    iEnd += 1;
  Delete(s,1,iEnd);
end;

procedure Microbit.DataReceivedHandler(sender: object; e: System.IO.Ports.SerialDataReceivedEventArgs);
  begin
    var sp := sender as System.IO.Ports.SerialPort;

    //  Читаем строку по ReadLine
    var message := sp.ReadLine;
      
      
    while message.Length > 0 do
      begin
        //  Очередная подстрока
        var block := ParseStr(message).TrimEnd(' ');
        if (block.Length > 0) and (OnReadString <> nil) then
          OnReadString(block);
      end;
  end;

class procedure MicroBit.CopyFile(filename : string);
begin
  var allDrives := System.IO.DriveInfo.GetDrives();
  
  foreach d: System.IO.DriveInfo in allDrives do
    begin
      if d.IsReady then
        if d.VolumeLabel = 'MICROBIT' then
          begin
            var dest : string := d.RootDirectory.ToString + '\\' + System.IO.Path.GetFileName(filename);
            System.IO.File.Copy(filename, dest);
            sleep(2000);
            exit;
          end;
    end;
  raise new NoBBCBoardFoundException('Не найдено устройство Microbit');
            
end;

end.