unit BFSystem;

//BF language support
//Version 2.1
//Copyright (c) 2006-2007 DarkStar

{$apptype console}

interface

uses System, PABCSystem, CRT;

const 
  BufferSize 		= 30000;
  ConsoleTextColor	= Green;
  InFileExt	 		= '.in';
  OutFileExt 		= '.out';
  
var
  BFField := new byte[BufferSize];
  BFCaretPos:integer;
  BFOutFile:Text;
  BFInFile:Text;
  
procedure WriteCaretValue;			//.
procedure ReadCaretValue;			//,

implementation

var ReadFromInFile:boolean;

procedure BFInit;
var fin_name: string;
begin
  BFCaretPos := 0;
  fin_name := ChangeFileNameExtension(GetEXEFileName, InFileExt);
  ReadFromInFile := FileExists(fin_name);
  if ReadFromInFile then begin
    AssignFile(BFInFile, fin_name);
    Reset(BFInFile);
  end;
  AssignFile(BFOutFile,ChangeFileNameExtension(GetEXEFileName, OutFileExt));
  Rewrite(BFOutFile);
  TextColor(ConsoleTextColor);
end;

procedure BFClose;
begin
  CloseFile(BFInFile);
  CloseFile(BFOutFile);
end;

procedure WriteCaretValue;
var c: char;
begin
  c := char(BFField[BFCaretPos]);
  Write(c);
  Write(BFOutFile, c);
end;

procedure ReadCaretValue;
var c: char;
begin
  if ReadFromInFile then begin
    if EOF(BFInFile) then
      BFField[BFCaretPos] := 0
    else begin
      Read(BFInFile, c);
      BFField[BFCaretPos] := byte(c);
    end;
  end else
    BFField[BFCaretPos] := byte(CurrentIOSystem.read_symbol);
end;

initialization
  BFInit;
finalization
  BFClose;
end.
