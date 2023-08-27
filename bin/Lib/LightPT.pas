/// Модуль LightPT автоматической легковесной проверки заданий
unit LightPT;
{$reference System.Net.Http.dll}
{$reference System.Security.dll}

interface

uses System.Runtime.InteropServices;

{==============================================================}
{                  Класс для формирования вывода               }
{==============================================================}
type
  /// Класс для формирования вывода
  ObjectList = class
    lst := new List<object>;
  public
    static function New: ObjectList := ObjectList.Create;
    procedure Add(o: object) := lst.Add(o);
    function AddRange(sq: sequence of integer): ObjectList; begin lst.AddRange(sq.Select(x -> object(x))); Result := Self end;
    function AddRange(sq: sequence of real): ObjectList; begin lst.AddRange(sq.Select(x -> object(x))); Result := Self end;
    function AddRange(sq: sequence of string): ObjectList; begin lst.AddRange(sq.Select(x -> object(x))); Result := Self end;
    function AddRange(sq: sequence of char): ObjectList; begin lst.AddRange(sq.Select(x -> object(x))); Result := Self end;
    function AddRange(sq: sequence of boolean): ObjectList; begin lst.AddRange(sq.Select(x -> object(x))); Result := Self end;
    function AddRange(sq: sequence of object): ObjectList; begin lst.AddRange(sq); Result := Self end;
    function AddRange(sq: sequence of System.Type): ObjectList; begin lst.AddRange(sq.Select(x -> object(x))); Result := Self end;
    function AddFill(n: integer; elem: object): ObjectList; begin lst.AddRange(ArrFill(n,elem)); Result := Self end;
    function AddArithm(n: integer; a0,step: integer): ObjectList; begin lst.AddRange(ArrGen(n,a0,x->x+step).Select(x -> object(x))); Result := Self end;
    function AddArithm(n: integer; a0,step: real): ObjectList; begin lst.AddRange(ArrGen(n,a0,x->x+step).Select(x -> object(x))); Result := Self end;
    function AddFib(n: integer): ObjectList; begin lst.AddRange(ArrGen(n,1,1,(x,y)->x+y).Select(x -> object(x))); Result := Self end;
    function AddGeom(n: integer; a0,step: integer): ObjectList; begin lst.AddRange(ArrGen(n,a0,x->x*step).Select(x -> object(x))); Result := Self end;
    function AddGeom(n: integer; a0,step: real): ObjectList; begin lst.AddRange(ArrGen(n,a0,x->x*step).Select(x -> object(x))); Result := Self end;
  end;
  
{==================================================================================}
{                                  Сервисные типы                                  }
{==================================================================================}
type
  MessageColorT = (MsgColorGreen, MsgColorRed, MsgColorOrange, MsgColorMagenta, MsgColorGray);
  TaskStatus = (NotUnderControl, Solved, IOError, BadSolution, PartialSolution, InitialTask, BadInitialTask, InitialTaskPT4, ErrFix, Demo); // Короткий результат для БД

{==================================================================================}
{     Переопределенные функций стандартного модуля с заполнением ввода и вывода    }
{==================================================================================}
/// Возвращает случайное целое в диапазоне от a до b
function Random(a, b: integer): integer;
/// Возвращает случайное целое в диапазоне от 0 до n-1
function Random(n: integer): integer;
/// Возвращает случайное вещественное в диапазоне [0..1)
function Random: real;
/// Возвращает случайное вещественное в диапазоне [a,b)
function Random(a, b: real): real;
/// Возвращает случайное вещественное в диапазоне [a,b] c количеством значащих цифр после точки, равным digits
function RandomReal(a, b: real; digits: integer := 1): real;
/// Возвращает случайный символ в диапазоне от a до b
function Random(a, b: char): char;
/// Возвращает случайное целое в диапазоне 
function Random(diap: IntRange): integer;
/// Возвращает случайное вещественное в диапазоне 
function Random(diap: RealRange): real;
/// Возвращает случайный символ в диапазоне 
function Random(diap: CharRange): char;

/// Возвращает кортеж из двух случайных целых в диапазоне от a до b
function Random2(a, b: integer): (integer, integer);
/// Возвращает кортеж из двух случайных вещественных в диапазоне от a до b
function Random2(a, b: real): (real, real);
/// Возвращает кортеж из двух случайных символов в диапазоне от a до b
function Random2(a, b: char): (char, char);
/// Возвращает кортеж из двух случайных целых в диапазоне
function Random2(diap: IntRange): (integer, integer);
/// Возвращает кортеж из двух случайных символов в диапазоне
function Random2(diap: CharRange): (char, char);
/// Возвращает кортеж из двух случайных вещественных в диапазоне
function Random2(diap: RealRange): (real, real);

/// Возвращает кортеж из трех случайных целых в диапазоне от a до b
function Random3(a, b: integer): (integer, integer, integer);
/// Возвращает кортеж из трех случайных вещественных в диапазоне от a до b
function Random3(a, b: real): (real, real, real);
/// Возвращает кортеж из трех случайных символов в диапазоне от a до b
function Random3(a, b: char): (char, char, char);
/// Возвращает кортеж из трех случайных целых в диапазоне
function Random3(diap: IntRange): (integer, integer, integer);
/// Возвращает кортеж из трех случайных вещественных в диапазоне
function Random3(diap: RealRange): (real, real, real);
/// Возвращает кортеж из трех случайных символов в диапазоне
function Random3(diap: CharRange): (char, char, char);

/// Возвращает массив размера n, заполненный случайными целыми значениями в диапазоне от a до b
function ArrRandomInteger(n: integer; a: integer; b: integer): array of integer;
/// Возвращает массив размера n, заполненный случайными целыми значениями в диапазоне от 0 до 100
function ArrRandomInteger(n: integer): array of integer;
/// Возвращает массив размера n, заполненный случайными вещественными значениями в диапазоне от a до b 
function ArrRandomReal(n: integer; a: real; b: real; digits: integer := 1): array of real;

/// Возвращает массив размера n, заполненный случайными вещественными значениями  в диапазоне от 0 до 10
function ArrRandomReal(n: integer; digits: integer := 1): array of real;

/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandomInteger(m: integer; n: integer; a: integer; b: integer): array [,] of integer;
/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandomInteger(m: integer; n: integer): array [,] of integer;
/// Возвращает двумерный массив размера m x n, заполненный случайными вещественными значениями
function MatrRandomReal(m: integer; n: integer; a: real; b: real): array [,] of real;
/// Возвращает двумерный массив размера m x n, заполненный случайными вещественными значениями
function MatrRandomReal(m: integer; n: integer): array [,] of real;

/// Возвращает двумерный массив размера m x n, заполненный элементами gen(i,j) 
//function MatrGen<T>(m, n: integer; gen: (integer,integer)->T): array [,] of T;

/// Выводит приглашение к вводу и возвращает значение типа integer, введенное с клавиатуры
function ReadInteger(prompt: string): integer;
/// Выводит приглашение к вводу и возвращает два значения типа integer, введенные с клавиатуры
function ReadInteger2(prompt: string): (integer, integer);
/// Выводит приглашение к вводу и возвращает значение типа integer, введенное с клавиатуры
function ReadlnInteger(prompt: string): integer;

/// Выводит приглашение к вводу и возвращает значение типа string, введенное с клавиатуры
function ReadString(prompt: string): string;
/// Выводит приглашение к вводу и возвращает значение типа string, введенное с клавиатуры
function ReadlnString(prompt: string): string;


///- procedure Print(a,b,...);
/// Выводит значения a,b,... на экран, после каждого значения выводит пробел
procedure Print(params args: array of object);
///- procedure Println(a,b,...);
/// Выводит значения a,b,... на экран, после каждого значения выводит пробел и переходит на новую строку
procedure Println(params args: array of object);
/// Выводит значение экран и выводит пробел
procedure Print(ob: object);
/// Выводит значение экран и выводит пробел
procedure Print(s: string);
/// Выводит значение экран и выводит пробел
procedure Print(c: char);

{=========================================================================}
{                        Сервисные процедуры                              }
{=========================================================================}

// Вывести цветовое сообщение в окно вывода
procedure ColoredMessage(msg: string; color: MessageColorT);

// Вывести сообщение красным цветом в окно вывода
procedure ColoredMessage(msg: string);

function ToObjArray(a: sequence of integer): array of object;

function ToObjArray(a: sequence of real): array of object;

function ToObjArray(a: sequence of string): array of object;

function ToObjArray(a: sequence of char): array of object;

function ToObjArray(a: sequence of boolean): array of object;



{=========================================================================}
{        Основные процедуры для проверки правильности ввода-вывода        }
{=========================================================================}

// Самые часто используемые: CheckInput, CheckOutput, CheckOutputSeq

/// Проверить типы вводимых данных
procedure CheckInput(a: array of System.Type);
/// Проверить значения при выводе
procedure CheckOutput(params arr: array of object);
/// Проверить значения при выводе. Сообщения ColoredMessage гасить. Нужно для повторных вызовов CheckOutput
procedure CheckOutputSilent(params arr: array of object);


/// Проверить, что данные не вводились
procedure CheckInputIsEmpty;

/// Проверить, что помимо начального ввода других данных не вводилось
procedure CheckInputIsInitial;

/// Синоним CheckInput
procedure CheckInputTypes(a: array of System.Type);

/// Проверить количество вводимых данных
procedure CheckInputCount(n: integer);
/// Проверить количество вводимых данных
procedure CheckInput2Count(i: integer);
/// Проверить количество выводимых данных
procedure CheckOutput2Count(i: integer);

/// Проверить последовательность значений при выводе
procedure CheckOutputSeq(a: sequence of integer);
/// Проверить последовательность значений при выводе
procedure CheckOutputSeq(a: sequence of real);
/// Проверить последовательность значений при выводе
procedure CheckOutputSeq(a: sequence of string);
/// Проверить последовательность значений при выводе
procedure CheckOutputSeq(a: sequence of char);
/// Проверить последовательность значений при выводе
procedure CheckOutputSeq(a: sequence of boolean);
/// Проверить последовательность значений при выводе
procedure CheckOutputSeq(a: sequence of object);
/// Проверить последовательность значений при выводе
procedure CheckOutputSeq(a: sequence of word);
/// Проверить последовательность значений при выводе
procedure CheckOutputSeq(a: ObjectList);

/// Проверить последовательность значений при выводе. Не выводить сообщения ColoredMessages
procedure CheckOutputSeqSilent(a: sequence of integer);
/// Проверить последовательность значений при выводе. Не выводить сообщения ColoredMessages
procedure CheckOutputSeqSilent(a: sequence of real);
/// Проверить последовательность значений при выводе. Не выводить сообщения ColoredMessages
procedure CheckOutputSeqSilent(a: sequence of string);
/// Проверить последовательность значений при выводе. Не выводить сообщения ColoredMessages
procedure CheckOutputSeqSilent(a: sequence of char);
/// Проверить последовательность значений при выводе. Не выводить сообщения ColoredMessages
procedure CheckOutputSeqSilent(a: sequence of boolean);
/// Проверить последовательность значений при выводе. Не выводить сообщения ColoredMessages
procedure CheckOutputSeqSilent(a: sequence of object);
/// Проверить последовательность значений при выводе. Не выводить сообщения ColoredMessages
procedure CheckOutputSeqSilent(a: sequence of word);
/// Проверить последовательность значений при выводе. Не выводить сообщения ColoredMessages
procedure CheckOutputSeqSilent(a: ObjectList);

/// Проверить вывод в виде строки
procedure CheckOutputString(str: string);

/// Сравнить типы выведенных значений с указанными
procedure CompareTypeWithOutput(params a: array of System.Type);

/// Сравнить два значения
function CompareValues(o1, o2: Object): boolean;

/// Сравнить два массива
function CompareArrValues(a,lst: array of object): boolean;

/// Сравнить значения с выводом
function CompareValuesWithOutput(params a: array of object): boolean;

// Для совместимости
procedure CheckOutputNew(params arr: array of object);
procedure CheckOutputSeqNew(a: sequence of integer);
procedure CheckOutputSeqNew(a: sequence of real);
procedure CheckOutputSeqNew(a: sequence of string);
procedure CheckOutputSeqNew(a: sequence of char);
procedure CheckOutputSeqNew(a: sequence of boolean);
procedure CheckOutputSeqNew(a: sequence of object);
procedure CheckOutputSeqNew(a: sequence of word);
procedure CheckOutputSeqNew(a: ObjectList);

{============================================================================================}
{   Подпрограммы для проверки начального ввода-вывода, представленного в заготовке задания   }
{============================================================================================}

/// Проверить типы данных начального ввода, начального вывода и ввода. Если ввод Input = nil, то он совпадает с начальным вводом
procedure CheckData(InitialInput: array of System.Type := nil; 
  InitialOutput: array of System.Type := nil; 
  Input: array of System.Type := nil);

procedure CheckInitialIO;
procedure CheckInitialIOIsEmpty;
procedure InitialOutput(a: array of System.Type);
procedure InitialInput(a: array of System.Type);
//procedure CheckInitialOutputValues(params a: array of object); - тут значений быть не должно!
procedure CheckInitialOutput(params a: array of System.Type);
procedure CheckInitialInput(params a: array of System.Type);

procedure CheckInitialOutputSeq(a: sequence of System.Type);
procedure CheckInitialInputSeq(a: sequence of System.Type);

procedure CheckInitialIOSeqs(input,output: sequence of System.Type);

procedure CheckOutputAfterInitial(params arr: array of object); // проверить только то, что после исходного вывода

procedure CheckOutputAfterInitialSilent(params arr: array of object); 

/// Проверить последовательность значений при выводе после начального вывода
procedure CheckOutputAfterInitialSeq(seq: sequence of integer);
/// Проверить последовательность значений при выводе после начального вывода
procedure CheckOutputAfterInitialSeq(seq: sequence of real);
/// Проверить последовательность значений при выводе после начального вывода
procedure CheckOutputAfterInitialSeq(seq: sequence of string);
/// Проверить последовательность значений при выводе после начального вывода
procedure CheckOutputAfterInitialSeq(seq: sequence of boolean);
/// Проверить последовательность значений при выводе после начального вывода
procedure CheckOutputAfterInitialSeq(seq: sequence of char);
/// Проверить последовательность значений при выводе после начального вывода
procedure CheckOutputAfterInitialSeq(seq: sequence of object);
/// Проверить последовательность значений при выводе после начального вывода
procedure CheckOutputAfterInitialSeq(seq: ObjectList);

/// Проверить последовательность значений при выводе после начального вывода. Не выводить сообщения ColoredMessages
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of integer);
/// Проверить последовательность значений при выводе после начального вывода. Не выводить сообщения ColoredMessages
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of real);
/// Проверить последовательность значений при выводе после начального вывода. Не выводить сообщения ColoredMessages
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of string);
/// Проверить последовательность значений при выводе после начального вывода. Не выводить сообщения ColoredMessages
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of boolean);
/// Проверить последовательность значений при выводе после начального вывода. Не выводить сообщения ColoredMessages
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of char);
/// Проверить последовательность значений при выводе после начального вывода. Не выводить сообщения ColoredMessages
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of object);
/// Проверить последовательность значений при выводе после начального вывода. Не выводить сообщения ColoredMessages
procedure CheckOutputAfterInitialSeqSilent(seq: ObjectList);

{=========================================================}
{       Функции для проверки элементов ввода-вывода       }
{=========================================================}

/// i-тый элемент ввода - целое 
function IsInt(i: integer): boolean;
/// i-тый элемент ввода - вещественное 
function IsRe(i: integer): boolean;
/// i-тый элемент ввода - строка 
function IsStr(i: integer): boolean;
/// i-тый элемент ввода - логическое 
function IsBoo(i: integer): boolean;
/// i-тый элемент ввода - символ 
function IsChr(i: integer): boolean;

/// i-тый элемент вывода - целое 
function OutIsInt(i: integer): boolean;
/// i-тый элемент вывода - вещественное 
function OutIsRe(i: integer): boolean;
/// i-тый элемент вывода - строка 
function OutIsStr(i: integer): boolean;
/// i-тый элемент вывода - логическое 
function OutIsBoo(i: integer): boolean;
/// i-тый элемент вывода - символ 
function OutIsChr(i: integer): boolean;

/// i-тый элемент ввода как целое 
function Int(i: integer): integer;
/// i-тый элемент ввода как вещественное 
function Re(i: integer): real;
/// i-тый элемент ввода как строка 
function Str(i: integer): string;
/// i-тый элемент ввода как логическое 
function Boo(i: integer): boolean;
/// i-тый элемент ввода как символ 
function Chr(i: integer): char;

/// i-тый элемент вывода как целое 
function OutInt(i: integer): integer;
/// i-тый элемент вывода как вещественное 
function OutRe(i: integer): real;
/// i-тый элемент вывода как строка 
function OutBoo(i: integer): boolean;
/// i-тый элемент вывода как логическое 
function OutChr(i: integer): char;
/// i-тый элемент вывода как символ 
function OutStr(i: integer): string;

/// Следующий элемент ввода как целое 
function Int: integer;
/// Следующий элемент ввода как вещественное
function Re: real;
/// Следующий элемент ввода как строка
function Str: string;
/// Следующий элемент ввода как логическое
function Boo: boolean;
/// Следующий элемент ввода как символ
function Chr: char;

/// Следующие два элемента ввода как целые 
function Int2: (integer, integer);
/// Следующие два элемента ввода как вещественные 
function Re2: (real, real);

/// Следующие n элементов ввода как массив целых
function IntArr(n: integer): array of integer;
/// Следующие n элементов ввода как массив вещественных
function ReArr(n: integer): array of real;
/// Следующие n элементов ввода как массив логических
function BooArr(n: integer): array of boolean;
/// Следующие n элементов ввода как массив символов
function ChrArr(n: integer): array of char;
/// Следующие n элементов ввода как массив строк
function StrArr(n: integer): array of string;

// функции, возвращающие входные и выходные списки, а также их срезы, приведенные к нужному типу

function InputListAsIntegers: array of integer;
function InputListAsReals: array of real;
function InputListAsBooleans: array of boolean;
function InputListAsChars: array of char;
function InputListAsStrings: array of string;

function OutputListAsIntegers: array of integer;
function OutputListAsReals: array of real;
function OutputListAsBooleans: array of boolean;
function OutputListAsChars: array of char;
function OutputListAsStrings: array of string;

function InSliceIntArr(a,b: integer): array of integer;
function InSliceReArr(a,b: integer): array of real;
function InSliceBooArr(a,b: integer): array of boolean;
function InSliceChrArr(a,b: integer): array of char;
function InSliceStrArr(a,b: integer): array of string;

/// Срез элементов вывода от a до b как массив целых
function OutSliceIntArr(a,b: integer): array of integer;
/// Срез элементов вывода от a до b как массив вещественных
function OutSliceReArr(a,b: integer): array of real;
/// Срез элементов вывода от a до b как массив логических
function OutSliceBooArr(a,b: integer): array of boolean;
/// Срез элементов вывода от a до b как массив символов
function OutSliceChrArr(a,b: integer): array of char;
/// Срез элементов вывода от a до b как массив строк
function OutSliceStrArr(a,b: integer): array of string;

{=========================================================}
{      Тип TestCell и функции для генерации тестов        }
{=========================================================}
type 
  [StructLayout(LayoutKind.Explicit)]
  /// Ячейка генерации данных для тестов
  TestCell = class
  public
    [FieldOffset(0)]
    typ: System.Type;
    [FieldOffset(8)]
    a: integer; 
    [FieldOffset(12)]
    b: integer; 
    [FieldOffset(8)]
    ra: real;
    [FieldOffset(16)]
    rb: real;
    [FieldOffset(24)]
    digits: integer;
    [FieldOffset(8)]
    ca: char;
    [FieldOffset(10)]
    cb: char;
    constructor (typ: System.Type);
    begin
      Self.typ := typ;
    end;
    function GenerateData: object;
    begin
      if typ = typeof(integer) then
        Result := Random(a,b)
      else if typ = typeof(char) then 
        Result := Random(ca,cb)
      else if typ = typeof(real) then 
        Result := RandomReal(ra,rb,digits)
      else if typ = typeof(boolean) then 
        Result := Random(2)=0 ? False : True
      else Result := nil;
    end;
  end;
  
/// Сгенерировать по последовательности спецификаций в тестовых ячейках список данных для теста
function GenValuesByTestCells(a: sequence of TestCell): List<object>;
  
/// Сгенерировать целую тестовую ячейку
function tInt(a: integer := 1; b: integer := 10): TestCell;

/// Сгенерировать вещественную тестовую ячейку
function tRe(ra: real := 1; rb: Real := 10; digits: integer := 0): TestCell;

/// Сгенерировать символьную тестовую ячейку
function tChr(ca: char := 'а'; cb: char := 'я'): TestCell;

/// Сгенерировать логическую тестовую ячейку
function tBoo: TestCell;


{=========================================================}
{                Процедуры генерации ошибок               }
{=========================================================}

/// Генерация ошибки неверного количества входных значений
procedure ErrorInputCount(Count, n: integer);

/// Генерация ошибки неверного количества выходных значений
procedure ErrorOutputCount(Count, n: integer);

/// Генерация ошибки неверного типа n-того входного значения
procedure ErrorInputType(n: integer; ExpectedType, ActualType: string);

/// Генерация ошибки неверного типа n-того выходного значения
procedure ErrorOutputType(n: integer; ExpectedType, ActualType: string);


{=========================================================}
{       Процедура генерации автоматических тестов         }
{=========================================================}

function AutoTest: integer->();

/// Генерация автоматических тестов - повторно вызывается заполнение из основной программы. Не использовать если в основной программе есть Read!!!
procedure GenerateAutoTests(n: integer);

/// Генерация тестов по описанию тестовых данных
procedure GenerateTests(n: integer; testcells: sequence of TestCell);

/// Генерация тестов по набору значений тестовых данных
procedure GenerateTests(params a: array of integer);

/// Генерация тестов по набору значений тестовых данных
procedure GenerateTests(params a: array of real);

/// Генерация тестов по набору значений тестовых данных
procedure GenerateTests(params a: array of string);

/// Генерация тестов по набору значений тестовых данных
procedure GenerateTests(params a: array of char);

/// Генерация тестов по набору значений тестовых данных
procedure GenerateTests(params a: array of boolean);

/// Генерация тестов по набору кортежей тестовых данных
procedure GenerateTests<T1,T2>(params a: array of (T1,T2));

/// Генерация тестов по набору кортежей тестовых данных
procedure GenerateTests<T1,T2,T3>(params a: array of (T1,T2,T3));

{=========================================================}
{               Фильтрация выходного списка               }
{=========================================================}
/// Преобразование строк, являющихся целыми, в целые в OutputList
procedure ConvertStringsToNumbersInOutputList;
/// Если в OutputList массивы, вытянуть их в единый список
procedure FlattenOutput;
/// Очистить выходной список от пробелов
procedure ClearOutputListFromSpaces;
/// Отфильтровать в выходном списке только числа
procedure FilterOnlyNumbers;
/// Отфильтровать в выходном списке только числа и логические
procedure FilterOnlyNumbersAndBools;

var 
  TaskResult: TaskStatus := NotUnderControl; // Записывается в БД
  /// Строка, содержащая вывод. Нужна для проверки решения как многострочной строки 
  OutputString := new StringBuilder;     
  /// Список выведенных элементов
  OutputList := new List<object>;
  /// Список введенных элементов
  InputList := new List<object>;
  /// Список типов элементов, выведенных в заготовке задания (cRe, cInt и т.д.)
  InitialOutputList := new List<System.Type>; 
  /// Список типов элементов, введенных в заготовке задания
  InitialInputList := new List<System.Type>;  
  /// Ссылка на основную процедуру проверки в модуле Task
  CheckTask: procedure(name: string);
  /// Погасить сообщения о неверном вводе-выводе если задача содержит только начальный ввод-вывод
  CancelMessagesIfInitial := True;
  /// Тихий режим - сообщения ColoredMessage гасятся. Нужен для обработки нескольких вызовов CheckOutput
  Silent := False;  

/// Целый тип для проверки ввода-вывода
function cInt: System.Type;
/// Вещественный тип для проверки ввода-вывода
function cRe: System.Type;
/// Строковый тип для проверки ввода-вывода
function cStr: System.Type;
/// Логический тип для проверки ввода-вывода
function cBool: System.Type;
/// Символьный тип для проверки ввода-вывода
function cChar: System.Type;

type 
  /// Константа для обозначения пустого ввода или вывода
  EmptyType = (Empty);
  TestModeType = (tmNone, tmTest, tmGenTest, tmAutoTest);
  
var
  TestCount := 0;
  GenerateTestData: integer -> () := nil;
  TestMode: TestModeType := tmNone;
  TestNumber: integer;

implementation

uses __RedirectIOMode;

uses System.Management;
uses System.Security.Cryptography;
uses System.IO;

uses System;
uses System.Net.Http;
uses System.Threading.Tasks;

// Исключения - для вывода сообщения
// TaskResult - для базы данных
// Запоминать исключение в глобальной переменной TaskException - для доп. параметров для БД в формате 
//   ПодтипИсключения(парам1,...,парамn)
// Эту строку также можно писать в БД как доп параметры TaskResultInfo
// Чем хороши исключения - их можно делать разными с абсолютно разными параметрами

// Сервисная функция - чтобы можно было вводу, начальному вводу или начальному выводу в CheckData присвоить Empty вместо nil
function operator implicit(Self: EmptyType): array of System.Type; extensionmethod := nil;

const lightptname = 'lightpt.dat';

type
  LightPTException = class(Exception) 
    function Info: string; virtual := 'NoInfo';
  end;

var
  CreateNewLineBeforeMessage := False;
  TaskResultInfo: string; // доп. информация о результате. Как правило пуста. Или содержит TaskException.Info. Или содержит для Solved и BadSolution информацию о модуле: Robot, Drawman, PT4
  TaskException: LightPTException := new LightPTException;

  WriteInfoCallBack: procedure (LessonName,TaskName,TaskPlatform: string; result: TaskStatus; AdditionalInfo: string);

  LessonName: string := '';
  TaskNamesMap := new Dictionary<string,string>;
  
  ServerAddr := 'https://air.mmcs.sfedu.ru/pascalabc';

{=========================================================}
{                  Типы, связанные с сетью                }
{=========================================================}
type 
  UserTypeEnum = (None, Student, Teacher, Admin);
  ServerAccessProvider = class
  private
    client: HTTPClient;
  public
    ServerAddr: string;
    auto property ShortFIO: string; 
    auto property FullFIO: string; 
    auto property Password: string; 
    auto property Group: string; 
    auto property UserType: UserTypeEnum; 
    constructor (ServerAddr: string);
    begin
      Self.ServerAddr := ServerAddr;
      client := new HttpClient();
      client.Timeout := TimeSpan.FromSeconds(10);
    end;
    
    function SendPostRequest(FullFIO, Password, LessonName, TaskName, TaskPlatform, TaskResult, text, TaskResultInfo: string): Task<string>;
    begin
      var values := Dict(
        ( 'shortFIO', '' ),  
        ( 'FIO', FullFIO ),
        ( 'taskName', TaskName ),
        ( 'lessonName', LessonName ),
        ( 'taskPlatform', TaskPlatform ),
        ( 'taskResult', TaskResult ),
        ( 'taskResultInfo', TaskResultInfo ),
        ( 'content', text ),
        ( 'password', Password )
      );
      var content := new FormUrlEncodedContent(values);
      var response := client.PostAsync(ServerAddr + '/add.php', content);
      Result := response.Result.Content.ReadAsStringAsync();
    end;
  end;

{=========================================================}
{               Типы исключений при решении               }
{=========================================================}
type
  InputCountException = class(LightPTException) // Ровно Count 
    Count: integer; // Count - сколько введено
    n: integer;     // n - сколько требуется ввести
    constructor(Count, n: integer);
    begin
      Self.Count := Count;
      Self.n := n;
      TaskResult := IOError;
      TaskException := Self;
    end;
    
    function Info: string; override := $'InputCount({Count},{n})';
  end;
  InputCount2Exception = class(LightPTException) // Не меньше Count
    Count: integer; // Count - сколько введено
    i: integer;     // i - какой номер требуется ввести (с нуля)
    constructor(Count, i: integer);
    begin
      Self.Count := Count;
      Self.i := i;
      TaskResult := IOError;
      TaskException := Self;
    end;
    
    function Info: string; override := $'InputCount2({Count},{i})';
  end;
  
  InputCountTest2Exception = class(LightPTException) // Не меньше Count
    Count: integer; // Count - сколько введено
    i: integer;     // i - какой номер требуется ввести (с нуля)
    constructor(Count, i: integer);
    begin
      Self.Count := Count;
      Self.i := i;
    end;
    
    function Info: string; override := $'InputCountTest2({Count},{i})';
  end;

  InputTypeException = class(LightPTException)
    n: integer; // номер параметра
    ExpectedType, ActualType: string;
    constructor(n: integer; ExpectedType, ActualType: string);
    begin
      Self.n := n;
      Self.ExpectedType := ExpectedType;
      Self.ActualType := ActualType;
      TaskResult := IOError;
      TaskException := Self;
    end;
    
    function Info: string; override := $'InputType({n},{ExpectedType},{ActualType})';
  end;
  
    InputTypeTestException = class(LightPTException)
    n: integer; // номер параметра
    ExpectedType, ActualType: string;
    constructor(n: integer; ExpectedType, ActualType: string);
    begin
      Self.n := n;
      Self.ExpectedType := ExpectedType;
      Self.ActualType := ActualType;
    end;
    
    function Info: string; override := $'InputTypeTest({n},{ExpectedType},{ActualType})';
  end;

  OutputCountException = class(LightPTException) // Ровно Count 
    Count: integer; // Count - сколько выведено
    n: integer;     // n - сколько требуется вывести
    constructor(Count, n: integer);
    begin
      Self.Count := Count;
      Self.n := n;
      TaskResult := IOError;
      TaskException := Self;
    end;
    
    function Info: string; override := $'OutputCount({Count},{n})';
  end;
  OutputCount2Exception = class(LightPTException) // Ровно Count 
    Count: integer; // Count - сколько выведено
    i: integer;     // n - какой номер требуется вывести
    constructor(Count, i: integer);
    begin
      Self.Count := Count;
      Self.i := i;
      TaskResult := IOError;
      TaskException := Self;
    end;
    
    function Info: string; override := $'Output2Count({Count},{i})';
  end;
  OutputTypeException = class(LightPTException)
    n: integer; // номер параметра
    ExpectedType, ActualType: string;
    constructor(n: integer; ExpectedType, ActualType: string);
    begin
      Self.n := n;
      Self.ExpectedType := ExpectedType;
      Self.ActualType := ActualType;
      TaskResult := IOError;
      TaskException := Self;
    end;
    
    function Info: string; override := $'OutputType({n},{ExpectedType},{ActualType})';
  end;
  
  NotInTestGenModeException = class(LightPTException)
    FuncName: string;
    constructor (FuncName: string) := Self.Funcname := Funcname;
  end;

  NotInTestModeException = class(LightPTException)
    FuncName: string;
    constructor (FuncName: string) := Self.Funcname := Funcname;
  end;

  InputTestCountMismatchException = class(LightPTException);
  
  InputTestTypesMismatchException = class(LightPTException);
  
var
  CurPosInInputList := 0;
  CurPosInInputListInTestMode := 0;
  TaskName := ExtractFileName(System.Environment.GetCommandLineArgs[0]).Replace('.exe', '');

{=========================================================}
{                Функции для генерации тестов             }
{=========================================================}

/// Сгенерировать по последовательности спецификаций в тестовых ячейках список данных для теста
function GenValuesByTestCells(a: sequence of TestCell): List<object>;
begin
  Result := a.Select(tc -> tc.GenerateData).ToList;
end;
  
/// Сгенерировать целую тестовую ячейку
function tInt(a: integer; b: integer): TestCell;
begin
  Result := new TestCell(typeof(integer));
  Result.a := a;
  Result.b := b;
end;

/// Сгенерировать вещественную тестовую ячейку
function tRe(ra: real; rb: real; digits: integer): TestCell;
begin
  Result := new TestCell(typeof(real));
  Result.ra := ra;
  Result.rb := rb;
  Result.digits := digits;
end;

/// Сгенерировать символьную тестовую ячейку
function tChr(ca: char; cb: char): TestCell;
begin
  Result := new TestCell(typeof(char));
  Result.ca := ca;
  Result.cb := cb;
end;

/// Сгенерировать логическую тестовую ячейку
function tBoo: TestCell;
begin
  Result := new TestCell(typeof(boolean));
end;

function operator*(cell: TestCell; n: integer): array of TestCell; extensionmethod;
begin
  Result := ArrFill(n,cell)
end;

function operator*(n: integer; cell: TestCell): array of TestCell; extensionmethod;
begin
  Result := ArrFill(n,cell)
end;

function operator*(cell: System.Type; n: integer): array of System.Type; extensionmethod;
begin
  Result := ArrFill(n,cell)
end;

function operator*(n: integer; cell: System.Type): array of System.Type; extensionmethod;
begin
  Result := ArrFill(n,cell)
end;

{=========================================================}
{                    Сервисные функции                    }
{=========================================================}

procedure ErrorInputCount(Count, n: integer) := raise new InputCountException(Count,n);

procedure ErrorOutputCount(Count, n: integer) := raise new OutputCountException(Count,n);

procedure ErrorInputType(n: integer; ExpectedType, ActualType: string)
  := raise new InputTypeException(n,ExpectedType,ActualType);

procedure ErrorOutputType(n: integer; ExpectedType, ActualType: string)
  := raise new OutputTypeException(n,ExpectedType,ActualType);
  
function AutoTest: integer->() := testnum -> (TestMode := tmAutoTest);

procedure GenerateAutoTests(n: integer);
begin
  TestCount := n;
  GenerateTestData := AutoTest;
end;

procedure GenerateTests(n: integer; testcells: sequence of TestCell);
begin
  // Нужно сравнить типы в InputList и здесь. При несоотверствии бросить исключение
  // Это можно делать только здесь - где количество входных данных заранее известно
  // В задачах где вначале вводится n, а потом массив из n элементов, это не работает - количество данных меняется от теста нк тесту
  var testTypes := testcells.Select(ts -> ts.typ).ToArray;
  var inputTypes := InputList.Select(ob -> ob.GetType).ToArray;
  if (testTypes.Length <> inputTypes.Length) then
    raise new InputTestCountMismatchException;
  for var i:=0 to testTypes.Length - 1 do
    if (testTypes[i] <> inputTypes[i]) then
      raise new InputTestTypesMismatchException;
    
  TestCount := n;
  GenerateTestData := tnum -> begin
    InputList := GenValuesByTestCells(testcells);
  end;
end;

var 
  _isPT, _isRobot, _isDrawman, _isLightPT: boolean;
  _IsPTcalculated := False;
  _IsLightPTcalculated := False;
  _IsRobotcalculated := False;
  _IsDrawmancalculated := False;
  
/// Является ли задание заданием для задачника PT
function IsPT: boolean;
begin
  if _IsPTcalculated then
    Result := _isPT
  else Result := System.Type.GetType('PT4.PT4') <> nil;
  _isPT := Result;
  _IsPTcalculated := True;
end;  

/// Является ли задание заданием для Робота
function IsRobot: boolean;
begin
  if _IsRobotcalculated then
    Result := _isRobot
  else Result := System.Type.GetType('RobotField.RobotField') <> nil;
  _isRobot := Result;
  _IsRobotcalculated := True;
end; 

/// Является ли задание заданием для Чертежника
function IsDrawman: boolean;
begin
  if _IsDrawmancalculated then
    Result := _isDrawman
  else Result := System.Type.GetType('DrawManField.DrawManField') <> nil;
  _isDrawman := Result;
  _IsDrawmancalculated := True;
end; 

/// Является ли задание заданием для легковесного задачника
function IsLightPT: boolean;
begin
  if _IsLightPTcalculated then
    Result := _IsLightPT
  else Result := not IsPT and not IsRobot and not IsDrawman;
  _IsLightPT := Result;
  _IsLightPTcalculated := True;
end; 

/// надо ли пополнять список ввода в функциях, используемых для ввода
function NeedAddDataToInputList: boolean := IsLightPT and ((TestMode = tmNone) or (TestMode = tmAutoTest));

/// Полный путь к папке auth-файла
function FindAuthDat: string;
begin
  var auth := 'auth.dat';
  Result := '';
  // В текущем каталоге
  if FileExists(auth) then
    Result := ExpandFileName(auth)
  // Если нет, в каталоге уровня выше
  else if FileExists(System.IO.Path.Combine('..',auth)) then
  begin  
    Result := ExpandFileName(System.IO.Path.Combine('..',auth))
  end
  // Если нет, в корневом каталоге сетевого диска - Это не работает на старых Win 10 куда не устанавливается NET 4/7/1
  {else begin
    var fullname := ExpandFileName(auth);
    var drive := ExtractFileDrive(fullname);
    var di := new System.IO.DriveInfo(drive);
    if di.DriveType = System.IO.DriveType.Network then
    begin
      var af := System.IO.Path.Combine(di.RootDirectory.FullName,auth);
      if FileExists(af) then
        Result := af;
    end
  end;}
end;

{=================================================================}
{    Функции для шифрования-дешифрования при записи в auth.dat    }
{=================================================================}

// На некоторых компьютерах давала сбой, поэтому просто возвращает константу
function ProcessorId: string;
begin
  {var mbs := new ManagementObjectSearcher('Select ProcessorId From Win32_processor');
  var mbsList := mbs.Get();
  Result := '';
  foreach var mo: ManagementObject in mbsList do
  begin
    var pId := mo['ProcessorId'];
    if pId <> nil then
      Result := pId.ToString()
    else Result := 'AAAAAAAAAAAAAAAA';
    break;
  end;}
  Result := 'AAAAAAAAAAAAAAAA';
end;

function Encrypt(src: string): array of byte; // записать в файл
begin
  var ae := Aes.Create();
  var key := Encoding.UTF8.GetBytes(ProcessorId);
  var crypt := ae.CreateEncryptor(key, key);
  var ms := new MemoryStream();
  var cs := new CryptoStream(ms, crypt, CryptoStreamMode.Write);
  var sw := new StreamWriter(cs);
  sw.Write(src);
  sw.Close;
  cs.Close;
  ms.Close;
  Result := ms.ToArray;
end;

function Decrypt(data: array of byte): string;
begin
  if data = nil then
  begin
    Result := '';
    exit
  end;
  var ae := Aes.Create();
  var key := Encoding.UTF8.GetBytes(ProcessorId);
  var crypt := ae.CreateDecryptor(key, key);
  var ms := new MemoryStream(data);
  var cs := new CryptoStream(ms, crypt, CryptoStreamMode.Read);
  var sr := new StreamReader(cs);
  var text := sr.ReadToEnd;
  sr.Close;
  cs.Close;
  ms.Close;
  Result := text;
end;

{=========================================================}
{            Функции для проверки ввода-вывода            }
{=========================================================}

function cInt := typeof(integer);
function cRe := typeof(real);
function cStr := typeof(string);
function cBool := typeof(boolean);
function cChar := typeof(char);


procedure CheckData(InitialInput, InitialOutput, Input: array of System.Type);
begin
  CheckInitialIOSeqs(InitialInput, InitialOutput);
  if Input = nil then
    CheckInputIsInitial
  else CheckInput(Input);
end;  

procedure CheckInitialIO;
begin
  if (OutputList.Count = InitialOutputList.Count) and (InputList.Count = InitialInputList.Count) then
    TaskResult := InitialTask
  else if (InputList.Count < InitialInputList.Count) or (InputList.Count = InitialInputList.Count) and (OutputList.Count < InitialOutputList.Count) then
    TaskResult := BadInitialTask;
end;

procedure CheckInitialIOIsEmpty := CheckInitialIO;

procedure InitialOutput(a: array of System.Type);
begin
  InitialOutputList.Clear;
  if a<>nil then
    InitialOutputList.AddRange(a);
end;

procedure InitialInput(a: array of System.Type);
begin
  InitialInputList.Clear;
  if a<>nil then
    InitialInputList.AddRange(a);
end;

function CompareValues(o1, o2: Object): boolean;
begin
  if (o1 is real) and (o2 is real) then
  begin
    var r1 := real(o1);
    var r2 := real(o2);
    Result := Abs(r1 - r2) < 0.0001;
    exit;
  end;
  Result := o1.Equals(o2);
end;

function CompareArrValues(a,lst: array of object): boolean;
begin
  Result := True;
  if a.Length <> OutputList.Count then
    Result := False;
  for var i := 0 to a.Length - 1 do
    if not CompareValues(a[i], lst[i]) then
    begin
      Result := False; 
      exit;           
    end;
end;

function CompareValuesWithOutput(params a: array of object): boolean := CompareArrValues(a,OutputList.ToArray);

{procedure CheckInitialOutputValues(params a: array of object);
begin
  if CompareArrValues(a,OutputList.ToArray) then
    TaskResult := InitialTask;   
end;}

// По сути отдельные функции - это неправильно. Необходим CheckInitialInputOutput
procedure CheckInitialOutput(params a: array of System.Type);
begin
  InitialOutput(a);
  CheckInitialIO;
end;

procedure CheckInitialInput(params a: array of System.Type);
begin
  InitialInput(a);
  CheckInitialIO;
end;

procedure CheckInitialOutputSeq(a: sequence of System.Type) := CheckInitialOutput(a.ToArray);

procedure CheckInitialInputSeq(a: sequence of System.Type) := CheckInitialInput(a.ToArray);

procedure CheckInitialIOSeqs(input,output: sequence of System.Type);
begin
  InitialInput(input?.ToArray);
  InitialOutput(output?.ToArray);
  CheckInitialIO;
end;

procedure CheckInputCount(n: integer);
begin
  if InputList.Count <> n then
    raise new InputCountException(InputList.Count, n)
end;

procedure CheckInput2Count(i: integer);
begin
  if InputList.Count <= i then
    raise new InputCount2Exception(InputList.Count, i + 1)
end;

procedure CheckInputTest2Count(i: integer);
begin
  if InputList.Count <= i then
    raise new InputCountTest2Exception(InputList.Count, i + 1)
end;

procedure CheckOutput2Count(i: integer);
begin
  if OutputList.Count <= i then
    raise new OutputCount2Exception(OutputList.Count, i + 1)
end;

function IsInt(i: integer) := InputList[i] is integer;
function IsRe(i: integer) := InputList[i] is real;
function IsStr(i: integer) := InputList[i] is string;
function IsBoo(i: integer) := InputList[i] is boolean;
function IsChr(i: integer) := InputList[i] is char;

function OutIsInt(i: integer) := OutputList[i] is integer;
function OutIsRe(i: integer) := OutputList[i] is real;
function OutIsStr(i: integer) := OutputList[i] is string;
function OutIsBoo(i: integer) := OutputList[i] is boolean;
function OutIsChr(i: integer) := OutputList[i] is char;


function Int(i: integer): integer;
begin
  CheckInput2Count(i);
  if not IsInt(i) then
    raise new InputTypeException(i + 1, 'integer', TypeName(InputList[i]));
  Result := integer(InputList[i]);
end;

function Re(i: integer): real;
begin
  CheckInput2Count(i);
  if not IsRe(i) then
    raise new InputTypeException(i + 1, 'real', TypeName(InputList[i]));
  Result := real(InputList[i]);
end;

function Str(i: integer): string;
begin
  CheckInput2Count(i);
  if not IsStr(i) then
    raise new InputTypeException(i + 1, 'string', TypeName(InputList[i]));
  Result := string(InputList[i]);
end;

function Boo(i: integer): boolean;
begin
  CheckInput2Count(i);
  if not IsBoo(i) then
    raise new InputTypeException(i + 1, 'boolean', TypeName(InputList[i]));
  Result := boolean(InputList[i]);
end;

function Chr(i: integer): char;
begin
  CheckInput2Count(i);
  if not IsChr(i) then
    raise new InputTypeException(i + 1, 'char', TypeName(InputList[i]));
  Result := char(InputList[i]);
end;

function IntTest(i: integer): integer;
begin
  CheckInputTest2Count(i);
  if not IsInt(i) then
    raise new InputTypeTestException(i + 1, 'integer', TypeName(InputList[i]));
  Result := integer(InputList[i]);
end;

function ReTest(i: integer): real;
begin
  CheckInputTest2Count(i);
  if not IsRe(i) then
    raise new InputTypeTestException(i + 1, 'real', TypeName(InputList[i]));
  Result := real(InputList[i]);
end;

function StrTest(i: integer): string;
begin
  CheckInputTest2Count(i);
  if not IsStr(i) then
    raise new InputTypeTestException(i + 1, 'string', TypeName(InputList[i]));
  Result := string(InputList[i]);
end;

function BooTest(i: integer): boolean;
begin
  CheckInputTest2Count(i);
  if not IsBoo(i) then
    raise new InputTypeTestException(i + 1, 'boolean', TypeName(InputList[i]));
  Result := boolean(InputList[i]);
end;

function ChrTest(i: integer): char;
begin
  CheckInputTest2Count(i);
  if not IsChr(i) then
    raise new InputTypeTestException(i + 1, 'char', TypeName(InputList[i]));
  Result := char(InputList[i]);
end;

function OutInt(i: integer): integer;
begin
  CheckOutput2Count(i);
  if not OutIsInt(i) then
    raise new OutputTypeException(i + 1, 'integer', TypeName(OutputList[i]));
  Result := integer(OutputList[i]);
end;

function OutRe(i: integer): real;
begin
  CheckOutput2Count(i);
  if not OutIsRe(i) then
    raise new OutputTypeException(i + 1, 'real', TypeName(OutputList[i]));
  Result := real(OutputList[i]);
end;

function OutBoo(i: integer): boolean;
begin
  CheckOutput2Count(i);
  if not OutIsBoo(i) then
    raise new OutputTypeException(i + 1, 'boolean', TypeName(OutputList[i]));
  Result := boolean(OutputList[i]);
end;

function OutChr(i: integer): char;
begin
  CheckOutput2Count(i);
  if not OutIsChr(i) then
    raise new OutputTypeException(i + 1, 'char', TypeName(OutputList[i]));
  Result := char(OutputList[i]);
end;

function OutStr(i: integer): string;
begin
  CheckOutput2Count(i);
  if not OutIsStr(i) then
    raise new OutputTypeException(i + 1, 'string', TypeName(OutputList[i]));
  Result := string(OutputList[i]);
end;

function Int: integer;
begin
  Result := Int(CurPosInInputList);
  CurPosInInputList += 1;
end;

function Re: real;
begin
  Result := Re(CurPosInInputList);
  CurPosInInputList += 1;
end;

function Str: string;
begin
  Result := Str(CurPosInInputList);
  CurPosInInputList += 1;
end;

function Boo: boolean;
begin
  Result := Boo(CurPosInInputList);
  CurPosInInputList += 1;
end;

function Chr: char;
begin
  Result := Chr(CurPosInInputList);
  CurPosInInputList += 1;
end;

function IntTest: integer;
begin
  Result := IntTest(CurPosInInputListInTestMode);
  CurPosInInputListInTestMode += 1;
end;

function ReTest: real;
begin
  Result := ReTest(CurPosInInputListInTestMode);
  CurPosInInputListInTestMode += 1;
end;

function StrTest: string;
begin
  Result := StrTest(CurPosInInputListInTestMode);
  CurPosInInputListInTestMode += 1;
end;

function BooTest: boolean;
begin
  Result := BooTest(CurPosInInputListInTestMode);
  CurPosInInputListInTestMode += 1;
end;

function ChrTest: char;
begin
  Result := ChrTest(CurPosInInputListInTestMode);
  CurPosInInputListInTestMode += 1;
end;


function Int2: (integer, integer) := (Int, Int);

function Re2: (real, real) := (Re, Re);

function IntArr(n: integer): array of integer := (1..n).Select(x -> Int).ToArray;
function ReArr(n: integer): array of real := (1..n).Select(x -> Re).ToArray;
function BooArr(n: integer): array of boolean := (1..n).Select(x -> Boo).ToArray;
function ChrArr(n: integer): array of char := (1..n).Select(x -> Chr).ToArray;
function StrArr(n: integer): array of string := (1..n).Select(x -> Str).ToArray;

// функции, возвращающие входные и выходные списки, а также их срезы, приведенные к нужному типу

function InputListAsIntegers: array of integer := InputList.Select((x,i) -> Int(i)).ToArray;
function InputListAsReals: array of real := InputList.Select((x,i) -> Re(i)).ToArray;
function InputListAsBooleans: array of boolean := InputList.Select((x,i) -> Boo(i)).ToArray;
function InputListAsChars: array of char := InputList.Select((x,i) -> Chr(i)).ToArray;
function InputListAsStrings: array of string := InputList.Select((x,i) -> Str(i)).ToArray;

function OutputListAsIntegers: array of integer := OutputList.Select((x,i) -> OutInt(i)).ToArray;
function OutputListAsReals: array of real := OutputList.Select((x,i) -> OutRe(i)).ToArray;
function OutputListAsBooleans: array of boolean := OutputList.Select((x,i) -> OutBoo(i)).ToArray;
function OutputListAsChars: array of char := OutputList.Select((x,i) -> OutChr(i)).ToArray;
function OutputListAsStrings: array of string := OutputList.Select((x,i) -> OutStr(i)).ToArray;

function InSliceIntArr(a,b: integer): array of integer := (a..b).Select(i->Int(i)).ToArray;
function InSliceReArr(a,b: integer): array of real := (a..b).Select(i->Re(i)).ToArray;
function InSliceBooArr(a,b: integer): array of boolean := (a..b).Select(i->Boo(i)).ToArray;
function InSliceChrArr(a,b: integer): array of char := (a..b).Select(i->Chr(i)).ToArray;
function InSliceStrArr(a,b: integer): array of string := (a..b).Select(i->Str(i)).ToArray;

function OutSliceIntArr(a,b: integer): array of integer := (a..b).Select(i->OutInt(i)).ToArray;
function OutSliceReArr(a,b: integer): array of real := (a..b).Select(i->OutRe(i)).ToArray;
function OutSliceBooArr(a,b: integer): array of boolean := (a..b).Select(i->OutBoo(i)).ToArray;
function OutSliceChrArr(a,b: integer): array of char := (a..b).Select(i->OutChr(i)).ToArray;
function OutSliceStrArr(a,b: integer): array of string := (a..b).Select(i->OutStr(i)).ToArray;

function ConvertOne(ob: Object): Object;
begin
  Result := ob;
  if ob is string then
  begin
    var s := string(ob);
    var ival: integer;
    var rval: real;
    if s.TryToInteger(ival) then
      Result := ival
    else if s.TryToReal(rval) then
      Result := rval
  end
end;

procedure ConvertStringsToNumbersInOutputList;
begin
  for var i:=0 to OutputList.Count - 1 do
    OutputList[i] := ConvertOne(OutputList[i]);
end;

function ToObjArray(a: sequence of integer): array of object := a.Select(x -> object(x)).ToArray;

function ToObjArray(a: sequence of real): array of object := a.Select(x -> object(x)).ToArray;

function ToObjArray(a: sequence of string): array of object := a.Select(x -> object(x)).ToArray;

function ToObjArray(a: sequence of char): array of object := a.Select(x -> object(x)).ToArray;

function ToObjArray(a: sequence of boolean): array of object := a.Select(x -> object(x)).ToArray;

function ToObjArray(a: sequence of word): array of object := a.Select(x -> object(x)).ToArray;


{============================================}
{     Методы расширения для List<object>     }
{============================================}
/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: sequence of integer); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.AddRange(ToObjArray(data));
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: sequence of real); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.AddRange(ToObjArray(data));
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: sequence of string); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.AddRange(ToObjArray(data));
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: sequence of char); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.AddRange(ToObjArray(data));
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: sequence of boolean); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.AddRange(ToObjArray(data));
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: sequence of word); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.AddRange(ToObjArray(data));
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: sequence of object); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.AddRange(data.ToArray);
end;


/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: integer); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.Add(data);
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: real); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.Add(data);
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: string); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.Add(data);
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: char); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.Add(data);
end;

/// Заполняет InputList в GenerateTestData. Вызывать только в GenerateTestData! 
procedure AddTestData(Self: List<object>; data: boolean); extensionmethod;
begin
  if TestMode <> tmGenTest then
    raise new NotInTestGenModeException('AddTestData');
  Self.Add(data);
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataInt(Self: List<object>): integer; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataInt');
  Result := IntTest;
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataReal(Self: List<object>): real; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataReal');
  Result := ReTest;
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataString(Self: List<object>): string; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataString');
  Result := StrTest;
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataChar(Self: List<object>): char; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataChar');
  Result := ChrTest;
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataBoolean(Self: List<object>): boolean; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataBoolean');
  Result := BooTest;
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataIntArr(Self: List<object>; n: integer): array of integer; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataIntArr');
  Result := (1..n).Select(x->IntTest).ToArray;
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataReArr(Self: List<object>; n: integer): array of real; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataReArr');
  Result := (1..n).Select(x->ReTest).ToArray;
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataStrArr(Self: List<object>; n: integer): array of string; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataStrArr');
  Result := (1..n).Select(x->StrTest).ToArray;
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataChrArr(Self: List<object>; n: integer): array of char; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataChrArr');
  Result := (1..n).Select(x->ChrTest).ToArray;
end;

/// Только для InputList и только во время тестирования!!! TestMode = tmTest! Это внутренний метод - разработчику тестов не вызывать!!!
function ReadTestDataBooArr(Self: List<object>; n: integer): array of boolean; extensionmethod;
begin
  if TestMode <> tmTest then
    raise new NotInTestModeException('ReadTestDataBooArr');
  Result := (1..n).Select(x->BooTest).ToArray;
end;


function SliceAsInt(Self: List<object>; a,b: integer): array of integer; extensionmethod 
  := Self[a:b].Select(x->integer(x)).ToArray;

function SliceAsReal(Self: List<object>; a,b: integer): array of real; extensionmethod 
  := Self[a:b].Select(x->real(x)).ToArray;

function SliceAsString(Self: List<object>; a,b: integer): array of string; extensionmethod 
  := Self[a:b].Select(x->string(x)).ToArray;

function SliceAsChar(Self: List<object>; a,b: integer): array of char; extensionmethod 
  := Self[a:b].Select(x->char(x)).ToArray;

function SliceAsBoolean(Self: List<object>; a,b: integer): array of boolean; extensionmethod 
  := Self[a:b].Select(x->boolean(x)).ToArray;
  
{=========================================================}
{           Сервисные функции - продолжение               }
{=========================================================}
procedure GenerateTests(params a: array of integer);
begin
  if InputList.Count <> 1 then
    raise new InputTestCountMismatchException;
  if InputList[0].GetType <> typeof(integer) then 
    raise new InputTestTypesMismatchException;
  
  TestCount := a.Length;
  GenerateTestData := tnum -> begin
    InputList.AddTestData(a[tnum-1]);
  end;
end;
  
procedure GenerateTests(params a: array of real);
begin
  if InputList.Count <> 1 then
    raise new InputTestCountMismatchException;
  if InputList[0].GetType <> typeof(real) then 
    raise new InputTestTypesMismatchException;
  
  TestCount := a.Length;
  GenerateTestData := tnum -> begin
    InputList.AddTestData(a[tnum-1]);
  end;
end;
  
procedure GenerateTests(params a: array of string);
begin
  if InputList.Count <> 1 then
    raise new InputTestCountMismatchException;
  if InputList[0].GetType <> typeof(string) then 
    raise new InputTestTypesMismatchException;
  
  TestCount := a.Length;
  GenerateTestData := tnum -> begin
    InputList.AddTestData(a[tnum-1]);
  end;
end;
  
procedure GenerateTests(params a: array of char);
begin
  if InputList.Count <> 1 then
    raise new InputTestCountMismatchException;
  if InputList[0].GetType <> typeof(char) then 
    raise new InputTestTypesMismatchException;
  
  TestCount := a.Length;
  GenerateTestData := tnum -> begin
    InputList.AddTestData(a[tnum-1]);
  end;
end;
  
procedure GenerateTests(params a: array of boolean);
begin
  if InputList.Count <> 1 then
    raise new InputTestCountMismatchException;
  if InputList[0].GetType <> typeof(boolean) then 
    raise new InputTestTypesMismatchException;
  
  TestCount := a.Length;
  GenerateTestData := tnum -> begin
    InputList.AddTestData(a[tnum-1]);
  end;
end;
  
procedure GenerateTests<T1,T2>(params a: array of (T1,T2));
begin
  if InputList.Count <> 2 then
    raise new InputTestCountMismatchException;
  if InputList[0].GetType <> typeof(T1) then 
    raise new InputTestTypesMismatchException;
  if InputList[1].GetType <> typeof(T2) then 
    raise new InputTestTypesMismatchException;
  
  TestCount := a.Length;
  GenerateTestData := tnum -> begin
    var tt := a[tnum-1];
    InputList.AddTestData(|object(tt[0]),object(tt[1])|);
  end;
end;

procedure GenerateTests<T1,T2,T3>(params a: array of (T1,T2,T3));
begin
  if InputList.Count <> 2 then
    raise new InputTestCountMismatchException;
  if InputList[0].GetType <> typeof(T1) then 
    raise new InputTestTypesMismatchException;
  if InputList[1].GetType <> typeof(T2) then 
    raise new InputTestTypesMismatchException;
  if InputList[2].GetType <> typeof(T3) then 
    raise new InputTestTypesMismatchException;
  
  TestCount := a.Length;
  GenerateTestData := tnum -> begin
    var tt := a[tnum-1];
    InputList.AddTestData(|object(tt[0]),object(tt[1]),object(tt[2])|);
  end;
end;

{=========================================================================}
{     Переопределенные функции PABCSystem с заполнением ввода и вывода    }
{=========================================================================}
/// Возвращает случайное целое в диапазоне от a до b
function Random(a, b: integer): integer;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataInt // считать следующее данное из заполненного в GenTestMode InputList
  else Result := PABCSystem.Random(a, b);
  
  if NeedAddDataToInputList then
    InputList.Add(Result);
end;

/// Возвращает случайное целое в диапазоне от 0 до n-1
function Random(n: integer): integer;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataInt
  else Result := PABCSystem.Random(n);
  if NeedAddDataToInputList then
    InputList.Add(Result);
end;

/// Возвращает случайное вещественное в диапазоне [0..1)
function Random: real;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataReal
  else Result := PABCSystem.Random;
  if NeedAddDataToInputList then
    InputList.Add(Result);
end;

/// Возвращает случайное вещественное в диапазоне [a,b)
function Random(a, b: real): real;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataReal
  else Result := PABCSystem.Random(a, b);
  if NeedAddDataToInputList then
    InputList.Add(Result);
end;

/// Возвращает случайный символ в диапазоне от a до b
function Random(a, b: char): char;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataChar
  else Result := PABCSystem.Random(a, b);
  if NeedAddDataToInputList then
    InputList.Add(Result);
end;

function RandomReal(a, b: real; digits: integer): real;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataReal
  else Result := PABCSystem.RandomReal(a, b, digits);
  if NeedAddDataToInputList then
    InputList.Add(Result);
end;

/// Возвращает случайное целое в диапазоне 
function Random(diap: IntRange): integer;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataInt
  else Result := PABCSystem.Random(diap);
  if NeedAddDataToInputList then
    InputList.Add(Result);
end;

/// Возвращает случайное вещественное в диапазоне 
function Random(diap: RealRange): real;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataReal
  else Result := PABCSystem.Random(diap);
  if NeedAddDataToInputList then
    InputList.Add(Result);
end;

/// Возвращает случайный символ в диапазоне 
function Random(diap: CharRange): char;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataChar
  else Result := PABCSystem.Random(diap);
  if NeedAddDataToInputList then
    InputList.Add(Result);
end;

/// Возвращает кортеж из двух случайных целых в диапазоне от a до b
function Random2(a, b: integer): (integer, integer);
begin
  Result := (Random(a, b), Random(a, b));
end;

/// Возвращает кортеж из двух случайных вещественных в диапазоне от a до b
function Random2(a, b: real): (real, real);
begin
  Result := (Random(a, b), Random(a, b));
end;

/// Возвращает кортеж из двух случайных символов в диапазоне от a до b
function Random2(a, b: char): (char, char);
begin
  Result := (Random(a, b), Random(a, b));
end;

/// Возвращает кортеж из двух случайных целых в диапазоне
function Random2(diap: IntRange): (integer, integer);
begin
  Result := (Random(diap), Random(diap));
end;

/// Возвращает кортеж из двух случайных символов в диапазоне
function Random2(diap: CharRange): (char, char);
begin
  Result := (Random(diap), Random(diap));
end;

/// Возвращает кортеж из двух случайных вещественных в диапазоне
function Random2(diap: RealRange): (real, real);
begin
  Result := (Random(diap), Random(diap));
end;

/// Возвращает кортеж из трех случайных целых в диапазоне от a до b
function Random3(a, b: integer): (integer, integer, integer);
begin
  Result := (Random(a, b), Random(a, b), Random(a, b));
end;

/// Возвращает кортеж из трех случайных вещественных в диапазоне от a до b
function Random3(a, b: real): (real, real, real);
begin
  Result := (Random(a, b), Random(a, b), Random(a, b));
end;

/// Возвращает кортеж из трех случайных символов в диапазоне от a до b
function Random3(a, b: char): (char, char, char);
begin
  Result := (Random(a, b), Random(a, b), Random(a, b));
end;

/// Возвращает кортеж из трех случайных целых в диапазоне
function Random3(diap: IntRange): (integer, integer, integer);
begin
  Result := (Random(diap), Random(diap), Random(diap));
end;

/// Возвращает кортеж из трех случайных вещественных в диапазоне
function Random3(diap: RealRange): (real, real, real);
begin
  Result := (Random(diap), Random(diap), Random(diap));
end;

/// Возвращает кортеж из трех случайных символов в диапазоне
function Random3(diap: CharRange): (char, char, char);
begin
  Result := (Random(diap), Random(diap), Random(diap));
end;

/// Возвращает массив размера n, заполненный случайными целыми значениями в диапазоне от a до b
function ArrRandomInteger(n: integer; a: integer; b: integer): array of integer;
begin
  // Есть три состояния:
  // 1. Вызов в основной программе (первый запуск) - TestMode = tmNone
  // 2. Вызов в основной программе (последующие запуски) - TestMode = tmTest
  // 3. Вызов в функции GenerateTestData - TestMode = tmGenTest - это только в состоянии TestMode = True
  {if GenTest then
  begin
    Result := PABCSystem.ArrRandomInteger(n, a, b); // приходится вызывать дважды!
    exit;
  end;}
  if TestMode = tmTest then
    Result := InputList.ReadTestDataIntArr(n)
  else Result := PABCSystem.ArrRandomInteger(n, a, b); 
  
  if NeedAddDataToInputList then // IsLightPT and (TestMode = tmNone)
  for var i:=0 to n-1 do
    InputList.Add(Result[i]);
end;

/// Возвращает массив размера n, заполненный случайными целыми значениями в диапазоне от 0 до 100
function ArrRandomInteger(n: integer): array of integer := ArrRandomInteger(n,0,100);

/// Возвращает массив размера n, заполненный случайными вещественными значениями в диапазоне от a до b 
function ArrRandomReal(n: integer; a: real; b: real; digits: integer): array of real;
begin
  if TestMode = tmTest then
    Result := InputList.ReadTestDataReArr(n)
  else Result := PABCSystem.ArrRandomReal(n, a, b, digits);
  
  if NeedAddDataToInputList then
  for var i:=0 to n-1 do
    InputList.Add(Result[i]);
end;

/// Возвращает массив размера n, заполненный случайными вещественными значениями  в диапазоне от 0 до 10
function ArrRandomReal(n: integer; digits: integer): array of real := ArrRandomReal(n,0,10,digits);

/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandomInteger(m: integer; n: integer; a: integer; b: integer): array [,] of integer;
begin
  if TestMode = tmTest then 
    Result := Matr(m,n,InputList.ReadTestDataIntArr(m*n))
  else Result := PABCSystem.MatrRandomInteger(m,n,a,b);

  if NeedAddDataToInputList then
  foreach var x in Result.ElementsByRow do
    InputList.Add(x);
end;

/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandomInteger(m: integer; n: integer): array [,] of integer := MatrRandomInteger(m,n,0,100);

/// Возвращает двумерный массив размера m x n, заполненный случайными вещественными значениями
function MatrRandomReal(m: integer; n: integer; a: real; b: real): array [,] of real;
begin
  if TestMode = tmTest then
    Result := Matr(m,n,InputList.ReadTestDataReArr(m*n))
  else Result := PABCSystem.MatrRandomReal(m,n,a,b);

  if NeedAddDataToInputList then
  foreach var x in Result.ElementsByRow do
    InputList.Add(x);
end;

/// Возвращает двумерный массив размера m x n, заполненный случайными вещественными значениями
function MatrRandomReal(m: integer; n: integer): array [,] of real := MatrRandomReal(m,n,0,10);

/// Выводит приглашение к вводу и возвращает значение типа integer, введенное с клавиатуры
function ReadInteger(prompt: string): integer;
begin
  Result := PABCSystem.ReadInteger(prompt);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  OutputList.RemoveAt(OutputList.Count - 1);
  CreateNewLineBeforeMessage := False;
end;

/// Выводит приглашение к вводу и возвращает значение типа integer, введенное с клавиатуры
function ReadlnInteger(prompt: string): integer;
begin
  Result := PABCSystem.ReadlnInteger(prompt);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  OutputList.RemoveAt(OutputList.Count - 1);
  CreateNewLineBeforeMessage := False;
end;

/// Выводит приглашение к вводу и возвращает два значения типа integer, введенные с клавиатуры
function ReadInteger2(prompt: string): (integer, integer);
begin
  Result := PABCSystem.ReadInteger2(prompt);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  OutputList.RemoveAt(OutputList.Count - 1);
  CreateNewLineBeforeMessage := False;
end;

function ReadString(prompt: string): string;
begin
  Result := PABCSystem.ReadString(prompt);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  OutputList.RemoveAt(OutputList.Count - 1);
  CreateNewLineBeforeMessage := False;
end;

function ReadlnString(prompt: string) := ReadString(prompt);


///- procedure Print(a,b,...);
/// Выводит значения a,b,... на экран, после каждого значения выводит пробел
procedure Print(params args: array of object);
begin
  foreach var ob in args do
  begin
    PABCSystem.Print(ob);
    if not IsPT then
      OutputList.RemoveAt(OutputList.Count - 1)
  end;
  CreateNewLineBeforeMessage := True;
end;

///- procedure Println(a,b,...);
/// Выводит значения a,b,... на экран, после каждого значения выводит пробел и переходит на новую строку
procedure Println(params args: array of object);
begin
  Print(args);
  Writeln;
  if IsPT then exit;
  CreateNewLineBeforeMessage := False;
end;

/// Выводит значение экран и выводит пробел
procedure Print(ob: object);
begin
  PABCSystem.Print(ob);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  CreateNewLineBeforeMessage := True;
end;

/// Выводит значение экран и выводит пробел
procedure Print(s: string);
begin
  PABCSystem.Print(s);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  CreateNewLineBeforeMessage := True;
end;

/// Выводит значение экран и выводит пробел
procedure Print(c: char);
begin
  PABCSystem.Print(object(c));
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  CreateNewLineBeforeMessage := True;
end;

// конец переопределенных функций с заполнением ввода-вывода  

procedure CompareTypeWithOutput(params a: array of System.Type);
begin
  TaskResult := Solved;
  var mn := Min(a.Length, OutputList.Count);
  for var i := 0 to mn - 1 do
    if a[i] <> OutputList[i].GetType then
      raise new OutputTypeException(i + 1, TypeToTypeName(a[i]), TypeName(OutputList[i]));
  if a.Length <> OutputList.Count then
    raise new OutputCountException(OutputList.Count, a.Length);
end;

/// Не меняет TaskResult если типы правильные
procedure CheckInputTypes(a: array of System.Type);
begin
  // Несоответствие количества вводимых параметров
  if a.Length <> InputList.Count then
    raise new InputCountException(InputList.Count, a.Length);
  for var i := 0 to a.Length - 1 do
    if a[i] <> InputList[i].GetType then
      raise new InputTypeException(i + 1, TypeToTypeName(a[i]), TypeName(InputList[i]));
end;

/// Синоним CheckInputTypes
procedure CheckInput(a: array of System.Type) := CheckInputTypes(a);
procedure CheckInputIsEmpty := CheckInputTypes(new System.Type[0]);

procedure CheckInputIsInitial;
begin
  CheckInput(InitialInputList.ToArray);
end; 

procedure CheckInput(seq: sequence of System.Type) := CheckInputTypes(seq.ToArray);

procedure CheckOutputAfterInitialSeq(seq: sequence of integer) := CheckOutputAfterInitial(ToObjArray(seq));
procedure CheckOutputAfterInitialSeq(seq: sequence of real) := CheckOutputAfterInitial(ToObjArray(seq));
procedure CheckOutputAfterInitialSeq(seq: sequence of string) := CheckOutputAfterInitial(ToObjArray(seq));
procedure CheckOutputAfterInitialSeq(seq: sequence of boolean) := CheckOutputAfterInitial(ToObjArray(seq));
procedure CheckOutputAfterInitialSeq(seq: sequence of char) := CheckOutputAfterInitial(ToObjArray(seq));
procedure CheckOutputAfterInitialSeq(seq: sequence of object) := CheckOutputAfterInitial(seq.ToArray);
procedure CheckOutputAfterInitialSeq(seq: ObjectList) := CheckOutputAfterInitial(seq.lst.ToArray);

procedure CheckOutputAfterInitialSeqSilent(seq: sequence of integer); begin Silent := True; CheckOutputAfterInitialSeq(seq); Silent := False; end;
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of real); begin Silent := True; CheckOutputAfterInitialSeq(seq); Silent := False; end;
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of string); begin Silent := True; CheckOutputAfterInitialSeq(seq); Silent := False; end;
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of boolean); begin Silent := True; CheckOutputAfterInitialSeq(seq); Silent := False; end;
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of char); begin Silent := True; CheckOutputAfterInitialSeq(seq); Silent := False; end;
procedure CheckOutputAfterInitialSeqSilent(seq: sequence of object); begin Silent := True; CheckOutputAfterInitialSeq(seq); Silent := False; end;
procedure CheckOutputAfterInitialSeqSilent(seq: ObjectList); begin Silent := True; CheckOutputAfterInitialSeq(seq); Silent := False; end;


procedure ClearOutputListFromSpaces;
begin
  OutputList := OutputList.Where(s -> (not (s is string)) or ((s as string) <> ' ')).ToList;
  OutputList := OutputList.Where(s -> (not (s is char)) or ((char(s)) <> ' ')).ToList;
end;

procedure FilterOnlyNumbers;
begin
  OutputList := OutputList.Where(x -> (x is integer) or (x is real) or (x is int64)).ToList;
end;

procedure FilterOnlyNumbersAndBools;
begin
  OutputList := OutputList.Where(x -> (x is integer) or (x is real) or (x is int64) or (x is boolean)).ToList;
end;

function MsgColorCode(color: MessageColorT): char;
begin
  Result := #65530; // Black
  case color of
    MsgColorGreen: Result := #65535;
    MsgColorRed: Result := #65534;
    MsgColorOrange: Result := #65533;
    MsgColorMagenta: Result := #65532;
    MsgColorGray: Result := #65531;
  end;
end;

procedure ColoredMessage(msg: string; color: MessageColorT);
begin
  if Silent then exit;

  if CreateNewLineBeforeMessage then
    Console.WriteLine;
  Console.WriteLine(MsgColorCode(color) + msg);
  CreateNewLineBeforeMessage := False;
end;

procedure ColoredMessage(msg: string);
begin
  if Silent then exit;
  if CreateNewLineBeforeMessage then
    Console.WriteLine;
  Console.WriteLine(MsgColorCode(MsgColorRed) + msg);
  CreateNewLineBeforeMessage := False;
end;

procedure CheckOutputHelper(i0: integer; params arr: array of object);
  procedure OutputTestResult;
  begin
    ColoredMessage($'Основной запуск верный',MsgColorGray);
    ColoredMessage($'Ошибочное решение на тесте:',MsgColorOrange);
    ColoredMessage($'Тестовые данные      : {InputList.JoinToString}',MsgColorGray);
    ColoredMessage($'Полученный результат : {OutputList.JoinToString}',MsgColorGray);
    if i0 = 0 then
      ColoredMessage($'Правильный результат : {arr.JoinToString}',MsgColorGray)
    else ColoredMessage($'Правильный результат : {(OutputList[:i0]+arr).JoinToString}',MsgColorGray)
  end;

begin
  if (TaskResult = InitialTask) and CancelMessagesIfInitial
     or (TaskResult = BadInitialTask) then
    exit;

  // Если мы попали сюда, то OutputList.Count >= InitialOutputList.Count
  var mn := Min(arr.Length, OutputList.Count - i0);
  
  for var i := i0 to i0 + mn - 1 do
  begin  
    // Если типы не совпадают 
    if (arr[i-i0].GetType.Name <> 'RuntimeType') and (arr[i-i0].GetType <> OutputList[i].GetType) or
      (arr[i-i0].GetType.Name = 'RuntimeType') and (arr[i-i0] <> OutputList[i].GetType)
    then 
    begin
      if TestNumber > 0 then
        OutputTestResult
      else
      begin
        if i > InitialOutputList.Count then
          ColoredMessage('Часть выведенных данных правильная',MsgColorGray);
      end;  
      raise new OutputTypeException(i + 1, TypeToTypeName(arr[i-i0].GetType), TypeName(OutputList[i]));           
    end;
    // Если значения не совпадают (если задан маркер типа, то проверка значений пропускается)
    if (arr[i-i0].GetType.Name <> 'RuntimeType') and not CompareValues(arr[i-i0], OutputList[i]) then
    begin
      if i >= InitialOutputList.Count then // ? Если первое данное неправильное - всё равно попадаем сюда!!!
      begin
        if TestNumber > 0 then
          OutputTestResult
        else
        begin
          if i > InitialOutputList.Count then
            ColoredMessage('Часть выведенных данных правильная',MsgColorGray);
          if (i0 = 0) and (arr.Length = 1) then
            //ColoredMessage($'Ожидалось значение {arr[i-i0]}, а выведено {OutputList[i]}',MsgColorGray)           
          else ColoredMessage($'Элемент {i + 1}: ожидалось значение {arr[i-i0]}, а выведено {OutputList[i]}',MsgColorGray);
        end;  
      end;  
      TaskResult := BadSolution;
      exit;           
    end;
  end;
  
  if arr.Length <> OutputList.Count - i0 then
  begin  
    if TestNumber > 0 then
      OutputTestResult
    else
    if OutputList.Count > 0 then begin
      if arr.Length > OutputList.Count - i0 then // выведено меньше чем надо
        ColoredMessage('Все выведенные данные правильны',MsgColorGray)
      else if arr.Length < OutputList.Count - i0 then // выведено больше чем надо
        ColoredMessage('Все необходимые выведенные данные правильны',MsgColorGray);
    end;  
    raise new OutputCountException(OutputList.Count, arr.Length + i0);
  end;
  TaskResult := Solved;  
end;

// Поправим сообщения об ошибке в соответствии с TestNumber
procedure CheckOutput(params arr: array of object);
begin
  CheckOutputHelper(0,arr);
end;

procedure CheckOutputSilent(params arr: array of object);
begin
  Silent := True;
  CheckOutput(arr);
  Silent := False;
end;

procedure CheckOutputAfterInitial(params arr: array of object);
begin
  CheckOutputHelper(InitialOutputList.Count,arr);
end;

procedure CheckOutputAfterInitialSilent(params arr: array of object);
begin
  Silent := True;
  CheckOutputAfterInitial(arr);
  Silent := False;
end;

procedure CheckOutputSeq(a: sequence of integer) := CheckOutput(ToObjArray(a));
procedure CheckOutputSeq(a: sequence of real) := CheckOutput(ToObjArray(a));
procedure CheckOutputSeq(a: sequence of string) := CheckOutput(ToObjArray(a));
procedure CheckOutputSeq(a: sequence of char) := CheckOutput(ToObjArray(a));
procedure CheckOutputSeq(a: sequence of boolean) := CheckOutput(ToObjArray(a));
procedure CheckOutputSeq(a: sequence of object) := CheckOutput(a.ToArray);
procedure CheckOutputSeq(a: ObjectList) := CheckOutput(a.lst.ToArray);
procedure CheckOutputSeq(a: sequence of word) := CheckOutputSeq(ToObjArray(a));

procedure CheckOutputSeqSilent(a: sequence of integer) := begin Silent := True; CheckOutputSeq(a); Silent := False end;
procedure CheckOutputSeqSilent(a: sequence of real) := begin Silent := True; CheckOutputSeq(a); Silent := False end;
procedure CheckOutputSeqSilent(a: sequence of string) := begin Silent := True; CheckOutputSeq(a); Silent := False end;
procedure CheckOutputSeqSilent(a: sequence of char) := begin Silent := True; CheckOutputSeq(a); Silent := False end;
procedure CheckOutputSeqSilent(a: sequence of boolean) := begin Silent := True; CheckOutputSeq(a); Silent := False end;
procedure CheckOutputSeqSilent(a: sequence of object) := begin Silent := True; CheckOutputSeq(a); Silent := False end;
procedure CheckOutputSeqSilent(a: ObjectList) := begin Silent := True; CheckOutputSeq(a); Silent := False end;
procedure CheckOutputSeqSilent(a: sequence of word) := begin Silent := True; CheckOutputSeq(a); Silent := False end;

procedure CheckOutputNew(params arr: array of object) := CheckOutput(arr);
procedure CheckOutputSeqNew(a: sequence of integer) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of real) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of string) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of char) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of boolean) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of object) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of word) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: ObjectList) := CheckOutputSeq(a);

procedure CheckOutputString(str: string);
  function Char2Str(c: char): string;
  begin
    if (c = #10) or (c = #13) then
      Result := '''ПереходНаНовуюСтроку'''
    else Result := '''' + c + '''';
  end;
begin
  if (TaskResult = InitialTask) or (TaskResult = BadInitialTask) then
    exit;
  var ostr := OutputString.ToString;
  var mn := Min(str.Length, ostr.Length);
  
  var ind := -1;
  for var i := 1 to mn do
  begin
    if str[i] <> ostr[i] then
    begin
      ind := i;
      ColoredMessage($'Элемент {i}: ожидался символ {Char2Str(str[i])}, а выведен символ {Char2Str(ostr[i])}',MsgColorGray);
      TaskResult := BadSolution;
      exit;           
    end;
  end;
  if ind = -1 then
    ind := mn;

  if str.Length <> ostr.Length then
  begin  
    if (ind = mn) and (ind<>0) then
      ColoredMessage('Все выведенные данные правильны',MsgColorGray)
    else if ind > 0 then
      ColoredMessage('Часть выведенных данных правильная',MsgColorGray);
    raise new OutputCountException(ostr.Length, str.Length);
  end;

  TaskResult := Solved;
end;


type 
  IntAr = array of integer;
  RealAr = array of real;
  IntAr2 = array [,] of integer;
  RealAr2 = array [,] of real;
  
function FlattenElement(x: object): List<object>; 
begin
  var lst := new List<object>;
  if x is IntAr (var iarr) then
    lst.AddRange(iarr.Select(x -> object(x)))
  else if x is RealAr (var rarr) then
    lst.AddRange(rarr.Select(x -> object(x)))
  else if x is IntAr2 (var iarr) then
    lst.AddRange(iarr.ElementsByRow.Select(x -> object(x)))
  else if x is RealAr2 (var rarr) then
    lst.AddRange(rarr.ElementsByRow.Select(x -> object(x)))
  else if x is List<integer> (var larr) then
    lst.AddRange(larr.Select(x -> object(x)))
  else if x is List<real> (var lrarr) then
    lst.AddRange(lrarr.Select(x -> object(x)))
  else lst.Add(x);
  Result := lst;
end;

procedure FlattenOutput;
begin
  // если в OutputList массивы, вытянуть их в единый список
  OutputList := OutputList.SelectMany(x -> FlattenElement(x)).ToList;
end;

function NValues(n: integer): string;
begin
  case n of
    0: Result := n + ' значений';
    1: Result := n + ' значение';
    2, 3, 4: Result := n + ' значения';
    5..100000: Result := n + ' значений';
  end;
end;

{=========================================================}
{          Итоговые функции для проверки решения          }
{=========================================================}

procedure RobotCheckSolution;
begin
  TaskResult := BadSolution;
  var t := System.Type.GetType('RobotField.RobotField');
  if t<>nil then
  begin
    TaskResultInfo := 'Robot';
    var f := t.GetField('RobField',System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.Static);
    var IsSol := f.FieldType.GetMethod('IsSolution');
    var bool := IsSol.Invoke(f.GetValue(nil),nil);
    if boolean(bool) then
      TaskResult := Solved;
  // RobField.TaskName
    var RBTaskNameInfo := f.FieldType.GetField('TaskName');
    var v := string(RBTaskNameInfo.GetValue(f.GetValue(nil)));
    // Добавлять RB к имени задания если его там нет
    if not v.ToLower.StartsWith('rb') then
      v := v.Insert(0,'RB');
    TaskName := v;
  end;
end;

procedure DrawmanCheckSolution;
begin
  TaskResult := BadSolution;
  var t := System.Type.GetType('DrawManField.DrawManField');
  if t<>nil then
  begin
    TaskResultInfo := 'Drawman';
    var f := t.GetField('DMField',System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.Static);
    var IsSol := f.FieldType.GetMethod('IsSolution');
    var bool := IsSol.Invoke(f.GetValue(nil),nil);
    if boolean(bool) then
      TaskResult := Solved;
  // DMField.TaskName
    var DMTaskNameInfo := f.FieldType.GetField('TaskName');
    var v := string(DMTaskNameInfo.GetValue(f.GetValue(nil)));
    // Добавлять DM к имени задания если его там нет
    if not v.ToLower.StartsWith('dm') then
      v := v.Insert(0,'DM');
    TaskName := v;
  end;
end;

// Вызов PT4.PT4.__FinalizeModule__ отражением
procedure PT4CheckSolution;
begin
  var t := System.Type.GetType('PT4.PT4');
  if t<>nil then
  begin
    var meth := t.GetMethod('__FinalizeModule__',System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.Static);
    if meth <> nil then
      meth.Invoke(nil,nil);
    //
    var f := t.GetField('TaskName',System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.Static);
    TaskName := string(f.GetValue(nil));
  end;
end;

// Вызов PT4.PT4.GetSolutionInfo отражением
function GetSolutionInfoPT4: string;
begin
  var t := System.Type.GetType('PT4.PT4');
  if t<>nil then
  begin
    var meth := t.GetMethod('GetSolutionInfo',System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.Static);
    if meth <> nil then
      Result := string(meth.Invoke(nil,nil))
    else Result := '';
  end;
end;

procedure CalcPT4Result(info: string; var TaskResult: TaskStatus; var TaskResultInfo: string);
// info - строка, возвращаемая GetSolutionInfoPT4
  
  function HasSubstr(s_ru, s_en: string): boolean;
  begin
    result := false;
    if Pos(s_ru, info) > 0 then
      result := true
    else if Pos(s_en, info) > 0 then
      result := true
  end;
  
  function TypeNamePT(s: string): string;
  begin
    result := '';
    case s of
      'логического типа', 'of logical type':
      result := 'boolean';
      'целого типа', 'of integer type':
      result := 'integer';
      'вещественного типа', 'of real-number type':
      result := 'real';
      'символьного типа', 'of character type':
      result := 'char';
      'строкового типа', 'of string type':
      result := 'string';
    end;
    if result = '' then
      if s.startswith('типа') then
        result := copy(s, 6, 100)
      else if s.startswith('of') then
      begin
        delete(s, 1, 3);
        delete(s, length(s) - 4, 100);
        result := s;
      end; 
  end;
  
  procedure ExtractParts(var p1, p2, p3: string);
  begin
    var m := Regex.Match(info, 'Для ввода (.*)-го элемента \((.*)\) использована переменная (.*).');
    if m.Success then
    begin
      p1 := m.Groups[1].Value;
      p2 := TypeNamePT(m.Groups[2].Value);
      p3 := TypeNamePT(m.Groups[3].Value);
      exit;
    end;
    m := Regex.Match(info, 'A variable (.*) is used for input of data item with the order number (.*) \((.*)\).');
    if m.Success then
    begin
      p1 := m.Groups[2].Value;
      p2 := TypeNamePT(m.Groups[3].Value);
      p3 := TypeNamePT(m.Groups[1].Value);
      exit;
    end;
    
    m := Regex.Match(info, 'Для вывода (.*)-го элемента \((.*)\) использовано выражение (.*).');
    if m.Success then
    begin
      p1 := m.Groups[1].Value;
      p2 := TypeNamePT(m.Groups[2].Value);
      p3 := TypeNamePT(m.Groups[3].Value);
      exit;
    end;
    m := Regex.Match(info, 'An expression (.*) is used for output of data item with the order number (.*) \((.*)\).');
    if m.Success then
    begin
      p1 := m.Groups[2].Value;
      p2 := TypeNamePT(m.Groups[3].Value);
      p3 := TypeNamePT(m.Groups[1].Value);
      exit;
    end;  
    
    m := Regex.Match(info, 'Количество выведенных данных: (.*) \(из (.*)\).');
    if m.Success then
    begin
      p1 := m.Groups[1].Value;
      p2 := m.Groups[2].Value;
      p3 := '';
      exit;
    end;
    m := Regex.Match(info, 'The program has output (.*) data item\(s\) \(the amount of the required items is (.*)\).');
    if m.Success then
    begin
      p1 := m.Groups[1].Value;
      p2 := m.Groups[2].Value;
      p3 := '';
      exit;
    end;
    
    m := Regex.Match(info, 'Количество прочитанных данных: (.*) \(из (.*)\).');
    if m.Success then
    begin
      p1 := m.Groups[1].Value;
      p2 := m.Groups[2].Value;
      p3 := '';
      exit;
    end;
    m := Regex.Match(info, 'The program has used (.*) input data item\(s\) \(the amount of the required items is (.*)\).');
    if m.Success then
    begin
      p1 := m.Groups[1].Value;
      p2 := m.Groups[2].Value;
      p3 := '';
      exit;
    end;
  end;

begin
  // Если мы сюда зашли, то TaskResult уже под контролем. Но она и так должна быть TaskStatus.InitialTaskPT4
  TaskResult := TaskStatus.InitialTaskPT4;
  TaskResultInfo := 'PT4';
  var p1, p2, p3: string;
  if info = '' then
    exit;
  if HasSubstr('Задание выполнено', 'The task is solved') then
    //ColoredMessage('Задание выполнено', MsgColorGreen)
    TaskResult := TaskStatus.Solved
  else if HasSubstr('Ошибочное решение', 'Wrong solution') then
    //ColoredMessage('Неверное решение')
    TaskResult := TaskStatus.BadSolution
  else if HasSubstr('Неверно указан тип при выводе результатов', 'Invalid type is used for an output data item') then
  begin
    ExtractParts(p1, p2, p3);
    TaskResult := TaskStatus.IOError;
    TaskResultInfo := $'OutputType({p1},{p2},{p3})';
    //ColoredMessage($'Ошибка вывода. При выводе {p1}-го элемента типа {p2} выведено значение типа {p3}');
  end
  else if HasSubstr('Выведены не все результирующие данные', 'Some data are not output') then  
  begin
    ExtractParts(p1, p2, p3);
    TaskResult := TaskStatus.IOError;
    TaskResultInfo := $'OutputCount({p1},{p2})';
    //ColoredMessage($'Выведено {p1}, а требуется вывести {p2}', MsgColorOrange);
  end
  else if HasSubstr('Неверно указан тип при вводе исходных данных', 'Invalid type is used for an input data item') then
  begin
    ExtractParts(p1, p2, p3);
    TaskResult := TaskStatus.IOError;
    TaskResultInfo := $'InputType({p1},{p2},{p3})';
    //ColoredMessage($'Ошибка ввода. При вводе {p1}-го элемента типа {p2} использована переменная типа {p3}');
  end
  else if HasSubstr('Введены не все требуемые исходные данные', 'Some required data are not input.') then  
  begin
    ExtractParts(p1, p2, p3);
    TaskResult := TaskStatus.IOError;
    TaskResultInfo := $'InputCount({p1},{p2})';
    //ColoredMessage($'Введено {p1}, а требуется ввести {p2}', MsgColorOrange);
  end
  else if HasSubstr('Запуск с правильным вводом данных', 'Correct data input') then  
  begin
    TaskResult := TaskStatus.PartialSolution;
    //ColoredMessage('Запуск с правильным вводом данных', MsgColorGray)
  end;
  {else
        ColoredMessage('PT4: '+info.Replace(#13#10, ' ').TrimStart().TrimEnd('.',' '));}
end;

function ConvertTaskName(tn: string): string;
begin
  var TName := tn;
  // Если есть номер с подчеркиванием в начале, то отбросить его
  var ind := TName.IndexOf('_');
  if (ind >= 0) and TName[1].IsDigit then // Значит, в начале - номер, он служит лишь для упорядочения файлов
    TName := TName.Remove(0,ind + 1);
  if TName.ToLower in TaskNamesMap then
    TName := TaskNamesMap[TName.ToLower];
  Result := TName;
end;

procedure ClearLists;
begin
  CurPosInInputList := 0;
  CurPosInInputListInTestMode := 0;
  OutputString.Clear;
  OutputList.Clear;
  InputList.Clear;
  InitialOutputList.Clear; 
  InitialInputList.Clear;  
end; 

function InitSolveProcPair: (System.Reflection.MethodInfo, System.Reflection.MethodInfo);
begin
  // Взял из PT.pas
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
  Result := (InitProc,SolveProc);  
end; 

procedure CheckMyPT;
begin
  if CheckTask = nil then
    exit;
  var TName := TaskName;
  try
    TName := ConvertTaskName(TaskName);
    TestMode := tmNone;
    TestNumber := 0;
    CheckTask(TName); // может выдавать сообщения, предваряющие неверное решение, в CheckOutput. 
    if {not IsPT and not IsRobot and not IsDrawMan and} (TestCount > 0) then // То это LightPT - т.к. только в LightPT TestCount м.б. > 0
    begin
      var (InitProc,SolveProc) := InitSolveProcPair;
        
      if (GenerateTestData <> nil) and (TaskResult = Solved) then
        for var i:=1 to TestCount do
        begin
          TestNumber := i;
          ClearLists;
          TestMode := tmGenTest;
          GenerateTestData(i);
          
          if TestMode<>tmAutoTest then
            TestMode := tmTest;
          try
            if InitProc<>nil then
              InitProc.Invoke(nil,nil);
            SolveProc.Invoke(nil,nil);
          except
            on e: System.Reflection.TargetInvocationException do
              raise e.InnerException;
          end;
          //InputList := InputList; 
          //OutputList := OutputList; 
          
          CheckTask(TName);
          if TaskResult = BadSolution then
            break; // хоть один тест неудачный - выходим!
          // Подумать над выводом ошибки при тестах
        end;
    end;
    Silent := False;
    // Если это задача из задачника, то результат будет NotUnderControl. И дальше необходимо это преобразовывать
    case TaskResult of
      Solved: ColoredMessage('Задание выполнено', MsgColorGreen);
      BadSolution: begin
        ColoredMessage('Неверное решение');
      end;
      InitialTask: ;
      BadInitialTask: ColoredMessage('Вы удалили часть кода - восстановите его!', MsgColorMagenta);
    end;
  except
    // При тестировании тут вряд ли будут ошибки. Но могут
    on e: OutputTypeException do
    begin
      Silent := False;
      ColoredMessage($'Ошибка вывода. При выводе {e.n}-го элемента типа {e.ExpectedType} выведено значение типа {e.ActualType}'); 
    end;
    on e: OutputCountException do
    begin
      Silent := False;
      if e.Count = 0 then
        ColoredMessage($'Требуется вывести {NValues(e.n)}', MsgColorGray)
      else ColoredMessage($'Выведено {NValues(e.Count)}, а требуется вывести {e.n}', MsgColorOrange); 
    end;
    on e: OutputCount2Exception do
    begin
      Silent := False;
      if e.Count = 0 then
        ColoredMessage($'Требуется вывести по крайней мере {NValues(e.i)}', MsgColorGray)
      else ColoredMessage($'Выведено {NValues(e.Count)}, а требуется вывести по крайней мере {e.i}', MsgColorOrange); 
    end;
    on e: InputTypeException do
    begin
      Silent := False;
      ColoredMessage($'Ошибка ввода. При вводе {e.n}-го элемента типа {e.ExpectedType} использована переменная типа {e.ActualType}'); 
    end;
    on e: InputCountException do
    begin
      Silent := False;
      if e.Count = 0 then
        ColoredMessage($'Требуется ввести {NValues(e.n)}', MsgColorGray)
      else if e.n <> 0 then 
        ColoredMessage($'Введено {NValues(e.Count)}, а требуется ввести {e.n}', MsgColorOrange)
      else ColoredMessage($'Введено {NValues(e.Count)}, хотя ничего вводить не требуется', MsgColorOrange); 
    end;
    on e: InputCount2Exception do
    begin
      Silent := False;
      if e.Count = 0 then
        ColoredMessage($'Требуется ввести по крайней мере {NValues(e.i)}', MsgColorGray)
      else ColoredMessage($'Введено {NValues(e.Count)}, а требуется ввести по крайней мере {e.i}', MsgColorOrange); 
    end;
    on e: NotInTestModeException do
    begin
      Silent := False;
      ColoredMessage($'Метод {e.FuncName} может вызываться только в режиме тестирования вне GenerateTestData! Он - внутренний и не предназначен для вызова разработчиком теста!', MsgColorOrange); 
    end;
    on e: NotInTestGenModeException do
    begin
      Silent := False;
      ColoredMessage($'Метод {e.FuncName} может вызываться только в режиме генерации тестов (в GenerateTestData)!', MsgColorOrange); 
    end;
    on e: InputCountTest2Exception do
    begin
      Silent := False;
      ColoredMessage($'Неверно составлен тест! Введено {NValues(e.Count)}, а требуется ввести по крайней мере {e.i}. Возможно, неверно заполнен InputList в GenerateTestData', MsgColorOrange); 
    end;
    on e: InputTypeTestException do
    begin
      Silent := False;
      ColoredMessage($'Неверно составлен тест! При вводе {e.n}-го элемента типа {e.ExpectedType} использована переменная типа {e.ActualType}. Возможно, неверно заполнен InputList в GenerateTestData'); 
    end;
    on e: InputTestCountMismatchException do
    begin
      Silent := False;
      ColoredMessage($'Неверно составлен тест! Несоответствие количества входных данных в тесте и основном запуске. Возможно, неверно заполнен InputList в GenerateTestData'); 
    end;
    on e: InputTestTypesMismatchException do
    begin
      Silent := False;
      ColoredMessage($'Неверно составлен тест! Несоответствие типов входных данных в тесте и основном запуске. Возможно, неверно заполнен InputList в GenerateTestData'); 
    end;
  end;
  // Для задачника надо вызывать процедуру __FinalizeModule__ из модуля PT4 jnhf;tybtv, а в модуле PT4 эту процедуру тогда не вызывть
  // Для задачника в CheckTaskPT надо сказать, что проверяется задача из задачника. И выводить на экран ничего не надо - только в базу.
  TaskResultInfo := TaskException.Info;
  
  var TaskPlatform := 'LT';
  
  // Теперь тщательно проверяем задачник и исполнителей
  if IsPT then
  begin
    PT4CheckSolution;
    TName := TaskName;
    var info := GetSolutionInfoPT4;
    CalcPT4Result(info,TaskResult,TaskResultInfo);
    TaskPlatform := 'PT';
  end
  else if IsRobot then
  begin  
    RobotCheckSolution;
    TName := TaskName;
    TaskPlatform := 'RB';
  end  
  else if IsDrawman then
  begin  
    DrawmanCheckSolution;
    TName := TaskName;
    TaskPlatform := 'DM';
  end;
  // Хотелось бы писать в БД для Робота и др. имя задания в Task
  if WriteInfoCallBack<>nil then
    WriteInfoCallBack(LessonName,TName,TaskPlatform,TaskResult,TaskResultInfo);
end;

procedure LoadLightPTInfo;
begin
  try
    var lines := ReadAllLines(lightptname);
    foreach var line in lines do
    begin
      if line.Trim = '' then 
        continue;
      if LessonName = '' then
        LessonName := line.ToWords.First // первое слово
      else begin
        var (name1,name2) := Regex.Split(line,'->');
        TaskNamesMap[name1.Trim.ToLower] := name2.Trim;
      end;
    end;
    if LessonName = '' then
    begin
      // Имя текущей папки. Плохо - в lightpt забыли написать имя урока
      //var ttt := ExtractFileDir(System.Environment.GetCommandLineArgs[0]);
      var ttt := ExpandFileName('.');
      var LastDir := ttt.ToWords(System.IO.Path.DirectorySeparatorChar).LastOrDefault;
      // Каталог может содержать пробелы. Брать первое слово
      if LastDir<>nil then
        LessonName := LastDir.ToWords.First;
    end;
  except
    
  end;
end;

{=========================================================}
{            Процедуры для записи в базы данных           }
{=========================================================}

procedure WriteInfoToRemoteDatabase(auth: string; LessonName, TaskName, TaskPlatform, TaskResult, text, AdditionalInfo: string);
begin
  // Считать логин пароль из auth
  var data := System.IO.File.ReadAllBytes(auth);
  var arr := Decrypt(data).Split(#10);
  var login,pass: string;
  if arr.Length >= 2 then
  begin
    login := arr[0];
    pass := arr[1];
    // Теперь как-то записать в БД информацию
    var User := new ServerAccessProvider(ServerAddr);
    var t2 := User.SendPostRequest(login, pass, LessonName, TaskName, TaskPlatform, TaskResult, text, AdditionalInfo);
    var v := t2.Result;
    v := v;
    if v <> 'Success' then
      ColoredMessage('Ошибка сервера: '+v, MsgColorGray);
    //Console.WriteLine(v);
  end;
end;

procedure WriteInfoToDatabases(LessonName,TaskName,TaskPlatform: string; TaskResult: TaskStatus; AdditionalInfo: string := '');
begin
  try
    System.IO.File.AppendAllText('db.txt', $'{LessonName} {TaskName} {dateTime.Now.ToString(''u'')} {TaskResult.ToString} {AdditionalInfo}' + #10);
  except
    on e: Exception do
      ColoredMessage('Ошибка записи в файл db.txt. Обратитесь к преподавателю',MsgColorGray);
  end; 
  // Разделили ошибки записи в локальную и глобальную базу
  try
    var auth := FindAuthDat(); // файл авторизации ищется либо в текущей папке либо в папке на уровень выше
    var args := System.Environment.GetCommandLineArgs;
    if (auth <> '') and (args.Length >= 3) and (args[2].ToLower = 'true') then
    begin  
      var text := '';
      if (TaskResult <> InitialTask) and (args.Length >= 4) then
        text := args[3];
      // Есть проблема паузы при плохой сети 
      WriteInfoToRemoteDatabase(auth,LessonName,TaskName,TaskPlatform,TaskResult.ToString, text, AdditionalInfo);
    end  
  except
    on e: System.AggregateException do
    begin
      foreach var x in e.InnerExceptions do
        if x is HTTPRequestException then
        begin
          if x.InnerException<>nil then
            ColoredMessage('Ошибка сервера: '+x.InnerException.Message,MsgColorGray)
          else ColoredMessage('Неизвестная ошибка сервера',MsgColorGray)
        end  
        else ColoredMessage('Ошибка сервера: '+x.Message,MsgColorGray); 
    end;
    on e: Exception do
      ColoredMessage(e.Message,MsgColorGray);
  end;  
end;

{===========================================================}
{        Переопределенные подсистемы ввода и вывода         }
{===========================================================}
type
  IOLightSystem = class(__ReadSignalOISystem)
  public
    procedure write(obj: object); override;
    begin
      if TestMode = tmNone then
        inherited write(obj);
      OutputString += _ObjectToString(obj);
      OutputList += obj;
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure writeln; override;
    begin
      if TestMode = tmNone then
        inherited writeln;
      OutputString += NewLine;
      CreateNewLineBeforeMessage := False;
    end;
    
    function ReadLine: string; override;
    begin
      if TestMode = tmTest then
        Result := InputList.ReadTestDataString
      else Result := inherited ReadLine;
      
      if not NeedAddDataToInputList then exit;
        
      InputList.Add(Result);
      CreateNewLineBeforeMessage := False;
    end;
    
    procedure readln; override;
    begin
      if TestMode = tmTest then
        
      else inherited readln;
      CreateNewLineBeforeMessage := False;
    end;
    
    procedure read(var x: integer); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataInt
      else inherited Read(x);

      if not NeedAddDataToInputList then exit;
        
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: real); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataReal
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: char); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataChar
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: string); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataString
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: byte); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataInt
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: shortint); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataInt
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: smallint); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataInt
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: word); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataInt
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: longword); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataInt
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: int64); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataInt
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: uint64); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataInt
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: single); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataReal
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: boolean); override;
    begin
      if TestMode = tmTest then
        x := InputList.ReadTestDataBoolean
      else inherited Read(x);
      
      if not NeedAddDataToInputList then exit;

      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: BigInteger); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
  end;

initialization
  // Если LightPT добавляется в конец uses, то ее секция инициализации вызывается первой
  // В этом случае ввод-вывод обязательно переключается, но потом он перекрывается и в основной программе не срабатывает
  // Но в CheckPT используется ColoredMessage, которая выводит с помощью Console.WriteLine - ей всё равно
  var tn := TypeName(CurrentIOSystem);
  if (tn = 'IOStandardSystem') or (tn = '__ReadSignalOISystem') or (tn = 'IOGraphABCSystem') then
    CurrentIOSystem := new IOLightSystem;
  WriteInfoCallBack := WriteInfoToDatabases;
  // Расшифровка LightPT.dat  
  LoadLightPTInfo;
finalization
  CheckMyPT  
end.