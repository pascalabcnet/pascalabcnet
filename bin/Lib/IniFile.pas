// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit IniFile;

interface

uses System.Collections;

type
  TIniFile = class
    fname: string;
    sl: SortedList;
    function IndexOfSection(Section: string): integer;
    function IndexOfIdent(SectionNum: integer; Ident: string): integer;
  public
    constructor Create(name: string);
    procedure Save;
    property FileName: string read fname;
    procedure WriteString(Section,Ident: string; Value: string);
    function ReadString(Section,Ident: string; DefaultValue: string): string;
    procedure WriteInteger(Section,Ident: string; Value: integer);
    procedure WriteBoolean(Section,Ident: string; Value: boolean);
    function ReadInteger(Section,Ident: string; DefaultValue: integer): integer;
    function ReadBoolean(Section,Ident: string; DefaultValue: boolean): boolean;
  end;
  
implementation  

constructor TIniFile.Create(name: string);
var 
  f: text;
  s,Section,Ident,Value: string;
  p: integer;
  sl1: SortedList;
begin
  sl := new SortedList;
  fname := name;
  assign(f,fname);
  if not FileExists(fname) then
  begin
    rewrite(f);
    closefile(f);
  end
  else
  begin
    try
      Section := '';
      reset(f);
      while not eof(f) do
      begin
        readln(f,s);
        s := Trim(s);
        if Length(s)=0 then 
          continue;
        if s[1]='[' then // Section
        begin
          section := Copy(s,2,Length(s)-2);
          // есть ли такая секция
          if IndexOfSection(Section)<0 then 
            sl.Add(Section,new SortedList);
        end
        else
        begin
          if Section = '' then 
            continue;
          p := Pos('=',s);
          if p=0 then 
            continue;
          Ident := Copy(s,1,p-1);
          Delete(s,1,p);
          Value := s;
          sl1 := SortedList(sl[Section]);
          sl1[Ident] := Value;
        end; 
      end;    
    except
      // погасить все исключения
    end;
    close(f);
  end;
end;

function TIniFile.IndexOfSection(Section: string): integer;
begin
  Result := sl.IndexOfKey(Section);
end;

function TIniFile.IndexOfIdent(SectionNum: integer; Ident: string): integer;
var sl1: SortedList;
begin
  sl1 := SortedList(sl.GetByIndex(SectionNum));
  Result := sl1.IndexOfKey(Ident);
end;


procedure TIniFile.Save;
var 
  sl1: SortedList;
  f: text;
begin
  assign(f,fname);
  rewrite(f);
  for var i:=0 to sl.Count-1 do    
  begin
    writeln(f,'['+sl.GetKey(i)+']');
    sl1 := SortedList(sl.GetByIndex(i));
    for var j:=0 to sl1.Count-1 do    
      writeln(f,sl1.GetKey(j)+'='+string(sl1.GetByIndex(j)));
    writeln(f);
  end;
  close(f);
end;

procedure TIniFile.WriteString(Section,Ident: string; Value: string);
var 
  i: integer;
  sl1: SortedList;
begin
  i := IndexOfSection(Section);
  if i<0 then 
  begin
    // Create Section
    sl1 := new SortedList;
    sl1[Ident] := Value;
    sl[Section] := sl1;
  end
  else
  begin
    sl1 := SortedList(sl.GetByIndex(i));
    sl1[Ident] := Value
  end;
end;

function TIniFile.ReadString(Section,Ident: string; DefaultValue: string): string;
var 
  i,j: integer;
  sl1: SortedList;
begin
  i := IndexOfSection(Section);
  if i<0 then 
    Result := DefaultValue
  else
  begin
    sl1 := SortedList(sl.GetByIndex(i));
    j := IndexOfIdent(i,Ident);
    if j<0 then 
      Result := DefaultValue
    else Result := string(sl1.GetByIndex(j));
  end;
end;

procedure TIniFile.WriteInteger(Section,Ident: string; Value: integer);
begin
  WriteString(Section,Ident,IntToStr(Value));
end;

procedure TIniFile.WriteBoolean(Section,Ident: string; Value: boolean);
begin
  if Value then
    WriteString(Section,Ident,'True')
  else WriteString(Section,Ident,'False')  
end;

function TIniFile.ReadInteger(Section,Ident: string; DefaultValue: integer): integer;
var 
  s: string;
  i,err: integer;
begin
  s := ReadString(Section,Ident,IntToStr(DefaultValue));
  Val(s,i,err);
  if err=0 then
    Result := i
  else Result := 0;  
end;

function TIniFile.ReadBoolean(Section,Ident: string; DefaultValue: boolean): boolean;
var s: string;
begin
  if DefaultValue then
    s := ReadString(Section,Ident,'True')
  else s := ReadString(Section,Ident,'False');
  Result := UpperCase(s) = 'TRUE'  
end;

end.  