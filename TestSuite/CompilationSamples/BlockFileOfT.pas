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

///Модуль содержащий тип BlockFileOf<T>
///Этот тип - альтернатива стандартному file of T
///Основное преимущество - скорость работы
unit BlockFileOfT;

interface

uses System.Runtime.InteropServices;
uses System.IO;

type
  ///--
  BlockFileBase = abstract class
    
    protected fi:FileInfo;
    protected _offset:int64;
    protected str:FileStream;
    protected bw:BinaryWriter;
    protected br:BinaryReader;
    
    protected linked := new List<BlockFileBase>;
    
    protected procedure Link(f:BlockFileBase);
    
    protected procedure UnLink;
    
    private class function MessageBox(wnd: System.IntPtr; message, caption: string; flags: cardinal):integer; 
    external 'User32.dll';
    
  end;
  
  ///Тип, записывающий данные в файл по принципу схожему с "file of <T>"
  ///Но в отличии от "file of <T>" - данный тип сохраняет всю запись одним блоком,
  ///так, как она записа в памяти.
  ///Это даёт значительное преимущество по скорости, но ограничевает,
  ///типы, которые могут быть использованы в виде полей <T>
  ///
  ///Ожидается, что в видет шаблонного параметра <T> будет передана запись,
  ///не содержащая динамичных полей.
  ///Иначе целостность данных будет терятся
  ///Это значит, что поля записи T и всех вложенных записей - НЕ могут быть:
  ///  -Указатели
  ///  -Ссылочных типов (то есть классами)
  ///  -Особых типов, которые .Net считает "опасными". Как char или System.DateTime
  ///Но эти ограничения можно обойти, про это в справке
  BlockFileOf<T>=class(BlockFileBase)
    
    private class sz: integer;
    
    private class procedure TestForRefT(tt: System.Type);
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
    
    private class constructor :=
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
    
    private function GetName:string;
    private function GetFullName:string;
    
    private function GetExists:boolean;
    
    private function GetFileSize:int64 := (GetByteFileSize - _offset) div sz;
    private procedure SetFileSize(size:int64) := SetByteFileSize(_offset + size*sz);
    
    private function GetByteFileSize:int64;
    private procedure SetByteFileSize(size:int64);
    
    private function GetPos:int64 := (GetPosByte-_offset) div sz;
    private procedure SetPos(pos:int64) := SetPosByte(_offset + pos*sz);
    private function GetPosByte:int64;
    private procedure SetPosByte(pos:int64);
    
    private function InternalReadLazy(c:integer; start_pos:int64):sequence of T;
    
    ///Инициализирует переменную файла, не привязывая её к файлу на диске
    public constructor := exit;
    ///Инициализирует переменную файла, привязывая её к файлу fname
    public constructor(fname:string) :=
    Assign(fname);
    ///Инициализирует переменную файла, привязывая её к файлу fname
    ///А так же устанавливая значение смещени от начала в байтах на offset
    public constructor(fname:string; offset:int64) :=
    Assign(fname, offset);
    ///- constructor BlockFileOf<>(f:BlockFileOf<>);
    ///Инициализирует новую переменную, создавая связку данной переменной
    ///После вызова этого конструктора - переданная и созданная переменная будут
    ///использовать общий файловый поток, но записывать разные типы данных (у них может быть разный T)
    ///Это значит, что переменная которую передали в конструктор должно уже иметь открытый файловый поток
    ///Метод Close разрывает эту связь.
    public constructor(f: BlockFileBase) :=
    Link(f);
    
    
    ///Размер блока из одного элемента типа T, в байтах
    ///Хоть это свойство и можно перезаписывать, но это не рекомендуется
    ///Если перезаписать на число, большее чем было - в файл будет сохранять лишние нули
    ///А если на меньшее - получите неопределённое поведение (скорее всего вылет, но далеко не сразу)
    public property TSize:integer read integer(sz) write sz := value;
    
    ///Смещение от начала файла до начала элементов (в байтах)
    public property Offset:int64 read _offset write _offset;
    ///Количество сохранённых в файл элементов типа T
    ///Чтоб установить длину файла - надо открыть файл. Но прочитать длину можно не открывая
    public property Size:int64 read GetFileSize write SetFileSize;
    ///Полный размер файла, в байтах
    ///Чтоб установить длину файла - надо открыть файл. Но прочитать длину можно не открывая
    public property ByteSize:int64 read GetByteFileSize write SetByteFileSize;
    ///Имя файла (только имя самого файла, без имени папки)
    public property Name:string read GetName;
    ///Полное имя файла (вместе с именами всех под-папок, вплодь до корня диска)
    public property FullName:string read GetFullName;
    ///Существует ли файл
    public property Exists:boolean read GetExists;
    
    ///Номер текущего элемета типа T в файле (нумеруя с 0)
    public property Pos:int64 read GetPos write SetPos;
    ///Номер текущего байта от начала файла (нумеруя с 0)
    public property PosByte:int64 read GetPosByte write SetPosByte;
    ///Привязана ли переменная к файлу
    public property Assigned:boolean read fi <> nil;
    ///Открыт ли файл
    public property Opened:boolean read str <> nil;
    ///Достигнут ли конец файла
    public property EOF:boolean read ByteSize-PosByte < sz;
    ///Основной поток открытого файла (или nil если файл не открыт)
    ///Внимание! Любое действие которое изменит этот поток - приведёт к неожиданным последствиям, используйте его только если знаете что делаете
    public property BaseStream:FileStream read str;
    ///Переменная, которая записывает данные в основной поток (или nil если файл не открыт)
    ///Внимание! Любое действие которое изменит основной поток файла - приведёт к неожиданным последствиям, используйте его только если знаете что делаете
    public property BinWriter:BinaryWriter read bw;
    ///Переменная, которая читает данные из основного потока (или nil если файл не открыт)
    ///Внимание! Любое действие которое изменит основной поток файла - приведёт к неожиданным последствиям, используйте его только если знаете что делаете
    public property BinReader:BinaryReader read br;
    ///Переменная, показывающая данные о файле (или nil, если переменная не привязана к файлу)
    public property FileInfo:System.IO.FileInfo read fi;
    
    
    
    ///Привязывает данную переменную к файлу {fname}
    ///Привязывать можно и к не существующим файлам, при откритии определёнными способами (как Rewrite) новый файл будет создан
    public procedure Assign(fname:string);
    ///Привязывает данную переменную к файлу {fname}
    ///А так же устанавливая значение смещени от начала в байтах на offset
    ///Привязывать можно и к не существующим файлам, при откритии определёнными способами (как Rewrite) новый файл будет создан
    public procedure Assign(fname:string; offset:int64);
    ///Убирает свять переменной и файла, если связь есть
    public procedure UnAssign;
    ///Открывает файл, способом описанным в переменной mode
    ///Чтоб получить переменную этого типа - пишите System.IO.FileMode.<способ_открытия_файла>
    public procedure Open(mode:FileMode);
    ///Удаляет связаный файл, если он существует
    public procedure Delete;
    ///Переименовывает файл
    ///Если указать другое расположение - файл будет перемещён
    public procedure Rename(NewName:string);
    
    ///Создает (или обнуляет) привязаный файл
    public procedure Rewrite;
    ///Привязывает данную переменную к файлу {fname} и создает (или обнуляет) этот файл
    public procedure Rewrite(fname:string);
    ///Привязывает данную переменную к файлу {fname} и создает (или обнуляет) этот файл
    ///А так же устанавливая значение смещени от начала в байтах на offset
    public procedure Rewrite(fname:string; offset:int64);
    
    ///Открывает файл (ожидается, что он уже существует) и устанавливает позицию на начало файла
    public procedure Reset;
    ///Привязывает данную переменную к файлу {fname}, открывает этот файл (ожидается, что файл уже существует) и устанавливает позицию на начало файла
    public procedure Reset(fname:string);
    ///Привязывает данную переменную к файлу {fname}, открывает этот файл (ожидается, что файл уже существует) и устанавливает позицию на начало файла
    ///А так же устанавливая значение смещени от начала в байтах на offset
    public procedure Reset(fname:string; offset:int64);
    
    ///Открывает файл (ожидается, что он уже существует) и устанавливает позицию в конце файла
    public procedure Append;
    ///Привязывает данную переменную к файлу {fname}, открывает этот файл (ожидается, что файл уже существует) и устанавливает позицию в конце файла
    public procedure Append(fname:string);
    ///Привязывает данную переменную к файлу {fname}, открывает этот файл (ожидается, что файл уже существует) и устанавливает позицию в конце файла
    ///А так же устанавливая значение смещени от начала в байтах на offset
    public procedure Append(fname:string; offset:int64);
    
    ///Переставляет позицию в файле на элемент #pos (нумеруя с 0)
    public procedure Seek(pos:int64) := self.Pos := pos;
    ///Переставляет файловый курсор на байт #pos (нумеруя с 0) от начала файла 
    public procedure SeekByte(pos:int64) := self.PosByte := pos;
    
    ///Записывает все изменения в файл и отчищает внутренние буферы
    ///До вызова Flush или Close - все изменения и кеш хранятся в оперативной памяти
    ///Поэтому, если вы записываете/читаете много (сотни мегабайт) - лучше вызывать Flush время от времени
    public procedure Flush;
    ///Сохраняет и закрывает файл, если он открыт
    public procedure Close;
    
    
    ///Записывает один элемент одним блоком в файл
    ///И переставляет файловый курсор на 1 элемет вперёд
    public procedure Write(o: T);
    ///Записывает массив элементов одним блоком в файл
    ///И переставляет файловый курсор на o.Length элеметов вперёд
    public procedure Write(params o:array of T);
    ///Записывает последовательность элементов, у которой можно узнать длину, одним блоком в файл
    ///И переставляет файловый курсор на o.Count элеметов вперёд
    public procedure Write(o:ICollection<T>);
    ///Записывает последовательность элементов, у которой нельзя узнать длину, по 1 элементу типа T в файл
    ///И переставляет файловый курсор на o.Count элеметов вперёд
    public procedure Write(o:sequence of T);
    ///Записывает count элементов массива, начиная с элемента #from, одним блоком в файл
    ///И переставляет файловый курсор на сount элеметов вперёд
    public procedure Write(o:array of T; from,count:integer);
    ///Записывает count элементов последовательности, у которой можно узнать длину, начиная с элемента #from, одним блоком в файл
    ///И переставляет файловый курсор на сount элеметов вперёд
    public procedure Write(o:ICollection<T>; from,count:integer);
    ///Записывает count элементов последовательности, у которой нельзя узнать длину, начиная с элемента #from, одним блоком в файл
    ///И после каждого элемента переставляет файловый курсор на 1 элемет вперёд
    public procedure Write(o:sequence of T; from,count:integer);
    
    ///Читает один элемент из файла одним блоком
    ///И переставляет файловый курсор на 1 элемет вперёд
    public function Read:T;
    ///Читает массив из count одним блоком
    ///И переставляет файловый курсор на count элеметов вперёд
    public function Read(count:integer):array of T;
    ///Читает массив из count элементов одним блоком, начиная с элемента #start_elm
    ///И переставляет файловый курсор на count элеметов вперёд
    public function Read(start_elm, count:integer):array of T;
    ///Возвращает ленивую последовательность из count элементов
    ///После завершения чтения - курсор окажется на эдлементе #(start_elm+count)
    ///Каждый раз читать будет начиная с элемента, на котором сейчас стоит файловый курсор
    public function ReadLazy(count:integer):sequence of T := InternalReadLazy(count, PosByte);
    ///Возвращает ленивую последовательность из count элементов, начиная с элемента #start_elm
    ///После прочтения i элементов последовательности - позиция в файле будет передвигаться на элемент #(start_elm+i)
    ///Каждый раз читать будет начиная с элемента #start_elm
    public function ReadLazy(start_elm, count:integer):sequence of T := InternalReadLazy(count, _offset + start_elm*sz);
    ///Возвращает ленивую последовательность из блоков-массивов с элементами типа T
    ///Каждый блок хранит столько элементов - чтоб не превышать объём в 4096 байт (4КБ)
    ///После прочтения каждого блока - файловый корсор будет переставлен на его конец
    public function ToSeqBlocks:sequence of array of T := ToSeqBlocks(4096);
    ///Возвращает ленивую последовательность из блоков-массивов с элементами типа T
    ///Каждый блок хранит столько элементов - чтоб не превышать объём в последовательность байт
    ///После прочтения каждого блока - файловый корсор будет переставлен на его конец
    public function ToSeqBlocks(blocks_size:integer):sequence of array of T;
    ///Возвращает ленивую последовательность из всех элементов хранящихся в файле
    ///После прочтения i элементов последовательности - позиция в файле будет передвигаться на элемент #i
    public function ToSeq:sequence of T;
    
    protected procedure Finalize; override;
    begin
      Close;
    end;
    
  end;

{$region Exception's}

type
  FileNotAssignedException = class(Exception)
    constructor :=
    inherited Create($'Данная переменная не была привязана к файлу{10}Используйте метод Assign');
  end;
  FileNotOpenedException = class(Exception)
    constructor(fname:string) :=
    inherited Create($'Файл {fname} ещё не открыт, откройте его с помощью Open, Reset, Append или Rewrite');
  end;
  FileNotClosedException = class(Exception)
    constructor(fname:string) :=
    inherited Create($'Файл {fname} ещё открыт, закройте его методом Close перед тем как продолжать');
  end;
  CannotReadAfterEOF = class(Exception)
    constructor :=
    inherited Create($'Нельзя читать за пределами файла. Можно только записывать');
  end;

{$endregion Exception's}

implementation

{$region Linking}

procedure BlockFileBase.Link(f:BlockFileBase);
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

function BlockFileOf<T>.GetName:string;
begin
  if fi = nil then raise new FileNotAssignedException;
  fi.Refresh;
  Result := fi.Name;
end;

function BlockFileOf<T>.GetFullName:string;
begin
  if fi = nil then raise new FileNotAssignedException;
  //fi.Refresh;//А тут не надо
  Result := fi.FullName;
end;



function BlockFileOf<T>.GetByteFileSize:int64;
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

procedure BlockFileOf<T>.SetByteFileSize(size:int64);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  str.SetLength(size);
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

procedure BlockFileOf<T>.Assign(fname:string; offset:int64);
begin
  Assign(fname);
  _offset := offset;
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

procedure BlockFileOf<T>.Delete;
begin
  if fi = nil then raise new FileNotAssignedException;
  fi.Delete;
end;

function BlockFileOf<T>.GetExists:boolean;
begin
  if fi = nil then raise new FileNotAssignedException;
  fi.Refresh;
  Result := fi.Exists;
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

procedure BlockFileOf<T>.Rewrite(fname:string; offset:int64);
begin
  Assign(fname, offset);
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

procedure BlockFileOf<T>.Reset(fname:string; offset:int64);
begin
  Assign(fname, offset);
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

procedure BlockFileOf<T>.Append(fname:string; offset:int64);
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
  System.Buffer.MemoryCopy(
    @o,
    gc_hnd.AddrOfPinnedObject.ToPointer,
    sz,sz
  );
  gc_hnd.Free;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(params o:array of T);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  
  var bl := sz*o.Length;
  var a := new byte[bl];
  var gc_hnd1 := GCHandle.Alloc(o, GCHandleType.Pinned);
  var gc_hnd2 := GCHandle.Alloc(a, GCHandleType.Pinned);
  System.Buffer.MemoryCopy(
    gc_hnd1.AddrOfPinnedObject.ToPointer,
    gc_hnd2.AddrOfPinnedObject.ToPointer,
    bl,bl
  );
  gc_hnd1.Free;
  gc_hnd2.Free;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o:ICollection<T>);
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
  gc_hnd.Free;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o:sequence of T) :=
if o is ICollection<T>(var c) then
  Write(c) else
foreach var el in o do
  Write(el);

procedure BlockFileOf<T>.Write(o:array of T; from,count:integer);
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  
  var bl := sz*count;
  var a := new byte[bl];
  var gc_hnd1 := GCHandle.Alloc(o, GCHandleType.Pinned);
  var gc_hnd2 := GCHandle.Alloc(a, GCHandleType.Pinned);
  System.Buffer.MemoryCopy(
    System.IntPtr.Add(gc_hnd1.AddrOfPinnedObject, from * sz).ToPointer,
    gc_hnd2.AddrOfPinnedObject.ToPointer,
    bl,bl
  );
  gc_hnd1.Free;
  gc_hnd2.Free;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o:ICollection<T>; from,count:integer);
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
  gc_hnd.Free;
  bw.Write(a);
end;

procedure BlockFileOf<T>.Write(o:sequence of T; from,count:integer) :=
if o is ICollection<T>(var c) then
  Write(c,from,count) else
  Write(o.Skip(from).Take(count));

{$endregion Write}

{$region Read}

function BlockFileOf<T>.Read:T;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < sz then raise new CannotReadAfterEOF;
  
  var a := br.ReadBytes(sz);
  var gc_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
  System.Buffer.MemoryCopy(
    gc_hnd.AddrOfPinnedObject.ToPointer,
    @Result,
    sz,sz
  );
  gc_hnd.Free;
end;

function BlockFileOf<T>.Read(count:integer):array of T;
begin
  if fi = nil then raise new FileNotAssignedException;
  if str = nil then raise new FileNotOpenedException(fi.FullName);
  if str.Length - str.Position < sz*count then raise new CannotReadAfterEOF;
  
  var bl := sz*count;
  var a := br.ReadBytes(bl);
  Result := new T[count];
  var gc_hnd1 := GCHandle.Alloc(a, GCHandleType.Pinned);
  var gc_hnd2 := GCHandle.Alloc(Result, GCHandleType.Pinned);
  System.Buffer.MemoryCopy(
    gc_hnd1.AddrOfPinnedObject.ToPointer,
    gc_hnd2.AddrOfPinnedObject.ToPointer,
    bl,bl
  );
  gc_hnd1.Free;
  gc_hnd2.Free;
end;

function BlockFileOf<T>.Read(start_elm, count:integer):array of T;
begin
  Pos := start_elm;
  Result := Read(count);
end;

function BlockFileOf<T>.InternalReadLazy(c:integer; start_pos:int64):sequence of T;
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

function BlockFileOf<T>.ToSeqBlocks(blocks_size:integer):sequence of array of T;
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

function BlockFileOf<T>.ToSeq:sequence of T;
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