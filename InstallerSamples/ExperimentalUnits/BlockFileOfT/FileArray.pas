///Модуль с массивом, хранящим данные в файле
unit FileArray;

interface

uses BlockFileOfT;

type
  ///Массив, хранящим данные в файле
  ///На много медленнее обычного, но хранит на много больший объём данных
  ///T может быть только запись, не содержащей ссылочные поля (указатели, классы и т.п.)
  FileArr<T> = sealed class
  where T: record;
    
    private f: BlockFileOf<T>;
    ///Автоматически отчищает кэш после каждого чтения и записи
    public AutoFlush := true;
    ///Указывает, удалять ли файл когда сборщик мусора отчистит данную переменную
    public DeleteOnExit: boolean;
    
    
    private function GetItem32(i:integer):T;
    private function GetItem64(i:int64):T;
    private function GetSection32(from,c:integer):array of T;
    private function GetSection64(from,c:int64):array of T;
    private function GetSectionLazy32(from,c:integer):sequence of T;
    private function GetSectionLazy64(from,c:int64):sequence of T;
    
    private procedure SetItem32(i:integer; o:T);
    private procedure SetItem64(i:int64; o:T);
    private procedure SetSection32(from,c:integer; o:array of T);
    private procedure SetSection64(from,c:int64; o:array of T);
    private procedure SetSectionLazy32(from,c:integer; o:sequence of T);
    private procedure SetSectionLazy64(from,c:int64; o:sequence of T);
    
    
    
    ///Количество элементов. В отличии от array of T, его можно устанавливать
    public property InnerFile: BlockFileOf<T> read f;
    
    ///Количество элементов. В отличии от array of T, его можно устанавливать
    public property Length: integer read f.FileSize write f.FileSize := value;
    ///Количество элементов. В отличии от array of T, его можно устанавливать
    public property Length64: int64 read f.FileSize write f.FileSize := value;
    
    ///Элемент с индексом i
    public property Item[i:integer]:T read GetItem32 write SetItem32;
    ///Элемент с индексом i
    public property Item64[i:int64]:T read GetItem64 write SetItem64; default;
    
    ///Срез начиная с элемента #from и длиной count
    public property Section[from,count:integer]:array of T read GetSection32 write SetSection32;
    ///Срез начиная с элемента #from и длиной count
    public property Section64[from,count:int64]:array of T read GetSection64 write SetSection64;
    
    ///Возвращает ленивую последовательность начиная с элемента #from и длиной count
    ///Устанавливает значение элементов начиная с элемента #from и длиной count
    public property SectionLazy[from,count:integer]:sequence of T read GetSectionLazy32 write SetSectionLazy32;
    ///Возвращает ленивую последовательность начиная с элемента #from и длиной count
    ///Устанавливает значение элементов начиная с элемента #from и длиной count
    public property SectionLazy64[from,count:int64]:sequence of T read GetSectionLazy64 write SetSectionLazy64;
    
    
    ///Отчищает кэш чтения/записи
    procedure Flush := f.BaseStream.Flush;
    
    
    ///Создаёт новый массив хранящий данные в файле fname. Если его нет - создаёт новый
    public constructor(fname: string) :=
    Create(fname, System.IO.FileMode.OpenOrCreate);
    ///Создаёт новый массив хранящий данные в файле fname.
    ///mode указывает каким образом открывать/создавать файл и имеет тип System.IO.FileMode
    public constructor(fname: string; mode: System.IO.FileMode);
    begin
      f := new BlockFileOf<T>(fname);
      f.Open(mode);
    end;
    ///Создаёт новый массив хранящий данные в новом временном файл (в системной папке)
    ///Созданный файл будет удалёт в произвольное время (после завершения пользования переменной) или при вызове Finalize
    public constructor;
    begin
      Create(System.IO.Path.GetTempFileName);
      DeleteOnExit := true;
    end;
    ///Выполняет необходимую отчистку при завершении работы с переменной
    ///Этот метот выполнится автоматически в произвольное время (после завершения пользования переменной)
    ///Если попытаться использовать переменную после вызова этого метода - поведение будет неопределённым (скорее всего System.NullReferenceException)
    public procedure Finalize; override;
    begin
      f.Close;
      if DeleteOnExit then f.Erase;
    end;
    
  end;
  
implementation

{$region index property's}

function FileArr<T>.GetItem32(i:integer) := GetItem64(i);

function FileArr<T>.GetSection32(from,c:integer) := GetSection64(from,c);

function FileArr<T>.GetSectionLazy32(from,c:integer) := GetSectionLazy64(from,c);

procedure FileArr<T>.SetItem32(i:integer; o:T) := SetItem64(i,o);

procedure FileArr<T>.SetSection32(from,c:integer; o:array of T) := SetSection64(from,c,o);

procedure FileArr<T>.SetSectionLazy32(from,c:integer; o:sequence of T) := SetSectionLazy64(from,c,o);

function FileArr<T>.GetItem64(i:int64):T;
begin
  f.Seek(i);
  Result := f.Read;
  if AutoFlush then f.BaseStream.Flush;
end;

function FileArr<T>.GetSection64(from,c:int64):array of T;
begin
  f.Seek(from);
  Result := f.Read(c);
  if AutoFlush then f.BaseStream.Flush;
end;

function FileArr<T>.GetSectionLazy64(from,c:int64):sequence of T;
begin
  f.Seek(from);
  Result := f.ReadLazy(c);
  if AutoFlush then f.BaseStream.Flush;
end;

procedure FileArr<T>.SetItem64(i:int64; o:T);
begin
  f.Seek(i);
  f.Write(o);
  if AutoFlush then f.BaseStream.Flush;
end;

procedure FileArr<T>.SetSection64(from,c:int64; o:array of T);
begin
  f.Seek(from);
  if c > o.Length then raise new System.IndexOutOfRangeException('count должно быть <= длинны переданного массива');
  f.Write(o,0,c);
  if AutoFlush then f.BaseStream.Flush;
end;

procedure FileArr<T>.SetSectionLazy64(from,c:int64; o:sequence of T);
begin
  f.Seek(from);
  f.Write(o,0,c);
  if AutoFlush then f.BaseStream.Flush;
end;

{$endregion index property's}

end.