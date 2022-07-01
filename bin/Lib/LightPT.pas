/// Модуль LightPT автоматической легковесной проверки заданий
unit LightPT;

uses __RedirectIOMode;

type 
  PTException = class(Exception) end;
  InputCountException = class(PTException)
    Count,n: integer;
    constructor (Count,n: integer);
    begin
      Self.Count := Count;
      Self.n := n;
    end;
  end;
  InputCount2Exception = class(InputCountException) 
  end;
  InputTypeException = class(PTException)
    n: integer;
    ExpectedType, InputType: string;
    constructor (n: integer; ExpectedType, InputType: string);
    begin
      Self.n := n;
      Self.ExpectedType := ExpectedType;
      Self.InputType := InputType;
    end;
  end;
  TaskStatus = (Completed,NotCompleted,AbsentTask);

var 
  OutputString := new StringBuilder;
  OutputList := new List<object>;
  InputList := new List<object>;

var Cur := 0;
  
function TaskName := ExtractFileName(System.Environment.GetCommandLineArgs[0]).Replace('.exe','');

procedure WriteInfoToDatabase(name: string; result: TaskStatus);
begin
  try
    System.IO.File.AppendAllText('db.txt',$'{name} {dateTime.Now.ToString(''u'')} {Result.ToString}'+#10);
  except
    on e: Exception do
      Print(e.Message);
  end;  
end;

procedure CheckInputCount(n: integer);
begin
  if InputList.Count < n then
    raise new InputCountException(InputList.Count,n)
end;

procedure CheckInput2Count(i: integer);
begin
  if InputList.Count <= i then
    raise new InputCount2Exception(InputList.Count,i+1)
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
    raise new InputTypeException(i+1,'integer',TypeName(InputList[i]));
  Result := integer(InputList[i]);
end;
 
function Re(i: integer): real;
begin
  CheckInput2Count(i);
  if not IsRe(i) then
    raise new InputTypeException(i+1,'real',TypeName(InputList[i]));
  Result := real(InputList[i]);
end;
  
function Str(i: integer): string;
begin
  CheckInput2Count(i);
  if not IsStr(i) then
    raise new InputTypeException(i+1,'string',TypeName(InputList[i]));
  Result := string(InputList[i]);
end;
  
function Boo(i: integer): boolean; 
begin
  CheckInput2Count(i);
  if not IsBoo(i) then
    raise new InputTypeException(i+1,'boolean',TypeName(InputList[i]));
  Result := boolean(InputList[i]);
end;
  
function Chr(i: integer): char;
begin
  CheckInput2Count(i);
  if not IsChr(i) then
    raise new InputTypeException(i+1,'char',TypeName(InputList[i]));
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

function Int2: (integer,integer) := (Int,Int);
function Re2: (real,real) := (Re,Re);
function IntArr(n: integer): array of integer := (1..n).Select(x->Int).ToArray;
function ReArr(n: integer): array of real := (1..n).Select(x->Re).ToArray;

function Random(a, b: integer): integer;
begin
  Result := PABCSystem.Random(a,b);
  InputList.Add(Result);
end;

function Random(a, b: real): real;
begin
  Result := PABCSystem.Random(a,b);
  InputList.Add(Result);
end;

function Random2(a, b: integer): (integer, integer);
begin
  Result := PABCSystem.Random2(a,b);
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;
  
function Random2(a, b: real): (real, real);
begin
  Result := PABCSystem.Random2(a,b);
  InputList.Add(Result[0]);
  InputList.Add(Result[1]);
end;

function ReadString: string;
begin
  Result := PABCSystem.ReadString;
  InputList.Add(Result);
end;

function ReadlnString := ReadString;
function ReadString2 := (ReadString,ReadString);
function ReadlnString2 := ReadString2;

function ReadInteger(prompt: string): integer;
begin
  Result := PABCSystem.ReadInteger(prompt);
  OutputList.RemoveAt(OutputList.Count-1);
  OutputList.RemoveAt(OutputList.Count-1);
end;

function ReadInteger2(prompt: string): (integer,integer);
begin
  Result := PABCSystem.ReadInteger2(prompt);
  OutputList.RemoveAt(OutputList.Count-1);
  OutputList.RemoveAt(OutputList.Count-1);
end;

  
procedure Print(params args: array of object);
begin
  foreach var ob in args do
  begin
    PABCSystem.Print(ob);
    OutputList.RemoveAt(OutputList.Count-1)
  end;
end;

procedure Println(params args: array of object);
begin
  Print(args);
  Writeln;
end;

procedure Print(ob: object);
begin
  PABCSystem.Print(ob);
  OutputList.RemoveAt(OutputList.Count-1)
end;

procedure Print(s: string);
begin
  PABCSystem.Print(s);
  OutputList.RemoveAt(OutputList.Count-1)
end;

type
  IOLightSystem = class(__ReadSignalOISystem)
  public
    procedure write(obj: object); override;
    begin
      inherited write(obj);
      OutputString += obj.ToString;
      OutputList += obj;
    end;
    procedure writeln; override;
    begin
      inherited writeln;
      OutputString += NewLine;
    end;
    procedure read(var x: integer); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: real); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: char); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: string); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: byte); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: shortint); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: smallint); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: word); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: longword); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: int64); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: uint64); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: single); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: boolean); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
    procedure read(var x: BigInteger); override;
    begin
      inherited Read(x);
      InputList.Add(x);
    end;
  end;
  
function CompareElements(o1,o2: Object): boolean;
begin
  if (o1 is real) and (o2 is real) then
  begin
    var r1 := real(o1);
    var r2 := real(o2);
    Result := Abs(r1-r2)<0.0001;
    exit;
  end;
  Result := (o1.GetType = o2.GetType) and o1.Equals(o2);
end;

function ToObjArray(a: array of integer) := a.Select(x->object(x)).ToArray;
function ToObjArray(a: array of real) := a.Select(x->object(x)).ToArray;
function ToObjArray(a: array of string) := a.Select(x->object(x)).ToArray;
function ToObjArray(a: array of char) := a.Select(x->object(x)).ToArray;
function ToObjArray(a: array of boolean) := a.Select(x->object(x)).ToArray;

function CompareWithOutput(params a: array of object): TaskStatus;
begin
  Result := NotCompleted;
  if a.Length <> OutputList.Count then
    exit;
  Result := if a.Zip(OutputList,(x,y) -> CompareElements(x,y)).All(x->x) 
              then Completed 
              else NotCompleted;
  {Result := Completed;
  for var i:=0 to a.Length-1 do
    if not CompareElements(a[i],OutputList[i]) then
    begin
      Result := NotCompleted;
      exit;           
    end;}
end;

function CompareSeqWithOutput(a: sequence of integer) := CompareWithOutput(ToObjArray(a.ToArray));
function CompareSeqWithOutput(a: sequence of real) := CompareWithOutput(ToObjArray(a.ToArray));
function CompareSeqWithOutput(a: sequence of string) := CompareWithOutput(ToObjArray(a.ToArray));
function CompareSeqWithOutput(a: sequence of char) := CompareWithOutput(ToObjArray(a.ToArray));
function CompareSeqWithOutput(a: sequence of boolean) := CompareWithOutput(ToObjArray(a.ToArray));

procedure ClearOutputListFromSpaces;
begin
  OutputList := OutputList.Where(s -> (not (s is string)) or ((s as string) <> ' ')).ToList;
end;

procedure FilterOnlyNumbers;
begin
  OutputList := OutputList.Where(x -> (x is integer) or (x is real)).ToList;
end;

var CheckTask: function(name: string): TaskStatus;

procedure CheckMyPT;
begin
 if CheckTask = nil then
   exit;
 try 
  var b := CheckTask(TaskName);
  case b of
    Completed: Writeln(#10'Задание выполнено');
    NotCompleted: Writeln(#10'Задание не выполнено');
  end;
 except
   on e: InputCount2Exception do
   begin
     if e.Count = 0 then
       Writeln(#10+$'Требуется ввести по крайней мере {e.n} значения')
     else Writeln(#10+$'Введено {e.Count} значения, а требуется ввести по крайней мере {e.n}'); 
   end;
   on e: InputCountException do
   begin
     if e.Count = 0 then
       Writeln(#10+$'Требуется ввести {e.n} значения')
     else Writeln(#10+$'Введено {e.Count} значения, а требуется ввести {e.n}'); 
   end;
   on e: InputTypeException do
   begin
     Writeln(#10+$'Неверно указан тип при вводе исходных данных'); 
     Writeln($'При вводе {e.n}-го элемента типа {e.ExpectedType} использована переменная типа {e.InputType}'); 
   end;
 end;
end;  
  
initialization
  CurrentIOSystem := new IOLightSystem;
finalization
  CheckMyPT  
end.