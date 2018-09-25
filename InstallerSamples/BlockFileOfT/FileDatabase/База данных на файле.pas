uses BlockFileOfT;
uses System.Runtime.InteropServices;//нужно чтоб не писать System.Runtime.InteropServices.StructLayout и т.п.
uses FileArray;

type
  DataType1 = record
    b: byte;
    
    constructor(b:byte) :=
    self.b := b;
  end;
  DataType2 = record
    i: integer;
    
    constructor(i:integer) :=
    self.i := i;
  end;
  DataType3 = record
    ch: char;
    
    constructor(ch:char) :=
    self.ch := ch;
  end;
  DataType4 = record
    r: real;
    
    constructor(r:real) :=
    self.r := r;
  end;
  
  DataType = (
    ByteData = 1,
    IntData = 2,
    CharData = 3,
    RealData = 4
  );
  
  [StructLayout(LayoutKind.&Explicit, Size=16)]//Значит - мы сами указываем куда ставит каждое поле
  DataUnit = record
    [FieldOffset(0)] DataT: byte;
    [FieldOffset(8)] DataT1: DataType1;//У этих 4 полей одинаковая позиция
    [FieldOffset(8)] DataT2: DataType2;//Значит, у них будет общая память
    [FieldOffset(8)] DataT3: DataType3;//Но это так же значит что если записать данные 1 типа -
    [FieldOffset(8)] DataT4: DataType4;//данные другого типа считать не выйдет (выведет мусор)
    
    constructor(data: DataType1);
    begin
      DataT := 1;
      DataT1 := data;
    end;
    
    constructor(data: DataType2);
    begin
      DataT := 2;
      DataT2 := data;
    end;
    
    constructor(data: DataType3);
    begin
      DataT := 3;
      DataT3 := data;
    end;
    
    constructor(data: DataType4);
    begin
      DataT := 4;
      DataT4 := data;
    end;
    
    function ToString:string; override;
    begin
      var sb := new StringBuilder;
      
      sb.AppendFormat(
        'DataUnit({0,8}, ',
        System.Enum.GetName(typeof(DataType), DataT)
      );
      
      case DataType(DataT) of
        ByteData: sb.AppendFormat('{0})',DataT1.b);
        IntData : sb.AppendFormat('{0})',DataT2.i);
        CharData: sb.AppendFormat('{0})',DataT3.ch);
        RealData: sb.AppendFormat('{0})',DataT4.r);
        else raise new System.FormatException($'Не правильный тип данных: {DataT}');
      end;
      
      Result := sb.ToString;
    end;
  end;
  
  Database = class(System.IDisposable)
    
    public version: byte;
    public DatabaseType: string;
    public BlockFile:BlockFileOf<DataUnit>;
    public Data: FileArr<DataUnit>;
    
    public constructor := exit;
    
    public constructor(fname:string; version:byte; DatabaseType:string);
    begin
      self.version := version;
      self.DatabaseType := DatabaseType;
      
      self.BlockFile := new BlockFileOf<DataUnit>(fname);
      self.BlockFile.Offset := 5;
      self.BlockFile.Open(System.IO.FileMode.Create);
      
      self.Data := new FileArr<DataUnit>(self.BlockFile);
    end;
    
    public class function Load(fname:string): Database;
    begin
      Result := new Database;
      
      Result.BlockFile := new BlockFileOf<DataUnit>(fname,5);
      Result.BlockFile.Reset;
      
      begin//заголовок
        var str := Result.BlockFile.BaseStream;
        var sr := new System.IO.StreamReader(Result.BlockFile.BaseStream, System.Text.Encoding.UTF8);
        
        //str.Position := 0;//Мы только что открыли файл, Reset само поставило указатель в начало
        
        Result.version := str.ReadByte;
        
        var buff := new char[4];
        sr.ReadBlock(buff,0,4);
        Result.DatabaseType := new string(buff);
        
      end;
      
      Result.Data := new FileArr<DataUnit>(Result.BlockFile);
      
      Result.Data.AutoFlush := false;
      Result.Data.DeleteOnExit := false;
      
    end;
    
    public procedure Save;
    begin
      
      begin//заголовок
        var str := self.BlockFile.BaseStream;
        var sw := new System.IO.StreamWriter(str, System.Text.Encoding.UTF8);
        
        str.Position := 0;
        
        str.WriteByte(self.version);
        
        var buff := (self.DatabaseType+'_'*4).ToCharArray;
        sw.Write(buff, 0,4);
        
        sw.Flush;
      end;
      
      self.Data.Flush;
    end;
    
    public function ToString:string; override;
    begin
      var sb := new StringBuilder;
      sb += $'version: {version}{#10}';
      sb += $'Database Type: {DatabaseType}{#10}';
      sb += $'Data:[{#10}';
      
      BlockFile.Pos := 0;
      foreach var du in BlockFile.Read(BlockFile.Size) do
        sb += $'{#9}{du}{#10}';
      
      sb += $']';
      Result := sb.ToString;
    end;
    
    public procedure Dispose;
    begin
      Data.Finalize;
    end;
    
  end;
  
begin
  var db1 := new Database('temp.bin',1,'JSBD');//JSBD = Just Some Basic Data
  
  db1.version := 5;//Была 1, изменили на 5
  db1.Data[0] := new DataUnit(new DataType1(123));
  db1.Data[1] := new DataUnit(new DataType2(456));
  db1.Data[2] := new DataUnit(new DataType3('A'));
  db1.Data[3] := new DataUnit(new DataType4(123.456));
  
  db1.Save;
  db1.Dispose;
  
  
  
  db1 := Database.Load('temp.bin');
  writeln(db1);
  
  writeln;
  writeln('int from real: ',db1.Data[3].DataT2.i);//Пытаемся прочитать переменную типа integer там - где записана переменная типа real
                                                  //Это, конечно, выводит мусор
  
end.