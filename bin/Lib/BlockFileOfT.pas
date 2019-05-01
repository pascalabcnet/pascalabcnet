(************************************************************************************)
// Copyright (©) Cergey Latchenko ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// This code is distributed under the Unlicense
// For details please see LICENSE.md or here:
// https://github.com/SunSerega/PascalABC.Net-BlockFileOfT/blob/master/LICENSE.md
(************************************************************************************)
// Copyright (©) Сергей латченко ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// Этот код распространяется под Unlicense
// Для деталей смотрите в файл LICENSE.md или сюда:
// https://github.com/SunSerega/PascalABC.Net-BlockFileOfT/blob/master/LICENSE.md
(************************************************************************************)

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
    protected bw: BinaryWriter;
    protected br: BinaryReader;
    
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
  /// - Особоопасными в .NET типами. Например, char и System.DateTime
  ///Однако, эти ограничения можно обойти, об этом в справке
  BlockFileOf<T> = class(BlockFileBase)
    
    private static sz: integer;
    
    private static procedure TestForRefT(tt: System.Type);
    begin
      if tt.IsClass then
      begin
        MessageBox(new System.IntPtr(nil),
          $'Тип {tt} ссылочный.{#10}Ссылочные типы нельзя сохранять в типизированный файл.{#10}Нажмите OK для выхода.',
          $'Тип T из BlockFileOf<T> содержет ссылочные типы',
          $10
        );
        Halt(-1);
      end;
      foreach var fi in
      tt.GetFields(
        System.Reflection.BindingFlags.GetField or
        System.Reflection.BindingFlags.Instance or
        System.Reflection.BindingFlags.Public or
        System.Reflection.BindingFlags.NonPublic
      ) do
        if not fi.IsLiteral then
          if fi.FieldType <> tt then//integer имеет поле типа integer, без этой строчки StackOwerflow
            TestForRefT(fi.FieldType);
    end;
    
    private static constructor :=
    try
      TestForRefT(typeof(T));
      
      (*
      try
        var a := new T[0];
        GCHandle.Alloc(a,GCHandleType.Pinned).Free;
      except
        on e: System.ArgumentException do
        begin
          MessageBox(new System.IntPtr(nil),
            $'.Net не принимает какой то из типов полей записи, для которой вы создали BlockFileOf<T>{#10}Говорит - его нельзя превратить в набор байт{#10}Нажмите OK для выхода.',
            $'Тип T содержет ссылочные типы',
            $10
          );
          Halt(-1);
        end;
      end;
      (**)
      
      sz := Marshal.SizeOf(typeof(T));
    except
      on e:Exception do
      begin
        MessageBox(new System.IntPtr(nil),
          e.ToString,
          $'При инициализации BlockFileOf<{typeof(T)}> произошла ошибка',
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
    
    private function InternalReadLazy(c: integer; start_pos: int64):sequence of T;
    
    ///Инициализирует переменную файла, не привязывая её к файлу на диске
    public constructor := exit;
    
    ///Инициализирует переменную файла, привязывая её к файлу с именем fname
    public constructor(fname: string) :=
    Assign(fname);
    
    ///Инициализирует переменную файла, привязывая её к файлу с именем fname
    ///Устанавливает значение смещения от начала в байтах на offset
    public constructor(fname: string; offset: int64) :=
    Assign(fname, offset);
    
    ///- constructor BlockFileOf<>(f:BlockFileOf<>);
    ///Инициализирует новую переменную, создавая связку с заданной переменной
    ///После вызова этого конструктора переданная и созданная переменные будут использовать общий файловый поток, но записывать разные типы данных (у них может быть разный T)
    ///Это значит, что переменная, которую передали в конструктор, уже должна иметь открытый файловый поток
    ///Метод Close разрывает эту связь.
    public constructor(f: BlockFileBase) :=
    Link(f);
    
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
    
    ///Возвращает номер текущего элемета в файле (нумеруя с 0)
    public property Pos: int64 read GetPos write SetPos;
    
    ///Возвращает номер текущего байта от начала файла (нумеруя с 0)
    public property PosByte: int64 read GetPosByte write SetPosByte;
    
    ///Определяет, привязана ли переменная к файлу
    public property Assigned: boolean read fi <> nil;
    
    ///Определяет, открыт ли файл
    public property Opened: boolean read str <> nil;
    
    ///Определяет, достигнут ли конец файла
    public property EOF: boolean read ByteSize-PosByte < sz;
    
    ///Возвращает поток текущего файла (или nil если файл не открыт)
    ///Внимание! Любое действие, связанное с изменением данного потока файла приведёт к неожиданным последствиям, используйте его только если знаете, что вы делаете
    public property BaseStream: FileStream read str;
    
    ///Возвращает BinaryWriter текущего файла (или nil если файл не открыт)
    ///Внимание! Любое действие, связанное с изменением основного потока файла приведёт к неожиданным последствиям, используйте его только если знаете, что вы делаете
    public property BinWriter: BinaryWriter read bw;
    
    ///Возвращает BinaryReader текущего файла (или nil если файл не открыт)
    ///Внимание! Любое действие, связанное с изменением основного потока файла приведёт к неожиданным последствиям, используйте его только если знаете, что вы делаете
    public property BinReader: BinaryReader read br;
    
    ///Возвращает FileInfo текущего файла (или nil, если переменная не привязана к файлу)
    public property FileInfo: System.IO.FileInfo read fi;
    
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
    
    ///Создает файл
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
    
    ///Ставит позицию в файле на элемент pos (нумеруя с 0)
    public procedure Seek(pos: int64) := self.Pos := pos;
    
    ///Ставит файловый курсор на байт pos (нумеруя с 0) от начала файла
    public procedure SeekByte(pos: int64) := self.PosByte := pos;
    
    ///Записывает все изменения в файл и отчищает внутренние буферы
    ///До вызова Flush или Close все изменения и кеш хранятся в оперативной памяти
    ///Поэтому, если вы проводите операции с большими объёмами памяти, лучше вызывать Flush время от времени
    public procedure Flush;
    
    ///Сохраняет и закрывает файл, если он открыт
    public procedure Close;
    
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
    
    ///Читает один элемент из файла одним блоком
    ///Переставляет файловый курсор на 1 элемет вперёд
    public function Read: T;
    
    ///Читает массив из count элементов из файла одним блоком
    ///Переставляет файловый курсор на count элеметов вперёд
    public function Read(count: integer): array of T;
    
    ///Читает массив из count элементов одним блоком, начиная с элемента start_elm
    ///И переставляет файловый курсор на count элеметов вперёд
    public function Read(start_elm, count: integer): array of T;
    
    ///Возвращает ленивую последовательность из count элементов
    ///После завершения чтения курсор окажется на последнем элементе возвращаемой последовательности
    ///Каждый раз чтение будет происходить, начиная с элемента, на котором в данный момент стоит файловый курсор
    public function ReadLazy(count: integer): sequence of T := InternalReadLazy(count, PosByte);
    
    ///Возвращает ленивую последовательность из count элементов, начиная с элемента start_elm
    ///После завершения чтения курсор окажется на последнем элементе возвращаемой последовательности
    ///Каждый раз чтение будет происходить, начиная с элемента start_elm
    public function ReadLazy(start_elm, count: integer): sequence of T := InternalReadLazy(count, _offset + start_elm*sz);
    
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
    inherited Create($'Файл {fname} открыт, закройте его методом Close перед тем как продолжать');
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
  self.br := f.br;
  self.bw := f.bw;
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
  //fi.Refresh;//А тут не надо
  Result := fi.FullName;
end;



function BlockFileOf<T>.GetByteFileSize: int64;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str <> nil then
  begin
    Result := str.Length;
    exit;
  end;
  fi.Refresh;
  Result := fi.Length;
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
  bw := new BinaryWriter(str);
  br := new BinaryReader(str);
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
    br := nil;
    bw := nil;
  end;
end;

{$endregion Closing}

{$endregion Setup IO}

{$region Write}

procedure BlockFileOf<T>.Write(o: T);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  var a := new byte[sz];
  var gc_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
  try
    System.Buffer.MemoryCopy(
      @o,
      gc_hnd.AddrOfPinnedObject.ToPointer,
      sz,sz
    );
  finally
    gc_hnd.Free;
  end;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(params o: array of T);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  
  var bl := sz*o.Length;
  var a := new byte[bl];
  var gc_hnd1 := GCHandle.Alloc(o, GCHandleType.Pinned);
  try
    var gc_hnd2 := GCHandle.Alloc(a, GCHandleType.Pinned);
    try
      System.Buffer.MemoryCopy(
        gc_hnd1.AddrOfPinnedObject.ToPointer,
        gc_hnd2.AddrOfPinnedObject.ToPointer,
        bl,bl
      );
    finally
      gc_hnd2.Free;
    end;
  finally
    gc_hnd1.Free;
  end;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o: ICollection<T>);
type TArr = array of T;
begin
  if o is TArr(var a) then
  begin
    Write(a);
    exit;
  end;
  
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  
  var a := new byte[sz*o.Count];
  var gc_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
  try
    var hnd := gc_hnd.AddrOfPinnedObject;
    foreach var el in o do
    begin
      System.Buffer.MemoryCopy(
        @el,
        hnd.ToPointer,
        sz,sz
      );
      System.IntPtr.Add(hnd, sz);
    end;
  finally
    gc_hnd.Free;
  end;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o: sequence of T) :=
if o is ICollection<T>(var c) then
  Write(c) else
foreach var el in o do
  Write(el);

procedure BlockFileOf<T>.Write(o: array of T; from, count: integer);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  
  var bl := sz*count;
  var a := new byte[bl];
  var gc_hnd1 := GCHandle.Alloc(o, GCHandleType.Pinned);
  try
    var gc_hnd2 := GCHandle.Alloc(a, GCHandleType.Pinned);
    try
      System.Buffer.MemoryCopy(
        System.IntPtr.Add(gc_hnd1.AddrOfPinnedObject, from * sz).ToPointer,
        gc_hnd2.AddrOfPinnedObject.ToPointer,
        bl,bl
      );
    finally
      gc_hnd2.Free;
    end;
  finally
    gc_hnd1.Free;
  end;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o: ICollection<T>; from, count: integer);
type TArr = array of T;
begin
  if o is TArr(var a) then
  begin
    Write(a, from, count);
    exit;
  end;
  
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  
  var a := new byte[sz*o.Count];
  var gc_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
  try
    var hnd := gc_hnd.AddrOfPinnedObject;
    foreach var el in o do
      if from > 0 then from -= 1 else
      if count > 0 then
      begin
        System.Buffer.MemoryCopy(
          @el,
          hnd.ToPointer,
          sz,sz
        );
        hnd := System.IntPtr.Add(hnd, sz);
        count -= 1;
      end;
  finally
    gc_hnd.Free;
  end;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o: sequence of T; from, count: integer) :=
if o is ICollection<T>(var c) then
  Write(c,from,count) else
  Write(o.Skip(from).Take(count));

{$endregion Write}

{$region Read}

function BlockFileOf<T>.Read: T;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < sz then raise new CannotReadAfterEOF;
  
  var a := br.ReadBytes(sz);
  var gc_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
  try
    System.Buffer.MemoryCopy(
      gc_hnd.AddrOfPinnedObject.ToPointer,
      @Result,
      sz,sz
    );
  finally
    gc_hnd.Free;
  end;
end;

function BlockFileOf<T>.Read(count: integer): array of T;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < sz*count then raise new CannotReadAfterEOF;
  
  var bl := sz*count;
  var a := br.ReadBytes(bl);
  Result := new T[count];
  var gc_hnd1 := GCHandle.Alloc(a, GCHandleType.Pinned);
  try
    var gc_hnd2 := GCHandle.Alloc(Result, GCHandleType.Pinned);
    try
      System.Buffer.MemoryCopy(
        gc_hnd1.AddrOfPinnedObject.ToPointer,
        gc_hnd2.AddrOfPinnedObject.ToPointer,
        bl,bl
      );
    finally
      gc_hnd2.Free;
    end;
  finally
    gc_hnd1.Free;
  end;
end;

function BlockFileOf<T>.Read(start_elm, count:integer): array of T;
begin
  Pos := start_elm;
  Result := Read(count);
end;

function BlockFileOf<T>.InternalReadLazy(c: integer; start_pos: int64): sequence of T;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - start_pos < sz*c then raise new CannotReadAfterEOF;
  
  for var i := 0 to c-1 do
  begin
    PosByte := start_pos + i*sz;
    yield Read;
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

{$endregion Read}

end.