uses BlockFileOfT;
uses System.Runtime.InteropServices;//нужно чтоб не писать System.Runtime.InteropServices.StructLayout и т.п.
uses FileArray;

type
  DataType1 = record
    b: byte;
  end;
  DataType2 = record
    i: integer;
  end;
  DataType3 = record
    ch: char;
  end;
  DataType4 = record
    r: real;
  end;
  
  DataType = (
    ByteData = 1,
    IntData = 2,
    CharData = 3,
    RealData = 4
  );
  
  [StructLayout(LayoutKind.&Explicit)]//Значит - мы сами указываем куда ставит каждое поле
  DataUnit = record
    [FieldOffset(0)] DataT: byte;
    [FieldOffset(1)] DataT1: DataType1;//У этих 3 полей одинаковая позиция
    [FieldOffset(1)] DataT2: DataType2;//Значит, у них будет общая память
    [FieldOffset(1)] DataT3: DataType3;//Но это так же значит что если записать данные 1 типа -
    [FieldOffset(1)] DataT4: DataType4;//данные другого типа считать не выйдет (выведет мусор)
    
    function ToString:string; override;
    begin
      var sb := new StringBuilder;
      
      sb.AppendFormat(
        'DataUnit(DataT: {0,8}, ',
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
    public Data: FileArr<DataUnit>;
    
    public constructor := exit;
    
    public class function Load(fname:string): Database;
    begin
      Result := new Database;
      Result.Data := new FileArr<DataUnit>(fname, System.IO.FileMode.Open);
      Result.Data.AutoFlush := false;
      Result.Data.DeleteOnExit := false;
      
      begin//заголовок
        var f: BlockFileOf<DataUnit> := Result.Data.InnerFile;
        f.Offset := 5;//1 версия+4 длина строки-типа базы данных
        
        Result.version := f.BinReader.ReadByte;
        
        var sr := new System.IO.StreamReader(f.BaseStream, System.Text.Encoding.UTF8);
        var buff := new char[4];
        sr.ReadBlock(buff,0,4);
        Result.DatabaseType := new string(buff);
        
      end;
    end;
    
    public procedure Save;
    begin
      var f:BlockFileOf<DataUnit> := Data.InnerFile;
      f.PosByte := 0;
      f.BinWriter.Write(version);
      f.BaseStream.Position := 1;
      var sw := new System.IO.StreamWriter(f.BaseStream,System.Text.Encoding.UTF8);
      sw.Write(DatabaseType.ToCharArray,0,4);
      sw.Flush;
      Data.Flush;
    end;
    
    public function ToString:string; override;
    begin
      var sb := new StringBuilder;
      sb += $'version: {version}{#10}';
      sb += $'Database Type: {DatabaseType}{#10}';
      sb += $'Data:[{#10}';
      
      Data.InnerFile.Pos := 0;
      foreach var du in Data.InnerFile.Read(Data.InnerFile.Size) do
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
  var db1 := new Database;
  db1.version := 1;
  db1.DatabaseType := 'JSBD';//Just Some Basic Data
  db1.Data := new FileArr<DataUnit>('temp database.bin', System.IO.FileMode.Create);
  db1.Data.InnerFile.Offset := 5;
  db1.Data.AutoFlush := false;
  db1.Data.DeleteOnExit := false;
  
  var c := 10;
  db1.Data.Section64[0,c] := ArrGen&<DataUnit>(c,
    i->
    begin
      Result := new DataUnit;
      Result.DataT := Random(1,4);
      case DataType(Result.DataT) of
        ByteData: Result.DataT1.b := Random(256);
        IntData : Result.DataT2.i := (Random(256*256) shl 16) + Random(256*256);
        CharData: Result.DataT3.ch := char(Random(word('A'),word('Z')));
        RealData: Result.DataT4.r := Random;
      end;
    end
  );
  
  db1.Save;
  db1.Dispose;
  db1 := nil;
  
  
  
  db1 := Database.Load('temp database.bin');
  writeln(db1);
  db1.Dispose;
  
end.