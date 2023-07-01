/// Модуль LightPT автоматической легковесной проверки заданий
unit LightPT;
{$reference System.Net.Http.dll}
{$reference System.Security.dll}

interface

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
function ArrRandomReal(n: integer; a: real; b: real): array of real;

/// Возвращает массив размера n, заполненный случайными вещественными значениями  в диапазоне от 0 до 10
function ArrRandomReal(n: integer): array of real;

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

procedure ColoredMessage(msg: string; color: MessageColorT);

procedure ColoredMessage(msg: string);

{=========================================================================}
{        Основные процедуры для проверки правильности ввода-вывода        }
{=========================================================================}

// Самые часто используемые: CheckInput, CheckOutput, CheckOutputSeq

/// Проверить типы вводимых данных
procedure CheckInput(a: array of System.Type);
/// Проверить значения при выводе
procedure CheckOutput(params arr: array of object);

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
  PTException = class(Exception) 
    function Info: string; virtual := 'NoInfo';
  end;

var
  CreateNewLineBeforeMessage := False;
  TaskResultInfo: string; // доп. информация о результате. Как правило пуста. Или содержит TaskException.Info. Или содержит для Solved и BadSolution информацию о модуле: Robot, Drawman, PT4
  TaskException: PTException := new PTException;

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
    
    function SendPostRequest(FullFIO, Password, LessonName, TaskName, TaskPlatform, TaskResult, TaskResultInfo: string): Task<string>;
    begin
      var values := Dict(
        ( 'shortFIO', '' ),  
        ( 'FIO', FullFIO ),
        ( 'taskName', TaskName ),
        ( 'lessonName', LessonName ),
        ( 'taskPlatform', TaskPlatform ),
        ( 'taskResult', TaskResult ),
        ( 'taskResultInfo', TaskResultInfo ),
        ( 'content', '' ),
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
  InputCountException = class(PTException) // Ровно Count 
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
  InputCount2Exception = class(PTException) // Не меньше Count
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
  InputTypeException = class(PTException)
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
  OutputCountException = class(PTException) // Ровно Count 
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
  OutputCount2Exception = class(PTException) // Ровно Count 
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
  OutputTypeException = class(PTException)
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

var
  Cur := 0;
  TaskName := ExtractFileName(System.Environment.GetCommandLineArgs[0]).Replace('.exe', '');

{=========================================================}
{                    Сервисные функции                    }
{=========================================================}

procedure ErrorInputCount(Count, n: integer) := raise new InputCountException(Count,n);

procedure ErrorOutputCount(Count, n: integer) := raise new OutputCountException(Count,n);

procedure ErrorInputType(n: integer; ExpectedType, ActualType: string)
  := raise new InputTypeException(n,ExpectedType,ActualType);

procedure ErrorOutputType(n: integer; ExpectedType, ActualType: string)
  := raise new OutputTypeException(n,ExpectedType,ActualType);

/// Является ли задание заданием для задачника PT
function IsPT := System.Type.GetType('PT4.PT4') <> nil;

/// Является ли задание заданием для Робота
function IsRobot := System.Type.GetType('RobotField.RobotField');

/// Является ли задание заданием для Чертежника
function IsDrawman := System.Type.GetType('DrawManField.DrawManField');

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
  Result := Int(Cur);
  Cur += 1;
end;

function Re: real;
begin
  Result := Re(Cur);
  Cur += 1;
end;

function Str: string;
begin
  Result := Str(Cur);
  Cur += 1;
end;

function Boo: boolean;
begin
  Result := Boo(Cur);
  Cur += 1;
end;

function Chr: char;
begin
  Result := Chr(Cur);
  Cur += 1;
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

{=========================================================================}
{     Переопределенные функции PABCSystem с заполнением ввода и вывода    }
{=========================================================================}
/// Возвращает случайное целое в диапазоне от a до b
function Random(a, b: integer): integer;
begin
  Result := PABCSystem.Random(a, b);
  if IsPT then exit;
  InputList.Add(Result);
end;

function Random(n: integer): integer;
begin
  Result := PABCSystem.Random(n);
  if IsPT then exit;
  InputList.Add(Result);
end;

/// Возвращает случайное вещественное в диапазоне [0..1)
function Random: real;
begin
  Result := PABCSystem.Random;
  if IsPT then exit;
  InputList.Add(Result);
end;

/// Возвращает случайное вещественное в диапазоне [a,b)
function Random(a, b: real): real;
begin
  Result := PABCSystem.Random(a, b);
  if IsPT then exit;
  InputList.Add(Result);
end;

/// Возвращает случайный символ в диапазоне от a до b
function Random(a, b: char): char;
begin
  Result := PABCSystem.Random(a, b);
  if IsPT then exit;
  InputList.Add(Result);
end;

/// Возвращает случайное целое в диапазоне 
function Random(diap: IntRange): integer;
begin
  Result := PABCSystem.Random(diap);
  if IsPT then exit;
  InputList.Add(Result);
end;

/// Возвращает случайное вещественное в диапазоне 
function Random(diap: RealRange): real;
begin
  Result := PABCSystem.Random(diap);
  if IsPT then exit;
  InputList.Add(Result);
end;

/// Возвращает случайный символ в диапазоне 
function Random(diap: CharRange): char;
begin
  Result := PABCSystem.Random(diap);
  if IsPT then exit;
  InputList.Add(Result);
end;

/// Возвращает кортеж из двух случайных целых в диапазоне от a до b
function Random2(a, b: integer): (integer, integer);
begin
  Result := PABCSystem.Random2(a, b);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

/// Возвращает кортеж из двух случайных вещественных в диапазоне от a до b
function Random2(a, b: real): (real, real);
begin
  Result := PABCSystem.Random2(a, b);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

/// Возвращает кортеж из двух случайных символов в диапазоне от a до b
function Random2(a, b: char): (char, char);
begin
  Result := PABCSystem.Random2(a, b);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

/// Возвращает кортеж из двух случайных целых в диапазоне
function Random2(diap: IntRange): (integer, integer);
begin
  Result := PABCSystem.Random2(diap);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

/// Возвращает кортеж из двух случайных символов в диапазоне
function Random2(diap: CharRange): (char, char);
begin
  Result := PABCSystem.Random2(diap);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

/// Возвращает кортеж из двух случайных вещественных в диапазоне
function Random2(diap: RealRange): (real, real);
begin
  Result := PABCSystem.Random2(diap);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

/// Возвращает кортеж из трех случайных целых в диапазоне от a до b
function Random3(a, b: integer): (integer, integer, integer);
begin
  Result := PABCSystem.Random3(a, b);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
  InputList.Add(Result[2]);
end;

/// Возвращает кортеж из трех случайных вещественных в диапазоне от a до b
function Random3(a, b: real): (real, real, real);
begin
  Result := PABCSystem.Random3(a, b);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
  InputList.Add(Result[2]);
end;

/// Возвращает кортеж из трех случайных символов в диапазоне от a до b
function Random3(a, b: char): (char, char, char);
begin
  Result := PABCSystem.Random3(a, b);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
  InputList.Add(Result[2]);
end;

/// Возвращает кортеж из трех случайных целых в диапазоне
function Random3(diap: IntRange): (integer, integer, integer);
begin
  Result := PABCSystem.Random3(diap);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
  InputList.Add(Result[2]);
end;

/// Возвращает кортеж из трех случайных вещественных в диапазоне
function Random3(diap: RealRange): (real, real, real);
begin
  Result := PABCSystem.Random3(diap);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
  InputList.Add(Result[2]);
end;

/// Возвращает кортеж из трех случайных символов в диапазоне
function Random3(diap: CharRange): (char, char, char);
begin
  Result := PABCSystem.Random3(diap);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
  InputList.Add(Result[2]);
end;

/// Возвращает массив размера n, заполненный случайными целыми значениями в диапазоне от a до b
function ArrRandomInteger(n: integer; a: integer; b: integer): array of integer;
begin
  Result := PABCSystem.ArrRandomInteger(n, a, b);
  if IsPT then exit;
  for var i:=0 to n-1 do
    InputList.Add(Result[i]);
end;

/// Возвращает массив размера n, заполненный случайными целыми значениями в диапазоне от 0 до 100
function ArrRandomInteger(n: integer): array of integer := ArrRandomInteger(n,0,100);

/// Возвращает массив размера n, заполненный случайными вещественными значениями в диапазоне от a до b 
function ArrRandomReal(n: integer; a: real; b: real): array of real;
begin
  Result := PABCSystem.ArrRandomReal(n, a, b);
  if IsPT then exit;
  for var i:=0 to n-1 do
    InputList.Add(Result[i]);
end;

/// Возвращает массив размера n, заполненный случайными вещественными значениями  в диапазоне от 0 до 10
function ArrRandomReal(n: integer): array of real := ArrRandomReal(n,0,10);

/// Непонятно, зачем эти функции переопределять!!! Только Read и Random!!!

{/// Возвращает массив из count элементов, заполненных значениями gen(i)
function ArrGen<T>(count: integer; gen: integer->T): array of T;
begin
  Result := PABCSystem.ArrGen(count,gen);
  if IsPT then exit;
  for var i:=0 to Result.Length-1 do
    InputList.Add(Result[i]);
end;

/// Возвращает массив из count элементов, заполненных значениями gen(i), начиная с i=from
function ArrGen<T>(count: integer; gen: integer->T; from: integer): array of T;
begin
  Result := PABCSystem.ArrGen(count,gen,from);
  if IsPT then exit;
  for var i:=0 to Result.Length-1 do
    InputList.Add(Result[i]);
end;

/// Возвращает массив из count элементов, начинающихся с first, с функцией next перехода от предыдущего к следующему 
function ArrGen<T>(count: integer; first: T; next: T->T): array of T;
begin
  Result := PABCSystem.ArrGen(count,first,next);
  if IsPT then exit;
  for var i:=0 to Result.Length-1 do
    InputList.Add(Result[i]);
end;

/// Возвращает массив из count элементов, начинающихся с first и second, с функцией next перехода от двух предыдущих к следующему 
function ArrGen<T>(count: integer; first, second: T; next: (T,T) ->T): array of T;
begin
  Result := PABCSystem.ArrGen(count,first,second,next);
  if IsPT then exit;
  for var i:=0 to Result.Length-1 do
    InputList.Add(Result[i]);
end;}

{/// Возвращает массив из n целых, введенных с клавиатуры
function ReadArrInteger(n: integer): array of integer;
begin
  Result := PABCSystem.ReadArrInteger(n); // и всё!!! Данные в InputList уже внесены!
end;

/// Возвращает массив из n вещественных, введенных с клавиатуры
function ReadArrReal(n: integer): array of real;
begin
  Result := PABCSystem.ReadArrReal(n);
end;

/// Возвращает массив из n строк, введенных с клавиатуры
function ReadArrString(n: integer): array of string;
begin
  Result := PABCSystem.ReadArrString(n);
end;

/// Возвращает матрицу m на n целых, введенных с клавиатуры
function ReadMatrInteger(m, n: integer): array [,] of integer;
begin
  Result := PABCSystem.ReadMatrInteger(m,n); // и всё!!! Данные в InputList уже внесены!
end;

/// Возвращает матрицу m на n вещественных, введенных с клавиатуры
function ReadMatrReal(m, n: integer): array [,] of real;
begin
  Result := PABCSystem.ReadMatrReal(m,n);
end;}

/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandomInteger(m: integer; n: integer; a: integer; b: integer): array [,] of integer;
begin
  Result := PABCSystem.MatrRandomInteger(m,n,a,b);
  if IsPT then exit;
  foreach var x in Result.ElementsByRow do
    InputList.Add(x);
end;

/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandomInteger(m: integer; n: integer): array [,] of integer := MatrRandomInteger(m,n,0,100);

/// Возвращает двумерный массив размера m x n, заполненный случайными вещественными значениями
function MatrRandomReal(m: integer; n: integer; a: real; b: real): array [,] of real;
begin
  Result := PABCSystem.MatrRandomReal(m,n,a,b);
  if IsPT then exit;
  foreach var x in Result.ElementsByRow do
    InputList.Add(x);
end;

/// Возвращает двумерный массив размера m x n, заполненный случайными вещественными значениями
function MatrRandomReal(m: integer; n: integer): array [,] of real := MatrRandomReal(m,n,0,10);

/// Возвращает двумерный массив размера m x n, заполненный элементами gen(i,j) 
{function MatrGen<T>(m, n: integer; gen: (integer,integer)->T): array [,] of T;
begin
  Result := PABCSystem.MatrGen(m,n,gen);
  if IsPT then exit;
  foreach var x in Result.ElementsByRow do
    InputList.Add(x);
end;}

{function ReadString: string;
begin
  Result := PABCSystem.ReadString;
  if IsPT then exit;
  InputList.Add(Result);
  CreateNewLineBeforeMessage := False;
end;

function ReadlnString := ReadString;

function ReadString2 := (ReadString, ReadString);

function ReadlnString2 := ReadString2;}

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

function ToObjArray(a: array of integer) := a.Select(x -> object(x)).ToArray;

function ToObjArray(a: array of real) := a.Select(x -> object(x)).ToArray;

function ToObjArray(a: array of string) := a.Select(x -> object(x)).ToArray;

function ToObjArray(a: array of char) := a.Select(x -> object(x)).ToArray;

function ToObjArray(a: array of boolean) := a.Select(x -> object(x)).ToArray;

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

procedure CheckOutputAfterInitialSeq(seq: sequence of integer) := CheckOutputAfterInitial(ToObjArray(seq.ToArray));
procedure CheckOutputAfterInitialSeq(seq: sequence of real) := CheckOutputAfterInitial(ToObjArray(seq.ToArray));
procedure CheckOutputAfterInitialSeq(seq: sequence of string) := CheckOutputAfterInitial(ToObjArray(seq.ToArray));
procedure CheckOutputAfterInitialSeq(seq: sequence of boolean) := CheckOutputAfterInitial(ToObjArray(seq.ToArray));
procedure CheckOutputAfterInitialSeq(seq: sequence of char) := CheckOutputAfterInitial(ToObjArray(seq.ToArray));
procedure CheckOutputAfterInitialSeq(seq: sequence of object) := CheckOutputAfterInitial(seq.ToArray);
procedure CheckOutputAfterInitialSeq(seq: ObjectList) := CheckOutputAfterInitial(seq.lst.ToArray);


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
  if CreateNewLineBeforeMessage then
    Console.WriteLine;
  Console.WriteLine(MsgColorCode(color) + msg);
  CreateNewLineBeforeMessage := False;
end;

procedure ColoredMessage(msg: string);
begin
  if CreateNewLineBeforeMessage then
    Console.WriteLine;
  Console.WriteLine(MsgColorCode(MsgColorRed) + msg);
  CreateNewLineBeforeMessage := False;
end;

/// Здесь проверялся RuntimeType - в новом CheckOutput RuntimeType вообще не проверяется
procedure CheckOutputOld(params arr: array of object);
begin
  if (TaskResult = InitialTask) or (TaskResult = BadInitialTask) then
    exit;

  var mn := Min(arr.Length, OutputList.Count);
  TaskResult := Solved;
  // Несоответствие типов
  for var i := 0 to mn - 1 do
  begin 
    if (arr[i].GetType.Name = 'RuntimeType') and (arr[i] <> OutputList[i].GetType) then
      raise new OutputTypeException(i + 1, TypeToTypeName(arr[i] as System.Type), TypeName(OutputList[i]))
    else if (arr[i].GetType.Name <> 'RuntimeType') and (arr[i].GetType <> OutputList[i].GetType) then
      raise new OutputTypeException(i + 1, TypeToTypeName(arr[i].GetType), TypeName(OutputList[i]));
  end;  
  
  // Несоответствие количества выводимых параметров
  if arr.Length <> OutputList.Count then
    raise new OutputCountException(OutputList.Count, arr.Length);
  
  // Несоответствие значений
  for var i := 0 to mn - 1 do
    if (arr[i].GetType.Name <> 'RuntimeType') and not CompareValues(arr[i], OutputList[i]) then
    begin
      TaskResult := BadSolution; // Если типы разные, то IOErrorSolution
      exit;           
    end;
end;

// Добавим сюда проверку типов RuntimeType
procedure CheckOutput(params arr: array of object);
begin
  // TaskResult = InitialTask - ничего выводить не надо
  // TaskResult = BadInitialTask - потом будет выведено исключение, что часть изначальных данных удалена
  if (TaskResult = InitialTask) or (TaskResult = BadInitialTask) then
    exit;

  var mn := Min(arr.Length, OutputList.Count);
  
  // Несоответствие типов или значений
  var ind := -1;
  for var i := 0 to mn - 1 do
  begin  
    // Если типы не совпадают 
    if (arr[i].GetType.Name <> 'RuntimeType') and (arr[i].GetType <> OutputList[i].GetType) or
      (arr[i].GetType.Name = 'RuntimeType') and (arr[i] <> OutputList[i].GetType)
    then
    begin
      ind := i; // то запомнить индекс первого несовпадения
      if ind > InitialOutputList.Count then
        ColoredMessage('Часть выведенных данных правильная',MsgColorGray);
      raise new OutputTypeException(i + 1, TypeToTypeName(arr[i].GetType), TypeName(OutputList[i]));           
    end;
    // Если значения не совпадают (если задан маркер типа, то проверка значений пропускается)
    if (arr[i].GetType.Name <> 'RuntimeType') and not CompareValues(arr[i], OutputList[i]) then
    begin
      ind := i; // то запомнить индекс первого несовпадения
      if ind > InitialOutputList.Count then
      begin
        ColoredMessage('Часть выведенных данных правильная',MsgColorGray);
        ColoredMessage($'Элемент {i + 1}: ожидалось значение {arr[i]}, а выведено {OutputList[i]}',MsgColorGray);
      end;  
      TaskResult := BadSolution;
      exit;           
    end;
  end;
  if ind = -1 then
    ind := mn;
  
  if arr.Length <> OutputList.Count then
  begin  
    if (ind = mn) and (ind<>0) then
      ColoredMessage('Все выведенные данные правильны',MsgColorGray)
    else if ind > 0 then
      ColoredMessage('Часть выведенных данных правильная',MsgColorGray);
    raise new OutputCountException(OutputList.Count, arr.Length);
  end;
  TaskResult := Solved;
end;

procedure CheckOutputAfterInitial(params arr: array of object);
begin
  if (TaskResult = InitialTask) or (TaskResult = BadInitialTask) then
    exit;
  // Если мы попали сюда, то OutputList.Count >= InitialOutputList.Count
  var mn := Min(arr.Length, OutputList.Count - InitialOutputList.Count);
  
  var i0 := InitialOutputList.Count;
  var ind := -1;
  for var i := i0 to i0 + mn - 1 do
  begin  
    // Если типы не совпадают 
    if (arr[i-i0].GetType.Name <> 'RuntimeType') and (arr[i-i0].GetType <> OutputList[i].GetType) or
      (arr[i-i0].GetType.Name = 'RuntimeType') and (arr[i-i0] <> OutputList[i].GetType)
    then
    begin
      ind := i; // то запомнить индекс первого несовпадения
      if ind >= InitialOutputList.Count then
        ColoredMessage('Часть выведенных данных правильная',MsgColorGray);
      raise new OutputTypeException(i + 1, TypeToTypeName(arr[i-i0].GetType), TypeName(OutputList[i]));           
    end;
    // Если значения не совпадают (если задан маркер типа, то проверка значений пропускается)
    if (arr[i-i0].GetType.Name <> 'RuntimeType') and not CompareValues(arr[i-i0], OutputList[i]) then
    begin
      ind := i; // то запомнить индекс первого несовпадения
      if ind > InitialOutputList.Count then
      begin
        ColoredMessage('Часть выведенных данных правильная',MsgColorGray);
        ColoredMessage($'Элемент {i + 1}: ожидалось значение {arr[i-i0]}, а выведено {OutputList[i]}',MsgColorGray);
      end;  
      TaskResult := BadSolution;
      exit;           
    end;
  end;
  if ind = -1 then
    ind := mn;
  
  if arr.Length <> OutputList.Count - InitialOutputList.Count then
  begin  
    if (ind = mn) and (ind<>i0) then
      ColoredMessage('Все выведенные данные правильны',MsgColorGray)
    else if ind > 0 then
      ColoredMessage('Часть выведенных данных правильная',MsgColorGray);
    raise new OutputCountException(OutputList.Count, arr.Length + i0);
  end;
  TaskResult := Solved;
end;

procedure CheckOutputAfterInitialOld(params arr: array of object); // проверить только то, что после исходного вывода
begin
  if (TaskResult = InitialTask) or (TaskResult = BadInitialTask) then
    exit;
  
  // Здесь всегда OutputList.Count > InitialOutputList.Count
  // Если arr.Length > OutputList.Count - InitialOutputList.Count, то мы не вывели часть данных
  // Если arr.Length < OutputList.Count - InitialOutputList.Count, то мы вывели больше чем надо
  
  if arr.Length <> OutputList.Count - InitialOutputList.Count then
    raise new OutputCountException(OutputList.Count, InitialOutputList.Count + arr.Length);
    
  TaskResult := Solved;
  // Несоответствие типов
  var a := OutputList.Count - arr.Length;
  for var i := a to OutputList.Count - 1 do
  begin 
    if (arr[i-a].GetType.Name = 'RuntimeType') and (arr[i-a] <> OutputList[i].GetType) then
      raise new OutputTypeException(i + 1, TypeToTypeName(arr[i-a] as System.Type), TypeName(OutputList[i]))
    else if (arr[i-a].GetType.Name <> 'RuntimeType') and (arr[i-a].GetType <> OutputList[i].GetType) then
      raise new OutputTypeException(i + 1, TypeName(arr[i-a]), TypeName(OutputList[i]));
  end;
  
  // Несоответствие значений
  for var i := a to OutputList.Count - 1 do
    if (arr[i-a].GetType.Name <> 'RuntimeType') and not CompareValues(arr[i-a], OutputList[i]) then
    begin
      TaskResult := BadSolution; // Если типы разные, то IOErrorSolution
      exit;           
    end;
    
    
end;

procedure CheckOutputSeq(a: sequence of integer) := CheckOutput(ToObjArray(a.ToArray));
procedure CheckOutputSeq(a: sequence of real) := CheckOutput(ToObjArray(a.ToArray));
procedure CheckOutputSeq(a: sequence of string) := CheckOutput(ToObjArray(a.ToArray));
procedure CheckOutputSeq(a: sequence of char) := CheckOutput(ToObjArray(a.ToArray));
procedure CheckOutputSeq(a: sequence of boolean) := CheckOutput(ToObjArray(a.ToArray));
procedure CheckOutputSeq(a: sequence of object) := CheckOutput(a.ToArray);
procedure CheckOutputSeq(a: ObjectList) := CheckOutput(a.lst.ToArray);
procedure CheckOutputSeq(a: sequence of word) := CheckOutputSeq(a.Select(x->object(x)));

procedure CheckOutputNew(params arr: array of object) := CheckOutput(arr);
procedure CheckOutputSeqNew(a: sequence of integer) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of real) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of string) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of char) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of boolean) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of object) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: sequence of word) := CheckOutputSeq(a);
procedure CheckOutputSeqNew(a: ObjectList) := CheckOutputSeq(a);

procedure CheckOutputSeqOld(a: sequence of integer) := CheckOutputOld(ToObjArray(a.ToArray));
procedure CheckOutputSeqOld(a: sequence of real) := CheckOutputOld(ToObjArray(a.ToArray));
procedure CheckOutputSeqOld(a: sequence of string) := CheckOutputOld(ToObjArray(a.ToArray));
procedure CheckOutputSeqOld(a: sequence of char) := CheckOutputOld(ToObjArray(a.ToArray));
procedure CheckOutputSeqOld(a: sequence of boolean) := CheckOutputOld(ToObjArray(a.ToArray));
procedure CheckOutputSeqOld(a: sequence of object) := CheckOutputOld(a.ToArray);
procedure CheckOutputSeqOld(a: ObjectList) := CheckOutputOld(a.lst.ToArray);

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


procedure CheckMyPT;
begin
  if CheckTask = nil then
    exit;
  var TName := TaskName;
  try
    TName := ConvertTaskName(TaskName);

    CheckTask(TName);
    // Если это задача из задачника, то результат будет NotUnderControl. И дальше необходимо это преобразовывать
    case TaskResult of
      Solved: ColoredMessage('Задание выполнено', MsgColorGreen);
      BadSolution: ColoredMessage('Неверное решение');
      InitialTask: ;
      BadInitialTask: ColoredMessage('Вы удалили часть кода - восстановите его!', MsgColorMagenta);
    end;
  except
    on e: OutputTypeException do
    begin
      //Writeln(#10+$'Неверно указан тип при выводе данных'); 
      ColoredMessage($'Ошибка вывода. При выводе {e.n}-го элемента типа {e.ExpectedType} выведено значение типа {e.ActualType}'); 
    end;
    on e: OutputCountException do
    begin
      if e.Count = 0 then
        ColoredMessage($'Требуется вывести {NValues(e.n)}', MsgColorGray)
      else ColoredMessage($'Выведено {NValues(e.Count)}, а требуется вывести {e.n}', MsgColorOrange); 
    end;
    on e: OutputCount2Exception do
    begin
      if e.Count = 0 then
        ColoredMessage($'Требуется вывести по крайней мере {NValues(e.i)}', MsgColorGray)
      else ColoredMessage($'Выведено {NValues(e.Count)}, а требуется вывести по крайней мере {e.i}', MsgColorOrange); 
    end;
    on e: InputTypeException do
    begin
      //Writeln(#10+$'Неверно указан тип при вводе исходных данных'); 
      ColoredMessage($'Ошибка ввода. При вводе {e.n}-го элемента типа {e.ExpectedType} использована переменная типа {e.ActualType}'); 
    end;
    on e: InputCountException do
    begin
      if e.Count = 0 then
        ColoredMessage($'Требуется ввести {NValues(e.n)}', MsgColorGray)
      else if e.n <> 0 then 
        ColoredMessage($'Введено {NValues(e.Count)}, а требуется ввести {e.n}', MsgColorOrange)
      else ColoredMessage($'Введено {NValues(e.Count)}, хотя ничего вводить не требуется', MsgColorOrange); 
    end;
    on e: InputCount2Exception do
    begin
      if e.Count = 0 then
        ColoredMessage($'Требуется ввести по крайней мере {NValues(e.i)}', MsgColorGray)
      else ColoredMessage($'Введено {NValues(e.Count)}, а требуется ввести по крайней мере {e.i}', MsgColorOrange); 
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

procedure WriteInfoToRemoteDatabase(auth: string; LessonName, TaskName, TaskPlatform, TaskResult, AdditionalInfo: string);
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
    var t2 := User.SendPostRequest(login, pass, LessonName, TaskName, TaskPlatform, TaskResult, AdditionalInfo);
    var v := t2.Result;
    v := v;
    //Console.WriteLine(v);
  end;
end;

procedure WriteInfoToDatabases(LessonName,TaskName,TaskPlatform: string; TaskResult: TaskStatus; AdditionalInfo: string := '');
begin
  try
    System.IO.File.AppendAllText('db.txt', $'{LessonName} {TaskName} {dateTime.Now.ToString(''u'')} {TaskResult.ToString} {AdditionalInfo}' + #10);
    var auth := FindAuthDat();
    var args := System.Environment.GetCommandLineArgs;
    if (auth <> '') and (args.Length = 3) and (args[2].ToLower = 'true') then
      // Есть проблема паузы при плохой сети 
      WriteInfoToRemoteDatabase(auth,LessonName,TaskName,TaskPlatform,TaskResult.ToString, AdditionalInfo);
  except
    on e: System.AggregateException do
    begin
      foreach var x in e.InnerExceptions do
        if x is HTTPRequestException then
          ColoredMessage(x.InnerException?.Message??'',MsgColorGray)
        else ColoredMessage(x.Message,MsgColorGray); 
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
      inherited write(obj);
      OutputString += _ObjectToString(obj);
      OutputList += obj;
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure writeln; override;
    begin
      inherited writeln;
      OutputString += NewLine;
      CreateNewLineBeforeMessage := False;
    end;
    
    function ReadLine: string; override;
    begin
      Result := inherited ReadLine;
      InputList.Add(Result);
      CreateNewLineBeforeMessage := False;
    end;
    
    procedure readln; override;
    begin
      inherited readln;
      CreateNewLineBeforeMessage := False;
    end;
    
    procedure read(var x: integer); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: real); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: char); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: string); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: byte); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: shortint); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: smallint); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: word); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: longword); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: int64); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: uint64); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: single); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      CreateNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: boolean); override;
    begin
      inherited Read(x);
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