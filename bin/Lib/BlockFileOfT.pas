unit BlockFileOfT;

interface

uses System.Runtime.InteropServices;
uses System.IO;

type
  ///Тип, записывающий данные в файл по принципу схожему с "file of <T>"
  ///Но в отличии от "file of <T>" - данный тип сохраняет всю запись одним блоком,
  ///так, как она записа в памяти.
  ///Это даёт значительное преимущество по скорости, но ограничевает,
  ///типы, которые могут быть использованы в виде полей <T>
  ///
  ///Ожидается, что в видет шаблонного параметра <T> будет передана запись,
  ///не содержащая динамичных полей.
  ///Иначе целостность данных будет терятся
  ///Это значит, что поля записи T и всех вложенных записей - не могут быть:
  ///  -Указатели
  ///  -Ссылочные типы (то есть все классы)
  ///
  ///В будущих билдах ожидаются так же модули "BlockString", "BlockArray" и т.п.
  ///содержащие соответствующие типы для работы с BlockFileOf
  BlockFileOf<T>=class
  where T: record;
    
    private class sz: integer;
    
    private fi:FileInfo;
    private str:FileStream;
    private bw:BinaryWriter;
    private br:BinaryReader;
    
    
    private class constructor;
    begin
      sz := Marshal.SizeOf(typeof(T));
    end;
    
    private function GetName:string;
    private function GetFullName:string;
    
    private function GetFileSize:int64;
    private procedure SetFileSize(size:int64);
    
    private function GetByteFileSize:int64;
    private procedure SetByteFileSize(size:int64);
    
    private function GetPos:int64;
    private procedure SetPos(pos:int64);
    private function GetPosByte:int64;
    private procedure SetPosByte(pos:int64);
    
    
    ///Размер блока из одного элемента типа T, в байтах
    public property TSize:integer read integer(sz);
    ///Количество сохранённых в файл элементов типа T
    ///Чтоб установить длину файла - надо открыть файл. Но прочитать длину можно не открывая
    public property FileSize:int64 read GetFileSize write SetFileSize;
    ///Размер файла, в байтах
    ///Чтоб установить длину файла - надо открыть файл. Но прочитать длину можно не открывая
    public property FileByteSize:int64 read GetByteFileSize write SetByteFileSize;
    ///Имя файла
    public property Name:string read GetName;
    ///Полное имя файла
    public property FullName:string read GetFullName;
    ///Возвращает номер текущего элемента в файле (нумеруя с 0)
    public property Pos:int64 read GetPos write SetPos;
    ///Возвращает номер текущего байта в файле (нумеруя с 0)
    public property PosByte:int64 read GetPosByte write SetPosByte;
    ///Основной поток открытого файла (или nil если файл не открыт)
    ///Внимание!!! Любое действие которое изменит этот поток - приведёт к неожиданным последствиям, используйте его только если знаете что делаете
    public property BaseStream:FileStream read str;
    ///Переменная, которая записывает данные в основной поток (или nil если файл не открыт)
    ///Внимание!!! Любое действие которое изменит основной поток файла - приведёт к неожиданным последствиям, используйте его только если знаете что делаете
    public property BinWriter:BinaryWriter read bw;
    ///Переменная, которая читает данные из основного потока (или nil если файл не открыт)
    ///Внимание!!! Любое действие которое изменит основной поток файла - приведёт к неожиданным последствиям, используйте его только если знаете что делаете
    public property BinReader:BinaryReader read br;
    ///Переменная, показывающая данные о файле (или nil, если переменная не привязана к файлу)
    public property FileInfo:System.IO.FileInfo read fi;
    
    
    
    ///Привязывает данную переменную к файлу {fname}
    ///Привязывать можно и к не существующим файлам, при откритии определёнными способами (как Rewrite) новый файл будет создан
    public procedure Assign(fname:string);
    ///Убирает свять переменной и файла, если связь есть
    public procedure UnAssign;
    ///Открывает файл, способом описанным в переменной mode
    ///Чтоб получить переменную этого типа - пишите System.IO.FileMode.<способ_открытия_файла>
    public procedure Open(mode:FileMode);
    ///Удаляет связаный файл, если он существует
    public procedure Erase;
    ///Переименовывает файл
    ///Если указать другое расположение - файл будет перемещён
    public procedure Rename(NewName:string);
    
    ///Создает (или обнуляет) привязаный файл
    public procedure Rewrite;
    ///Привязывает данную переменную к файлу {fname} и создает (или обнуляет) этот файл
    public procedure Rewrite(fname:string);
    
    ///Открывает файл на чтение (ожидается, что он уже существует) и устанавливает позицию на начало файла
    public procedure Reset;
    ///Привязывает данную переменную к файлу {fname}, открывает этот файл на чтение (ожидается, что файл уже существует) и устанавливает позицию на начало файла
    public procedure Reset(fname:string);
    
    ///Открывает файл на чтение (ожидается, что он уже существует) и устанавливает позицию в конце файла
    public procedure Append;
    ///Привязывает данную переменную к файлу {fname}, открывает этот файл на чтение (ожидается, что файл уже существует) и устанавливает позицию в конце файла
    public procedure Append(fname:string);
    
    ///Переставляет позицию в файле на элемент #pos (нумеруя с 0)
    public procedure Seek(pos:int64);
    ///Переставляет позицию в файле на байт #pos (нумеруя с 0)
    public procedure SeekByte(pos:int64);
    ///Достигнут ли конец файла
    public function EOF := FileByteSize-PosByte < sz;
    ///Существует ли файл
    public function Exists := fi.Exists;
    
    ///Записывает все изменения в файл
    public procedure Flush;
    ///Сохраняет и закрывает файл, если он был открыт
    public procedure Close;
    
    
    ///Записывает один элемент одним блоком в файл
    public procedure Write(o: T);
    ///Записывает массив элементов одним блоком в файл
    public procedure Write(params o:array of T);
    ///Записывает последовательность элементов, у которой можно узнать длину, одним блоком в файл
    public procedure Write(o:ICollection<T>);
    ///Записывает последовательность элементов, у которой нельзя узнать длину, по 1 элементу типа T в файл
    public procedure Write(o:sequence of T);
    
    ///Читает один элемент из файла одним блоком
    public function Read:T;
    ///Читает массив элементов из файла одним блоком
    public function Read(c:integer):array of T;
    ///Возвращает ленивую последовательность
    ///При попытке доступа к элементам этой последовательности - начнёт читать элементы из файла, по блоку на элемент типа T
    public function ReadLazy(c:integer):sequence of T;
    private function InternalReadLazy(c:integer; start_pos:int64):sequence of T;
    
  end;

implementation

{$region Exception's}

type
  FileNotAssignedException = class(Exception)
    constructor :=
    Create($'Данная переменная не была привязана к файлу{10}Используйте метод Assign');
  end;
  FileNotOpenedException = class(Exception)
    constructor(fname:string) :=
    Create($'Файл {fname} ещё не открыт, откройте его с помощью Open, Reset или Append');
  end;
  FileNotClosedException = class(Exception)
    constructor(fname:string) :=
    Create($'Файл {fname} ещё открыт, закройте его методом Close перед тем как продолжать');
  end;

{$endregion Exception's}

{$region property implementation}

function BlockFileOf<T>.GetName:string;
begin
  if fi = nil then raise new FileNotAssignedException;
  Result := fi.Name;
end;

function BlockFileOf<T>.GetFullName:string;
begin
  if fi = nil then raise new FileNotAssignedException;
  Result := fi.FullName;
end;



function BlockFileOf<T>.GetFileSize:int64;
begin
  if fi = nil then raise new FileNotAssignedException;
  Result := fi.Length div sz;
end;

procedure BlockFileOf<T>.SetFileSize(size:int64);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  str.SetLength(size*sz);
end;


function BlockFileOf<T>.GetByteFileSize:int64;
begin
  if fi = nil then raise new FileNotAssignedException;
  Result := fi.Length;
end;

procedure BlockFileOf<T>.SetByteFileSize(size:int64);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  str.SetLength(size);
end;



function BlockFileOf<T>.GetPos:int64;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  Result := str.Position div sz;
end;

procedure BlockFileOf<T>.SetPos(pos:int64);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  str.Position := pos*sz;
end;


function BlockFileOf<T>.GetPosByte:int64;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  Result := str.Position;
end;

procedure BlockFileOf<T>.SetPosByte(pos:int64);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  str.Position := pos;
end;

{$endregion property implementation}

{$region Setup IO}

{$region Basic}

procedure BlockFileOf<T>.Assign(fname:string);
begin
  if str <> nil then raise new FileNotClosedException(fi.FullName);
  fi := new System.IO.FileInfo(fname);
end;

procedure BlockFileOf<T>.UnAssign;
begin
  if str <> nil then raise new FileNotClosedException(fi.FullName);
  fi := nil;
end;

procedure BlockFileOf<T>.Open(mode:FileMode);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str <> nil then raise new FileNotClosedException(fi.FullName);
  str := fi.Open(mode);
  bw := new BinaryWriter(str);
  br := new BinaryReader(str);
end;

procedure BlockFileOf<T>.Erase;
begin
  if fi = nil then raise new FileNotAssignedException;
  fi.Delete;
end;

procedure BlockFileOf<T>.Rename(NewName:string);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str <> nil then raise new FileNotClosedException(fi.FullName);
  fi.MoveTo(NewName);
end;

{$endregion Assign}

{$region Rewrite}

procedure BlockFileOf<T>.Rewrite :=
Open(FileMode.Create);

procedure BlockFileOf<T>.Rewrite(fname:string);
begin
  Assign(fname);
  Rewrite;
end;

{$endregion Rewrite}

{$region Reset}

procedure BlockFileOf<T>.Reset :=
Open(FileMode.Open);

procedure BlockFileOf<T>.Reset(fname:string);
begin
  Assign(fname);
  Reset;
end;

{$endregion Reset}

{$region Append}

procedure BlockFileOf<T>.Append :=
Open(FileMode.Append);

procedure BlockFileOf<T>.Append(fname:string);
begin
  Assign(fname);
  Append;
end;

{$endregion Append}

{$region Seek}

procedure BlockFileOf<T>.Seek(pos:int64);
begin
  if str = nil then raise new FileNotOpenedException;
  str.Position := pos*sz;
end;

procedure BlockFileOf<T>.SeekByte(pos:int64);
begin
  if str = nil then raise new FileNotOpenedException;
  str.Position := pos;
end;

{$endregion Seek}

{$region Closing}

procedure BlockFileOf<T>.Flush;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  str.Flush;
end;

procedure BlockFileOf<T>.Close;
begin
  if str <> nil then
  begin
    str.Close;
    str := nil;
    br := nil;
    bw := nil;
  end;
end;

{$endregion Closing}

{$endregion Setup IO}

{$region Write}

procedure BlockFileOf<T>.Write(o: T);
begin
  var a := new byte[sz];
  var ptr:^T := pointer(@a[0]);
  ptr^ := o;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(params o:array of T);
begin
  var a := new byte[sz*o.Length];
  var ptr_id := integer(@a[0]);
  for var i := 0 to o.Length - 1 do
  begin
    var ptr:^T := pointer(ptr_id);
    ptr^ := o[i];
    ptr_id += sz;
  end;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o:ICollection<T>);
begin
  var a := new byte[sz*o.Count];
  var ptr_id := integer(@a[0]);
  foreach var el in o do
  begin
    var ptr:^T := pointer(ptr_id);
    ptr^ := el;
    ptr_id += sz;
  end;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o:sequence of T) :=
foreach var el in o do
  Write(el);

{$endregion Write}

{$region Read}

function BlockFileOf<T>.Read:T;
begin
  var a := br.ReadBytes(sz);
  var ptr:^T := pointer(@a[0]);
  Result := ptr^;
end;

function BlockFileOf<T>.Read(c:integer):array of T;
begin
  var a := br.ReadBytes(sz*c);
  Result := new T[c];
  var ptr_id := integer(@a[0]);
  for var i := 0 to c - 1 do
  begin
    var ptr:^T := pointer(ptr_id);
    Result[i] := ptr^;
    ptr_id += sz;
  end;
end;

function BlockFileOf<T>.ReadLazy(c:integer):sequence of T :=
InternalReadLazy(c,PosByte);

function BlockFileOf<T>.InternalReadLazy(c:integer; start_pos:int64):sequence of T;
begin
  PosByte := start_pos;
  loop c do
    yield Read;
end;







{$endregion Read}

end.