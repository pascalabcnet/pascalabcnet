
//*****************************************************************************************************\\
// Copyright (©) Cergey Latchenko ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// This code is distributed under the Unlicense
// For details see LICENSE file or this:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\
// Copyright (©) Сергей Латченко ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// Этот код распространяется с лицензией Unlicense
// Подробнее в файле LICENSE или тут:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\

///Модуль, содержащий тип BlockFileOf<T>
///Тип-альтернатива стандартному file of T
///Главное преимущество - скорость работы
unit BlockFileOfT;

interface

uses System.Runtime.InteropServices;
uses System.IO;

type
  ///--
  BlockFileBase = abstract class
    
    protected fi: FileInfo;
    protected _offset: int64;
    protected str: FileStream;
    
    protected linked := new List<BlockFileBase>;
    
    protected procedure Link(f: BlockFileBase);
    
    protected procedure UnLink;
    
    private static function MessageBox(wnd: System.IntPtr; message, caption: string; flags: cardinal): integer; external 'User32.dll';
    
  end;
  
  ///Тип, записывающий данные в файл по схожему с "file of <T>" принципу
  ///Но, в отличии от типизированных файлов, данный тип сохраняет всю запись одним блоком так, как она записа в памяти.
  ///Это даёт значительное преимущество по скорости, но ограничивает типы, которые могут быть использованы в качестве полей типа <T>
  ///
  ///Это значит, что поля записи T и всех вложенных записей не могут быть:
  /// - Указателями
  /// - Ссылочными типами (классами / динамическими массивами)
  ///Однако, эти ограничения можно обойти, об этом в справке
  BlockFileOf<T> = class(BlockFileBase) where T: record;
    
    {$region Internal}
    
    private static sz: integer;
    
    private static procedure TestForRefT(tt: System.Type);
    begin
      if tt = typeof(System.IntPtr) then exit; // IntPtr содержит 1 поле типа pointer. Но IntPtr это не указатель а число, с размером как у pointer
      
      if tt.IsClass then raise new System.InvalidOperationException($'Тип {tt} ссылочный.{#10}Ссылочные типы нельзя сохранять в типизированный файл');
      
      foreach var fi in tt.GetFields(
        System.Reflection.BindingFlags.GetField or
        System.Reflection.BindingFlags.Instance or
        System.Reflection.BindingFlags.Public or
        System.Reflection.BindingFlags.NonPublic
      ) do
        if not fi.IsLiteral then
          if fi.FieldType <> tt then // тип integer имеет поле типа integer, без этой строчки StackOverflowException
            TestForRefT(fi.FieldType);
    end;
    
    private static constructor :=
    try
      TestForRefT(typeof(T));
      sz := Marshal.SizeOf&<T>;
    except
      on e:Exception do
      begin
        MessageBox(new System.IntPtr(nil),
          e.ToString,
          $'BlockFileOf<{typeof(T)}> не может инициализироваться:',
          $10
        );
        Halt(-1);
      end;
    end;
    
    private function GetName: string;
    private function GetFullName: string;
    
    private function GetExists: boolean;
    
    private function GetFileSize: int64 := (GetByteFileSize - _offset) div sz;
    private procedure SetFileSize(size: int64) := SetByteFileSize(_offset + size*sz);
    
    private function GetByteFileSize: int64;
    private procedure SetByteFileSize(size: int64);
    
    private function GetPos: int64 := (GetPosByte-_offset) div sz;
    private procedure SetPos(pos: int64) := SetPosByte(_offset + pos*sz);
    private function GetPosByte: int64;
    private procedure SetPosByte(pos: int64);
    
    {$endregion Internal}
    
    {$region constructor's}
    
    ///Инициализирует переменную файла, не привязывая её к файлу на диске
    public constructor := exit;
    
    ///Инициализирует переменную файла, привязывая её к файлу с именем fname
    public constructor(fname: string) :=
    Assign(fname);
    
    ///Инициализирует переменную файла, привязывая её к файлу с именем fname
    ///Устанавливает значение смещения от начала файла в байтах на offset
    public constructor(fname: string; offset: int64) :=
    Assign(fname, offset);
    
    ///- constructor BlockFileOf<>(f: BlockFileOf<>);
    ///Инициализирует новую переменную, создавая связку с заданной переменной
    ///После вызова этого конструктора переданная и созданная переменные будут использовать общий файловый поток, но записывать разные типы данных (у них может быть разный T)
    ///Это значит, что переменная, которую передали в конструктор, уже должна иметь открытый файловый поток
    ///Метод Close разрывает эту связь.
    public constructor(f: BlockFileBase) :=
    Link(f);
    
    {$endregion constructor's}
    
    {$region property's}
    
    ///Возвращает или задаёт размер блока из одного элемента в байтах
    ///Задавать это свойство не рекомендуется
    public property TSize: integer read integer(sz) write sz := value;
    
    ///Возвращает или задаёт смещение от начала файла до начала элементов в байтах
    public property Offset: int64 read _offset write _offset;
    
    ///Возвращает или задаёт количество сохранённых в файл элементов
    ///Задавать можно только после открытия файла
    public property Size: int64 read GetFileSize write SetFileSize;
    
    ///Возвращает или задаёт размер файла в байтах
    ///Задавать можно только после открытия файла
    public property ByteSize: int64 read GetByteFileSize write SetByteFileSize;
    
    ///Возвращает неполное имя файла
    public property Name: string read GetName;
    
    ///Возвращает полное имя файла
    public property FullName: string read GetFullName;
    
    ///Определяет, существует ли файл
    public property Exists: boolean read GetExists;
    
    ///Возвращает или задаёт номер текущего элемета в файле (нумеруя с 0)
    public property Pos: int64 read GetPos write SetPos;
    
    ///Возвращает или задаёт номер текущего байта от начала файла (нумеруя с 0)
    public property PosByte: int64 read GetPosByte write SetPosByte;
    
    ///Определяет, привязана ли переменная к файлу
    public property Assigned: boolean read fi<>nil;
    
    ///Определяет, открыт ли файл
    public property Opened: boolean read str<>nil;
    
    ///Определяет, достигнут ли конец файла
    public property EOF: boolean read ByteSize-PosByte < sz;
    
    ///Возвращает поток текущего файла (или nil если файл не открыт)
    ///Внимание! Любое действие, связанное с изменением данного потока файла приведёт к неожиданным последствиям, используйте его только если знаете, что вы делаете
    public property BaseStream: FileStream read str;
    
    ///Возвращает FileInfo текущего файла (или nil, если переменная не привязана к файлу)
    public property FileInfo: System.IO.FileInfo read fi;
    
    {$endregion property's}
    
    {$region Setup IO}
    
    ///Привязывает текущий экземпляр BlockFileOf<T> к файлу с именем fname
    ///Привязывать можно и к несуществующим файлам, при открытии на запись будет создан новый файл
    public procedure Assign(fname: string);
    
    ///Привязывает данную переменную к файлу с именем fname
    ///Устанавливает смещение от начала файла до начала элементов в байтах на offset
    ///Привязывать можно и к несуществующим файлам, при открытии чем то вроде Rewrite будет создан новый файл
    public procedure Assign(fname: string; offset: int64);
    
    ///Удаляет связь переменной с файлом, если связь есть
    public procedure UnAssign;
    
    ///Открывает файл способом mode
    public procedure Open(mode: FileMode);
    
    ///Удаляет связаный файл, если он существует
    public procedure Delete;
    
    ///Переименовывает файл
    ///При указании иного расположения файл будет перемещён
    public procedure Rename(NewName: string);
    
    ///Создает (или обнуляет) файл
    public procedure Rewrite;
    
    ///Привязывает данную переменную к файлу с именем fname и создает (или обнуляет) его
    public procedure Rewrite(fname: string);
    
    ///Привязывает данную переменную к файлу с именем fname и создает (или обнуляет) его
    ///Устанавливает смещение от начала файла до начала элементов в байтах на offset
    public procedure Rewrite(fname: string; offset: int64);
    
    ///Открывает файл на чтение
    public procedure Reset;
    
    ///Привязывает данную переменную к файлу с именем fname и открывает его на чтение
    public procedure Reset(fname: string);
    
    ///Привязывает данную переменную к файлу с именем fname и открывает его на чтение
    ///Устанавливает смещение от начала файла до начала элементов в байтах на offset
    public procedure Reset(fname: string; offset: int64);
    
    ///Открывает файл и устанавливает позицию на конец файла
    public procedure Append;
    
    ///Привязывает данную переменную к файлу с именем fname и открывает его, устанавливая позицию на конец файла
    public procedure Append(fname: string);
    
    ///Привязывает данную переменную к файлу с именем fname и открывает его, устанавливая позицию на конец файла
    ///Устанавливает смещение от начала файла до начала элементов в байтах на offset
    public procedure Append(fname: string; offset: int64);
    
    ///Записывает все изменения в файл и отчищает внутренние буферы
    ///До вызова Flush или Close все изменения и кеш хранятся в оперативной памяти
    ///Если вы проводите операции с большими объёмами памяти - рекомендуется вызывать Flush время от времени
    public procedure Flush;
    
    ///Сохраняет и закрывает файл, если он открыт
    public procedure Close;
    
    {$endregion Setup IO}
    
    {$region Write}
    
    ///Записывает один элемент одним блоком в файл
    ///Переставляет файловый курсор на 1 элемет вперёд
    public procedure Write(o: T);
    
    ///Записывает массив элементов одним блоком в файл
    ///Переставляет файловый курсор на количество элементов, равному количеству элементов в переданном массиве
    public procedure Write(params o: array of T);
    
    ///Записывает последовательность элементов, у которой можно узнать длину, одним блоком в файл
    ///Переставляет файловый курсор на количество элементов, равному количеству элементов в переданной коллекции
    public procedure Write(o: ICollection<T>);
    
    ///Записывает последовательность элементов, у которой нельзя узнать длину, по 1 элементу в файл
    ///Переставляет файловый курсор на количество элементов, равному количеству элементов в переданной последовательности
    public procedure Write(o: sequence of T);
    
    ///Записывает count элементов массива, начиная с элемента from, одним блоком в файл
    ///Переставляет файловый курсор на сount элеметов вперёд
    public procedure Write(o: array of T; from, count: integer);
    
    ///Записывает count элементов последовательности, у которой можно узнать длину, начиная с элемента from, одним блоком в файл
    ///Переставляет файловый курсор на сount элеметов вперёд
    public procedure Write(o: ICollection<T>; from, count: integer);
    
    ///Записывает count элементов последовательности, у которой нельзя узнать длину, начиная с элемента from, одним блоком в файл
    ///После каждого элемента переставляет файловый курсор на 1 элемет вперёд
    public procedure Write(o: sequence of T; from, count: integer);
    
    {$endregion Write}
    
    {$region Read}
    
    ///Читает один элемент из файла одним блоком
    ///Переставляет файловый курсор на 1 элемет вперёд
    public function Read: T;
    
    ///Читает массив из count элементов из файла одним блоком
    ///Переставляет файловый курсор на count элеметов вперёд
    public function Read(count: integer): array of T;
    
    ///Читает массив из count элементов одним блоком, начиная с элемента start_elm
    ///И переставляет файловый курсор на count элеметов вперёд
    public function Read(start_elm, count: integer): array of T;
    
    ///Читает один элемент из файла одним блоком и записывает в уже существующую переменную
    ///Переставляет файловый курсор на 1 элемет вперёд
    public procedure Read(var o: T);
    
    ///Читает o.Length элементов из файла одним блоком и записывает их в уже существующий массив
    ///Переставляет файловый курсор на o.Length элеметов вперёд
    public procedure Read(o: array of T);
    
    ///Читает count элементов одним блоком, начиная с элемента start_elm
    ///Записывает их в массив o начиная с индекса arr_offset
    ///И переставляет файловый курсор на count элеметов вперёд
    public procedure Read(o: array of T; start_elm, arr_offset, count: integer);
    
    private function InternalReadLazy(c: integer; start_pos: int64): sequence of T;
    
    ///Возвращает ленивую последовательность из count элементов
    ///После завершения чтения курсор окажется на последнем элементе возвращаемой последовательности
    ///Перед чтением каждого элемента файловый курсор переставляется на start_elm+n
    ///Где start_elm - значение Pos на момент вызова ReadLazy, а n - количество уже считанных элементов
    public function ReadLazy(count: integer): sequence of T := InternalReadLazy(count, PosByte);
    
    ///Возвращает ленивую последовательность из count элементов, начиная с элемента start_elm
    ///После завершения чтения курсор окажется на последнем элементе возвращаемой последовательности
    ///Перед чтением каждого элемента файловый курсор переставляется на start_elm+n
    ///Где n - количество уже считанных элементов
    public function ReadLazy(start_elm, count: integer): sequence of T := InternalReadLazy(count, _offset + start_elm*sz);
    
    {$endregion Read}
    
    {$region Utils}
    
    ///Ставит позицию в файле на элемент pos (нумеруя с 0)
    ///Рекомендуется использовать свойство Pos вместо данного метода
    public procedure Seek(pos: int64) := self.Pos := pos;
    
    ///Ставит файловый курсор на байт pos (нумеруя с 0) от начала файла
    ///Рекомендуется использовать свойство PosByte вместо данного метода
    public procedure SeekByte(pos: int64) := self.PosByte := pos;
    
    ///Возвращает ленивую последовательность из блоков-массивов с элементами типа T
    ///Каждый блок хранит такое количество байт, которое не превышает 4 КБ
    ///После прочтения каждого блока файловый корсор будет переставлен на его конец
    public function ToSeqBlocks: sequence of array of T := ToSeqBlocks(4096);
    
    ///Возвращает ленивую последовательность из блоков-массивов с элементами типа T
    ///Каждый блок хранит такое количество байт, которое не превышает blocks_size
    ///После чтения каждого блока файловый корсор будет переставлен на его конец
    public function ToSeqBlocks(blocks_size: integer): sequence of array of T;
    
    ///Возвращает ленивую последовательность из всех элементов, хранящихся в файле
    ///После прохода по элементам последовательности позиция в файле будет передвигаться на последний использованный элемент
    public function ToSeq: sequence of T;
    
    protected procedure Finalize; override;
    begin
      Close;
    end;
    
    {$endregion Utils}
    
  end;

{$region Exception's}

type
  FileNotAssignedException = class(Exception)
    constructor :=
    inherited Create($'Данная переменная не привязана к файлу{10}Используйте метод Assign');
  end;
  FileNotOpenedException = class(Exception)
    constructor(fname:string) :=
    inherited Create($'Файл {fname} не открыт, откройте его с помощью Open, Reset, Append или Rewrite');
  end;
  FileNotClosedException = class(Exception)
    constructor(fname:string) :=
    inherited Create($'Файл {fname} открыт, закройте его методом Close перед тем как продолжить');
  end;
  CannotReadAfterEOF = class(Exception)
    constructor :=
    inherited Create($'Нельзя читать за пределами файла');
  end;

{$endregion Exception's}

implementation

{$region Linking}

procedure BlockFileBase.Link(f: BlockFileBase);
begin
  
  if f.str = nil then raise new FileNotOpenedException($'{f.fi.FullName}, чью переменную передали в конструктор BlockFileOf<T>,');
  
  foreach var l in f.linked + f do
  begin
    self.linked.Add(l);
    l.linked.Add(self);
  end;
  
  self.str := f.str;
  self.fi := new FileInfo(f.fi.FullName);
  
end;

procedure BlockFileBase.UnLink;
begin
  
  foreach var l in self.linked do
    l.linked.Remove(self);
  
  self.linked.Clear;
  
end;

{$endregion Linking}

{$region property implementation}

function BlockFileOf<T>.GetName: string;
begin
  if fi = nil then raise new FileNotAssignedException;
  fi.Refresh;
  Result := fi.Name;
end;

function BlockFileOf<T>.GetFullName: string;
begin
  if fi = nil then raise new FileNotAssignedException;
  //fi.Refresh; // А тут не надо
  Result := fi.FullName;
end;



function BlockFileOf<T>.GetByteFileSize: int64;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str <> nil then
    Result := str.Length else
  begin
    fi.Refresh;
    Result := fi.Length;
  end;
end;

procedure BlockFileOf<T>.SetByteFileSize(size: int64);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  str.SetLength(size);
end;



function BlockFileOf<T>.GetPosByte: int64;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  Result := str.Position;
end;

procedure BlockFileOf<T>.SetPosByte(pos: int64);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  str.Position := pos;
end;

{$endregion property implementation}

{$region Setup IO}

{$region Basic}

procedure BlockFileOf<T>.Assign(fname: string);
begin
  if str <> nil then raise new FileNotClosedException(fi.FullName);
  fi := new System.IO.FileInfo(fname);
end;

procedure BlockFileOf<T>.Assign(fname: string; offset: int64);
begin
  Assign(fname);
  _offset := offset;
end;

procedure BlockFileOf<T>.UnAssign;
begin
  if str <> nil then raise new FileNotClosedException(fi.FullName);
  fi := nil;
end;

procedure BlockFileOf<T>.Open(mode: FileMode);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str <> nil then raise new FileNotClosedException(fi.FullName);
  str := fi.Open(mode);
end;

procedure BlockFileOf<T>.Delete;
begin
  if fi = nil then raise new FileNotAssignedException;
  fi.Delete;
end;

function BlockFileOf<T>.GetExists: boolean;
begin
  if fi = nil then raise new FileNotAssignedException;
  fi.Refresh;
  Result := fi.Exists;
end;

procedure BlockFileOf<T>.Rename(NewName: string);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str <> nil then raise new FileNotClosedException(fi.FullName);
  fi.MoveTo(NewName);
end;

{$endregion Assign}

{$region Rewrite}

procedure BlockFileOf<T>.Rewrite :=
Open(FileMode.Create);

procedure BlockFileOf<T>.Rewrite(fname: string);
begin
  Assign(fname);
  Rewrite;
end;

procedure BlockFileOf<T>.Rewrite(fname: string; offset: int64);
begin
  Assign(fname, offset);
  Rewrite;
end;

{$endregion Rewrite}

{$region Reset}

procedure BlockFileOf<T>.Reset :=
Open(FileMode.Open);

procedure BlockFileOf<T>.Reset(fname: string);
begin
  Assign(fname);
  Reset;
end;

procedure BlockFileOf<T>.Reset(fname: string; offset: int64);
begin
  Assign(fname, offset);
  Reset;
end;

{$endregion Reset}

{$region Append}

procedure BlockFileOf<T>.Append;
begin
  Open(FileMode.Open);
  str.Position := str.Length;
end;

procedure BlockFileOf<T>.Append(fname: string);
begin
  Assign(fname);
  Append;
end;

procedure BlockFileOf<T>.Append(fname: string; offset: int64);
begin
  Assign(fname, offset);
  Append;
end;

{$endregion Append}

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
    if linked.Count <> 0 then
      UnLink else
      str.Close;
    str := nil;
  end;
end;

{$endregion Closing}

{$endregion Setup IO}

{$region IO Utils}

procedure CopyMem<T1,T2>(var o1: T1; var o2: T2; count: integer) :=
System.Buffer.MemoryCopy(
  @o1, @o2,
  count, count
);

{$endregion IO Utils}

{$region Write}

procedure BlockFileOf<T>.Write(o: T);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  var a := new byte[sz];
  CopyMem(o,a[0],sz);
  str.Write(a,0,sz);
end;

procedure BlockFileOf<T>.Write(params o: array of T);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  
  var bl := sz*o.Length;
  var a := new byte[bl];
  CopyMem(o[0],a[0],bl);
  str.Write(a,0,bl);
end;

procedure BlockFileOf<T>.Write(o: ICollection<T>);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  
  var bl := sz*o.Count;
  var a := new byte[bl];
  
  var p := 0;
  var enm: IEnumerator<T> := o.GetEnumerator();
  while enm.MoveNext do
  begin
    var v := enm.Current;
    CopyMem(v,a[p],sz);
    p += sz;
  end;
  
  str.Write(a,0,bl);
end;

procedure BlockFileOf<T>.Write(o: sequence of T) :=
foreach var el in o do
  Write(el);

procedure BlockFileOf<T>.Write(o: array of T; from, count: integer);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if from+count > o.Length then raise new System.IndexOutOfRangeException;
  
  var bl := sz*count;
  var a := new byte[bl];
  CopyMem(o[from],a[0],bl);
  str.Write(a,0,bl);
end;

procedure BlockFileOf<T>.Write(o: ICollection<T>; from, count: integer);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if from+count > o.Count then raise new System.IndexOutOfRangeException;
  
  var bl := sz*count;
  var a := new byte[bl];
  
  var p := 0;
  var enm: IEnumerator<T> := o.Skip(from).Take(count).GetEnumerator;
  while enm.MoveNext do
  begin
    var v := enm.Current;
    CopyMem(v,a[p],sz);
    p += sz;
  end;
  
  str.Write(a,0,bl);
end;

procedure BlockFileOf<T>.Write(o: sequence of T; from, count: integer) :=
Write(o.Skip(from).Take(count));

{$endregion Write}

{$region Read}

function BlockFileOf<T>.Read: T;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < sz then raise new CannotReadAfterEOF;
  
  var a := new byte[sz];
  str.Read(a,0,sz);
  CopyMem(a[0],Result,sz);
  
end;

function BlockFileOf<T>.Read(count: integer): array of T;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < sz*count then raise new CannotReadAfterEOF;
  
  var bl := sz*count;
  var a := new byte[bl];
  str.Read(a,0,bl);
  Result := new T[count];
  CopyMem(a[0],Result[0],bl);
  
end;

function BlockFileOf<T>.Read(start_elm, count:integer): array of T;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < sz*count then raise new CannotReadAfterEOF;
  
  Pos := start_elm;
  Result := Read(count);
  
end;

procedure BlockFileOf<T>.Read(var o: T);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < sz then raise new CannotReadAfterEOF;
  
  var a := new byte[sz];
  str.Read(a,0,sz);
  CopyMem(a[0],o,sz);
  
end;

procedure BlockFileOf<T>.Read(o: array of T);
begin
  var bl := sz*o.Length;
  
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < bl then raise new CannotReadAfterEOF;
  
  var a := new byte[bl];
  str.Read(a,0,bl);
  CopyMem(a[0],o[0],bl);
  
end;

procedure BlockFileOf<T>.Read(o: array of T; start_elm, arr_offset, count: integer);
begin
  var bl := sz*count;
  
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < bl then raise new CannotReadAfterEOF;
  if arr_offset+count > o.Length then raise new System.IndexOutOfRangeException;
  
  Pos := start_elm;
  var a := new byte[bl];
  str.Read(a,0,bl);
  CopyMem(a[0],o[start_elm],bl);
  
end;

{$endregion Read}

{$region Utils}

function BlockFileOf<T>.InternalReadLazy(c: integer; start_pos: int64): sequence of T;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - start_pos < sz*c then raise new CannotReadAfterEOF;
  
  loop c do
  begin
    PosByte := start_pos;
    yield Read;
    start_pos += sz;
  end;
  
end;

function BlockFileOf<T>.ToSeqBlocks(blocks_size: integer): sequence of array of T;
begin
  var c := blocks_size div sz;
  var i := 0;
  
  while true do
  begin
    var left := Size - i;
    
    if left < c then
    begin
      if left > 0 then
      begin
        Pos := i;
        yield Read(left);
      end;
      exit;
    end else
    begin
      Pos := i;
      yield Read(c);
      i += c;
    end;
    
  end;
  
end;

function BlockFileOf<T>.ToSeq: sequence of T;
begin
  var i := 0;
  
  while not EOF do
  begin
    Pos := i;
    yield Read;
    i += 1;
  end;
  
end;

{$endregion Utils}

end.