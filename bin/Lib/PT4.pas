/// Модуль электронного задачника Programming Taskbook 4
unit PT4;  

//------------------------------------------------------------------------------
// Модуль для подключения задачника Programming Taskbook
// Версия 4.15
// Copyright (c) 2006-2008 DarkStar, SSM
// Copyright (c) 2010 М.Э.Абрамян, дополнения к версии 1.3
// Copyright (c) 2014-2015 М.Э.Абрамян, дополнения к версии 4.13
// Copyright (c) 2015 М.Э.Абрамян, дополнения к версии 4.14
// Copyright (c) 2016 М.Э.Абрамян, дополнения к версии 4.15
// Электронный задачник Programming Taskbook Copyright (c)М.Э.Абрамян, 1998-2017
//------------------------------------------------------------------------------

{$apptype windows}
{$platformtarget x86}

interface

uses System,
     System.Collections,
     System.Runtime.InteropServices;
type
  /// Тип указателя на узел списка
  PNode = ^TNode;
  /// Тип узла списка
  TNode = record
    Data: integer;
    Next,Prev,Left,Right,Parent: PNode;
  end;

  InternalNode = record
    Data: integer;
    Next,Prev,Left,Right,Parent: IntPtr;
  end;
  
  IOPT4System = class(IOStandardSystem)
  public
    procedure write(obj: object); override;
    procedure writeln;            override;
  end;

  PT4Exception = Exception;
  
  Node = class(IDisposable)
  private
    isAllocMem: boolean;
    isDisposed: boolean;
    x: InternalNode;
    addr: IntPtr;
    constructor Create(x: InternalNode; a: IntPtr);
    procedure Init(Data: integer; Next,Prev,Left,Right,Parent: Node);
    procedure internalDispose(disposing: boolean);
    procedure setNext(value: Node);
    function getNext: Node;
    procedure setPrev(value: Node);
    function getPrev: Node;
    procedure setLeft(value: Node);
    function getLeft: Node;
    procedure setRight(value: Node);
    function getRight: Node;
    procedure setParent(value: Node);
    function getParent: Node;
    procedure setData(value: integer);
    function getData: integer;
  public
    constructor Create;
    constructor Create(aData: integer);
    constructor Create(aData: integer; aNext: Node);
    constructor Create(aData: integer; aNext,aPrev: Node);
    constructor Create(left,right: Node; data: integer);
    constructor Create(left,right: Node; data: integer; parent: Node);
    procedure Dispose;
    property Next: Node read getNext write SetNext;
    property Prev: Node read getPrev write SetPrev;
    property Left: Node read getLeft write SetLeft;
    property Right: Node read getRight write SetRight;
    property Parent: Node read getParent write SetParent;
    property Data: integer read getData write SetData;
  end;

/// Вывести формулировку задания
procedure Task(name: string);

/// Ввести и вернуть значение целого типа
function GetInt: integer;
/// Ввести и вернуть значение целого типа
function GetInteger: integer;
/// Ввести и вернуть значение вещественного типа
function GetReal: real;
/// Ввести и вернуть значение вещественного типа
function GetDouble: real;
/// Ввести и вернуть значение символьного типа
function GetChar: char;
/// Ввести и вернуть значение строкового типа
function GetString: string;
/// Ввести и вернуть значение логического типа
function GetBool: boolean;
/// Ввести и вернуть значение логического типа
function GetBoolean: boolean;
/// Ввести и вернуть значение типа Node 
function GetNode: Node;
/// Ввести и вернуть значение типа PNode 
function GetPNode: PNode;

/// Возвращает введенное значение типа integer
function ReadInteger: integer;
/// Возвращает введенное значение типа real
function ReadReal: real;
/// Возвращает введенное значение типа char
function ReadChar: char;
/// Возвращает введенное значение типа string
function ReadString: string;
/// Возвращает введенное значение типа boolean
function ReadBoolean: boolean;
/// Возвращает введенное значение типа PNode
function ReadPNode: PNode;
/// Возвращает введенное значение типа Node
function ReadNode: Node;

/// Возвращает введенное значение типа integer
function ReadlnInteger: integer;
/// Возвращает введенное значение типа real
function ReadlnReal: real;
/// Возвращает введенное значение типа char
function ReadlnChar: char;
/// Возвращает введенное значение типа string
function ReadlnString: string;
/// Возвращает введенное значение типа boolean
function ReadlnBoolean: boolean;
/// Возвращает введенное значение типа PNode
function ReadlnPNode: PNode;
/// Возвращает введенное значение типа Node
function ReadlnNode: Node;

// == Версия 4.15. Дополнения ==

/// Возвращает введенное значение типа integer.
/// Строковое приглашение prompt игнорируется
function ReadInteger(prompt: string): integer;
/// Возвращает введенное значение типа real.
/// Строковое приглашение prompt игнорируется
function ReadReal(prompt: string): real;
/// Возвращает введенное значение типа char.
/// Строковое приглашение prompt игнорируется
function ReadChar(prompt: string): char;
/// Возвращает введенное значение типа string.
/// Строковое приглашение prompt игнорируется
function ReadString(prompt: string): string;
/// Возвращает введенное значение типа boolean.
/// Строковое приглашение prompt игнорируется
function ReadBoolean(prompt: string): boolean;
/// Возвращает введенное значение типа PNode.
/// Строковое приглашение prompt игнорируется
function ReadPNode(prompt: string): PNode;
/// Возвращает введенное значение типа Node.
/// Строковое приглашение prompt игнорируется
function ReadNode(prompt: string): Node;

/// Возвращает введенное значение типа integer.
/// Строковое приглашение prompt игнорируется
function ReadlnInteger(prompt: string): integer;
/// Возвращает введенное значение типа real.
/// Строковое приглашение prompt игнорируется
function ReadlnReal(prompt: string): real;
/// Возвращает введенное значение типа char.
/// Строковое приглашение prompt игнорируется
function ReadlnChar(prompt: string): char;
/// Возвращает введенное значение типа string.
/// Строковое приглашение prompt игнорируется
function ReadlnString(prompt: string): string;
/// Возвращает введенное значение типа boolean.
/// Строковое приглашение prompt игнорируется
function ReadlnBoolean(prompt: string): boolean;
/// Возвращает введенное значение типа PNode.
/// Строковое приглашение prompt игнорируется
function ReadlnPNode(prompt: string): PNode;
/// Возвращает введенное значение типа Node.
/// Строковое приглашение prompt игнорируется
function ReadlnNode(prompt: string): Node;

// == Версия 4.15. Конец дополнений ==

procedure GetR(var param: real);
procedure GetN(var param: integer);
procedure GetC(var param: char);
procedure GetS(var param: string);
procedure GetB(var param: boolean);
procedure GetP(var param: PNode);
procedure GetP(var param: Node);

procedure PutR(param: real);
procedure PutN(param: integer);
procedure PutC(param: char);
procedure PutS(param: string);
procedure PutB(param: boolean);
procedure PutP(param: PNode);
procedure PutP(param: Node);

procedure Put(params args: array of Object);
procedure Put(param: real);
procedure Put(param: integer);
procedure Put(param: char);
procedure Put(param: string);
procedure Put(param: boolean);
procedure Put(param: PNode);
procedure Put(param: Node);

//Ввод этих данных не поддерживается
///- read(a,b,...)
/// Вводит значения a,b,... из окна электронного задачника
procedure read(var x: byte); 
///--
procedure read(var x: shortint); 
///--
procedure read(var x: smallint); 
///--
procedure read(var x: word); 
///--
procedure read(var x: longword); 
///--
procedure read(var x: int64); 
///--
procedure read(var x: uint64); 
///--
procedure read(var x: single); 
//---------------------------------

///--
procedure Read(var val: integer);
///--
procedure Read(var val: real);  
///--
procedure Read(var val: char);  
///--
procedure Read(var val: string);        
///--
procedure Read(var val: boolean);
///--
procedure Read(var val: Node);
///--
procedure Read(var val: PNode);
///--
procedure Readln;

procedure Print(params args: array of object);
procedure Println(params args: array of object);

// == Версия 4.15. Дополнения ==

procedure Print(s: string);
procedure Println(s: string);

// == Версия 4.15. Конец дополнений ==

/// Освобождает память, выделенную динамически, на которую указывает p
procedure Dispose(p: pointer);

// Фиктивная пустая процедура - для Intellisense
// Нет, нельзя пользоваться - закрываются процедуры write системного модуля
///- write(a,b,...)
/// Выводит значения a,b,... в окно электронного задачника
//procedure write;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

// == Версия 1.3. Дополнения ==

/// Выводит строку S в разделе отладки окна задачника
procedure Show(S: string);

/// Выводит число A с комментарием S в разделе отладки
/// окна задачника; для вывода числа отводится W экранных позиций
procedure Show(S: string; A: integer; W: integer);

/// Выводит число A с комментарием S в разделе отладки
/// окна задачника; для вывода числа отводится W экранных позиций
procedure Show(S: string; A: real; W: integer);

/// Выводит число A с комментарием S в разделе отладки окна задачника
procedure Show(S: string; A: integer);

/// Выводит число A с комментарием S в разделе отладки окна задачника
procedure Show(S: string; A: real);

/// Выводит число A в разделе отладки окна задачника;
/// для вывода отводится W экранных позиций
procedure Show(A: integer; W: integer);

/// Выводит число A в разделе отладки окна задачника;
/// для вывода отводится W экранных позиций
procedure Show(A: real; W: integer);

/// Выводит число A в разделе отладки окна задачника
procedure Show(A: integer);

/// Выводит число A в разделе отладки окна задачника
procedure Show(A: real);

/// Выполняет переход на новую экранную строку
/// в разделе отладки окна задачника
procedure ShowLine;

/// Выводит строку S в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку
procedure ShowLine(S: string);

/// Выводит число A с комментарием S в разделе отладки
/// окна задачника; для вывода числа отводится W экранных позиций.
/// После вывода данных выполняет переход на новую экранную строку
procedure ShowLine(S: string; A: integer; W: integer);

/// Выводит число A с комментарием S в разделе отладки
/// окна задачника; для вывода числа отводится W экранных позиций.
/// После вывода данных выполняет переход на новую экранную строку
procedure ShowLine(S: string; A: real; W: integer);

/// Выводит число A с комментарием S в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку
procedure ShowLine(S: string; A: integer);

/// Выводит число A с комментарием S в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку
procedure ShowLine(S: string; A: real);

/// Выводит число A в разделе отладки окна задачника;
/// для вывода отводится W экранных позиций.
/// После вывода числа выполняет переход на новую экранную строку
procedure ShowLine(A: integer; W: integer);

/// Выводит число A в разделе отладки окна задачника;
/// для вывода отводится W экранных позиций.
/// После вывода числа выполняет переход на новую экранную строку
procedure ShowLine(A: real; W: integer);

/// Выводит число A в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку
procedure ShowLine(A: integer);

/// Выводит число A в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку
procedure ShowLine(A: real);

/// Настраивает формат вывода вещественных чисел в разделе отладки
/// окна задачника. Если N > 0, то число выводится в формате
/// с фиксированной точкой и N дробными знаками. Если N = 0,
/// то число выводится в экспоненциальном формате, число дробных
/// знаков определяется шириной поля вывода
procedure SetPrecision(N: integer);

/// Обеспечивает автоматическое скрытие всех разделов
/// окна задачника, кроме раздела отладки
procedure HideTask;

// == Конец дополнений к версии 1.3 ==

// == Версия 4.14. Дополнения ==

/// Вводит n целых чисел
/// и возвращает введенные числа в виде массива
function ReadArrInteger(n: integer): array of integer;

/// Вводит n вещественных чисел
/// и возвращает введенные числа в виде массива
function ReadArrReal(n: integer): array of real;

/// Вводит n строк 
/// и возвращает введенные строки в виде массива
function ReadArrString(n: integer): array of string;

/// Вводит n целых чисел
/// и возвращает введенные числа в виде последовательности
function ReadSeqInteger(n: integer): sequence of integer;

/// Вводит n вещественных чисел
/// и возвращает введенные числа в виде последовательности
function ReadSeqReal(n: integer): sequence of real;

/// Вводит n строк 
/// и возвращает введенные строки в виде последовательности
function ReadSeqString(n: integer): sequence of string;

/// Вводит размер набора целых чисел и его элементы
/// и возвращает введенный набор в виде последовательности
function ReadSeqInteger(): sequence of integer;

/// Вводит размер набора вещественных чисел и его элементы
/// и возвращает введенный набор в виде последовательности
function ReadSeqReal(): sequence of real;

/// Вводит размер набора строк и его элементы
/// и возвращает введенный набор в виде последовательности
function ReadSeqString(): sequence of string;

/// Вводит размер набора целых чисел и его элементы
/// и возвращает введенный набор в виде массива
function ReadArrInteger(): array of integer;

/// Вводит размер набора вещественных чисел и его элементы
/// и возвращает введенный набор в виде массива
function ReadArrReal(): array of real;

/// Вводит размер набора строк и его элементы
/// и возвращает введенный набор в виде массива
function ReadArrString(): array of string;

/// Вводит целую матрицу размера m на n по строкам
function  ReadMatrInteger(m,n: integer): array [,] of integer;

/// Вводит размеры матрицы и затем целую матрицу указанных размеров по строкам
function  ReadMatrInteger(): array [,] of integer;

/// Вводит вещественную матрицу размера m на n по строкам
function  ReadMatrReal(m,n: integer): array [,] of real;

/// Вводит размеры матрицы и затем вещественную матрицу указанных размеров по строкам
function  ReadMatrReal(): array [,] of real;

/// Вводит матрицу из строк размера m на n по строкам
function  ReadMatrString(m,n: integer): array [,] of string;

/// Вводит размеры матрицы и затем строковую матрицу указанных размеров по строкам
function  ReadMatrString(): array [,] of string;

procedure ReadMatr(var m, n: integer; var a: array [,] of integer);

procedure ReadMatr(var m, n: integer; var a: array [,] of real);

procedure ReadMatr(var m, n: integer; var a: array [,] of string);

procedure ReadMatr(var m: integer; var a: array [,] of integer);

procedure ReadMatr(var m: integer; var a: array [,] of real);

procedure ReadMatr(var m: integer; var a: array [,] of string);

procedure ReadMatr(var m, n: integer; var a: array of array of integer);

procedure ReadMatr(var m, n: integer; var a: array of array of real);

procedure ReadMatr(var m, n: integer; var a: array of array of string);

procedure ReadMatr(var m: integer; var a: array of array of integer);

procedure ReadMatr(var m: integer; var a: array of array of real);

procedure ReadMatr(var m: integer; var a: array of array of string);

procedure ReadMatr(var m, n: integer; var a: List<List<integer>>);

procedure ReadMatr(var m, n: integer; var a: List<List<real>>);

procedure ReadMatr(var m, n: integer; var a: List<List<string>>);

procedure ReadMatr(var m: integer; var a: List<List<integer>>);

procedure ReadMatr(var m: integer; var a: List<List<real>>);

procedure ReadMatr(var m: integer; var a: List<List<string>>);

procedure WriteMatr<T>(a: array[,] of T);

procedure WriteMatr<T>(a: array of array of T);

procedure WriteMatr<T>(a: List<List<T>>);

// == Дополнения 2016.07

procedure PrintMatr<T>(a: array[,] of T);

procedure PrintMatr<T>(a: array of array of T);

procedure PrintMatr<T>(a: List<List<T>>);

// == Конец дополнений 2016.07

// == Конец дополнений к версии 4.14 ==

implementation

const
  NotSupportedReadTypeMessage = 'Ввод данных типа {0} не поддерживается';
  NotSupportedWriteTypeMessage = 'Вывод данных типа {0} не поддерживается';
  eMessage = 'Попытка обращения к объекту Node после вызова его метода Dispose';

var 
  loadNodes: ArrayList;
  InfoT: integer;
  InfoS: string;
  ExceptionThrowed := false;

procedure PutInt(val: integer); forward;
procedure PutReal(val: real); forward;
procedure PutChar(val: char); forward;
procedure PutString(val: string); forward;
procedure PutBoolean(val: boolean); forward;
procedure PutNode(val: Node); forward;
procedure PutPNode(val: PNode); forward;

procedure StartPT(options: integer);    external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'startpt';
procedure FreePT;                                       external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'freept';
function CheckPT(var res: integer):string;external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'checkptf';
procedure RaisePT(s1,s2: string);       external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'raisept';
procedure Task(name: string);           external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'task';
procedure GetR(var param: real);                external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'getr';
procedure PutR(param: real);                            external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'putr';
procedure GetN(var param: integer);     external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'getn';
procedure PutN(param: integer);         external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'putn';
procedure GetC(var param: char);                external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'getc';
procedure PutC(param: char);                    external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'putc';
procedure _GetS(param: System.Text.StringBuilder);external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'gets';
procedure PutS(param: string);          external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'puts';
procedure _GetB(var param: integer);    external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'getb';
procedure _PutB(param: integer);                external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'putb';
procedure _GetP(var param: IntPtr);     external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'getp';
procedure _PutP(param: IntPtr); external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'putp';
procedure DisposeP(sNode: IntPtr);      external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'disposep';
function FinishPT(): integer;      external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'finishpt';  // == 4.13 ==

procedure Dispose(p: pointer);
begin
  DisposeP(IntPtr(p));
end;

procedure GetS(var param: string);
begin
  param := GetString();
end;

procedure GetB(var param: boolean);
begin
  param := GetBoolean();
end;

procedure PutB(param: boolean);
begin
  PutBoolean(param);
end;

procedure GetP(var param: PNode);
begin
  param := GetPNode();
end;

procedure GetP(var param: Node);
begin
  param := GetNode();
end;

procedure PutP(param: PNode);
begin
  PutPNode(param);
end;

procedure PutP(param: Node);
begin
  PutNode(param);
end;

procedure Put(params args: array of Object);
begin
  foreach x: Object in args do
  begin
    if x.GetType = typeof(integer) then
      Put(integer(x))
    else if x.GetType = typeof(real) then
      Put(real(x)) 
    else if x.GetType = typeof(char) then
      Put(char(x)) 
    else if x.GetType = typeof(string) then
      Put(string(x)) 
    else if x.GetType = typeof(boolean) then
      Put(boolean(x)) 
    else if x.GetType = typeof(Node) then
      Put(Node(x)) 
//    else if x.GetType = typeof(PNode) then
//      Put(PNode(x)) 
  end;
end;

procedure Put(param: real);
begin
  PutR(param);  
end;

procedure Put(param: integer);
begin
  PutN(param);  
end;

procedure Put(param: char);
begin
  PutC(param);  
end;

procedure Put(param: string);
begin
  PutS(param);  
end;

procedure Put(param: boolean);
begin
  PutB(param);  
end;

procedure Put(param: PNode);
begin
  PutP(param);  
end;

procedure Put(param: Node);
begin
  PutP(param);  
end;

// -----------------------------------------------------
//                         Node
// -----------------------------------------------------
constructor Node.Create(x: InternalNode; a: IntPtr);
begin
  Self.x.Data := x.Data;
  Self.x.Next := x.Next;
  Self.x.Prev := x.Prev;
  Self.x.Left := x.Left;
  Self.x.Right := x.Right;
  Self.x.Parent := x.Parent;
  addr := a;
  isAllocMem := false;
  isDisposed := false;
end;

constructor Node.Create;
begin
  Init(0, nil, nil, nil, nil, nil);
end;

constructor Node.Create(aData: integer);
begin
  Init(aData, nil, nil, nil, nil, nil);
end;

constructor Node.Create(aData: integer; aNext: Node);
begin
  Init(aData, aNext, nil, nil, nil, nil);
end;

constructor Node.Create(aData: integer; aNext,aPrev: Node);
begin
  Init(aData, aNext, aPrev, nil, nil, nil);
end;

constructor Node.Create(left,right: Node; data: integer);
begin
  Init(data,nil,nil,left,right,nil);
end;

constructor Node.Create(left,right: Node; data: integer; parent: Node);
begin
  Init(data,nil,nil,left,right,parent);
end;

procedure Node.Dispose;
begin
  GC.SuppressFinalize(self);
  internalDispose(true);
end;

procedure Node.Init(Data: integer; Next,Prev,Left,Right,Parent: Node);
begin
  x.Data := Data;

  if Next = nil then
    x.Next := IntPtr.Zero
  else x.Next := Next.addr;

  if Prev = nil then        
    x.Prev := IntPtr.Zero
  else x.Prev := Prev.addr;

  if Left = nil then        
    x.Left := IntPtr.Zero
  else x.Left := Left.addr;

  if Right = nil then        
    x.Right := IntPtr.Zero
  else x.Right := Right.addr;

  if Parent = nil then        
    x.Parent := IntPtr.Zero
  else x.Parent := Parent.addr;

  addr := Marshal.AllocHGlobal(sizeof(InternalNode));
  Marshal.StructureToPtr(x as object, addr, false);
  isAllocMem := true;
  isDisposed := false;
  loadNodes.Add(self); 
end;

procedure Node.setNext(value:Node);
begin
  if isDisposed then
    raise new ObjectDisposedException(ToString, eMessage);
  if value = nil then 
  begin
    x.Next := IntPtr.Zero; 
    Marshal.StructureToPtr(x as object, addr, isAllocMem); 
  end else 
  begin
    x.Next := value.addr;
    Marshal.StructureToPtr(x as object, addr, isAllocMem);                                             
  end;
end;

function Node.getNext:Node;
var tmp: IntPtr;
    tmpNode: Node;
begin
  result := nil;
  if isDisposed then
     raise new ObjectDisposedException(ToString, eMessage);
  if x.Next <> IntPtr.Zero then 
  begin
     tmp := x.Next;
     for var i:=0 to loadNodes.Count-1 do
       if tmp.Equals(Node(loadNodes[i]).addr) then 
         result:=Node(loadNodes[i]);
     if result=nil then 
     begin
       tmpNode := new Node(InternalNode(Marshal.PtrToStructure(tmp,typeof(InternalNode))), tmp); 
       loadNodes.Add(tmpNode);
       result := tmpNode;                   
     end;  
  end;
end;

procedure Node.setPrev(value: Node);
begin
  if isDisposed then
     raise new ObjectDisposedException(ToString, eMessage);
  if value = nil then 
  begin
    x.Prev := IntPtr.Zero; 
    Marshal.StructureToPtr(x as object, addr, isAllocMem); 
  end else 
  begin
    x.Prev := value.addr;
    Marshal.StructureToPtr(x as object, addr, isAllocMem);                                             
  end;
end;

function Node.getPrev: Node;
var tmp: IntPtr;
    tmpNode:Node;
begin
  result := nil;
  if isDisposed then
     raise new ObjectDisposedException(ToString, eMessage);
  if x.Prev <> IntPtr.Zero then begin
     tmp := x.Prev;
     for var i:=0 to loadNodes.Count-1 do
       if tmp.Equals(Node(loadNodes[i]).addr) then 
         result:=Node(loadNodes[i]);
     if result=nil then begin           
       tmpNode := new Node(InternalNode(Marshal.PtrToStructure(tmp,typeof(InternalNode))), tmp);
       loadNodes.Add(tmpNode);
       result := tmpNode;                   
     end;  
  end;
end;

procedure Node.setLeft(value: Node);
begin
  if isDisposed then
     raise new ObjectDisposedException(ToString, eMessage);
  if value = nil then 
  begin
    x.Left := IntPtr.Zero; 
    Marshal.StructureToPtr(x as object, addr, isAllocMem); 
  end else 
  begin
    x.Left := value.addr;
    Marshal.StructureToPtr(x as object, addr, isAllocMem);                                             
  end;
end;

function Node.getLeft: Node;
var tmp: IntPtr;
    tmpNode:Node;
begin
  result := nil;
  if isDisposed then
     raise new ObjectDisposedException(ToString, eMessage);
  if x.Left <> IntPtr.Zero then begin
     tmp := x.Left;
     for var i:=0 to loadNodes.Count-1 do
       if tmp.Equals(Node(loadNodes[i]).addr) then 
         result:=Node(loadNodes[i]);
     if result=nil then begin           
       tmpNode := new Node(InternalNode(Marshal.PtrToStructure(tmp,typeof(InternalNode))), tmp);
       loadNodes.Add(tmpNode);
       result := tmpNode;                   
     end;  
  end;
end;

procedure Node.setRight(value: Node);
begin
  if isDisposed then
     raise new ObjectDisposedException(ToString, eMessage);
  if value = nil then 
  begin
    x.Right := IntPtr.Zero; 
    Marshal.StructureToPtr(x as object, addr, isAllocMem); 
  end else 
  begin
    x.Right := value.addr;
    Marshal.StructureToPtr(x as object, addr, isAllocMem);                                             
  end;
end;

function Node.getRight: Node;
var tmp: IntPtr;
    tmpNode:Node;
begin
  result := nil;
  if isDisposed then
     raise new ObjectDisposedException(ToString, eMessage);
  if x.Right <> IntPtr.Zero then begin
     tmp := x.Right;
     for var i:=0 to loadNodes.Count-1 do
       if tmp.Equals(Node(loadNodes[i]).addr) then 
         result:=Node(loadNodes[i]);
     if result=nil then begin           
       tmpNode := new Node(InternalNode(Marshal.PtrToStructure(tmp,typeof(InternalNode))), tmp);
       loadNodes.Add(tmpNode);
       result := tmpNode;                   
     end;  
  end;
end;

procedure Node.setParent(value: Node);
begin
  if isDisposed then
     raise new ObjectDisposedException(ToString, eMessage);
  if value = nil then 
  begin
    x.Parent := IntPtr.Zero; 
    Marshal.StructureToPtr(x as object, addr, isAllocMem); 
  end else 
  begin
    x.Parent := value.addr;
    Marshal.StructureToPtr(x as object, addr, isAllocMem);                                             
  end;
end;

function Node.getParent: Node;
var tmp: IntPtr;
    tmpNode:Node;
begin
  result := nil;
  if isDisposed then
     raise new ObjectDisposedException(ToString, eMessage);
  if x.Parent <> IntPtr.Zero then begin
     tmp := x.Parent;
     for var i:=0 to loadNodes.Count-1 do
       if tmp.Equals(Node(loadNodes[i]).addr) then 
         result:=Node(loadNodes[i]);
     if result=nil then begin           
       tmpNode := new Node(InternalNode(Marshal.PtrToStructure(tmp,typeof(InternalNode))), tmp);
       loadNodes.Add(tmpNode);
       result := tmpNode;                   
     end;  
  end;
end;

procedure Node.setData(value: integer);
begin
  if isDisposed then
    raise new ObjectDisposedException(ToString, eMessage);
  self.x.Data := value;
  Marshal.StructureToPtr(x as object, addr, isAllocMem);
end;

function Node.getData: integer;
begin
  if isDisposed then
    raise new ObjectDisposedException(ToString, eMessage);
  result := self.x.Data;
end;

procedure Node.internalDispose(disposing: boolean);
begin
  if not isDisposed then
  //lock(self)
  begin
    if disposing then
      DisposeP(addr);
    if isAllocMem then
      Marshal.FreeHGlobal(addr);
    isDisposed := true;
  end;  
end;

procedure internalWrite(args: array of object); forward;

// -----------------------------------------------------
//                      IOPT4System
// -----------------------------------------------------
procedure IOPT4System.write(obj: object);
var args: array of object;
begin
  SetLength(args,1);
  args[0] := obj;
  internalWrite(args);
end;

procedure IOPT4System.writeln; 
begin
end;

// -----------------------------------------------------
//                      Функции Get
// -----------------------------------------------------
function GetInt: integer;
var val:integer;
begin
  Getn(val);
  result := val;
end;

function GetInteger: integer;
begin
  result := GetInt;
end;

function GetReal: real;
var val: real;
begin
  getr(val);
  result := val;
end;

function GetDouble: real;
begin
  result := GetReal;
end;

function GetChar: char;
var val: char;
begin
  getc(val);
  result:=val;
end;

function GetString: string;
var val: System.Text.StringBuilder;
begin
  val := new System.Text.StringBuilder(200);//TODO почему 200?
  _gets(val);
  result := val.ToString;
end;

function GetBool: boolean;
var val: integer;
begin
  _getb(val);
  result := val=1;
end;
                
function GetBoolean: boolean;
begin
  result := GetBool;
end;

function GetNode: Node;
  procedure GetPtr(var sNode:InternalNode; var pNode: IntPtr);
  var p:IntPtr;
  begin
    p := IntPtr.Zero;
    _GetP(p);
    pNode := p;
    if p <> IntPtr.Zero then 
      sNode := InternalNode(Marshal.PtrToStructure(p, typeof(InternalNode)));
  end;
var 
  p: IntPtr;
  sNode: InternalNode;
begin
  //raise new NotSupportedException('Работа с динамическими структурами задачника PT4 не поддерживается в этой версии компилятора. Исправление ошибки планируется в следуйщей версии');
  //result := new PT4Node(sNode, p);// fixme здесь ошибка генерации кода!
  p := IntPtr.Zero;
  sNode.Data := 0;
  sNode.Next := IntPtr.Zero;
  sNode.Prev := IntPtr.Zero;
  sNode.Left := IntPtr.Zero;
  sNode.Right := IntPtr.Zero;
  sNode.Parent := IntPtr.Zero;
  GetPtr(sNode, p);  
  if p = IntPtr.Zero then
    result := nil
  else begin  
    for var i:=0 to loadNodes.Count-1 do 
      if sNode=Node(loadNodes[i]).x then
         result := Node(loadNodes[i]);
    if result = nil then begin
      result := new Node(sNode, p);
      loadNodes.Add(result);
    end;
  end;
end;

function GetPNode: PNode;
var ip: IntPtr;
begin
  _GetP(ip);
  Result := PNode(pointer(ip));
end;

function ReadInteger: integer;
begin
  Result := GetInt;
end;

function ReadReal: real;
begin
  Result := GetReal;
end;

function ReadChar: char;
begin
  Result := GetChar;
end;

function ReadString: string;
begin
  Result := GetString;
end;

function ReadBoolean: boolean;
begin
  Result := GetBool;
end;

function ReadPNode: PNode;
begin
  Result := GetPNode;
end;

function ReadNode: Node;
begin
  Result := GetNode;
end;

function ReadlnInteger: integer;
begin
  Result := GetInt;
end;

function ReadlnReal: real;
begin
  Result := GetReal;
end;

function ReadlnChar: char;
begin
  Result := GetChar;
end;

function ReadlnString: string;
begin
  Result := GetString;
end;

function ReadlnBoolean: boolean;
begin
  Result := GetBool;
end;

function ReadlnPNode: PNode;
begin
  Result := GetPNode;
end;

function ReadlnNode: Node;
begin
  Result := GetNode;
end;

// == Версия 4.15. Дополнения ==

function ReadInteger(prompt: string): integer;
begin
  Result := GetInt;
end;

function ReadReal(prompt: string): real;
begin
  Result := GetReal;
end;

function ReadChar(prompt: string): char;
begin
  Result := GetChar;
end;

function ReadString(prompt: string): string;
begin
  Result := GetString;
end;

function ReadBoolean(prompt: string): boolean;
begin
  Result := GetBool;
end;

function ReadPNode(prompt: string): PNode;
begin
  Result := GetPNode;
end;

function ReadNode(prompt: string): Node;
begin
  Result := GetNode;
end;

function ReadlnInteger(prompt: string): integer;
begin
  Result := GetInt;
end;

function ReadlnReal(prompt: string): real;
begin
  Result := GetReal;
end;

function ReadlnChar(prompt: string): char;
begin
  Result := GetChar;
end;

function ReadlnString(prompt: string): string;
begin
  Result := GetString;
end;

function ReadlnBoolean(prompt: string): boolean;
begin
  Result := GetBool;
end;

function ReadlnPNode(prompt: string): PNode;
begin
  Result := GetPNode;
end;

function ReadlnNode(prompt: string): Node;
begin
  Result := GetNode;
end;

// == Версия 4.15. Конец дополнений ==

// -----------------------------------------------------
//                      Процедуры Put
// -----------------------------------------------------
procedure PutInt(val: integer);
begin
  putn(val);
end;

procedure PutReal(val: Real);
begin
  putr(val);
end;

procedure PutChar(val: char);
begin
  putc(val);
end;

procedure PutString(val: string);
begin
  puts(val);
end;

procedure PutBoolean(val: boolean);
begin
  if val then 
    _putb(1)
  else _putb(0);
end;

procedure PutNode(val: Node);
var p: IntPtr;
begin
  p := IntPtr.Zero;
  if val <> nil then begin
    if val.isDisposed then
      raise new ObjectDisposedException(val.ToString, eMessage);
    p := val.addr; 
  end;   
  _PutP(p);
end;

procedure PutPNode(val: PNode);
var pp: pointer;
begin
  pp := pointer(val);
  _PutP(IntPtr(pp));
end;

// -----------------------------------------------------
//                  Генерация исключений
// -----------------------------------------------------
procedure RaisePTAndCheckPT(e: Exception);
begin
  RaisePT(e.GetType.ToString, e.Message);
  InfoS := CheckPT(InfoT);
  if InfoT = 0 then 
    System.Console.WriteLine(InfoS);
  FreePT;
end;

procedure GenerateNotSupportedTypeException(MsgTemplate, TypeName: string);
var e: Exception;
begin
  e := new PT4Exception(string.Format(MsgTemplate,TypeName));
  RaisePTAndCheckPT(e);
  ExceptionThrowed := true;
  raise e;
end;

procedure GenerateNotSupportedWriteTypeException(TypeName: string);
begin
  GenerateNotSupportedTypeException(NotSupportedWriteTypeMessage,TypeName);
end;

procedure GenerateNotSupportedReadTypeException(TypeName: string);
begin
  GenerateNotSupportedTypeException(NotSupportedReadTypeMessage,TypeName);
end;

// -----------------------------------------------------
//                        read
// -----------------------------------------------------
procedure read(var x: byte);
begin
  GenerateNotSupportedReadTypeException(x.GetType.ToString);
end; 

procedure read(var x: shortint); 
begin
  GenerateNotSupportedReadTypeException(x.GetType.ToString);
end; 

procedure read(var x: smallint); 
begin
  GenerateNotSupportedReadTypeException(x.GetType.ToString);
end; 

procedure read(var x: word); 
begin
  GenerateNotSupportedReadTypeException(x.GetType.ToString);
end; 

procedure read(var x: longword); 
begin
  GenerateNotSupportedReadTypeException(x.GetType.ToString);
end; 

procedure read(var x: int64); 
begin
  GenerateNotSupportedReadTypeException(x.GetType.ToString);
end; 

procedure read(var x: uint64); 
begin
  GenerateNotSupportedReadTypeException(x.GetType.ToString);
end; 

procedure read(var x: single); 
begin
  GenerateNotSupportedReadTypeException(x.GetType.ToString);
end; 

procedure Read(var val: integer);
begin
  val := GetInt;
end;

procedure Read(var val: real);  
begin
  val := GetReal;
end;

procedure Read(var val: char);  
begin
  val := GetChar;
end;

procedure Read(var val: string);
begin
  val := GetString;
end;

procedure Read(var val: boolean);
begin
  val := GetBoolean;
end;

procedure Read(var val: Node);
begin
  val := GetNode;
end;

procedure Read(var val: PNode);
begin
  val := GetPNode;
end;

procedure Readln;
begin
end;

procedure Print(params args: array of object);
begin
  if args.Length = 0 then
    exit;
  for var i := 0 to args.length - 1 do
    write(args[i]);
end;

procedure Println(params args: array of object);
begin
  Print(args);
end;

// == Версия 4.15. Дополнения ==

procedure Print(s: string);
begin
  write(s);
end;

procedure Println(s: string);
begin
  write(s);
end;

// == Версия 4.15. Конец дополнений ==

{procedure write;
begin
end;}

// -----------------------------------------------------
//                        InternalWrite
// -----------------------------------------------------
procedure InternalWrite(args: array of object);
begin
  if (args[0] is PABCSystem.Text) then 
  begin
    for var i:=1 to args.length-1 do
      PABCSystem.Write(PABCSystem.Text(args[0]),args[i]);
  end 
  else
  for var i:=0 to args.length-1 do
    if args[i] = nil              then PutNode(nil) else
    if args[i] is integer then PutInt(integer(args[i])) else
    if args[i] is shortint then PutInt(shortint(args[i])) else
    if args[i] is smallint then PutInt(smallint(args[i])) else
    if args[i] is int64 then PutInt(int64(args[i])) else
    if args[i] is byte then PutInt(byte(args[i])) else
    if args[i] is word then PutInt(word(args[i])) else
    if args[i] is longword then PutInt(longword(args[i])) else
    if args[i] is uint64 then PutInt(uint64(args[i])) else

    if args[i] is real          then PutReal(real(args[i])) else
          if args[i] is char            then PutChar(char(args[i])) else
        if args[i] is string    then PutString(string(args[i])) else
        if args[i] is boolean then PutBoolean(boolean(args[i])) else
        if args[i] is Node      then PutNode(Node(args[i])) else
        if args[i] is PointerOutput     then 
        begin
          var ip := IntPtr(PointerOutput(args[i]).p);
          _PutP(ip);
        end  
        else GenerateNotSupportedWriteTypeException(args[i].GetType.ToString);
end;

procedure PT4_ExecuteBeforeProcessTerminateIn__Mode(e: Exception);
begin
  if not ExceptionThrowed then 
    RaisePTAndCheckPT(e);
end;

var __initialized := false;

procedure __InitModule;
begin
  CurrentIOSystem := new IOPT4System;
  PrintDelimDefault := '';
  loadNodes := new ArrayList;  
  ExecuteBeforeProcessTerminateIn__Mode += PT4_ExecuteBeforeProcessTerminateIn__Mode;
  StartPT(512);    
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitModule;
  end;
end;

procedure __FinalizeModule__;
begin
  InfoS := CheckPT(InfoT);
  if InfoT=0 then 
    Console.WriteLine(InfoS);
  FreePT;
  // == Начало дополнений к версии 4.13 == 
  var fpt := FinishPT;
  if fpt = 1 then exit;
  var asm := System.Reflection.Assembly.GetExecutingAssembly;
  var nm := asm.FullName;
  Delete(nm, Pos(',', nm), length(nm));
  var prg := asm.GetType(nm+'.Program');
  var solveproc := prg.GetMethod('$Main');
  var initproc := prg.GetMethod('$_InitVariables_');
  if (solveproc = nil) or (initproc = nil) then
    foreach var prg0 in asm.GetTypes() do
    begin
      prg := prg0;
      solveproc := prg0.GetMethod('$Main');
      initproc := prg0.GetMethod('$_InitVariables_');
      if (solveproc <> nil) and (initproc <> nil) then
        break;
  end;
  var examunit := asm.GetType('PT4Exam.PT4Exam');
  var finexamproc: System.Reflection.MethodInfo := nil;
  if examunit <> nil then
    finexamproc := examunit.GetMethod('FinExam');
  var i := 0;
  while (fpt = 0) and (i < 10) do 
  begin
    StartPT(512);    
    try
      foreach var f in prg.GetFields() do
        if not f.Name.StartsWith('$') then 
        try
          f.SetValue(nil, nil);
        except
        end;
      if initproc <> nil then
      begin
        initproc.Invoke(nil,nil);
      end;  
      solveproc.Invoke(nil,nil);
    except
      on e: Exception do
        RaisePT(e.InnerException.GetType.ToString, e.InnerException.Message);
    end;
    if finexamproc <> nil then
      finexamproc.Invoke(nil,nil);
    InfoS := CheckPT(InfoT);
    FreePT;
    inc(i);
    fpt := FinishPT;
  end;
  // == Конец дополнений к версии 4.13 == 
end;

// == Версия 1.3. Дополнения ==

var 
  D: integer := 2;

procedure Show(s: string); external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'show';

procedure Show(S: string; A: Integer; W: Integer);
var s0: string;
begin
  Str(A:W, s0);
  Show(S + s0);
end;

procedure Show(S: string; A: Real; W: Integer);
var s0: string;
begin
  if D > 0 then
    s0 := string.Format('{0,'+W+':f'+D+'}', A).Replace(',','.')
  else
    s0 := string.Format('{0,'+W+':e}', A).Replace(',','.');
  Show(S + s0);
end;

procedure Show(S: string; A: Integer);
begin
  Show(S, A, 0);
end;

procedure Show(S: string; A: Real);
begin
  Show(S, A, 0);
end;

procedure Show(A: Integer; W: Integer);
begin
  Show('', A, W);
end;

procedure Show(A: Real; W: Integer);
begin
  Show('', A, W);
end;

procedure Show(A: Integer);
begin
  Show('', A, 0);
end;

procedure Show(A: Real);
begin
  Show('', A, 0);
end;

procedure ShowLine(S: string);
begin
  Show(S + #13);
end;

procedure ShowLine;
begin
  Show(#13);
end;

procedure ShowLine(S: string; A: Integer; W: Integer);
begin
  Show(S, A, W);
  Show(#13);
end;

procedure ShowLine(S: string; A: Real; W: Integer);
begin
  Show(S, A, W);
  Show(#13);
end;

procedure ShowLine(S: string; A: Integer);
begin
  Show(S, A, 0);
  Show(#13);
end;

procedure ShowLine(S: string; A: Real);
begin
  Show(S, A, 0);
  Show(#13);
end;

procedure ShowLine(A: Integer; W: Integer);
begin
  Show('', A, W);
  Show(#13);
end;

procedure ShowLine(A: Real; W: Integer);
begin
  Show('', A, W);
  Show(#13);
end;

procedure ShowLine(A: Integer);
begin
  Show('', A, 0);
  Show(#13);
end;

procedure ShowLine(A: Real);
begin
  Show('', A, 0);
  Show(#13);
end;

procedure HideTask; external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'hidetask';

procedure SetPrecision(N: Integer);
begin
  if N <= 0 then
    D := 0
  else
    D := N;
end;

// == Конец дополнений к версии 1.3 ==

// == Версия 4.14. Дополнения ==

function  ReadSeqInteger(): sequence of integer;
begin
  result := Range(1, GetInteger()).Select(e -> GetInteger()).ToArray();
end;  

function  ReadSeqReal(): sequence of real;
begin
  result := Range(1, GetInteger()).Select(e -> GetReal()).ToArray();
end;  

function ReadSeqString(): sequence of string;
begin
  result := Range(1, GetInteger()).Select(e -> GetString()).ToArray();
end;           

function  ReadSeqInteger(n: integer): sequence of integer;
begin
  result := Range(1, n).Select(e -> GetInteger()).ToArray();
end;  

function  ReadSeqReal(n: integer): sequence of real;
begin
  result := Range(1, n).Select(e -> GetReal()).ToArray();
end;  

function ReadSeqString(n: integer): sequence of string;
begin
  result := Range(1, n).Select(e -> GetString()).ToArray();
end;           

function  ReadArrInteger(): array of integer;
begin
  result := Range(1, GetInteger()).Select(e -> GetInteger()).ToArray();
end;  

function  ReadArrReal(): array of real;
begin
  result := Range(1, GetInteger()).Select(e -> GetReal()).ToArray();
end;  

function ReadArrString(): array of string;
begin
  result := Range(1, GetInteger()).Select(e -> GetString()).ToArray();
end;           

function  ReadArrInteger(n: integer): array of integer;
begin
  result := Range(1, n).Select(e -> GetInteger()).ToArray();
end;  

function  ReadArrReal(n: integer): array of real;
begin
  result := Range(1, n).Select(e -> GetReal()).ToArray();
end;  

function ReadArrString(n: integer): array of string;
begin
  result := Range(1, n).Select(e -> GetString()).ToArray();
end;   

function ReadMatrInteger(m,n: integer): array [,] of integer;
begin
  result := new integer[m,n];
  for var i := 0 to m-1 do
    for var j := 0 to n-1 do
      result[i,j] := ReadInteger;
end;

function ReadMatrInteger(): array [,] of integer;
begin
  result := ReadMatrInteger(ReadInteger,ReadInteger);
end;

function  ReadMatrReal(m,n: integer): array [,] of real;
begin
  result := new real[m,n];
  for var i := 0 to m-1 do
    for var j := 0 to n-1 do
      result[i,j] := ReadReal;
end;

function  ReadMatrReal(): array [,] of real;
begin
  result := ReadMatrReal(ReadInteger,ReadInteger);
end;

function  ReadMatrString(m,n: integer): array [,] of string;
begin
  result := new string[m,n];
  for var i := 0 to m-1 do
    for var j := 0 to n-1 do
      result[i,j] := ReadString;
end;

function  ReadMatrString(): array [,] of string;
begin
  result := ReadMatrString(ReadInteger,ReadInteger);
end;        

procedure ReadMatr(var m, n: integer; var a: array [,] of integer);
begin
  read(m); read(n);
  a := new integer[m, n];
  for var i := 0 to m - 1 do
    for var j := 0 to n - 1 do
      read(a[i,j]);
end;

procedure ReadMatr(var m, n: integer; var a: array [,] of real);
begin
  read(m); read(n);
  a := new real[m, n];
  for var i := 0 to m - 1 do
    for var j := 0 to n - 1 do
      read(a[i,j]);
end;

procedure ReadMatr(var m, n: integer; var a: array [,] of string);
begin
  read(m); read(n);
  a := new string[m, n];
  for var i := 0 to m - 1 do
    for var j := 0 to n - 1 do
      read(a[i,j]);
end;

procedure ReadMatr(var m: integer; var a: array [,] of integer);
begin
  read(m);
  a := new integer[m, m];
  for var i := 0 to m - 1 do
    for var j := 0 to m - 1 do
      read(a[i,j]);
end;

procedure ReadMatr(var m: integer; var a: array [,] of real);
begin
  read(m);
  a := new real[m, m];
  for var i := 0 to m - 1 do
    for var j := 0 to m - 1 do
      read(a[i,j]);
end;

procedure ReadMatr(var m: integer; var a: array [,] of string);
begin
  read(m);
  a := new string[m, m];
  for var i := 0 to m - 1 do
    for var j := 0 to m - 1 do
      read(a[i,j]);
end;

procedure ReadMatr(var m, n: integer; var a: array of array of integer);
begin
  read(m); read(n);
  SetLength(a, m);
  for var i := 0 to m - 1 do
    a[i] := ReadArrInteger(n);
end;

procedure ReadMatr(var m, n: integer; var a: array of array of real);
begin
  read(m); read(n);
  SetLength(a, m);
  for var i := 0 to m - 1 do
    a[i] := ReadArrReal(n);
end;

procedure ReadMatr(var m, n: integer; var a: array of array of string);
begin
  read(m); read(n);
  SetLength(a, m);
  for var i := 0 to m - 1 do
    a[i] := ReadArrString(n);
end;

procedure ReadMatr(var m: integer; var a: array of array of integer);
begin
  read(m);
  SetLength(a, m);
  for var i := 0 to m - 1 do
    a[i] := ReadArrInteger(m);
end;

procedure ReadMatr(var m: integer; var a: array of array of real);
begin
  read(m);
  SetLength(a, m);
  for var i := 0 to m - 1 do
    a[i] := ReadArrReal(m);
end;

procedure ReadMatr(var m: integer; var a: array of array of string);
begin
  read(m);
  SetLength(a, m);
  for var i := 0 to m - 1 do
    a[i] := ReadArrString(m);
end;

procedure ReadMatr(var m, n: integer; var a: List<List<integer>>);
begin
  read(m); read(n);
  a := new List<List<integer>>(m);
  for var i := 0 to m - 1 do
    a.Add(ReadSeqInteger(n).ToList);
end;

procedure ReadMatr(var m, n: integer; var a: List<List<real>>);
begin
  read(m); read(n);
  a := new List<List<real>>(m);
  for var i := 0 to m - 1 do
    a.Add(ReadSeqReal(n).ToList);
end;

procedure ReadMatr(var m, n: integer; var a: List<List<string>>);
begin
  read(m); read(n);
  a := new List<List<string>>(m);
  for var i := 0 to m - 1 do
    a.Add(ReadSeqString(n).ToList);
end;

procedure ReadMatr(var m: integer; var a: List<List<integer>>);
begin
  read(m);
  a := new List<List<integer>>(m);
  for var i := 0 to m - 1 do
    a.Add(ReadSeqInteger(m).ToList);
end;

procedure ReadMatr(var m: integer; var a: List<List<real>>);
begin
  read(m);
  a := new List<List<real>>(m);
  for var i := 0 to m - 1 do
    a.Add(ReadSeqReal(m).ToList);
end;

procedure ReadMatr(var m: integer; var a: List<List<string>>);
begin
  read(m);
  a := new List<List<string>>(m);
  for var i := 0 to m - 1 do
    a.Add(ReadSeqString(m).ToList);
end;

procedure WriteMatr<T>(a: array[,] of T);
begin
  for var i := 0 to a.GetLength(0)-1 do
    for var j := 0 to a.GetLength(1)-1 do
      write(a[i,j]);
end;

procedure WriteMatr<T>(a: array of array of T);
begin
  for var i := 0 to a.Length-1 do
    for var j := 0 to a[i].Length-1 do
      write(a[i][j]);
end;

procedure WriteMatr<T>(a: List<List<T>>);
begin
  for var i := 0 to a.Count-1 do
    for var j := 0 to a[i].Count-1 do
      write(a[i][j]);
end;

/// Выводит размер и элементы последовательности
procedure WriteAll<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  PT4.Put(b.Length);
  foreach e : T in b do
    PT4.Put(e);
end;

/// Выводит элементы последовательности
procedure Write<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  foreach e : T in b do
    PT4.Put(e);
end;

/// Выводит элементы динамического массива
procedure Write<T>(self: array of T); extensionmethod;
begin
  for var i:=0 to self.Length-1 do
    PT4.Put(self[i]);
end;

/// Выводит элементы матрицы
procedure Write<T>(self: array [,] of T); extensionmethod;
begin
  for var i:=0 to self.GetLength(0)-1 do
  for var j:=0 to self.GetLength(1)-1 do
    PT4.Put(self[i,j]);
end;

// == Дополнения 2016.07

procedure PrintMatr<T>(a: array[,] of T);
begin
  for var i := 0 to a.GetLength(0)-1 do
    for var j := 0 to a.GetLength(1)-1 do
      write(a[i,j]);
end;

procedure PrintMatr<T>(a: array of array of T);
begin
  for var i := 0 to a.Length-1 do
    for var j := 0 to a[i].Length-1 do
      write(a[i][j]);
end;

procedure PrintMatr<T>(a: List<List<T>>);
begin
  for var i := 0 to a.Count-1 do
    for var j := 0 to a[i].Count-1 do
      write(a[i][j]);
end;

/// Выводит размер и элементы последовательности
procedure PrintAll<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  PT4.Put(b.Length);
  foreach e : T in b do
    PT4.Put(e);
end;

/// Выводит размер и элементы динамического массива
procedure PrintAll<T>(self: array of T); extensionmethod;
begin
  PT4.Put(self.Length);
  for var i:=0 to self.Length-1 do
    PT4.Put(self[i]);
end;

/// Выводит размер и элементы динамического массива
procedure WriteAll<T>(self: array of T); extensionmethod;
begin
  PT4.Put(self.Length);
  for var i:=0 to self.Length-1 do
    PT4.Put(self[i]);
end;

/// Выводит элементы последовательности
procedure Writeln<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  foreach e : T in b do
    PT4.Put(e);
end;

/// Выводит элементы динамического массива
procedure Writeln<T>(self: array of T); extensionmethod;
begin
  for var i:=0 to self.Length-1 do
    PT4.Put(self[i]);
end;

/// Выводит элементы матрицы
procedure Writeln<T>(self: array [,] of T); extensionmethod;
begin
  for var i:=0 to self.GetLength(0)-1 do
  for var j:=0 to self.GetLength(1)-1 do
    PT4.Put(self[i,j]);
end;

// == Конец дополнений 2016.07


/// Выводит в разделе отладки окна задачника 
/// комментарий cmt, размер последовательности и значения, 
/// полученные из элементов последовательности 
/// с помощью указанного лямбда-выражения

function Show<TSource>(self: sequence of TSource; cmt: string; selector: System.Func<TSource, string>): sequence of TSource; extensionmethod;
begin
  var b := self.Select(selector).ToArray();
  PT4.Show(cmt);
  PT4.Show((b.Length + ':').PadLeft(3));
  foreach var e in b do
    PT4.Show(e);
  PT4.ShowLine();
  result := self; 
end;

/// Выводит в разделе отладки окна задачника 
/// размер последовательности и значения, 
/// полученные из элементов последовательности 
/// с помощью указанного лямбда-выражения
function Show<TSource>(Self: sequence of TSource; selector: System.Func<TSource, string>): sequence of TSource; extensionmethod;
begin
  result := self.Show('', selector); 
end;

/// Выводит в разделе отладки окна задачника 
/// комментарий cmt, размер последовательности и ее элементы
function Show<TSource>(Self: sequence of TSource; cmt: string): sequence of TSource; extensionmethod;
begin
  result := self;
  var a := self.ToArray;
  var s := '';
  var t := '';
  if a.Length > 0 then
    t := a[0].GetType.Name;
  if t = 'Double' then
    if D > 0 then
      s := '{0,0:f' + D + '}'
    else
      s := '{0,0:e}';
  a.Show(cmt, e -> s = '' ? e.ToString() : 
    string.Format(s, e).Replace(',', '.'));
end;


/// Выводит в разделе отладки окна задачника 
/// размер последовательности и ее элементы.
function Show<TSource>(Self: sequence of TSource): sequence of TSource; extensionmethod;
begin
  result := self.Show(''); 
end;

// == Конец дополнений к версии 4.14 ==

initialization
  __InitModule;
finalization
  __FinalizeModule__;
end.