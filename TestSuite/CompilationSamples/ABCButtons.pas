// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit ABCButtons;

interface

uses GraphABC, ABCObjects, Events, GraphABCHelper;

type
  ButtonABC = class(UIElementABC)
  public
    OnClick: procedure;
    OnClickExt: procedure(Sender: ButtonABC);
  protected
    procedure Init(x,y,w,h: integer; txt: string; cl: GraphABC.Color);
    begin
      inherited Init(x,y,w,h,cl);
      TextScale := 0.7;
      TextVisible := True;
      Text := txt;
      OnClick := nil;
      OnClickExt := nil;
      InternalDraw;
    end;
  public
    constructor Create(x,y,w,h: integer; txt: string; cl: GraphABC.Color);
    begin
      Init(x,y,w,h,txt,cl);
      InternalDraw;
    end;
    constructor Create(x,y,w: integer; txt: string; cl: GraphABC.Color);
    begin
      Init(x,y,w,30,txt,cl);
      InternalDraw;
    end;
    procedure Draw(x,y: integer; g: System.Drawing.Graphics); override;
    var z,z1: integer;
    begin
      z := BorderWidth div 2;
      z1 := (BorderWidth-1) div 2;
      SetBrushColor(Color);
      SetPenColor(BorderColor);
      SetPenWidth(BorderWidth);
      RoundRect(x+z,y+z,x+Width-z1,y+Height-z1,10,10,g);
      DrawText(x,y,g);
    end;
  end;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

var __pressedButton: ButtonABC;

type MouseProc = procedure(x,y,mb: integer);

var OldOnMouseDown,OldOnMouseUp: MouseProc;

procedure ButtonsMouseDown(x,y,mb: integer);
begin
  if mb=1 then
    __pressedButton := ButtonABC(UIElementUnderPoint(x,y));
  if __pressedButton<>nil then
    __pressedButton.MoveOn(1,1)
  else if OldOnMouseDown<>nil then
    OldOnMouseDown(x,y,mb);
end;

procedure ButtonsMouseUp(x,y,mb: integer);
var b: ButtonABC;
begin
  if __pressedButton<>nil then
  begin
    __pressedButton.MoveOn(-1,-1);
    __pressedButton := nil;
    b:=ButtonABC(UIElementUnderPoint(x,y));
    if (b<>nil) and (b.OnClick<>nil) then
      b.OnClick;
    if (b<>nil) and (b.OnClickExt<>nil) then
      b.OnClickExt(b);
    if (b=nil) and (OldOnMouseDown<>nil) then
      OldOnMouseUp(x,y,mb);
  end;
end;

var __initialized := false;

procedure __InitModule;
begin
  __pressedButton := nil;
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    ABCObjects.__InitModule__;
    GraphABC.__InitModule__;
    __InitModule;
  end;
end;

procedure __FinalizeModule__;
begin
  OldOnMouseDown := OnMouseDown;
  OnMouseDown := ButtonsMouseDown;
  OldOnMouseUp := OnMouseUp;
  OnMouseUp := ButtonsMouseUp;   
end;

initialization
  __InitModule;
finalization
  __FinalizeModule__;
end.
