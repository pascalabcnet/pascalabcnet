// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit FilesOperations;

interface

function ReadFileToEnd(name: string): string;
procedure WriteStringToFile(name: string; s: string);

implementation

function ReadFileToEnd(name: string): string;
var f: text;
begin
  AssignFile(f,name);
  Reset(f);
  var sb := new System.Text.StringBuilder();
  var s : string;
  while not eof(f) do
  begin
	readln(f,s);
	sb.AppendLine(s);
  end;
  ReadFileToEnd:=sb.ToString();
  CloseFile(f);{}
end;

procedure WriteStringToFile(name: string;  s:string);
var f: text;
begin
  AssignFile(f,name);
  Rewrite(f);
  Write(f,s);
  CloseFile(f);{}
end;



end.