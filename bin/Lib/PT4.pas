/// ������ ������������ ��������� Programming Taskbook 4
unit PT4;  

//------------------------------------------------------------------------------
// ������ ��� ����������� ��������� Programming Taskbook
// ������ 4.15
// Copyright (c) 2006-2008 DarkStar, SSM
// Copyright (c) 2010 �.�.�������, ���������� � ������ 1.3
// Copyright (c) 2014-2015 �.�.�������, ���������� � ������ 4.13
// Copyright (c) 2015 �.�.�������, ���������� � ������ 4.14
// Copyright (c) 2016 �.�.�������, ���������� � ������ 4.15
// ����������� �������� Programming Taskbook Copyright (c)�.�.�������, 1998-2017
//------------------------------------------------------------------------------

{$apptype windows}
{$platformtarget x86}

interface

uses System,
     System.Collections,
     System.Runtime.InteropServices;
type
  /// ��� ��������� �� ���� ������
  PNode = ^TNode;
  /// ��� ���� ������
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

/// ������� ������������ �������
procedure Task(name: string);

/// ������ � ������� �������� ������ ����
function GetInt: integer;
/// ������ � ������� �������� ������ ����
function GetInteger: integer;
/// ������ � ������� �������� ������������� ����
function GetReal: real;
/// ������ � ������� �������� ������������� ����
function GetDouble: real;
/// ������ � ������� �������� ����������� ����
function GetChar: char;
/// ������ � ������� �������� ���������� ����
function GetString: string;
/// ������ � ������� �������� ����������� ����
function GetBool: boolean;
/// ������ � ������� �������� ����������� ����
function GetBoolean: boolean;
/// ������ � ������� �������� ���� Node 
function GetNode: Node;
/// ������ � ������� �������� ���� PNode 
function GetPNode: PNode;

/// ���������� ��������� �������� ���� integer
function ReadInteger: integer;
/// ���������� ��������� �������� ���� real
function ReadReal: real;
/// ���������� ��������� �������� ���� char
function ReadChar: char;
/// ���������� ��������� �������� ���� string
function ReadString: string;
/// ���������� ��������� �������� ���� boolean
function ReadBoolean: boolean;
/// ���������� ��������� �������� ���� PNode
function ReadPNode: PNode;
/// ���������� ��������� �������� ���� Node
function ReadNode: Node;

/// ���������� ��������� �������� ���� integer
function ReadlnInteger: integer;
/// ���������� ��������� �������� ���� real
function ReadlnReal: real;
/// ���������� ��������� �������� ���� char
function ReadlnChar: char;
/// ���������� ��������� �������� ���� string
function ReadlnString: string;
/// ���������� ��������� �������� ���� boolean
function ReadlnBoolean: boolean;
/// ���������� ��������� �������� ���� PNode
function ReadlnPNode: PNode;
/// ���������� ��������� �������� ���� Node
function ReadlnNode: Node;

// == ������ 4.15. ���������� ==

/// ���������� ��������� �������� ���� integer.
/// ��������� ����������� prompt ������������
function ReadInteger(prompt: string): integer;
/// ���������� ��������� �������� ���� real.
/// ��������� ����������� prompt ������������
function ReadReal(prompt: string): real;
/// ���������� ��������� �������� ���� char.
/// ��������� ����������� prompt ������������
function ReadChar(prompt: string): char;
/// ���������� ��������� �������� ���� string.
/// ��������� ����������� prompt ������������
function ReadString(prompt: string): string;
/// ���������� ��������� �������� ���� boolean.
/// ��������� ����������� prompt ������������
function ReadBoolean(prompt: string): boolean;
/// ���������� ��������� �������� ���� PNode.
/// ��������� ����������� prompt ������������
function ReadPNode(prompt: string): PNode;
/// ���������� ��������� �������� ���� Node.
/// ��������� ����������� prompt ������������
function ReadNode(prompt: string): Node;

/// ���������� ��������� �������� ���� integer.
/// ��������� ����������� prompt ������������
function ReadlnInteger(prompt: string): integer;
/// ���������� ��������� �������� ���� real.
/// ��������� ����������� prompt ������������
function ReadlnReal(prompt: string): real;
/// ���������� ��������� �������� ���� char.
/// ��������� ����������� prompt ������������
function ReadlnChar(prompt: string): char;
/// ���������� ��������� �������� ���� string.
/// ��������� ����������� prompt ������������
function ReadlnString(prompt: string): string;
/// ���������� ��������� �������� ���� boolean.
/// ��������� ����������� prompt ������������
function ReadlnBoolean(prompt: string): boolean;
/// ���������� ��������� �������� ���� PNode.
/// ��������� ����������� prompt ������������
function ReadlnPNode(prompt: string): PNode;
/// ���������� ��������� �������� ���� Node.
/// ��������� ����������� prompt ������������
function ReadlnNode(prompt: string): Node;

// == ������ 4.15. ����� ���������� ==

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

//���� ���� ������ �� ��������������
///- read(a,b,...)
/// ������ �������� a,b,... �� ���� ������������ ���������
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

// == ������ 4.15. ���������� ==

procedure Print(s: string);
procedure Println(s: string);

// == ������ 4.15. ����� ���������� ==

/// ����������� ������, ���������� �����������, �� ������� ��������� p
procedure Dispose(p: pointer);

// ��������� ������ ��������� - ��� Intellisense
// ���, ������ ������������ - ����������� ��������� write ���������� ������
///- write(a,b,...)
/// ������� �������� a,b,... � ���� ������������ ���������
//procedure write;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

// == ������ 1.3. ���������� ==

/// ������� ������ S � ������� ������� ���� ���������
procedure Show(S: string);

/// ������� ����� A � ������������ S � ������� �������
/// ���� ���������; ��� ������ ����� ��������� W �������� �������
procedure Show(S: string; A: integer; W: integer);

/// ������� ����� A � ������������ S � ������� �������
/// ���� ���������; ��� ������ ����� ��������� W �������� �������
procedure Show(S: string; A: real; W: integer);

/// ������� ����� A � ������������ S � ������� ������� ���� ���������
procedure Show(S: string; A: integer);

/// ������� ����� A � ������������ S � ������� ������� ���� ���������
procedure Show(S: string; A: real);

/// ������� ����� A � ������� ������� ���� ���������;
/// ��� ������ ��������� W �������� �������
procedure Show(A: integer; W: integer);

/// ������� ����� A � ������� ������� ���� ���������;
/// ��� ������ ��������� W �������� �������
procedure Show(A: real; W: integer);

/// ������� ����� A � ������� ������� ���� ���������
procedure Show(A: integer);

/// ������� ����� A � ������� ������� ���� ���������
procedure Show(A: real);

/// ��������� ������� �� ����� �������� ������
/// � ������� ������� ���� ���������
procedure ShowLine;

/// ������� ������ S � ������� ������� ���� ���������,
/// ����� ���� ��������� ������� �� ����� �������� ������
procedure ShowLine(S: string);

/// ������� ����� A � ������������ S � ������� �������
/// ���� ���������; ��� ������ ����� ��������� W �������� �������.
/// ����� ������ ������ ��������� ������� �� ����� �������� ������
procedure ShowLine(S: string; A: integer; W: integer);

/// ������� ����� A � ������������ S � ������� �������
/// ���� ���������; ��� ������ ����� ��������� W �������� �������.
/// ����� ������ ������ ��������� ������� �� ����� �������� ������
procedure ShowLine(S: string; A: real; W: integer);

/// ������� ����� A � ������������ S � ������� ������� ���� ���������,
/// ����� ���� ��������� ������� �� ����� �������� ������
procedure ShowLine(S: string; A: integer);

/// ������� ����� A � ������������ S � ������� ������� ���� ���������,
/// ����� ���� ��������� ������� �� ����� �������� ������
procedure ShowLine(S: string; A: real);

/// ������� ����� A � ������� ������� ���� ���������;
/// ��� ������ ��������� W �������� �������.
/// ����� ������ ����� ��������� ������� �� ����� �������� ������
procedure ShowLine(A: integer; W: integer);

/// ������� ����� A � ������� ������� ���� ���������;
/// ��� ������ ��������� W �������� �������.
/// ����� ������ ����� ��������� ������� �� ����� �������� ������
procedure ShowLine(A: real; W: integer);

/// ������� ����� A � ������� ������� ���� ���������,
/// ����� ���� ��������� ������� �� ����� �������� ������
procedure ShowLine(A: integer);

/// ������� ����� A � ������� ������� ���� ���������,
/// ����� ���� ��������� ������� �� ����� �������� ������
procedure ShowLine(A: real);

/// ����������� ������ ������ ������������ ����� � ������� �������
/// ���� ���������. ���� N > 0, �� ����� ��������� � �������
/// � ������������� ������ � N �������� �������. ���� N = 0,
/// �� ����� ��������� � ���������������� �������, ����� �������
/// ������ ������������ ������� ���� ������
procedure SetPrecision(N: integer);

/// ������������ �������������� ������� ���� ��������
/// ���� ���������, ����� ������� �������
procedure HideTask;

// == ����� ���������� � ������ 1.3 ==

// == ������ 4.14. ���������� ==

/// ������ n ����� �����
/// � ���������� ��������� ����� � ���� �������
function ReadArrInteger(n: integer): array of integer;

/// ������ n ������������ �����
/// � ���������� ��������� ����� � ���� �������
function ReadArrReal(n: integer): array of real;

/// ������ n ����� 
/// � ���������� ��������� ������ � ���� �������
function ReadArrString(n: integer): array of string;

/// ������ n ����� �����
/// � ���������� ��������� ����� � ���� ������������������
function ReadSeqInteger(n: integer): sequence of integer;

/// ������ n ������������ �����
/// � ���������� ��������� ����� � ���� ������������������
function ReadSeqReal(n: integer): sequence of real;

/// ������ n ����� 
/// � ���������� ��������� ������ � ���� ������������������
function ReadSeqString(n: integer): sequence of string;

/// ������ ������ ������ ����� ����� � ��� ��������
/// � ���������� ��������� ����� � ���� ������������������
function ReadSeqInteger(): sequence of integer;

/// ������ ������ ������ ������������ ����� � ��� ��������
/// � ���������� ��������� ����� � ���� ������������������
function ReadSeqReal(): sequence of real;

/// ������ ������ ������ ����� � ��� ��������
/// � ���������� ��������� ����� � ���� ������������������
function ReadSeqString(): sequence of string;

/// ������ ������ ������ ����� ����� � ��� ��������
/// � ���������� ��������� ����� � ���� �������
function ReadArrInteger(): array of integer;

/// ������ ������ ������ ������������ ����� � ��� ��������
/// � ���������� ��������� ����� � ���� �������
function ReadArrReal(): array of real;

/// ������ ������ ������ ����� � ��� ��������
/// � ���������� ��������� ����� � ���� �������
function ReadArrString(): array of string;

/// ������ ����� ������� ������� m �� n �� �������
function  ReadMatrInteger(m,n: integer): array [,] of integer;

/// ������ ������� ������� � ����� ����� ������� ��������� �������� �� �������
function  ReadMatrInteger(): array [,] of integer;

/// ������ ������������ ������� ������� m �� n �� �������
function  ReadMatrReal(m,n: integer): array [,] of real;

/// ������ ������� ������� � ����� ������������ ������� ��������� �������� �� �������
function  ReadMatrReal(): array [,] of real;

/// ������ ������� �� ����� ������� m �� n �� �������
function  ReadMatrString(m,n: integer): array [,] of string;

/// ������ ������� ������� � ����� ��������� ������� ��������� �������� �� �������
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

// == ���������� 2016.07

procedure PrintMatr<T>(a: array[,] of T);

procedure PrintMatr<T>(a: array of array of T);

procedure PrintMatr<T>(a: List<List<T>>);

// == ����� ���������� 2016.07

// == ����� ���������� � ������ 4.14 ==

implementation

const
  NotSupportedReadTypeMessage = '���� ������ ���� {0} �� ��������������';
  NotSupportedWriteTypeMessage = '����� ������ ���� {0} �� ��������������';
  eMessage = '������� ��������� � ������� Node ����� ������ ��� ������ Dispose';

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
//                      ������� Get
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
  val := new System.Text.StringBuilder(200);//TODO ������ 200?
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
  //raise new NotSupportedException('������ � ������������� ����������� ��������� PT4 �� �������������� � ���� ������ �����������. ����������� ������ ����������� � ��������� ������');
  //result := new PT4Node(sNode, p);// fixme ����� ������ ��������� ����!
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

// == ������ 4.15. ���������� ==

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

// == ������ 4.15. ����� ���������� ==

// -----------------------------------------------------
//                      ��������� Put
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
//                  ��������� ����������
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

// == ������ 4.15. ���������� ==

procedure Print(s: string);
begin
  write(s);
end;

procedure Println(s: string);
begin
  write(s);
end;

// == ������ 4.15. ����� ���������� ==

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
  // == ������ ���������� � ������ 4.13 == 
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
  // == ����� ���������� � ������ 4.13 == 
end;

// == ������ 1.3. ���������� ==

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

// == ����� ���������� � ������ 1.3 ==

// == ������ 4.14. ���������� ==

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

/// ������� ������ � �������� ������������������
procedure WriteAll<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  PT4.Put(b.Length);
  foreach e : T in b do
    PT4.Put(e);
end;

/// ������� �������� ������������������
procedure Write<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  foreach e : T in b do
    PT4.Put(e);
end;

/// ������� �������� ������������� �������
procedure Write<T>(self: array of T); extensionmethod;
begin
  for var i:=0 to self.Length-1 do
    PT4.Put(self[i]);
end;

/// ������� �������� �������
procedure Write<T>(self: array [,] of T); extensionmethod;
begin
  for var i:=0 to self.GetLength(0)-1 do
  for var j:=0 to self.GetLength(1)-1 do
    PT4.Put(self[i,j]);
end;

// == ���������� 2016.07

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

/// ������� ������ � �������� ������������������
procedure PrintAll<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  PT4.Put(b.Length);
  foreach e : T in b do
    PT4.Put(e);
end;

/// ������� ������ � �������� ������������� �������
procedure PrintAll<T>(self: array of T); extensionmethod;
begin
  PT4.Put(self.Length);
  for var i:=0 to self.Length-1 do
    PT4.Put(self[i]);
end;

/// ������� ������ � �������� ������������� �������
procedure WriteAll<T>(self: array of T); extensionmethod;
begin
  PT4.Put(self.Length);
  for var i:=0 to self.Length-1 do
    PT4.Put(self[i]);
end;

/// ������� �������� ������������������
procedure Writeln<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  foreach e : T in b do
    PT4.Put(e);
end;

/// ������� �������� ������������� �������
procedure Writeln<T>(self: array of T); extensionmethod;
begin
  for var i:=0 to self.Length-1 do
    PT4.Put(self[i]);
end;

/// ������� �������� �������
procedure Writeln<T>(self: array [,] of T); extensionmethod;
begin
  for var i:=0 to self.GetLength(0)-1 do
  for var j:=0 to self.GetLength(1)-1 do
    PT4.Put(self[i,j]);
end;

// == ����� ���������� 2016.07


/// ������� � ������� ������� ���� ��������� 
/// ����������� cmt, ������ ������������������ � ��������, 
/// ���������� �� ��������� ������������������ 
/// � ������� ���������� ������-���������

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

/// ������� � ������� ������� ���� ��������� 
/// ������ ������������������ � ��������, 
/// ���������� �� ��������� ������������������ 
/// � ������� ���������� ������-���������
function Show<TSource>(Self: sequence of TSource; selector: System.Func<TSource, string>): sequence of TSource; extensionmethod;
begin
  result := self.Show('', selector); 
end;

/// ������� � ������� ������� ���� ��������� 
/// ����������� cmt, ������ ������������������ � �� ��������
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


/// ������� � ������� ������� ���� ��������� 
/// ������ ������������������ � �� ��������.
function Show<TSource>(Self: sequence of TSource): sequence of TSource; extensionmethod;
begin
  result := self.Show(''); 
end;

// == ����� ���������� � ������ 4.14 ==

initialization
  __InitModule;
finalization
  __FinalizeModule__;
end.