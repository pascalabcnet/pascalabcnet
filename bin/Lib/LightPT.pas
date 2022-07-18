/// Модуль LightPT автоматической легковесной проверки заданий
unit LightPT;

uses __RedirectIOMode;

// Исключения - для вывода сообщения
// TaskResult - для базы данных
// Запоминать исключение в глобальной переменной TaskException - для доп. параметров для БД в формате 
//   ПодтипИсключения(парам1,...,парамn)
// Эту строку также можно писать в БД как доп параметры TaskResult
// Чем хороши исключения - их можно делать разными с абсолютно разными параметрами

type
  MessageColorT = (MsgColorGreen, MsgColorRed, MsgColorOrange, MsgColorMagenta, MsgColorGray);
  TaskStatus = (Solved, IOError, BadSolution, PartialSolution, InitialTask, BadInitialTask, NotUnderControl); // Короткий результат для БД

type
  PTException = class(Exception) 
    function Info: string; virtual := 'NoInfo';
  end;

var
  NewLineBeforeMessage := True;
  TaskResult: TaskStatus := NotUnderControl; // Записывается в БД
  TaskException: PTException := new PTException;

  WriteInfoCallBack: procedure (name: string; result: TaskStatus; AdditionalInfo: string);

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
    
    function Info: string; override := $'InputType({Count},{n})';
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
    
    function Info: string; override := $'InputType({n},{ExpectedType},{ActualType})';
  end;

var
  OutputString := new StringBuilder;
  OutputList := new List<object>;
  InputList := new List<object>;
  InitialOutputList := new List<object>;
  InitialInputList := new List<object>;
  
  CheckTask: procedure(name: string);
  
  Cur := 0;

function TaskName := ExtractFileName(System.Environment.GetCommandLineArgs[0]).Replace('.exe', '');

function cInt := typeof(integer);
function cRe := typeof(real);
function cStr := typeof(string);
function cBool := typeof(boolean);
function cChar := typeof(char);

procedure CheckInitialIO;
begin
  if (OutputList.Count = InitialOutputList.Count) and (InputList.Count = InitialInputList.Count) then
    TaskResult := InitialTask
  else if (InputList.Count < InitialInputList.Count) or (InputList.Count = InitialInputList.Count) and (OutputList.Count < InitialOutputList.Count) then
    TaskResult := BadInitialTask;
end;

{function CheckBadInitialTask: boolean;
begin
  Result := (InputList.Count < InitialInputList.Count) or (InputList.Count = InitialInputList.Count) and (OutputList.Count < InitialOutputList.Count);
end;}

procedure InitialOutput(params a: array of object);
begin
  InitialOutputList.Clear;
  InitialOutputList.AddRange(a);
end;

procedure InitialInput(params a: array of object);
begin
  InitialInputList.Clear;
  InitialInputList.AddRange(a);
end;

procedure CheckInitialOutput(params a: array of object);
begin
  InitialOutput(a);
  CheckInitialIO;
end;

procedure CheckInitialInput(params a: array of object);
begin
  InitialInput(a);
  CheckInitialIO;
end;

procedure WriteInfoToLocalDatabase(name: string; result: TaskStatus; AdditionalInfo: string := '');
begin
  try
    System.IO.File.AppendAllText('db.txt', $'{name} {dateTime.Now.ToString(''u'')} {Result.ToString} {AdditionalInfo}' + #10);
  except
    on e: Exception do
      Print(e.Message);
  end;  
end;

procedure CheckInputCount(n: integer);
begin
  if InputList.Count < n then
    raise new InputCountException(InputList.Count, n)
end;

procedure CheckInput2Count(i: integer);
begin
  if InputList.Count <= i then
    raise new InputCount2Exception(InputList.Count, i + 1)
end;

function IsInt(i: integer) := InputList[i] is integer;

function IsRe(i: integer) := InputList[i] is real;

function IsStr(i: integer) := InputList[i] is string;

function IsBoo(i: integer) := InputList[i] is boolean;

function IsChr(i: integer) := InputList[i] is char;

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

function Random(a, b: integer): integer;
begin
  Result := PABCSystem.Random(a, b);
  InputList.Add(Result);
end;

function Random: real;
begin
  Result := PABCSystem.Random;
  InputList.Add(Result);
end;

function Random(a, b: real): real;
begin
  Result := PABCSystem.Random(a, b);
  InputList.Add(Result);
end;

function Random2(a, b: integer): (integer, integer);
begin
  Result := PABCSystem.Random2(a, b);
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

function Random2(a, b: real): (real, real);
begin
  Result := PABCSystem.Random2(a, b);
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

function ReadString: string;
begin
  Result := PABCSystem.ReadString;
  InputList.Add(Result);
  NewLineBeforeMessage := True;
end;

function ReadlnString := ReadString;

function ReadString2 := (ReadString, ReadString);

function ReadlnString2 := ReadString2;

function ReadInteger(prompt: string): integer;
begin
  Result := PABCSystem.ReadInteger(prompt);
  OutputList.RemoveAt(OutputList.Count - 1);
  OutputList.RemoveAt(OutputList.Count - 1);
  NewLineBeforeMessage := True;
end;

function ReadInteger2(prompt: string): (integer, integer);
begin
  Result := PABCSystem.ReadInteger2(prompt);
  OutputList.RemoveAt(OutputList.Count - 1);
  OutputList.RemoveAt(OutputList.Count - 1);
  NewLineBeforeMessage := True;
end;


procedure Print(params args: array of object);
begin
  foreach var ob in args do
  begin
    PABCSystem.Print(ob);
    OutputList.RemoveAt(OutputList.Count - 1)
  end;
  NewLineBeforeMessage := False;
end;

procedure Println(params args: array of object);
begin
  Print(args);
  Writeln;
  NewLineBeforeMessage := True;
end;

procedure Print(ob: object);
begin
  PABCSystem.Print(ob);
  OutputList.RemoveAt(OutputList.Count - 1);
  NewLineBeforeMessage := False;
end;

procedure Print(s: string);
begin
  PABCSystem.Print(s);
  OutputList.RemoveAt(OutputList.Count - 1);
  NewLineBeforeMessage := False;
end;

type
  IOLightSystem = class(__ReadSignalOISystem)
  public
    procedure write(obj: object); override;
    begin
      inherited write(obj);
      OutputString += obj.ToString;
      OutputList += obj;
      NewLineBeforeMessage := False;
    end;
    
    procedure writeln; override;
    begin
      inherited writeln;
      OutputString += NewLine;
      NewLineBeforeMessage := True;
    end;
    
    function ReadLine: string; override;
    begin
      Result := inherited ReadLine;
      NewLineBeforeMessage := True;
    end;
    
    procedure readln; virtual;
    begin
      inherited readln;
      NewLineBeforeMessage := True;
    end;
    
    procedure read(var x: integer); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: real); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: char); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: string); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: byte); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: shortint); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: smallint); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: word); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: longword); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: int64); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: uint64); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: single); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: boolean); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
    
    procedure read(var x: BigInteger); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      NewLineBeforeMessage := False;
    end;
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
  Result := (*(o1.GetType = o2.GetType) and*) o1.Equals(o2);
end;

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

procedure CompareWithOutput(params a: array of object);
begin
  if (TaskResult = InitialTask) or (TaskResult = BadInitialTask) then
    exit;
  //Result := NotCompleted;
  {if a.Length <> OutputList.Count then
    exit;
  Result := if a.Zip(OutputList,(x,y) -> CompareElements(x,y)).All(x->x) 
              then Completed 
              else NotCompleted;}
  var mn := Min(a.Length, OutputList.Count);
  TaskResult := Solved;
  // Несоответствие типов
  for var i := 0 to mn - 1 do
  begin  
    if (a[i].GetType.Name = 'RuntimeType') and (a[i] <> OutputList[i].GetType) then
      raise new OutputTypeException(i + 1, TypeToTypeName(a[i] as System.Type), TypeName(OutputList[i]))
    else if (a[i].GetType.Name <> 'RuntimeType') and (a[i].GetType <> OutputList[i].GetType) then
      raise new OutputTypeException(i + 1, TypeName(a[i]), TypeName(OutputList[i]));
  end;  
  
  // Несоответствие количества выводимых параметров
  if a.Length <> OutputList.Count then
    raise new OutputCountException(OutputList.Count, a.Length);
  
  // Несоответствие значений
  for var i := 0 to mn - 1 do
    if (a[i].GetType.Name <> 'RuntimeType') and not CompareValues(a[i], OutputList[i]) then
    begin
      TaskResult := BadSolution; // Если типы разные, то IOErrorSolution
      exit;           
    end;
end;

procedure CompareSeqWithOutput(a: sequence of integer) := CompareWithOutput(ToObjArray(a.ToArray));

procedure CompareSeqWithOutput(a: sequence of real) := CompareWithOutput(ToObjArray(a.ToArray));

procedure CompareSeqWithOutput(a: sequence of string) := CompareWithOutput(ToObjArray(a.ToArray));

procedure CompareSeqWithOutput(a: sequence of char) := CompareWithOutput(ToObjArray(a.ToArray));

procedure CompareSeqWithOutput(a: sequence of boolean) := CompareWithOutput(ToObjArray(a.ToArray));

procedure ClearOutputListFromSpaces;
begin
  OutputList := OutputList.Where(s -> (not (s is string)) or ((s as string) <> ' ')).ToList;
end;

procedure FilterOnlyNumbers;
begin
  OutputList := OutputList.Where(x -> (x is integer) or (x is real)).ToList;
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

procedure ColoredMessage(msg: string; color: MessageColorT := MsgColorRed);
begin
  if not NewLineBeforeMessage then
    Writeln;
  Writeln(MsgColorCode(color) + msg);
end;

function NValues(n: integer): string;
begin
  case n of
    1: Result := n + ' значение';
    2, 3, 4: Result := n + ' значения';
    5..20: Result := n + ' значений';
  end;
end;

procedure RobotCheckSolution;
begin
  TaskResult := BadSolution;
  var a := System.Reflection.Assembly.GetExecutingAssembly();
  var t := a.GetType('RobotField.RobotField');
  if t<>nil then
  begin
    var f := t.GetField('RobField',System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.Static);
    var IsSol := f.FieldType.GetMethod('IsSolution');
    var bool := IsSol.Invoke(f.GetValue(nil),nil);
    if boolean(bool) then
      TaskResult := Solved;
  end;
end;

procedure CheckMyPT;
begin
  if CheckTask = nil then
    exit;
  try
    CheckTask(TaskName);
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
    on e: InputTypeException do
    begin
      //Writeln(#10+$'Неверно указан тип при вводе исходных данных'); 
      ColoredMessage($'Ошибка ввода. При вводе {e.n}-го элемента типа {e.ExpectedType} использована переменная типа {e.ActualType}'); 
    end;
    on e: InputCountException do
    begin
      if e.Count = 0 then
        ColoredMessage($'Требуется ввести {NValues(e.n)}', MsgColorGray)
      else ColoredMessage($'Введено {NValues(e.Count)}, а требуется ввести {e.n}', MsgColorOrange); 
    end;
    on e: InputCount2Exception do
    begin
      if e.Count = 0 then
        ColoredMessage($'Требуется ввести по крайней мере {NValues(e.i)}', MsgColorGray)
      else ColoredMessage($'Введено {NValues(e.Count)}, а требуется ввести по крайней мере {e.i}', MsgColorOrange); 
    end;
  end;
  if WriteInfoCallBack<>nil then
    WriteInfoCallBack(TaskName, TaskResult, TaskException.Info);
end;

initialization
  CurrentIOSystem := new IOLightSystem; 
  WriteInfoCallBack := WriteInfoToLocalDatabase;
finalization
  CurrentIOSystem := new IOLightSystem; // Для Robot - т.к. он переопределяет. Лучше вставлять uses LightPT после Робота
  CheckMyPT  
end.