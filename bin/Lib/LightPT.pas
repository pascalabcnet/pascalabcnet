/// Модуль LightPT автоматической легковесной проверки заданий
unit LightPT;

uses __RedirectIOMode;

// Исключения - для вывода сообщения
// TaskResult - для базы данных
// Запоминать исключение в глобальной переменной TaskException - для доп. параметров для БД в формате 
//   ПодтипИсключения(парам1,...,парамn)
// Эту строку также можно писать в БД как доп параметры TaskResult
// Чем хороши исключения - их можно делать разными с абсолютно разными параметрами

const lightptname = 'lightpt.dat';

type
  MessageColorT = (MsgColorGreen, MsgColorRed, MsgColorOrange, MsgColorMagenta, MsgColorGray);
  TaskStatus = (Solved, IOError, BadSolution, PartialSolution, InitialTask, BadInitialTask, NotUnderControl, InitialTaskPT4, ErrFix, Demo); // Короткий результат для БД

type
  PTException = class(Exception) 
    function Info: string; virtual := 'NoInfo';
  end;

var
  DoNewLineBeforeMessage := False;
  TaskResult: TaskStatus := NotUnderControl; // Записывается в БД
  TaskResultInfo: string; // доп. информация о результате. Как правило пуста. Или содержит TaskException.Info. Или содержит для Solved и BadSolution информацию о модуле: Robot, Drawman, PT4
  TaskException: PTException := new PTException;

  WriteInfoCallBack: procedure (name: string; result: TaskStatus; AdditionalInfo: string);

  LessonName: string := '';
  TaskNamesMap := new Dictionary<string,string>;
  
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
    function AddFill(n: integer; elem: object): ObjectList; begin lst.AddRange(ArrFill(n,elem)); Result := Self end;
    function AddArithm(n: integer; a0,step: integer): ObjectList; begin lst.AddRange(ArrGen(n,a0,x->x+step).Select(x -> object(x))); Result := Self end;
    function AddArithm(n: integer; a0,step: real): ObjectList; begin lst.AddRange(ArrGen(n,a0,x->x+step).Select(x -> object(x))); Result := Self end;
    function AddFib(n: integer): ObjectList; begin lst.AddRange(ArrGen(n,1,1,(x,y)->x+y).Select(x -> object(x))); Result := Self end;
    function AddGeom(n: integer; a0,step: integer): ObjectList; begin lst.AddRange(ArrGen(n,a0,x->x*step).Select(x -> object(x))); Result := Self end;
    function AddGeom(n: integer; a0,step: real): ObjectList; begin lst.AddRange(ArrGen(n,a0,x->x*step).Select(x -> object(x))); Result := Self end;
  end;

var
  OutputString := new StringBuilder;
  OutputList := new List<object>;
  InputList := new List<object>;
  InitialOutputList := new List<object>;
  InitialInputList := new List<object>;
  
  CheckTask: procedure(name: string);
  
  Cur := 0;

var TaskName := ExtractFileName(System.Environment.GetCommandLineArgs[0]).Replace('.exe', '');

function IsPT := System.Type.GetType('PT4.PT4') <> nil;

function IsRobot := System.Type.GetType('RobotField.RobotField');

function IsDrawman := System.Type.GetType('DrawManField.DrawManField');

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

procedure CheckInitialOutputValues(params a: array of object);
begin
  if CompareArrValues(a,OutputList.ToArray) then
    TaskResult := InitialTask;   
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

procedure CheckInitialOutputSeq(a: sequence of System.Type) := CheckInitialOutput(a.Select(x->object(x)).ToArray);

procedure CheckInitialInputSeq(a: sequence of System.Type) := CheckInitialInput(a.Select(x->object(x)).ToArray);

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
  if IsPT then exit;
  InputList.Add(Result);
end;

function Random: real;
begin
  Result := PABCSystem.Random;
  if IsPT then exit;
  InputList.Add(Result);
end;

function Random(a, b: real): real;
begin
  Result := PABCSystem.Random(a, b);
  if IsPT then exit;
  InputList.Add(Result);
end;

function Random2(a, b: integer): (integer, integer);
begin
  Result := PABCSystem.Random2(a, b);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

function Random2(a, b: real): (real, real);
begin
  Result := PABCSystem.Random2(a, b);
  if IsPT then exit;
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

function ReadString: string;
begin
  Result := PABCSystem.ReadString;
  if IsPT then exit;
  InputList.Add(Result);
  DoNewLineBeforeMessage := False;
end;

function ReadlnString := ReadString;

function ReadString2 := (ReadString, ReadString);

function ReadlnString2 := ReadString2;

function ReadInteger(prompt: string): integer;
begin
  Result := PABCSystem.ReadInteger(prompt);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  OutputList.RemoveAt(OutputList.Count - 1);
  DoNewLineBeforeMessage := False;
end;

function ReadInteger2(prompt: string): (integer, integer);
begin
  Result := PABCSystem.ReadInteger2(prompt);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  OutputList.RemoveAt(OutputList.Count - 1);
  DoNewLineBeforeMessage := False;
end;


procedure Print(params args: array of object);
begin
  foreach var ob in args do
  begin
    PABCSystem.Print(ob);
    if not IsPT then
      OutputList.RemoveAt(OutputList.Count - 1)
  end;
  DoNewLineBeforeMessage := True;
end;

procedure Println(params args: array of object);
begin
  Print(args);
  Writeln;
  if IsPT then exit;
  DoNewLineBeforeMessage := False;
end;

procedure Print(ob: object);
begin
  PABCSystem.Print(ob);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  DoNewLineBeforeMessage := True;
end;

procedure Print(s: string);
begin
  PABCSystem.Print(s);
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  DoNewLineBeforeMessage := True;
end;

procedure Print(c: char);
begin
  PABCSystem.Print(object(c));
  if IsPT then exit;
  OutputList.RemoveAt(OutputList.Count - 1);
  DoNewLineBeforeMessage := True;
end;

type
  IOLightSystem = class(__ReadSignalOISystem)
  public
    procedure write(obj: object); override;
    begin
      inherited write(obj);
      OutputString += obj.ToString;
      OutputList += obj;
      DoNewLineBeforeMessage := True;
    end;
    
    procedure writeln; override;
    begin
      inherited writeln;
      OutputString += NewLine;
      DoNewLineBeforeMessage := False;
    end;
    
    function ReadLine: string; override;
    begin
      Result := inherited ReadLine;
      DoNewLineBeforeMessage := False;
    end;
    
    procedure readln; override;
    begin
      inherited readln;
      DoNewLineBeforeMessage := False;
    end;
    
    procedure read(var x: integer); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: real); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: char); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: string); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: byte); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: shortint); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: smallint); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: word); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: longword); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: int64); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: uint64); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: single); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: boolean); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
    
    procedure read(var x: BigInteger); override;
    begin
      inherited Read(x);
      InputList.Add(x);
      DoNewLineBeforeMessage := True;
    end;
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

procedure CheckOutput(params a: array of object);
begin
  if (TaskResult = InitialTask) or (TaskResult = BadInitialTask) then
    exit;

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

procedure CheckOutputSeq(a: sequence of integer) := CheckOutput(ToObjArray(a.ToArray));

procedure CheckOutputSeq(a: sequence of real) := CheckOutput(ToObjArray(a.ToArray));

procedure CheckOutputSeq(a: sequence of string) := CheckOutput(ToObjArray(a.ToArray));

procedure CheckOutputSeq(a: sequence of char) := CheckOutput(ToObjArray(a.ToArray));

procedure CheckOutputSeq(a: sequence of boolean) := CheckOutput(ToObjArray(a.ToArray));

procedure CheckOutputSeq(a: sequence of object) := CheckOutput(a.ToArray);

procedure CheckOutputSeq(a: ObjectList) := CheckOutput(a.lst.ToArray);

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
  if DoNewLineBeforeMessage then
    Console.WriteLine;
  Console.WriteLine(MsgColorCode(color) + msg);
  DoNewLineBeforeMessage := False;
end;

function NValues(n: integer): string;
begin
  case n of
    1: Result := n + ' значение';
    2, 3, 4: Result := n + ' значения';
    5..1000: Result := n + ' значений';
  end;
end;

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
  // Для задачника надо вызывать процедуру __FinalizeModule__ из модуля PT4 jnhf;tybtv, а в модуле PT4 эту процедуру тогда не вызывть
  // Для задачника в CheckTaskPT надо сказать, что проверяется задача из задачника. И выводить на экран ничего не надо - только в базу.
  TaskResultInfo := TaskException.Info;
  
  // Теперь тщательно проверяем задачник и исполнителей
  if IsPT then
  begin
    PT4CheckSolution;
    TName := TaskName;
    var info := GetSolutionInfoPT4;
    CalcPT4Result(info,TaskResult,TaskResultInfo);
  end
  else if IsRobot then
  begin  
    RobotCheckSolution;
    TName := TaskName;
  end  
  else if IsDrawman then
  begin  
    DrawmanCheckSolution;
    TName := TaskName;
  end;
  // Хотелось бы писать в БД для Робота и др. имя задания в Task
  if WriteInfoCallBack<>nil then
    WriteInfoCallBack(TName, TaskResult, TaskResultInfo);
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
        LessonName := line
      else begin
        var (name1,name2) := Regex.Split(line,'->');
        TaskNamesMap[name1.Trim.ToLower] := name2.Trim;
      end;
        
    end;
  except
    
  end;
end;

initialization
  // Если LightPT добавляется в конец uses, то ее секция инициализации вызывается первой
  // В этом случае ввод-вывод обязательно переключается, но потом он перекрывается и в основной программе не срабатывает
  // Но в CheckPT используется ColoredMessage, которая выводит с помощью Console.WriteLine - ей всё равно
  var tn := TypeName(CurrentIOSystem);
  if (tn = 'IOStandardSystem') or (tn = '__ReadSignalOISystem') or (tn = 'IOGraphABCSystem') then
    CurrentIOSystem := new IOLightSystem;
  WriteInfoCallBack := WriteInfoToLocalDatabase;
  // Расшифровка LightPT.dat  
  LoadLightPTInfo;
finalization
  CheckMyPT  
end.