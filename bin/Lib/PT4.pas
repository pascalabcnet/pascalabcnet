/// Модуль электронного задачника Programming Taskbook 4
unit PT4;  

//------------------------------------------------------------------------------
// Модуль для подключения задачника Programming Taskbook
// Версия 4.23
// Copyright © 2006-2008 DarkStar, SSM
// Copyright © 2010 М.Э.Абрамян, дополнения к версии 1.3
// Copyright © 2014-2015 М.Э.Абрамян, дополнения к версии 4.13
// Copyright © 2015 М.Э.Абрамян, дополнения к версии 4.14
// Copyright © 2016 М.Э.Абрамян, дополнения к версии 4.15
// Copyright © 2017 М.Э.Абрамян, дополнения к версии 4.17
// Copyright © 2018 М.Э.Абрамян, дополнения к версии 4.18
// Copyright © 2019 М.Э.Абрамян, дополнения к версии 4.19
// Copyright © 2021 М.Э.Абрамян, дополнения к версии 4.21
// Copyright © 2023 М.Э.Абрамян, дополнения к версии 4.23
// Электронный задачник Programming Taskbook Copyright (c)М.Э.Абрамян, 1998-2023
//------------------------------------------------------------------------------

{$apptype windows}
//{$platformtarget x86}

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
(*    isAllocMem: boolean;
    isDisposed: boolean;
    x: InternalNode;
    addr: IntPtr;
    constructor Create(x: InternalNode; a: IntPtr);*)
    a: integer;
    disposed: boolean;
    procedure Init(Data: integer; Next,Prev,Left,Right,Parent: Node);
    constructor Create(addr, none: integer);
    static function NodeToInt(n: Node): integer;
    static function IntToNode(a:integer): Node;
//    procedure internalDispose(disposing: boolean);
    function getNodeProp(n: integer; name: string): Node;
    procedure setNodeProp(n: integer; name: string; value: Node);

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

var TaskName: string;

/// Вывести формулировку задания
procedure Task(name: string);


{
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
}

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

// == Версия 4.17. Дополнения ==

/// Возвращает кортеж из двух введенных значений типа integer
function ReadInteger2: (integer, integer);
/// Возвращает кортеж из двух введенных значений типа real
function ReadReal2: (real, real);
/// Возвращает кортеж из двух введенных значений типа char
function ReadChar2: (char, char);
/// Возвращает кортеж из двух введенных значений типа string
function ReadString2: (string, string);
/// Возвращает кортеж из двух введенных значений типа boolean
function ReadBoolean2: (boolean, boolean);
/// Возвращает кортеж из двух введенных значений типа Node
function ReadNode2: (Node, Node);

/// Возвращает кортеж из двух введенных значений типа integer
function ReadlnInteger2: (integer, integer);
/// Возвращает кортеж из двух введенных значений типа real
function ReadlnReal2: (real, real);
/// Возвращает кортеж из двух введенных значений типа char
function ReadlnChar2: (char, char);
/// Возвращает кортеж из двух введенных значений типа string
function ReadlnString2: (string, string);
/// Возвращает кортеж из двух введенных значений типа boolean
function ReadlnBoolean2: (boolean, boolean);
/// Возвращает кортеж из двух введенных значений типа Node
function ReadlnNode2: (Node, Node);

/// Возвращает кортеж из трех введенных значений типа integer
function ReadInteger3: (integer, integer, integer);
/// Возвращает кортеж из трех введенных значений типа real
function ReadReal3: (real, real, real);
/// Возвращает кортеж из трех введенных значений типа char
function ReadChar3: (char, char, char);
/// Возвращает кортеж из трех введенных значений типа string
function ReadString3: (string, string, string);
/// Возвращает кортеж из трех введенных значений типа boolean
function ReadBoolean3: (boolean, boolean, boolean);
/// Возвращает кортеж из трех введенных значений типа Node
function ReadNode3: (Node, Node, Node);

/// Возвращает кортеж из трех введенных значений типа integer
function ReadlnInteger3: (integer, integer, integer);
/// Возвращает кортеж из трех введенных значений типа real
function ReadlnReal3: (real, real, real);
/// Возвращает кортеж из трех введенных значений типа char
function ReadlnChar3: (char, char, char);
/// Возвращает кортеж из трех введенных значений типа string
function ReadlnString3: (string, string, string);
/// Возвращает кортеж из трех введенных значений типа boolean
function ReadlnBoolean3: (boolean, boolean, boolean);
/// Возвращает кортеж из трех введенных значений типа Node
function ReadlnNode3: (Node, Node, Node);

/// Возвращает кортеж из четырех введенных значений типа integer
function ReadInteger4: (integer, integer, integer, integer);
/// Возвращает кортеж из четырех введенных значений типа real
function ReadReal4: (real, real, real, real);
/// Возвращает кортеж из четырех введенных значений типа char
function ReadChar4: (char, char, char, char);
/// Возвращает кортеж из четырех введенных значений типа string
function ReadString4: (string, string, string, string);
/// Возвращает кортеж из четырех введенных значений типа boolean
function ReadBoolean4: (boolean, boolean, boolean, boolean);
/// Возвращает кортеж из четырех введенных значений типа Node
function ReadNode4: (Node, Node, Node, Node);

/// Возвращает кортеж из четырех введенных значений типа integer
function ReadlnInteger4: (integer, integer, integer, integer);
/// Возвращает кортеж из четырех введенных значений типа real
function ReadlnReal4: (real, real, real, real);
/// Возвращает кортеж из четырех введенных значений типа char
function ReadlnChar4: (char, char, char, char);
/// Возвращает кортеж из четырех введенных значений типа string
function ReadlnString4: (string, string, string, string);
/// Возвращает кортеж из четырех введенных значений типа boolean
function ReadlnBoolean4: (boolean, boolean, boolean, boolean);
/// Возвращает кортеж из четырех введенных значений типа Node
function ReadlnNode4: (Node, Node, Node, Node);


/// Возвращает кортеж из двух введенных значений типа integer.
/// Строковое приглашение prompt игнорируется
function ReadInteger2(prompt: string): (integer, integer);
/// Возвращает кортеж из двух введенных значений типа real.
/// Строковое приглашение prompt игнорируется
function ReadReal2(prompt: string): (real, real);
/// Возвращает кортеж из двух введенных значений типа char.
/// Строковое приглашение prompt игнорируется
function ReadChar2(prompt: string): (char, char);
/// Возвращает кортеж из двух введенных значений типа string.
/// Строковое приглашение prompt игнорируется
function ReadString2(prompt: string): (string, string);
/// Возвращает кортеж из двух введенных значений типа boolean.
/// Строковое приглашение prompt игнорируется
function ReadBoolean2(prompt: string): (boolean, boolean);
/// Возвращает кортеж из двух введенных значений типа Node.
/// Строковое приглашение prompt игнорируется
function ReadNode2(prompt: string): (Node, Node);

/// Возвращает кортеж из двух введенных значений типа integer.
/// Строковое приглашение prompt игнорируется
function ReadlnInteger2(prompt: string): (integer, integer);
/// Возвращает кортеж из двух введенных значений типа real.
/// Строковое приглашение prompt игнорируется
function ReadlnReal2(prompt: string): (real, real);
/// Возвращает кортеж из двух введенных значений типа char.
/// Строковое приглашение prompt игнорируется
function ReadlnChar2(prompt: string): (char, char);
/// Возвращает кортеж из двух введенных значений типа string.
/// Строковое приглашение prompt игнорируется
function ReadlnString2(prompt: string): (string, string);
/// Возвращает кортеж из двух введенных значений типа boolean.
/// Строковое приглашение prompt игнорируется
function ReadlnBoolean2(prompt: string): (boolean, boolean);
/// Возвращает кортеж из двух введенных значений типа Node.
/// Строковое приглашение prompt игнорируется
function ReadlnNode2(prompt: string): (Node, Node);

/// Возвращает кортеж из трех введенных значений типа integer.
/// Строковое приглашение prompt игнорируется
function ReadInteger3(prompt: string): (integer, integer, integer);
/// Возвращает кортеж из трех введенных значений типа real.
/// Строковое приглашение prompt игнорируется
function ReadReal3(prompt: string): (real, real, real);
/// Возвращает кортеж из трех введенных значений типа char.
/// Строковое приглашение prompt игнорируется
function ReadChar3(prompt: string): (char, char, char);
/// Возвращает кортеж из трех введенных значений типа string.
/// Строковое приглашение prompt игнорируется
function ReadString3(prompt: string): (string, string, string);
/// Возвращает кортеж из трех введенных значений типа boolean.
/// Строковое приглашение prompt игнорируется
function ReadBoolean3(prompt: string): (boolean, boolean, boolean);
/// Возвращает кортеж из трех введенных значений типа Node.
/// Строковое приглашение prompt игнорируется
function ReadNode3(prompt: string): (Node, Node, Node);

/// Возвращает кортеж из трех введенных значений типа integer.
/// Строковое приглашение prompt игнорируется
function ReadlnInteger3(prompt: string): (integer, integer, integer);
/// Возвращает кортеж из трех введенных значений типа real.
/// Строковое приглашение prompt игнорируется
function ReadlnReal3(prompt: string): (real, real, real);
/// Возвращает кортеж из трех введенных значений типа char.
/// Строковое приглашение prompt игнорируется
function ReadlnChar3(prompt: string): (char, char, char);
/// Возвращает кортеж из трех введенных значений типа string.
/// Строковое приглашение prompt игнорируется
function ReadlnString3(prompt: string): (string, string, string);
/// Возвращает кортеж из трех введенных значений типа boolean.
/// Строковое приглашение prompt игнорируется
function ReadlnBoolean3(prompt: string): (boolean, boolean, boolean);
/// Возвращает кортеж из трех введенных значений типа Node.
/// Строковое приглашение prompt игнорируется
function ReadlnNode3(prompt: string): (Node, Node, Node);

/// Возвращает кортеж из четырех введенных значений типа integer.
/// Строковое приглашение prompt игнорируется
function ReadInteger4(prompt: string): (integer, integer, integer, integer);
/// Возвращает кортеж из четырех введенных значений типа real.
/// Строковое приглашение prompt игнорируется
function ReadReal4(prompt: string): (real, real, real, real);
/// Возвращает кортеж из четырех введенных значений типа char.
/// Строковое приглашение prompt игнорируется
function ReadChar4(prompt: string): (char, char, char, char);
/// Возвращает кортеж из четырех введенных значений типа string.
/// Строковое приглашение prompt игнорируется
function ReadString4(prompt: string): (string, string, string, string);
/// Возвращает кортеж из четырех введенных значений типа boolean.
/// Строковое приглашение prompt игнорируется
function ReadBoolean4(prompt: string): (boolean, boolean, boolean, boolean);
/// Возвращает кортеж из четырех введенных значений типа Node.
/// Строковое приглашение prompt игнорируется
function ReadNode4(prompt: string): (Node, Node, Node, Node);

/// Возвращает кортеж из четырех введенных значений типа integer.
/// Строковое приглашение prompt игнорируется
function ReadlnInteger4(prompt: string): (integer, integer, integer, integer);
/// Возвращает кортеж из четырех введенных значений типа real.
/// Строковое приглашение prompt игнорируется
function ReadlnReal4(prompt: string): (real, real, real, real);
/// Возвращает кортеж из четырех введенных значений типа char.
/// Строковое приглашение prompt игнорируется
function ReadlnChar4(prompt: string): (char, char, char, char);
/// Возвращает кортеж из четырех введенных значений типа string.
/// Строковое приглашение prompt игнорируется
function ReadlnString4(prompt: string): (string, string, string, string);
/// Возвращает кортеж из четырех введенных значений типа boolean.
/// Строковое приглашение prompt игнорируется
function ReadlnBoolean4(prompt: string): (boolean, boolean, boolean, boolean);
/// Возвращает кортеж из четырех введенных значений типа Node.
/// Строковое приглашение prompt игнорируется
function ReadlnNode4(prompt: string): (Node, Node, Node, Node);

// == Версия 4.17. Конец дополнений ==


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

procedure Print(s: char);
procedure Println(s: char);

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

// == Версия 4.18. Изменения ==

(*

/// Выводит число A в разделе отладки окна задачника
procedure Show(A: integer);

/// Выводит число A в разделе отладки окна задачника
procedure Show(A: real);

/// Выводит строку S в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку
procedure ShowLine(S: string);

/// Выводит число A в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку
procedure ShowLine(A: integer);

/// Выводит число A в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку
procedure ShowLine(A: real);

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

*)

/// Выводит строку S в разделе отладки окна задачника
procedure Show(S: string);

/// Выводит набор данных в разделе отладки окна задачника.
/// Вещественные числа выводятся в формате, настроенном
/// с помощью функции SetPrecision (по умолчанию 2 дробных знака).
/// Если аргументом является последовательность, то после вывода
/// ее элементов выполняется автоматический переход на новую строку.
procedure Show(params args: array of object);

/// Выполняет переход на новую экранную строку
/// в разделе отладки окна задачника
procedure ShowLine;

/// Выводит набор данных в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку.
/// Вещественные числа выводятся в формате, настроенном
/// с помощью функции SetPrecision (по умолчанию 2 дробных знака).
/// Если аргументом является последовательность, то после вывода
/// ее элементов выполняется автоматический переход на новую строку.
procedure ShowLine(params args: array of object);


/// Задает ширину W области вывода для числовых и строковых данных
/// в разделе отладки. Влияет на последующие вызовы функций
/// Show и ShowLine. 
procedure SetWidth(W: Integer);

// == Версия 4.18. Конец изменений ==

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

/// Вводит целочисленную матрицу размера m на n по строкам
function  ReadMatrInteger(m,n: integer): array [,] of integer;

/// Вводит размеры матрицы и затем целочисленную матрицу указанных размеров по строкам
function  ReadMatrInteger(): array [,] of integer;

/// Вводит вещественную матрицу размера m на n по строкам
function  ReadMatrReal(m,n: integer): array [,] of real;

/// Вводит размеры матрицы и затем вещественную матрицу указанных размеров по строкам
function  ReadMatrReal(): array [,] of real;

/// Вводит строковую матрицу размера m на n по строкам
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

// == Изменения в версии 4.18
/// Вводит квадратную целочисленную матрицу порядка m по строкам
function  ReadMatrInteger(m: integer): array [,] of integer;
/// Вводит квадратную вещественную матрицу порядка m по строкам
function  ReadMatrReal(m: integer): array [,] of real;
/// Вводит квадратную строковую матрицу порядка m по строкам
function  ReadMatrString(m: integer): array [,] of string;

/// Вводит размер набора целых чисел и его элементы
/// и возвращает введенный набор в виде списка List
function ReadListInteger(): List<integer>;
/// Вводит размер набора вещественных чисел и его элементы
/// и возвращает введенный набор в виде списка List
function ReadListReal(): List<real>;
/// Вводит размер набора строк и его элементы
/// и возвращает введенный набор в виде списка List
function ReadListString(): List<string>;
/// Вводит n целых чисел
/// и возвращает введенные числа в виде списка List
function ReadListInteger(n: integer): List<integer>;
/// Вводит n вещественных чисел
/// и возвращает введенные числа в виде списка List
function ReadListReal(n: integer): List<real>;
/// Вводит n строк 
/// и возвращает введенные строки в виде списка List
function ReadListString(n: integer): List<string>;

/// Вводит целочисленную матрицу размера m на n по строкам
/// и возвращает ее в виде массива массивов
function ReadArrArrInteger(m,n: integer): array of array of integer;
/// Вводит размеры матрицы и затем целочисленную матрицу указанных размеров по строкам
/// и возвращает ее в виде массива массивов
function ReadArrArrInteger(): array of array of integer;
/// Вводит вещественную матрицу размера m на n по строкам
/// и возвращает ее в виде массива массивов
function ReadArrArrReal(m,n: integer): array of array of real;
/// Вводит квадратную целочисленную матрицу порядка m по строкам
/// и возвращает ее в виде массива массивов
function ReadArrArrInteger(m: integer): array of array of integer;
/// Вводит квадратную вещественную матрицу порядка m по строкам
/// и возвращает ее в виде массива массивов
function ReadArrArrReal(m: integer): array of array of real;
/// Вводит квадратную строковую матрицу порядка m по строкам
/// и возвращает ее в виде массива массивов
function ReadArrArrString(m: integer): array of array of string;
/// Вводит размеры матрицы и затем вещественную матрицу указанных размеров по строкам
/// и возвращает ее в виде массива массивов
function ReadArrArrReal(): array of array of real;
/// Вводит строковую матрицу размера m на n по строкам
/// и возвращает ее в виде массива массивов
function ReadArrArrString(m,n: integer): array of array of string;
/// Вводит размеры матрицы и затем строковую матрицу указанных размеров по строкам
/// и возвращает ее в виде массива массивов
function ReadArrArrString(): array of array of string;

/// Вводит целочисленную матрицу размера m на n по строкам
/// и возвращает ее в виде списка списков
function ReadListListInteger(m,n: integer): List<List<integer>>;
/// Вводит квадратную целочисленную матрицу порядка m по строкам
/// и возвращает ее в виде списка списков
function ReadListListInteger(m: integer): List<List<integer>>;
/// Вводит размеры матрицы и затем целочисленную матрицу указанных размеров по строкам
/// и возвращает ее в виде списка списков
function ReadListListInteger(): List<List<integer>>;
/// Вводит вещественную матрицу размера m на n по строкам
/// и возвращает ее в виде списка списков
function ReadListListReal(m,n: integer): List<List<real>>;
/// Вводит квадратную вещественную матрицу порядка m по строкам
/// и возвращает ее в виде списка списков
function ReadListListReal(m: integer): List<List<real>>;
/// Вводит размеры матрицы и затем вещественную матрицу указанных размеров по строкам
/// и возвращает ее в виде списка списков
function ReadListListReal(): List<List<real>>;
/// Вводит строковую матрицу размера m на n по строкам
/// и возвращает ее в виде списка списков
function ReadListListString(m,n: integer): List<List<string>>;
/// Вводит квадратную строковую матрицу порядка m по строкам
/// и возвращает ее в виде списка списков
function ReadListListString(m: integer): List<List<string>>;
/// Вводит размеры матрицы и затем строковую матрицу указанных размеров по строкам
/// и возвращает ее в виде списка списков
function ReadListListString(): List<List<string>>;

(*
procedure WriteMatr<T>(a: array[,] of T);

procedure WriteMatr<T>(a: array of array of T);

procedure WriteMatr<T>(a: List<List<T>>);

// == Дополнения 2016.07
procedure PrintMatr<T>(a: array[,] of T);

procedure PrintMatr<T>(a: array of array of T);

procedure PrintMatr<T>(a: List<List<T>>);
// == Конец дополнений 2016.07
*)

// == Конец изменений в версии 4.18

// == Конец дополнений к версии 4.14 ==

function GetSolutionInfo: string;

implementation

// == Версия 4.18. Дополнения ==
uses ABCDatabases, System.Text;
// == Версия 4.18. Конец дополнений ==

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

procedure StartPT(options: integer);    external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'startpt';
procedure FreePT;                                       external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'freept';
procedure _CheckPT(InfoS: array of byte; var InfoT: integer);external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'checkpt'; //2021.05
procedure _RaisePT(s1,s2: array of byte);       external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'raisept';
procedure _Task(name: string);           external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'task';
procedure GetR(var param: real);                external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'getr';
procedure PutR(param: real);                            external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'putr';
procedure GetN(var param: integer);     external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'getn';
procedure PutN(param: integer);         external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'putn';
procedure GetC(var param: char);                external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'getc';
procedure PutC(param: char);                    external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'putc';
procedure _GetS(param: array of byte);external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'gets';
procedure _PutS(param: array of byte);          external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'puts';
procedure _GetB(var param: integer);    external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'getb';
procedure _PutB(param: integer);                external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'putb';
procedure _GetP(var param: IntPtr);     external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'getp';
procedure _PutP(param: IntPtr); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'putp';
procedure _PutNode(param: integer); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'nodeputp';
procedure DisposeP(sNode: IntPtr);      external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'disposep';
procedure _NodeNew(var n: integer; data, next, prev, left, right, parent: integer); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'nodenew';
procedure _NodeGetP(var n: integer); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'nodegetp';
procedure _NodePutP(n: integer); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'nodeputp';
procedure _NodeGetF(n, fn: integer; var f, err: integer); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'nodegetf';
procedure _NodeSetF(n, fn, f: integer; var err: integer); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'nodesetf';
procedure _NodeDispose(n: integer; var err: integer); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'nodedispose';
function FinishPT(): integer;      external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'finishpt';  // == 4.13 ==
procedure _GetSolutionInfo(info: array of byte);external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'getsolutioninfo';

//2023.03>

function ToBytes(s: string): array of byte;
begin
  var utf8 := Encoding.Unicode;
  var ansi := Encoding.GetEncoding(1251);
  result := Encoding.Convert(utf8, ansi, utf8.GetBytes(s));
end;

function FromBytes(a: array of byte): string;
begin
  var utf8 := Encoding.Unicode;
  var ansi := Encoding.GetEncoding(1251);
  var res := Encoding.Convert(ansi, utf8, a);
  result := utf8.GetString(res);
  result := result.substring(0, result.IndexOf(#0));
end;

//2023.03<


//2021.05
procedure SetLibLoader(p: IntPtr); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'setlibloader';  // == 4.21 ==
var activate, inittaskgroup: System.Reflection.MethodInfo;

function GetTopic(LibName: string): string;
begin
  var s := System.IO.Path.GetFileNameWithoutExtension(LibName).Substring(4);
  var p := s.LastIndexOf('_');
  if p > -1 then
    s := s.Substring(0, p);
  result := s;
end;

function LoadNETLib(LibName: string; Step: integer): integer;
begin
  if Step = 1 then
  begin
    var lib: System.Reflection.Assembly := nil;
    var libClass: System.Type := nil;      
    activate := nil;
    inittaskgroup := nil;
    try
      lib := System.Reflection.Assembly.LoadFrom(LibName+'.dll');
      LibName := GetTopic(LibName);
      libClass := lib.GetType('xPT4'+LibName + '.xPT4' + LibName);
      if libClass = nil then
      begin  
        LibName := LibName.ToUpper();
        foreach var t in lib.GetTypes do
        begin
          if t.FullName.ToUpper = 'XPT4'+LibName + '.XPT4' + LibName then
          begin  
            libClass := t;
            break;
          end;
        end;
      end;                      
      if libClass = nil then
      begin  
        result := 1;
        exit;
      end;
      inittaskgroup := libClass.GetMethod('inittaskgroup');
      activate := libClass.GetMethod('activate');
      if (inittaskgroup <> nil) and (activate = nil) then
        result := 2 // dll and inittaskgroup found, activate not found
      else if (inittaskgroup = nil) or (activate = nil) then
        result := 3 // dll found, activate or inittaskgroup not found
      else
        result := 0; // 1st step OK
    except
      result := 4;
    end;
  end
  else  
  begin  
    if (activate <> nil) and (inittaskgroup <> nil) then
      try
        activate.Invoke(nil, Arr(LibName as Object));
        inittaskgroup.Invoke(nil, nil);
        result := 0; // 2nd step OK
      except  
        result := 2; // error while run methods
      end
    else
      result := 1;  // empty methods
  end;  
end;    

var PLoadNETLib := LoadNETLib;

procedure Task(name: string);
begin
  SetLibLoader(Marshal.GetFunctionPointerForDelegate(PLoadNETLib));
  _Task(name);
  TaskName := name;
end;

function CheckPT(var res: integer): string;
begin
  var InfoS := new byte[200];
  _CheckPT(InfoS, res);
  result := FromBytes(InfoS);
end;


procedure Dispose(p: pointer);
begin
  DisposeP(IntPtr(p));
end;


// -----------------------------------------------------
//                         Node
// -----------------------------------------------------
var nodes := new Dictionary<integer, Node>;

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
  if disposed then
    exit;
  var err := 0;
  _NodeDispose(a, err);
  disposed := true;
end;

constructor Node.Create(addr, none: integer);
begin
  a := addr;
  disposed := false;
  nodes[a] := Self;
end;  
  
class function Node.NodeToInt(n: Node): integer;
begin
  if n = nil then
    result := 0
  else
    result := n.a;
end;

class function Node.IntToNode(a: integer): Node;
begin
  if a = 0 then
    result := nil
  else if nodes.ContainsKey(a) then
    result := nodes[a]
  else
    result := new Node(a, 0);
end;

procedure Node.Init(Data: integer; Next,Prev,Left,Right,Parent: Node);
begin
  var n: integer;
  _NodeNew(n, data, NodeToInt(next), NodeToInt(prev),
     NodeToInt(left), NodeToInt(right), NodeToInt(parent));
  a := n;
  disposed := false;
  nodes[a] := Self;
end;


function Node.getNodeProp(n: integer; name: string): Node;
begin
  if disposed then
    raise new ObjectDisposedException('cannot access a disposed Node');
  var val, err: integer;;
  _NodeGetF(a, n, val, err);
  if err <> 0 then
    raise new ArgumentException('cannot get the ' + name + ' property');
  result := IntToNode(val);
end;        

procedure Node.setNodeProp(n: integer; name: string; value: Node);
begin
  if disposed then
    raise new ObjectDisposedException('cannot access a disposed Node');
  var err: integer;;
  _NodeSetF(a, n, NodeToInt(value), err);
  if err <> 0 then
    raise new ArgumentException('cannot set the ' + name + ' property');
end;

procedure Node.setNext(value: Node) := SetNodeProp(1, 'Next', value);
function Node.getNext := GetNodeProp(1, 'Next');

procedure Node.setPrev(value: Node) := SetNodeProp(2, 'Prev', value);
function Node.getPrev := GetNodeProp(2, 'Prev');

procedure Node.setLeft(value: Node) := SetNodeProp(3, 'Left', value);
function Node.getLeft := GetNodeProp(3, 'Left');

procedure Node.setRight(value: Node) := SetNodeProp(4, 'Right', value);
function Node.getRight := GetNodeProp(4, 'Right');

procedure Node.setParent(value: Node) := SetNodeProp(5, 'Parent', value);
function Node.getParent := GetNodeProp(5, 'Parent');

procedure Node.setData(value: integer);
begin
  if disposed then
    raise new ObjectDisposedException('cannot access a disposed Node');
  var err: integer;
  _NodeSetF(a, 0, value, err);
  if err <> 0 then
    raise new ArgumentException('cannot set the Data property');
end;

function Node.getData: integer;
begin
  if disposed then
    raise new ObjectDisposedException('cannot access a disposed Node');
  var val, err: integer;  
  _NodeGetF(a, 0, val, err);
  if err <> 0 then
    raise new ArgumentException('cannot get the Data property');
  result := val;
end;

(*procedure Node.internalDispose(disposing: boolean);
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
*)










































































































































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
begin
  var val := new byte[200];
  _gets(val);
  result := FromBytes(val);
end;

procedure PutS(param: string);
begin
  _puts(ToBytes(param));
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
begin
  var n: integer;
  _NodeGetP(n);
  result := Node.IntToNode(n);
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

// == Версия 4.17. Дополнения ==

function ReadInteger2: (integer, integer);
begin
  Result := (GetInt, GetInt);
end;

function ReadReal2: (real, real);
begin
  Result := (GetReal, GetReal);
end;

function ReadChar2: (char, char);
begin
  Result := (GetChar, GetChar);
end;

function ReadString2: (string, string);
begin
  Result := (GetString, GetString);
end;

function ReadBoolean2: (boolean, boolean);
begin
  Result := (GetBool, GetBool);
end;

function ReadNode2: (Node, Node);
begin
  Result := (GetNode, GetNode);
end;

function ReadlnInteger2: (integer, integer);
begin
  Result := (GetInt, GetInt);
end;

function ReadlnReal2: (real, real);
begin
  Result := (GetReal, GetReal);
end;

function ReadlnChar2: (char, char);
begin
  Result := (GetChar, GetChar);
end;

function ReadlnString2: (string, string);
begin
  Result := (GetString, GetString);
end;

function ReadlnBoolean2: (boolean, boolean);
begin
  Result := (GetBool, GetBool);
end;

function ReadlnNode2: (Node, Node);
begin
  Result := (GetNode, GetNode);
end;

function ReadInteger2(prompt: string): (integer, integer);
begin
  Result := (GetInt, GetInt);
end;

function ReadReal2(prompt: string): (real, real);
begin
  Result := (GetReal, GetReal);
end;

function ReadChar2(prompt: string): (char, char);
begin
  Result := (GetChar, GetChar);
end;

function ReadString2(prompt: string): (string, string);
begin
  Result := (GetString, GetString);
end;

function ReadBoolean2(prompt: string): (boolean, boolean);
begin
  Result := (GetBool, GetBool);
end;

function ReadNode2(prompt: string): (Node, Node);
begin
  Result := (GetNode, GetNode);
end;

function ReadlnInteger2(prompt: string): (integer, integer);
begin
  Result := (GetInt, GetInt);
end;

function ReadlnReal2(prompt: string): (real, real);
begin
  Result := (GetReal, GetReal);
end;

function ReadlnChar2(prompt: string): (char, char);
begin
  Result := (GetChar, GetChar);
end;

function ReadlnString2(prompt: string): (string, string);
begin
  Result := (GetString, GetString);
end;

function ReadlnBoolean2(prompt: string): (boolean, boolean);
begin
  Result := (GetBool, GetBool);
end;

function ReadlnNode2(prompt: string): (Node, Node);
begin
  Result := (GetNode, GetNode);
end;

function ReadInteger3: (integer, integer, integer);
begin
  Result := (GetInt, GetInt, GetInt);
end;

function ReadReal3: (real, real, real);
begin
  Result := (GetReal, GetReal, GetReal);
end;

function ReadChar3: (char, char, char);
begin
  Result := (GetChar, GetChar, GetChar);
end;

function ReadString3: (string, string, string);
begin
  Result := (GetString, GetString, GetString);
end;

function ReadBoolean3: (boolean, boolean, boolean);
begin
  Result := (GetBool, GetBool, GetBool);
end;

function ReadNode3: (Node, Node, Node);
begin
  Result := (GetNode, GetNode, GetNode);
end;

function ReadlnInteger3: (integer, integer, integer);
begin
  Result := (GetInt, GetInt, GetInt);
end;

function ReadlnReal3: (real, real, real);
begin
  Result := (GetReal, GetReal, GetReal);
end;

function ReadlnChar3: (char, char, char);
begin
  Result := (GetChar, GetChar, GetChar);
end;

function ReadlnString3: (string, string, string);
begin
  Result := (GetString, GetString, GetString);
end;

function ReadlnBoolean3: (boolean, boolean, boolean);
begin
  Result := (GetBool, GetBool, GetBool);
end;

function ReadlnNode3: (Node, Node, Node);
begin
  Result := (GetNode, GetNode, GetNode);
end;

function ReadInteger3(prompt: string): (integer, integer, integer);
begin
  Result := (GetInt, GetInt, GetInt);
end;

function ReadReal3(prompt: string): (real, real, real);
begin
  Result := (GetReal, GetReal, GetReal);
end;

function ReadChar3(prompt: string): (char, char, char);
begin
  Result := (GetChar, GetChar, GetChar);
end;

function ReadString3(prompt: string): (string, string, string);
begin
  Result := (GetString, GetString, GetString);
end;

function ReadBoolean3(prompt: string): (boolean, boolean, boolean);
begin
  Result := (GetBool, GetBool, GetBool);
end;

function ReadNode3(prompt: string): (Node, Node, Node);
begin
  Result := (GetNode, GetNode, GetNode);
end;

function ReadlnInteger3(prompt: string): (integer, integer, integer);
begin
  Result := (GetInt, GetInt, GetInt);
end;

function ReadlnReal3(prompt: string): (real, real, real);
begin
  Result := (GetReal, GetReal, GetReal);
end;

function ReadlnChar3(prompt: string): (char, char, char);
begin
  Result := (GetChar, GetChar, GetChar);
end;

function ReadlnString3(prompt: string): (string, string, string);
begin
  Result := (GetString, GetString, GetString);
end;

function ReadlnBoolean3(prompt: string): (boolean, boolean, boolean);
begin
  Result := (GetBool, GetBool, GetBool);
end;

function ReadlnNode3(prompt: string): (Node, Node, Node);
begin
  Result := (GetNode, GetNode, GetNode);
end;


function ReadInteger4: (integer, integer, integer, integer);
begin
  Result := (GetInt, GetInt, GetInt, GetInt);
end;

function ReadReal4: (real, real, real, real);
begin
  Result := (GetReal, GetReal, GetReal, GetReal);
end;

function ReadChar4: (char, char, char, char);
begin
  Result := (GetChar, GetChar, GetChar, GetChar);
end;

function ReadString4: (string, string, string, string);
begin
  Result := (GetString, GetString, GetString, GetString);
end;

function ReadBoolean4: (boolean, boolean, boolean, boolean);
begin
  Result := (GetBool, GetBool, GetBool, GetBool);
end;

function ReadNode4: (Node, Node, Node, Node);
begin
  Result := (GetNode, GetNode, GetNode, GetNode);
end;

function ReadlnInteger4: (integer, integer, integer, integer);
begin
  Result := (GetInt, GetInt, GetInt, GetInt);
end;

function ReadlnReal4: (real, real, real, real);
begin
  Result := (GetReal, GetReal, GetReal, GetReal);
end;

function ReadlnChar4: (char, char, char, char);
begin
  Result := (GetChar, GetChar, GetChar, GetChar);
end;

function ReadlnString4: (string, string, string, string);
begin
  Result := (GetString, GetString, GetString, GetString);
end;

function ReadlnBoolean4: (boolean, boolean, boolean, boolean);
begin
  Result := (GetBool, GetBool, GetBool, GetBool);
end;

function ReadlnNode4: (Node, Node, Node, Node);
begin
  Result := (GetNode, GetNode, GetNode, GetNode);
end;

function ReadInteger4(prompt: string): (integer, integer, integer, integer);
begin
  Result := (GetInt, GetInt, GetInt, GetInt);
end;

function ReadReal4(prompt: string): (real, real, real, real);
begin
  Result := (GetReal, GetReal, GetReal, GetReal);
end;

function ReadChar4(prompt: string): (char, char, char, char);
begin
  Result := (GetChar, GetChar, GetChar, GetChar);
end;

function ReadString4(prompt: string): (string, string, string, string);
begin
  Result := (GetString, GetString, GetString, GetString);
end;

function ReadBoolean4(prompt: string): (boolean, boolean, boolean, boolean);
begin
  Result := (GetBool, GetBool, GetBool, GetBool);
end;

function ReadNode4(prompt: string): (Node, Node, Node, Node);
begin
  Result := (GetNode, GetNode, GetNode, GetNode);
end;

function ReadlnInteger4(prompt: string): (integer, integer, integer, integer);
begin
  Result := (GetInt, GetInt, GetInt, GetInt);
end;

function ReadlnReal4(prompt: string): (real, real, real, real);
begin
  Result := (GetReal, GetReal, GetReal, GetReal);
end;

function ReadlnChar4(prompt: string): (char, char, char, char);
begin
  Result := (GetChar, GetChar, GetChar, GetChar);
end;

function ReadlnString4(prompt: string): (string, string, string, string);
begin
  Result := (GetString, GetString, GetString, GetString);
end;

function ReadlnBoolean4(prompt: string): (boolean, boolean, boolean, boolean);
begin
  Result := (GetBool, GetBool, GetBool, GetBool);
end;

function ReadlnNode4(prompt: string): (Node, Node, Node, Node);
begin
  Result := (GetNode, GetNode, GetNode, GetNode);
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
  if param = nil then
    _PutNode(0)
  else
    _PutNode(param.a);
end;

// == Версия 4.17. Конец дополнений ==


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
begin
  if val = nil then
    _PutNode(0)
  else
    _PutNode(val.a);
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
procedure RaisePT(s1, s2: string);
begin
  _RaisePT(ToBytes(s1), ToBytes(s2));
end;

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

procedure Print(s: char);
begin
  write(s);
end;

procedure Println(s: char);
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
// == Версия 4.17. Дополнения ==
    else if args[i].GetType.FullName.StartsWith('System.Tuple') then
       foreach var e in args[i].GetType.GetProperties do
          InternalWrite(Arr(e.GetValue(args[i],nil)))
    else if args[i] is IEnumerable then
    begin
       var e := (args[i] as IEnumerable).GetEnumerator;
       while e.MoveNext do
          InternalWrite(Arr(e.Current));
    end      
// == Версия 4.17. Конец дополнений ==
// == Версия 4.18. Дополнения ==
    else 
    begin
      var res := PrintAttributeString(args[i]);
      if res <> nil then
          PutString(res)
      else GenerateNotSupportedWriteTypeException(args[i].GetType.ToString);
    end
// == Версия 4.18. Конец дополнений ==
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
  PrintMatrixWithFormat := False;
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

var finalized := False;

procedure __FinalizeModule__;
begin
  if finalized then
    exit;
  finalized := True;
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

var 
  _Width: integer := 0;


procedure _show(s: array of byte); external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'show';


procedure ShowStr(s: string);
begin
  var utf8 := new UTF8Encoding();
  var byteCount := utf8.GetByteCount(s);
  var bytes := new Byte[byteCount];
  var bytesEncodedCount := utf8.GetBytes(s, 0, s.Length, bytes, 0);
  _show(bytes);


//  _show(ToBytes(s));
end;

procedure Show(s: string);
begin
  ShowStr(s.PadRight(_Width));
end;

procedure Show(s: char);
begin
  ShowStr(s.ToString);
end;

procedure Show(A: Real);
begin
  ShowStr((D > 0 ? string.Format('{0,'+_Width+':f'+D+'}', a)
              : (D = 0 ? string.Format('{0,'+_Width+':e}', a)
              : string.Format('{0,'+_Width+':e'+(-D)+'}', a))).Replace(',','.'));
end;

procedure Show(A: Integer);
begin
  ShowStr(A.ToString.PadLeft(_Width));
end;

procedure ShowLine;
begin
  Show(#13);
end;

procedure ShowLine(S: string);
begin
  Show(S);
  ShowLine;
end;

procedure ShowLine(A: Real);
begin
  Show(A);
  ShowLine;
end;

procedure ShowLine(A: Integer);
begin
  Show(A);
  ShowLine;
end;

(*
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
  Show(A);
  Show(#13);
end;
*)

// == Версия 4.18. Дополнения ==

var LineBreak := false;

procedure ShowArray(a: System.Array; indexes: array of integer; i: integer); forward;
var spaces := 0;

function GetNullBasedArray(arr: object): System.Array;
var
  fi: System.Reflection.FieldInfo;
begin
  fi := arr.GetType.GetField('NullBasedArray');
  if fi <> nil then
    Result := System.Array(fi.GetValue(arr))
  else
    Result := nil;
end;

procedure Show(params args: array of object);
begin
  var b := false;
  for var i:=0 to args.length-1 do
  begin
    if args[i] = nil then begin Show('nil'); LineBreak := false end else
    if args[i] is integer then begin Show(integer(args[i])); LineBreak := false end else
    if args[i] is shortint then begin ShowStr(shortint(args[i]).ToString.PadLeft(_Width)); LineBreak := false end else
    if args[i] is smallint then begin ShowStr(smallint(args[i]).ToString.PadLeft(_Width)); LineBreak := false end else
    if args[i] is int64 then begin ShowStr(int64(args[i]).ToString.PadLeft(_Width)); LineBreak := false end else
    if args[i] is byte then begin ShowStr(byte(args[i]).ToString.PadLeft(_Width)); LineBreak := false end else
    if args[i] is word then begin ShowStr(word(args[i]).ToString.PadLeft(_Width)); LineBreak := false end else
    if args[i] is longword then begin ShowStr(longword(args[i]).ToString.PadLeft(_Width)); LineBreak := false end else
    if args[i] is uint64 then begin ShowStr(uint64(args[i]).ToString.PadLeft(_Width)); LineBreak := false end else
    if args[i] is real then begin Show(real(args[i])); LineBreak := false end else
    if args[i] is char then begin Show(char(args[i])); LineBreak := false end else
    if args[i] is string then begin Show(string(args[i])); LineBreak := false end else
    if args[i] is boolean then begin Show(boolean(args[i]).ToString); LineBreak := false end else
    if args[i] is Node then begin Show('Node'); LineBreak := false end else
    if args[i] is PointerOutput then 
        begin
          LineBreak := false;
          var ip := IntPtr(PointerOutput(args[i]).p);
          ShowStr(ip.ToString.PadLeft(_Width));
        end  
    else if args[i].GetType.FullName.StartsWith('System.Tuple') then
    begin
       LineBreak := false;
       Show('(');
       foreach var e in args[i].GetType.GetProperties do
       begin
          if b then
            Show(',')
          else
            b := true;
          Show(e.GetValue(args[i],nil));
       end;   
       Show(')');
       b := false;
    end      
    else if args[i].GetType.Name.StartsWith('KeyValuePair') then
    begin
       LineBreak := false;
       Show('(');
       foreach var e in args[i].GetType.GetProperties do
       begin
          if b then
            Show(':')
          else
            b := true;
          Show(e.GetValue(args[i],nil));
       end;   
       Show(')');
       b := false;
    end     
    else if args[i] is System.Array then
    begin
      var a := args[i] as System.Array;  
      ShowArray(a, new integer[a.Rank], 0); 
    end
    else if args[i] is IEnumerable then
    begin
       var isdictorset := args[i].GetType.Name.Equals('Dictionary`2') or args[i].GetType.Name.Equals('SortedDictionary`2') 
         or (args[i].GetType = typeof(TypedSet)) or args[i].GetType.Name.Equals('HashSet`1') 
         or args[i].GetType.Name.Equals('SortedSet`1');
       var (lbr, rbr) := isdictorset ? ('{', '}') : ('[', ']');
       if LineBreak then
         loop spaces do
           Show(' ');
       Show(lbr);
       LineBreak := false;
       var e := (args[i] as IEnumerable).GetEnumerator;
       while e.MoveNext do
       begin
          if b then
          begin
            if not LineBreak then
              Show(',');
          end  
          else
            b := true;
          spaces += 1;  
          Show(e.Current);
          spaces -= 1;  
       end;   
       if LineBreak then
         loop spaces do
           Show(' ');
       Show(rbr);
       Show(#13);
       LineBreak := true;
       b := false;
    end      
    else 
    begin
      var nba := GetNullBasedArray(args[i]);
      if nba <> nil then
        Show(nba)
      else
      begin
      var res := PrintAttributeString(args[i]);
      if res <> nil then
          Show(res)
      else 
          Show(args[i].ToString);
      end;    
    end;
    end;
end;

procedure ShowArray(a: System.Array; indexes: array of integer; i: integer);
begin
  if i = a.Rank then
    Show(a.GetValue(indexes))
  else
  begin
    if LineBreak then
      loop spaces do
        Show(' ');
    Show('[');
    LineBreak := false;
    for var k := 0 to a.GetLength(i) - 1 do
    begin
      indexes[i] := k;
      spaces += 1;
      ShowArray(a, indexes, i + 1);
      spaces -= 1;
      if k < a.GetLength(i) - 1 then
        if not LineBreak then
        Show({LineBreak ? ' ' : }',');            
    end;
    if LineBreak then
      loop spaces do
        Show(' ');
    Show(']');
    Show(#13);
    LineBreak := true;
  end;
end;


procedure ShowLine(params args: array of object);
begin
  LineBreak := false;
  Show(args);
  if not LineBreak then
    ShowLine;
end;

procedure SetWidth(W: Integer);
begin
  if W >= 0 then
    _Width := W;
end;

// == Конец дополнений к версии 4.18 ==

procedure HideTask; external '%PABCSYSTEM%\PT4\xPT4PABC2.dll' name 'hidetask';

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

// == Версия 4.18. Дополнения ==

function  ReadMatrInteger(m: integer): array [,] of integer;
begin
  result := ReadMatrInteger(m,m);
end;

function  ReadMatrReal(m: integer): array [,] of real;
begin
  result := ReadMatrReal(m,m);
end;

function  ReadMatrString(m: integer): array [,] of string;
begin
  result := ReadMatrString(m,m);
end;

function  ReadArrArrInteger(m, n: integer): array of array of integer;
begin
  SetLength(result, m);
  for var i := 0 to m - 1 do
    result[i] := ReadArrInteger(n);
end;

function  ReadArrArrReal(m, n: integer): array of array of real;
begin
  SetLength(result, m);
  for var i := 0 to m - 1 do
    result[i] := ReadArrReal(n);
end;

function  ReadArrArrString(m, n: integer): array of array of string;
begin
  SetLength(result, m);
  for var i := 0 to m - 1 do
    result[i] := ReadArrString(n);
end;

function  ReadArrArrInteger(m: integer): array of array of integer := ReadArrArrInteger(m, m);
function  ReadArrArrReal(m: integer): array of array of real := ReadArrArrReal(m, m);
function  ReadArrArrString(m: integer): array of array of string := ReadArrArrString(m, m);

function  ReadArrArrInteger: array of array of integer;
begin
  var (m, n) := ReadInteger2;
  result := ReadArrArrInteger(m, n);
end;

function  ReadArrArrReal: array of array of real;
begin
  var (m, n) := ReadInteger2;
  result := ReadArrArrReal(m, n);
end;

function  ReadArrArrString: array of array of string;
begin
  var (m, n) := ReadInteger2;
  result := ReadArrArrString(m, n);
end;

function  ReadListInteger(n: integer): List<integer> := Range(1, n).Select(e -> GetInteger()).ToList();
function  ReadListReal(n: integer): List<real> := Range(1, n).Select(e -> GetReal()).ToList();
function  ReadListString(n: integer): List<string> := Range(1, n).Select(e -> GetString()).ToList();
function  ReadListInteger: List<integer> := ReadListInteger(ReadInteger);
function  ReadListReal: List<real> := ReadListReal(ReadInteger);
function  ReadListString: List<string> := ReadListString(ReadInteger);

function  ReadListListInteger(m, n: integer): List<List<integer>>;
begin
  result := new List<List<integer>>(m);
  loop m do
    result.Add(ReadListInteger(n));
end;

function  ReadListListReal(m, n: integer): List<List<real>>;
begin
  result := new List<List<real>>(m);
  loop m do
    result.Add(ReadListReal(n));
end;

function  ReadListListString(m, n: integer): List<List<string>>;
begin
  result := new List<List<string>>(m);
  loop m do
    result.Add(ReadListString(n));
end;

function  ReadListListInteger(m: integer): List<List<integer>> := ReadListListInteger(m, m);
function  ReadListListReal(m: integer): List<List<real>> := ReadListListReal(m, m);
function  ReadListListString(m: integer): List<List<string>> := ReadListListString(m, m);

function  ReadListListInteger: List<List<integer>>;
begin
  var (m, n) := ReadInteger2;
  result := ReadListListInteger(m, n);
end;

function  ReadListListReal: List<List<real>>;
begin
  var (m, n) := ReadInteger2;
  result := ReadListListReal(m, n);
end;

function  ReadListListString: List<List<string>>;
begin
  var (m, n) := ReadInteger2;
  result := ReadListListString(m, n);
end;

// == Конец дополнений к версии 4.18 ==




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
    Write(e);
end;

/// Выводит элементы последовательности
procedure Write<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  foreach e : T in b do
    Write(e);
end;

/// Выводит элементы динамического массива
procedure Write<T>(self: array of T); extensionmethod;
begin
  for var i:=0 to self.Length-1 do
    Write(self[i]);
end;

/// Выводит элементы матрицы
procedure Write<T>(self: array [,] of T); extensionmethod;
begin
  for var i:=0 to self.GetLength(0)-1 do
  for var j:=0 to self.GetLength(1)-1 do
    Write(self[i,j]);
end;

// == Дополнения 2016.07
(*
// == Удалено в версии 4.18

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
*)
/// Выводит размер и элементы последовательности
procedure PrintAll<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  PT4.Put(b.Length);
  foreach e : T in b do
    Write(e);
end;

/// Выводит размер и элементы динамического массива
procedure PrintAll<T>(self: array of T); extensionmethod;
begin
  PT4.Put(self.Length);
  for var i:=0 to self.Length-1 do
    Write(self[i]);
end;

/// Выводит размер и элементы динамического массива
procedure WriteAll<T>(self: array of T); extensionmethod;
begin
  PT4.Put(self.Length);
  for var i:=0 to self.Length-1 do
    Write(self[i]);
end;

/// Выводит элементы последовательности
procedure Writeln<T>(self: sequence of T); extensionmethod;
begin
  var b := self.ToArray();
  foreach e : T in b do
    Write(e);
end;

/// Выводит элементы динамического массива
procedure Writeln<T>(self: array of T); extensionmethod;
begin
  for var i:=0 to self.Length-1 do
    Write(self[i]);
end;

/// Выводит элементы матрицы
procedure Writeln<T>(self: array [,] of T); extensionmethod;
begin
  for var i:=0 to self.GetLength(0)-1 do
  for var j:=0 to self.GetLength(1)-1 do
    Write(self[i,j]);
end;

// == Конец дополнений 2016.07


/// Выводит в разделе отладки окна задачника 
/// комментарий cmt, размер последовательности и значения, 
/// полученные из элементов последовательности 
/// с помощью указанного лямбда-выражения.
/// После этого переходит на новую экранную строку.
function Show<TSource>(self: sequence of TSource; cmt: string; selector: System.Func<TSource, object>): sequence of TSource; extensionmethod;
begin
  var b := self.Select(selector);//.ToArray();
  if cmt <> '' then
    PT4.Show(cmt);
  PT4.Show((b.Count + ':').PadLeft(3));
//  foreach var e in b do
    PT4.Show(b);
  if not LineBreak then
    PT4.ShowLine();
  result := self; 
end;

/// Выводит в разделе отладки окна задачника 
/// размер последовательности и значения, 
/// полученные из элементов последовательности 
/// с помощью указанного лямбда-выражения.
/// После этого переходит на новую экранную строку.
function Show<TSource>(Self: sequence of TSource; selector: System.Func<TSource, object>): sequence of TSource; extensionmethod;
begin
  result := self.Show('', selector); 
end;

/// Выводит в разделе отладки окна задачника 
/// комментарий cmt, размер последовательности и ее элементы.
/// После этого переходит на новую экранную строку.
function Show<TSource>(Self: sequence of TSource; cmt: string): sequence of TSource; extensionmethod;
begin
  result := self;
  self.Show(cmt, e -> object(e));
end;


/// Выводит в разделе отладки окна задачника 
/// размер последовательности и ее элементы.
/// После этого переходит на новую экранную строку.
function Show<TSource>(Self: sequence of TSource): sequence of TSource; extensionmethod;
begin
  result := self;
  self.Show('', e -> e as object);
end;

// == Конец дополнений к версии 4.14 ==

function GetSolutionInfo: string;
begin
  var val := new byte[500];
  _getsolutioninfo(val);
  result := FromBytes(val);
end;

initialization
  __InitModule;
finalization
  __FinalizeModule__;
  GetSolutionInfo;
end.