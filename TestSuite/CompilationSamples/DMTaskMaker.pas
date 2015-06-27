// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit DMTaskMaker;

interface

uses System.Collections.Generic;

procedure Field(szx,szy: integer);
procedure Field(szx,szy,x0,y0: integer);
procedure SetOrigin(x0,y0: integer);
procedure DoToPoint(x,y: integer); 
procedure DoOnVector(dx,dy: integer); 
procedure DoPenUp; 
procedure DoPenDown; 

procedure TaskText(s: string);

procedure CorrectFieldBounds;
procedure Stop;
procedure Start;

function IsSolution: boolean;

type TaskProcType = procedure;

procedure RegisterTask(name: string; p: TaskProcType);

procedure RegisterGroup(name,description,unitname: string; count: integer);

var TasksDictionary := new Dictionary<string,TaskProcType>;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

uses DrawmanField;

procedure RegisterTask(name: string; p: TaskProcType);
begin
  TasksDictionary.Add(name,p);
end;

procedure CorrectFieldBounds;
begin
  DrawmanField.CorrectWHLT;
end;

procedure Field(szx,szy: integer);
begin
  DMField.Clear;
  DMField.SetDim(szx,szy,30);
end;

procedure Field(szx,szy,x0,y0: integer);
begin
  Field(szx,szy);
  SetOrigin(x0,y0);
end;

procedure DoToPoint(x,y: integer); 
begin
  DMField.MakerToPoint(x,y);
end;

procedure DoOnVector(dx,dy: integer); 
begin
  DMField.MakerOnVector(dx,dy);
end;

procedure DoPenUp; 
begin
  DMField.MakerPenUp; 
end;

procedure DoPenDown; 
begin
  DMField.MakerPenDown; 
end;

procedure SetOrigin(x0,y0: integer);
begin
  DMField.SetOrigin(x0,y0);
end;

procedure TaskText(s: string);
begin
  DMField.TaskText(s);
end;

procedure Stop;
begin
  DMField.Stop;
end;

procedure Start;
begin
  DMField.Start;
end;

function IsSolution: boolean;
begin
  Result := DMField.IsSolution;
end;

procedure registerdm(UnitName, GroupName, Description: string; TaskCount: integer);
  external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'registerdm';

procedure RegisterGroup(name,description,unitname: string; count: integer);
begin
try
  registerdm(unitname,name,description,count);
except
end;
end;

var __initialized := false;
var __finalized := false;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    DrawmanField.__InitModule__;
  end;  
end;

procedure __FinalizeModule__;
begin
  if not __finalized then
  begin
    __finalized := true;
    DrawmanField.__FinalizeModule__;
  end;  
end;

end.

