/// Модуль электронного задачника PT for Exam
unit PT4Exam;

//------------------------------------------------------------------------------
// Модуль для подключения задачника PT for Exam
// Версия 1.2
// Copyright (c) 2014-2017 М.Э.Абрамян
//------------------------------------------------------------------------------


interface

procedure Task(name: string);
procedure FinExam;

//1.2
procedure Show(S: string); 
procedure Show(S: string; A: Integer; W: Integer); 
procedure Show(S: string; A: Real; W: Integer); 
procedure Show(S: string; A: Integer); 
procedure Show(S: string; A: Real); 
procedure Show(A: Integer; W: Integer); 
procedure Show(A: Real; W: Integer); 
procedure Show(A: Integer); 
procedure Show(A: Real); 
procedure ShowLine; 
procedure ShowLine(S: string); 
procedure ShowLine(S: string; A: Integer; W: Integer); 
procedure ShowLine(S: string; A: Real; W: Integer); 
procedure ShowLine(S: string; A: Integer); 
procedure ShowLine(S: string; A: Real); 
procedure ShowLine(A: Integer; W: Integer); 
procedure ShowLine(A: Real; W: Integer); 
procedure ShowLine(A: Integer); 
procedure ShowLine(A: Real); 
procedure SetPrecision(N: integer);
procedure HideTask;


implementation

uses 
  PT4, System.IO, System.Text;

procedure ToWin(s: string);
begin
  var fs: StreamReader := new StreamReader(s);
  var fs1: StreamWriter := new StreamWriter('$$.tmp', false, 
    Encoding.Default); 
  fs1.Write(fs.ReadToEnd);
  fs.Close;
  fs1.Close;    
  System.IO.File.Delete(s);
  System.IO.File.Move('$$.tmp',s);
end;

var 
  NextTask: boolean := False;
  s1, s2: string;

procedure Task(name: string);
var 
  f: textfile;
begin
  PT4.Task(name);
  if NextTask then
    exit;
  NextTask := True;  
  GetS(s1);
  GetS(s2);
  if not FileExists(s1) then
  begin
    s1 := 'null1.tst';
    Assign(f, s1);
    Rewrite(f);
    Close(f);
  end;
  if s2 = '' then
    s2 := 'null2.tst';
  Assign(input, s1);
  Reset(input);
  Assign(output, s2);
  Rewrite(output);
end;

procedure FinExam;
begin
  Close(input);
  Close(output);
  if s1 = 'null1.tst' then
    Erase(input);
  if s2 = 'null2.tst' then
    Erase(output)
  else  
    ToWin(s2);
  NextTask := false;  
end;

procedure Show(S: string);
begin
  PT4.Show(S);
end;

procedure Show(S: string; A: Integer; W: Integer);
begin
  PT4.Show(S, A, W);
end;

procedure Show(S: string; A: Real; W: Integer);
begin
  PT4.Show(S, A, W);
end;

procedure Show(S: string; A: Integer);
begin
  PT4.Show(S, A);
end;

procedure Show(S: string; A: Real);
begin
  PT4.Show(S, A);
end;

procedure Show(A: Integer; W: Integer);
begin
  PT4.Show(A, W);
end;

procedure Show(A: Real; W: Integer);
begin
  PT4.Show(A, W);
end;

procedure Show(A: Integer);
begin
  PT4.Show(A);
end;

procedure Show(A: Real);
begin
  PT4.Show(A);
end;

procedure ShowLine(S: string);
begin
  PT4.ShowLine(S);
end;

procedure ShowLine;
begin
  PT4.ShowLine;
end;

procedure ShowLine(S: string; A: Integer; W: Integer);
begin
  PT4.ShowLine(S, A, W);
end;

procedure ShowLine(S: string; A: Real; W: Integer);
begin
  PT4.ShowLine(S, A, W);
end;

procedure ShowLine(S: string; A: Integer);
begin
  PT4.ShowLine(S, A);
end;

procedure ShowLine(S: string; A: Real);
begin
  PT4.ShowLine(S, A);
end;

procedure ShowLine(A: Integer; W: Integer);
begin
  PT4.ShowLine(A, W);
end;

procedure ShowLine(A: Real; W: Integer);
begin
  PT4.ShowLine(A, W);
end;

procedure ShowLine(A: Integer);
begin
  PT4.ShowLine(A);
end;

procedure ShowLine(A: Real);
begin
  PT4.ShowLine(A);
end;

procedure HideTask;
begin
  PT4.HideTask;
end;

procedure SetPrecision(N: Integer);
begin
  PT4.SetPrecision(N);
end;


initialization

finalization

  FinExam;

end.
