unit PT4MakerNetX;

// Конструктор учебных заданий для задачника Programming Taskbook
// М.Э.Абрамян, 2016
// Версия 1.1 от 9.11.2016

interface

uses PT4TaskMakerNET;

procedure TaskText(topic, s: string);
procedure TaskText(s: string);

procedure DataComm(Comm: string);
procedure Data(Comm: string; a: object);
procedure Data(Comm1: string; a1: object; Comm2: string);
procedure Data(Comm1: string; a1: object; Comm2: string; a2: object);
procedure Data(Comm1: string; a1: object; Comm2: string; a2: object; Comm3: string);
procedure Data(Comm1: string; a1: object; Comm2: string; a2: object; Comm3: string; a3: object);
procedure Data(seq: sequence of boolean);
procedure Data(seq: sequence of integer);
procedure Data(seq: sequence of real);
procedure Data(seq: sequence of char);
procedure Data(seq: sequence of string);

procedure ResComm(Comm: string);
procedure Res(Comm: string; a: object);
procedure Res(Comm1: string; a1: object; Comm2: string);
procedure Res(Comm1: string; a1: object; Comm2: string; a2: object);
procedure Res(Comm1: string; a1: object; Comm2: string; a2: object; Comm3: string);
procedure Res(Comm1: string; a1: object; Comm2: string; a2: object; Comm3: string; a3: object);
procedure Res(seq: sequence of boolean);
procedure Res(seq: sequence of integer);
procedure Res(seq: sequence of real);
procedure Res(seq: sequence of char);
procedure Res(seq: sequence of string);

procedure SetPrecision(n: integer);
procedure SetTestCount(n: integer);
procedure SetRequiredDataCount(n: integer);
function CurrentTest: integer;

function RandomN(M, N: integer): integer;
function RandomR(A, B: real): real;
function Random(A, B: real): real;
function RandomName(len: integer): string;

procedure CreateGroup(GroupName, GroupDescription, GroupAuthor: string);
procedure ActivateNET(S: string);
procedure UseTask(GroupName: string; TaskNumber: integer);

function GetWords: array of string;
function GetEnWords: array of string;
function GetSentences: array of string;
function GetEnSentences: array of string;
function GetTexts: array of string;
function GetEnTexts: array of string;

procedure DataFileInteger(FileName: string);
procedure DataFileReal(FileName: string);
procedure DataFileChar(FileName: string);
procedure DataFileString(FileName: string);
procedure DataText(FileName: string; LineCount: integer := 4);

procedure ResFileInteger(FileName: string);
procedure ResFileReal(FileName: string);
procedure ResFileChar(FileName: string);
procedure ResFileString(FileName: string);
procedure ResText(FileName: string; LineCount: integer := 5);

implementation

uses System.Reflection, Microsoft.Win32;

const
  alphabet = '0123456789abcdefghijklmnopqrstuvwxyz';
  ErrMes1 = 'Error: Раздел размером более 5 строк не может содержать файловые данные.';
  ErrMes2 = 'Error: При наличии файловых данных раздел не может содержать более 5 строк.';
  ErrMes3 = 'Error: Количество исходных данных превысило 200.';
  ErrMes4 = 'Error: Количество результирующих данных превысило 200.';

var
  yd, yr, nd, nr, pr: integer;
  fd, fr: boolean;
  fmt: string;
  tasks := new List<MethodInfo>(100);

function ErrorMessage(s: string): string;
begin
  result := Copy('==' + s + new string('=', 100), 1, 78);
end;

function RandomName(len: integer): string;
begin
  result := ArrRandom(len, 1, alphabet.Length)
    .Select(e -> alphabet[e]).JoinIntoString('');
end;

procedure SetPrecision(n: integer);
begin
  if abs(n) > 10 then exit;
  pr := n;
  if n < 0 then
  begin
    fmt := 'e' + IntToStr(-n);
    n := 0;
  end
  else if n = 0 then
    fmt := 'e6'
  else
    fmt := 'f' + IntToStr(n); 
  PT4TaskMakerNET.SetPrecision(n);
end;

procedure TaskText(topic, s: string);
begin
  PT4TaskMakerNET.CreateTask(topic);
  PT4TaskMakerNET.TaskText(s);
  yd := 0;
  yr := 0;
  nd := 0;
  nr := 0;
  fd := false;
  fr := false;
  pr := 2;
  fmt := 'f2';
end;

function wreal(w: integer; x: real): integer;
begin
  result := w;
  if w = 0 then
  begin
    result := Format('{0,0:' + fmt + '}', x).Length;
    if (pr <= 0) and (x >= 0) then
      result := result + 1;
  end;
end;

function winteger(w: integer; x: integer): integer;
begin
  result := w;
  if w = 0 then
    result := IntToStr(x).Length;
end;

procedure TaskText(s: string);
begin
  TaskText('', s);
end;

procedure Data(s: string; a: object; x, y, w: integer);
begin
  if (y > 5) and fd then
  begin
    PT4TaskMakerNET.TaskText(ErrorMessage(ErrMes2), 0, 1);
    PT4TaskMakerNET.TaskText(ErrorMessage(''), 0, 2);
    exit;  
  end;
  inc(nd);
  if nd > 200 then
  begin
    PT4TaskMakerNET.TaskText(ErrorMessage(ErrMes3), 0, 1);
    PT4TaskMakerNET.TaskText(ErrorMessage(''), 0, 2);
    exit;  
  end;
  if a is boolean then
    DataB(s, boolean(a), x, y)
  else if a is integer then
    DataN(s, integer(a), x, y, winteger(w, integer(a)))
  else if a is real then
    DataR(s, real(a), x, y, wreal(w, real(a)))
  else if a is char then
    DataC(s, char(a), x, y)
  else if a is string then
    DataS(s, string(a), x, y)
  else
    DataComment(Copy(s + '!WrongType:' + a.GetType.Name, 1, 38), x, y);
end;

procedure Res(s: string; a: object; x, y, w: integer);
begin
  if (y > 5) and fr then
  begin
    PT4TaskMakerNET.TaskText(ErrorMessage(ErrMes2), 0, 1);
    PT4TaskMakerNET.TaskText(ErrorMessage(''), 0, 2);
    exit;  
  end;
  inc(nr);
  if nr > 200 then
  begin
    PT4TaskMakerNET.TaskText(ErrorMessage(ErrMes4), 0, 1);
    PT4TaskMakerNET.TaskText(ErrorMessage(''), 0, 2);
    exit;  
  end;
  if a is boolean then
    ResultB(s, boolean(a), x, y)
  else if a is integer then
    ResultN(s, integer(a), x, y, winteger(w, integer(a)))
  else if a is real then
    ResultR(s, real(a), x, y, wreal(w, real(a)))
  else if a is char then
    ResultC(s, char(a), x, y)
  else if a is string then
    ResultS(s, string(a), x, y)  
  else
    ResultComment(Copy(s + '!WrongType:' + a.GetType.Name, 1, 38), x, y);
end;

procedure DataComm(comm: string);
begin
  inc(yd);
  DataComment(comm, 0, yd);
end;

procedure Data(comm: string; a: object);
begin
  inc(yd);
  Data(comm, a, 0, yd, 0);
end;

procedure Data(comm1: string; a1: object; comm2: string);
begin
  inc(yd);
  Data(comm1, a1, xLeft, yd, 0);
  DataComment(comm2, xRight, yd);
end;

procedure Data(comm1: string; a1: object; comm2: string; a2: object);
begin
  inc(yd);
  Data(comm1, a1, xLeft, yd, 0);
  Data(comm2, a2, xRight, yd, 0);
end;

procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  inc(yd);
  Data(comm1, a1, xLeft, yd, 0);
  Data(comm2, a2, 0, yd, 0);
  DataComment(comm3, xRight, yd);
end;

procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  inc(yd);
  Data(comm1, a1, xLeft, yd, 0);
  Data(comm2, a2, 0, yd, 0);
  Data(comm3, a3, xRight, yd, 0);
end;

procedure Data(seq: sequence of boolean);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := 5;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of integer);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := seq.Select(e -> IntToStr(e)).Max(e -> e.Length);
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of real);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := seq.Select(e -> wreal(0, e)(*Format('{0,0:'+fmt+'}', e)*)).Max;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of char);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := 3;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of string);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := seq.Max(e -> e.Length) + 2;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure ResComm(comm: string);
begin
  inc(yr);
  ResultComment(comm, 0, yr);
end;

procedure Res(comm: string; a: object);
begin
  inc(yr);
  Res(comm, a, 0, yr, 0);
end;

procedure Res(comm1: string; a1: object; comm2: string);
begin
  inc(yr);
  Res(comm1, a1, xLeft, yr, 0);
  ResultComment(comm2, xRight, yr);
end;

procedure Res(comm1: string; a1: object; comm2: string; a2: object);
begin
  inc(yr);
  Res(comm1, a1, xLeft, yr, 0);
  Res(comm2, a2, xRight, yr, 0);
end;

procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  inc(yr);
  Res(comm1, a1, xLeft, yr, 0);
  Res(comm2, a2, 0, yr, 0);
  ResultComment(comm3, xRight, yr);
end;

procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  inc(yr);
  Res(comm1, a1, xLeft, yr, 0);
  Res(comm2, a2, 0, yr, 0);
  Res(comm3, a3, xRight, yr, 0);
end;

procedure Res(seq: sequence of boolean);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := 5;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of integer);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := seq.Select(e -> IntToStr(e)).Max(e -> e.Length);
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of real);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := seq.Select(e -> wreal(0, e)).Max;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of char);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := 3;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of string);
begin
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := seq.Max(e -> e.Length) + 2;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;


procedure ActivateNET(S: string);
begin
  PT4TaskMakerNET.ActivateNET(S);
end;

function TryAssembly(path: string): Assembly;
begin
  result := nil;
  if FileExists(path) then
    try
      result := Assembly.LoadFrom(path);
    except
    end;
end;

function GetAssembly(nm: string): Assembly;
begin
  result := TryAssembly('C:\PABCWork.NET\' + nm);
  if result = nil then
    result := TryAssembly('C:\Program Files (x86)\PascalABC.NET\PT4\Lib\' + nm);
  if result = nil then
    result := TryAssembly('C:\Program Files\PascalABC.NET\PT4\Lib\' + nm);
  if result <> nil then
    exit;
  var dir: string := '';
  var rk: RegistryKey := nil;
  try
    try
      rk := Registry.CurrentUser.OpenSubKey('Software\PascalABC.NET');
      if rk <> nil then
        dir := string(rk.GetValue('Install Directory', ''));
    finally
      if rk <> nil then
        rk.Close;
    end;
  except
  end;
  if dir <> '' then 
    result := TryAssembly(dir + '\PT4\Lib\' + nm);
end;



procedure RunTask(num: integer);
begin
  try
    if (num > 0) and (num <= tasks.Count) then
      tasks[num - 1].Invoke(nil, nil);
  except
    on e: TargetInvocationException do
    begin
      PT4TaskMakerNET.TaskText(ErrorMessage('Error ' 
          + e.InnerException.GetType.Name + ': '
          + e.InnerException.message), 0, 1);
      PT4TaskMakerNET.TaskText(ErrorMessage(''), 0, 2);
      raise;  
    end;
  end;
end;


procedure CreateGroup(GroupName, GroupDescription, GroupAuthor: string);
begin
  if CurrentLanguage <> lgPascalABCNET then exit; // задания доступны только для PascalABC.NET
  var nm := 'PT4' + GroupName;
  tasks.Clear;
  var ass: Assembly := GetAssembly(nm + '.dll');
  if ass = nil then exit;  
  foreach var e in ass.GetType(nm + '.' + nm).GetMethods do
    if e.Name.ToUpper.StartsWith('TASK') then
      tasks.Add(e);
  PT4TaskMakerNET.CreateGroup(GroupName, GroupDescription, GroupAuthor, 
      '111' + nm + '222', tasks.Count, RunTask);
end;


procedure SetTestCount(n: integer);
begin
  PT4TaskMakerNET.SetTestCount(n);
end;

procedure SetRequiredDataCount(n: integer);
begin
  PT4TaskMakerNET.SetRequiredDataCount(n);
end;

function RandomN(M, N: integer): integer;
begin
  result := PT4TaskMakerNET.RandomN(M, N);
end;

function RandomR(A, B: real): real;
begin
  result := PT4TaskMakerNET.RandomR(A, B);
end;

function Random(A, B: real): real;
begin
  result := PT4TaskMakerNET.RandomR(A, B);
end;

function CurrentTest: integer;
begin
  result := PT4TaskMakerNET.CurrentTest;
end;

procedure UseTask(GroupName: string; TaskNumber: integer);
begin
  PT4TaskMakerNET.UseTask(GroupName, TaskNumber);
end;

function GetWords: array of string;
begin
  Result := ArrGen(WordCount, i -> WordSample(i));
end;

function GetEnWords: array of string;
begin
  Result := ArrGen(EnWordCount, i -> EnWordSample(i));
end;

function GetSentences: array of string;
begin
  Result := ArrGen(SentenceCount, i -> SentenceSample(i));
end;

function GetEnSentences: array of string;
begin
  Result := ArrGen(EnSentenceCount, i -> EnSentenceSample(i));
end;

function GetTexts: array of string;
begin
  Result := ArrGen(TextCount, i -> TextSample(i));
end;

function GetEnTexts: array of string;
begin
  Result := ArrGen(EnTextCount, i -> EnTextSample(i));
end;

procedure DataFileInteger(FileName: string);
begin
  inc(yd);
  if yd > 5 then
  begin
    DataComm(ErrorMessage(ErrMes1));
    exit;
  end;
  var w := 0;
  try
    var f: file of integer;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: integer;
      read(f, a);
      var s := IntToStr(a);
      if s.Length > w then
        w := s.Length;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      DataComm(ErrorMessage('FileError(' + FileName + '): ' + ex.Message));
      exit;
    end;  
  end;
  fd := true;
  DataFileN(FileName, yd, w + 2);
end;

procedure DataFileReal(FileName: string);
begin
  inc(yd);
  if yd > 5 then
  begin
    DataComm(ErrorMessage(ErrMes1));
    exit;
  end;
  var w := 0;
  try
    var f: file of real;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: real;
      read(f, a);
      var s := wreal(0, a);
      if s > w then
        w := s;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      DataComm(ErrorMessage('FileError(' + FileName + '): ' + ex.Message));
      exit;
    end;  
  end;
  fd := true;
  DataFileR(FileName, yd, w + 2);
end;

procedure DataFileChar(FileName: string);
begin
  inc(yd);
  if yd > 5 then
  begin
    DataComm(ErrorMessage(ErrMes1));
    exit;
  end;
  fd := true;
  DataFileC(FileName, yd, 5);
end;

procedure DataFileString(FileName: string);
begin
  inc(yd);
  if yd > 5 then
  begin
    DataComm(ErrorMessage(ErrMes1));
    exit;
  end;
  var w := 0;
  try
    var f: file of ShortString;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: ShortString;
      read(f, a);
      if a.Length > w then
        w := a.Length;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      DataComm(ErrorMessage('FileError(' + FileName + '): ' + ex.Message));
      exit;
    end;  
  end;
  fd := true;
  DataFileS(FileName, yd, w + 4);
end;

procedure DataText(FileName: string; LineCount: integer);
begin
  inc(yd);
  if yd > 5 then
  begin
    ResComm(ErrorMessage(ErrMes1));
    exit;
  end;
  fd := true;
  var yd2 := yd + LineCount - 1;
  if yd2 > 5 then yd2 := 5;
  DataFileT(FileName, yd, yd2);
  yd := yd2;
end;

procedure ResFileInteger(FileName: string);
begin
  inc(yr);
  if yr > 5 then
  begin
    ResComm(ErrorMessage(ErrMes1));
    exit;
  end;
  var w := 0;
  try
    var f: file of integer;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: integer;
      read(f, a);
      var s := IntToStr(a);
      if s.Length > w then
        w := s.Length;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      ResComm(ErrorMessage('FileError(' + FileName + '): ' + ex.Message));
      exit;
    end;  
  end;
  fr := true;
  ResultFileN(FileName, yr, w + 2);
end;

procedure ResFileReal(FileName: string);
begin
  ResComm(fmt);
  inc(yr);
  if yr > 5 then
  begin
    ResComm(ErrorMessage(ErrMes1));
    exit;
  end;
  var w := 0;
  try
    var f: file of real;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: real;
      read(f, a);
      var s := wreal(0, a);
      if s > w then
        w := s;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      ResComm(ErrorMessage('FileError(' + FileName + '): ' + ex.Message));
      exit;
    end;  
  end;
  fr := true;
  ResultFileR(FileName, yr, w + 2);
end;

procedure ResFileChar(FileName: string);
begin
  inc(yr);
  if yr > 5 then
  begin
    ResComm(ErrorMessage(ErrMes1));
    exit;
  end;
  fr := true;
  ResultFileC(FileName, yr, 5);
end;

procedure ResFileString(FileName: string);
begin
  inc(yr);
  if yr > 5 then
  begin
    ResComm(ErrorMessage(ErrMes1));
    exit;
  end;
  var w := 0;
  try
    var f: file of ShortString;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: ShortString;
      read(f, a);
      if a.Length > w then
        w := a.Length;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      ResComm(ErrorMessage('FileError(' + FileName + '): ' + ex.Message));
      exit;
    end;  
  end;
  fr := true;
  ResultFileS(FileName, yr, w + 4);
end;

procedure ResText(FileName: string; LineCount: integer);
begin
  inc(yr);
  if yr > 5 then
  begin
    ResComm(ErrorMessage(ErrMes1));
    exit;
  end;
  fr := true;
  var yr2 := yr + LineCount - 1;
  if yr2 > 5 then yr2 := 5;
  ResultFileT(FileName, yr, yr2);
  yr := yr2;
end;

end.