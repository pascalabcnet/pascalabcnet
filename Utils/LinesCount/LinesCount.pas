// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
uses CRT,
     System.IO;

var  FilesCount,LinesCount:integer;
     Size: longint;
  
function GetLinesCount(FileName: string):integer;
var sr: StreamReader;
begin
  sr := new StreamReader(FileName, System.Text.Encoding.GetEncoding(1251));
  while not sr.EndOfStream do begin
    //if sr.ReadLine<>'' then
    sr.ReadLine;
    result := result + 1;
  end;
  sr.Close;
end;

function GetSize(FileName: string):longint;
var sr: StreamReader;
begin
  sr := new StreamReader(FileName, System.Text.Encoding.GetEncoding(1251));
  result := sr.BaseStream.Length;   
  sr.Close;
end;

procedure ScanDirectory(directory,mask: string);
var Files: array of FileInfo;
    Dirs: array of DirectoryInfo;
    DI: DirectoryInfo;
    i: integer;
begin
  DI := new DirectoryInfo(directory);
  Files := DI.GetFiles(mask);
  for i:=0 to Files.Length-1 do begin
    LinesCount := LinesCount + GetLinesCount(Files[i].FullName);
    FilesCount := FilesCount + 1;
    Size := Size + GetSize(Files[i].FullName);
    System.Console.CursorLeft := 0;
    Write(string.Format('Files: {0}, lines: {1}, size {2}kb, lineSize {3}', FilesCount, LinesCount, (Size/1024):2:3, Size/LinesCount:3:1),'             ');
  end;
  Dirs := DI.GetDirectories;
  for i:=0 to Dirs.Length-1 do
    if directory.ToLower <> Dirs[i].FullName.ToLower then
       ScanDirectory(Dirs[i].FullName, mask);    	
end;

begin
  if CommandLineArgs.Length=0 then begin
    Writeln('linescount mask');
    Exit;
  end;
  ScanDirectory(System.Environment.CurrentDirectory, CommandLineArgs[0]);
  Writeln;
end.