// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit RobotTaskMaker;

interface

uses System.Collections.Generic;

procedure Field(szx,szy: integer);
procedure HorizontalWall(x,y,len: integer);
procedure VerticalWall(x,y,len: integer);
procedure RobotBegin(x,y: integer);
procedure RobotEnd(x,y: integer);
procedure RobotBeginEnd(x,y,x1,y1: integer);
procedure Tag(x,y: integer);
procedure TagRect(x,y,x1,y1: integer);
procedure MarkPainted(x,y: integer);
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

implementation

uses RobotField;

procedure RegisterTask(name: string; p: TaskProcType);
begin
  TasksDictionary.Add(name,p);
end;

procedure CorrectFieldBounds;
begin
  RobotField.CorrectWHLT;
end;

procedure Field(szx,szy: integer);
begin
  RobField.Clear;
  RobField.SetDim(szx,szy,30);
end;

procedure HorizontalWall(x,y,len: integer);
begin
  RobField.HorizWall(x+1,y,len);
end;

procedure VerticalWall(x,y,len: integer);
begin
  RobField.VertWall(x,y+1,len);
end;

procedure RobotBegin(x,y: integer);
begin
  RobField.SetFirstRobotPos(x,y);
end;

procedure RobotEnd(x,y: integer);
begin
  RobField.SetLastRobotPos(x,y);
end;

procedure RobotBeginEnd(x,y,x1,y1: integer);
begin
  RobField.SetFirstLastRobotPos(x,y,x1,y1);
end;

procedure Tag(x,y: integer);
begin
  RobField.SetTag(x,y)
end;

procedure TagRect(x,y,x1,y1: integer);
begin
  RobField.SetTagRect(x,y,x1,y1);
end;

procedure MarkPainted(x,y: integer);
begin
  RobField.SetPaintMaker(x,y);
end;

procedure TaskText(s: string);
begin
  RobField.TaskText(s);
end;

procedure Stop;
begin
  RobField.Stop;
end;

procedure Start;
begin
  RobField.Start;
end;

function IsSolution: boolean;
begin
  Result := RobField.IsSolution;
end;

procedure registerrb(UnitName, GroupName, Description: string; TaskCount: integer);
  external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'registerrb';

procedure RegisterGroup(name,description,unitname: string; count: integer);
begin
try
  registerrb(unitname,name,description,count);
except
end;
end;

var __initialized := false;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    RobotField.__InitModule__;
  end;  
end;
end.

