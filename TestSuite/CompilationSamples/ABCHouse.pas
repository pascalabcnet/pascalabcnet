// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit ABCHouse;

interface

uses System.Drawing, GraphABC, ABCObjects;

type
  DoorABC = class(RectangleABC)
  public
    constructor Create(x,y,w,h: integer; cl: GColor);
    constructor Create(g: DoorABC);
    procedure Draw(x,y: integer; g: Graphics); override;
    function Clone0: ObjectABC; override;
    function Clone: DoorABC; 
  end;

  WindowABC = class(RectangleABC)
    ww: integer;
  private
    procedure SetWidth2(w: integer);
  protected
    procedure Init(x,y,w,h,www: integer; cl: GColor);
    procedure InitBy(g: WindowABC);
  public
    constructor Create(x,y,w,h,www: integer; cl: GColor);
    constructor Create(g: WindowABC);
    procedure Draw(x,y: integer; g: Graphics); override;
    property Width2: integer read ww write SetWidth2;
    function Clone0: ObjectABC; override; 
    function Clone: WindowABC; 
  end;

  HouseABC = class(ContainerABC)
  private
    function GetWindow: WindowABC; 
    function GetDoor: DoorABC; 
    function GetWall: RectangleABC; 
  protected
    procedure Init(x,y,w,h: integer; cl: GColor);
    procedure InitBy(g: HouseABC);
  public
    constructor Create(x,y,w,h: integer; cl: GColor);
    constructor Create(g: HouseABC);
    property Door: DoorABC read GetDoor;
    property Window: WindowABC read GetWindow;
    property Wall: RectangleABC read GetWall;
    function Clone0: ObjectABC; override; 
    function Clone: HouseABC; 
  end;

///--
procedure __InitModule__;

implementation

uses GraphABCHelper;

//------ DoorABC ------
constructor DoorABC.Create(x,y,w,h: integer; cl: GColor);
begin
  inherited Init(x,y,w,h,cl);
  InternalDraw;
end;

constructor DoorABC.Create(g: DoorABC);
begin
  inherited InitBy(g);
end;

procedure DoorABC.Draw(x,y: integer; g: Graphics);
begin
  inherited Draw(x,y,g);
  SetPenWidth(1);
  SetBrushColor(clWhite);
  Ellipse(x+Width-20,y+Height div 2 - 5,x+Width-10,y+Height div 2 + 5,g);
end;

function DoorABC.Clone0: ObjectABC;
begin 
  Result := new DoorABC(Self); 
end;

function DoorABC.Clone: DoorABC; 
begin 
  Result := new DoorABC(Self); 
end;

//------ WindowABC ------
procedure WindowABC.Init(x,y,w,h,www: integer; cl: GColor);
begin
  ww := www + 1;
  inherited Init(x,y,w,h,cl);
end;

procedure WindowABC.InitBy(g: WindowABC);
begin
  ww := g.ww;
  inherited InitBy(g);
end;

constructor WindowABC.Create(x,y,w,h,www: integer; cl: GColor);
begin
  Init(x,y,w,h,www,cl);
  InternalDraw;
end;

constructor WindowABC.Create(g: WindowABC);
begin
  InitBy(g);
end;

procedure WindowABC.Draw(x,y: integer; g: Graphics);
var v: integer;
begin
  v := (Width-ww*3) div 2;
  inherited Draw(x,y,g);
  SetBrushColor(clWhite);
  SetPenWidth(1);
  Rectangle(x+ww,y+ww,x+ww+v+1,y+Height-ww,g);
  Rectangle(x+ww*2+v,y+ww,x+Width-ww,y+ww+v+1,g);
  Rectangle(x+ww*2+v,y+ww*2+v,x+Width-ww,y+Height-ww,g);
end;

procedure WindowABC.SetWidth2(w: integer);
begin
  if ww=w+1 then Exit;
  ww := w + 1;
  Redraw;
end;

function WindowABC.Clone0: ObjectABC; 
begin 
  Result := new WindowABC(Self);
end;

function WindowABC.Clone: WindowABC; 
begin 
  Result := new WindowABC(Self);
end;

//------ HouseABC ------
procedure HouseABC.Init(x,y,w,h: integer; cl: GColor);
var zazw,doorw,doorh,winw,winh: integer;
begin
  zazw := w div 6;
  doorw := w div 4;
  doorh := 2*h div 3;
  winw := w div 4;
  winh := h div 2;
  inherited Init(x,y);
  Add(new RectangleABC(0,0,w,h,cl));
  Add(new DoorABC(zazw,h-doorh,doorw,doorh,clWhite));
  Add(new WindowABC(w-zazw-winw,h div 4,winw,winh,2,clWhite));
end;

procedure HouseABC.InitBy(g: HouseABC);
begin
  inherited InitBy(g);
  Add(g[0]);
  Add(g[1]);
  Add(g[2]);
end;

constructor HouseABC.Create(x,y,w,h: integer; cl: GColor);
begin
  Init(x,y,w,h,cl);
  InternalDraw;
end;

constructor HouseABC.Create(g: HouseABC);
begin
  InitBy(g);
end;

function HouseABC.GetWindow: WindowABC; 
begin 
  Result := WindowABC(l[2]);
end;

function HouseABC.GetDoor: DoorABC; 
begin 
  Result := DoorABC(l[1]);
end;

function HouseABC.GetWall: RectangleABC; 
begin 
  Result := RectangleABC(l[0]);
end;

function HouseABC.Clone0: ObjectABC;
begin 
  Result := HouseABC.Create(Self);
end;

function HouseABC.Clone: HouseABC; 
begin 
  Result := HouseABC.Create(Self); 
end;

var __initialized := false;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    ABCObjects.__InitModule__;
    GraphABC.__InitModule__;
  end;
end;

end.
