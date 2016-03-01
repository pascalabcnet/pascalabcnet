// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

///Модуль ABCSprites реализует спрайты - анимационные объекты с автоматически меняющимися кадрами. 
///Спрайт представляется классом SpriteABC и является разновидностью мультикартинки MultiPictureABC.
unit ABCSprites;

//#savepcu false

interface

uses ABCObjects,Events,GraphABC,Utils;

type
  SpriteState = record
    Name: string;   // имена состояний
    Beg: integer;   // номера картинок, являющихся началами состояний
    Count: integer; // длины состояний
    constructor Create(n: string; b,c: integer);
    begin
      Name := n;
      Beg := b;
      Count := c;
    end;
  end;

  SpriteABC = class(MultiPictureABC)
  private
    States: Array of SpriteState;
    curst: integer;          // номер текущего состояния
    ticks: integer;          // сколько тиков проходит до смены кадра, обратно пропорционален скорости: 0..10
    act: boolean;            // активен ли спрайт
    curtick: integer;        // текущий тик, когда он становится равным
    procedure SetStateName(n: string);
    function GetStateName: string;
    procedure SetState(n: integer);
    function GetStateCount: integer;
    procedure SetSpeed(n: integer);
    function GetSpeed: integer;
    procedure SetActive(b: boolean);
    procedure SetFrame(n: integer);
    function GetFrame: integer;
  protected  
    procedure Init0;
    procedure Init(x,y: integer; fname: string);   // после этого придется вызывать Add и AddState, затем CheckStates
    procedure Init(x,y,w: integer; fname: string); // после этого придется вызывать AddState, затем CheckStates
    procedure Init(x,y,w: integer; p: Picture);
    procedure InitWithStates(x,y: integer; fname: string);
    procedure InitBy(g: SpriteABC);
  public
    /// Создает спрайт, загружая его из файла с именем fname. 
    ///Имя fname может быть либо именем графического файла, либо именем информационного файла спрайта с расширением .spinf 
    ///Если имя является именем графического файла, то создается спрайт с одним кадром. Остальные кадры добавляются методом Add.
    ///После этого при необходимости добавляются состояния методом AddStates и вызывается метод CheckStates 
    ///Если файл имеет расширение .spinf, то он содержит информацию о кадрах и состояниях спрайта и
    ///должен сопровождаться соответствующим графическим файлом.
    ///После создания спрайт отображается на экране в позиции (x,y)
    constructor Create(x,y: integer; fname: string);   
    /// Создает спрайт, загружая его из файла fname. Файл должен хранить рисунок, представляющий собой 
    ///последовательность кадров одного размера, расположенных по горизонтали. 
    ///Каждый кадр считается имеющим ширину w. Если ширина рисунка в файле fname не кратна w, то возникает исключение. 
    ///После этого при необходимости добавляются состояния методом AddStates и вызывается метод CheckStates 
    ///После создания спрайт отображается на экране в позиции (x,y)
    constructor Create(x,y,w: integer; fname: string); 
    /// Создает спрайт, загружая его из объекта p: Picture. Он должен хранить рисунок, представляющий собой 
    ///последовательность кадров одного размера, расположенных по горизонтали. 
    ///Каждый кадр считается имеющим ширину w. Если ширина рисунка не кратна w, то возникает исключение. 
    ///После этого при необходимости добавляются состояния методом AddStates и вызывается метод CheckStates 
    ///После создания спрайт отображается на экране в позиции (x,y)
    constructor Create(x,y,w: integer; p: Picture);
    /// Создает спрайт - копию спрайта g
    constructor Create(g: SpriteABC);
    /// Удаляет спрайт
    destructor Destroy;
    /// Добавляет состояние к спрайту. После добавления всех состояний следует вызвать CheckStates
    procedure AddState(name: string; count: integer);
    /// Проверяет корректность набора состояний. Вызывается после добавления всех состояний
    procedure CheckStates;
    /// Добавляет кадр к спрайту
    procedure Add(fname: string);
    /// Сохраняет графический и информационный файлы спрайта
    /// Имя fname задает имя графического файла
    /// Информационный файл сохраняется в тот же каталог, что и графический, имеет то же имя и расширение .spinf
    procedure SaveWithInfo(fname: string); 
    /// Переходит к следующему кадру в текущем состоянии
    procedure NextFrame;     
    /// Переходит к следующему тику таймера; если он равен ticks, то он сбрасывается в 1 и вызывается NextFrame
    procedure NextTick;      
    /// Имя состояния
    property StateName: string read GetStateName write SetStateName;
    /// Номер состояния (1..StateCount)
    property State: integer read curst write SetState;
    /// Количество состояний
    property StateCount: integer read GetStateCount;
    /// Скорость спрайта (1..10)
    property Speed: integer read GetSpeed write SetSpeed;
    /// Активность спрайта: True, если спрайт активен (т.е. происходит его анимация), и False в противном случае
    property Active: boolean read act write SetActive;
    /// Текущий кадр в текущем состоянии
    property Frame: integer read GetFrame write SetFrame;
    /// Возвращает количество кадров в текущем состоянии
    function FrameCount: integer;
    /// Возвращает начальный кадр в текущем состоянии
    function FrameBeg: integer;
    /// Возвращает клон объекта
    function Clone0: ObjectABC; override;
    /// Возвращает клон объекта
    function Clone: SpriteABC; 
  end;

/// Стартует анимацию всех спрайтов
procedure StartSprites;
/// Останавливает анимацию всех спрайтов
procedure StopSprites;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

const infoext = '.spinf';
  
var
  _Sprites: System.Collections.ArrayList; // массив спрайтов
  _t: System.Timers.Timer;                // таймер спрайтов
  timerMs: integer;
  
procedure SpriteABC.Init0;
begin
  SetLength(States,1);
  States[0].Name := '';
  States[0].Beg := 1;
  States[0].Count := 1;
  curst := 1;
  curtick := 1;
  Speed := 5;
  act := True;
  _Sprites.Add(Self);
end;

procedure SpriteABC.Init(x,y: integer; fname: string);
begin
  Init0;
  inherited Init(x,y,fname);
end;

procedure SpriteABC.Init(x,y,w: integer; fname: string);
begin
  Init0;
  inherited Init(x,y,w,fname);
  States[0].Count := Count;
end;

procedure SpriteABC.Init(x,y,w: integer; p: Picture);
begin
  Init0;
  inherited Init(x,y,w,p);
  States[0].Count := Count;
end;

procedure SpriteABC.InitWithStates(x,y: integer; fname: string);
var
  vs,sname: string;
  f: PABCSystem.text;
  i,j,w,sp,num: integer;
begin
{  if not FileExists(fname) then
    fname := StandardImageFolder + fname;}
  if not FileExists(fname) then
    raise Exception.Create('Файл '+ExtractFileName(fname)+' не найден');
    
  Init0;

{  s := LowerCase(ExtractFileExt(fname));
  i := Pos(s,fname);
  s := fname;
  Delete(s,i,Length(s));
  s := s + infoext;
  if not FileExists(s) then
    raise Exception.Create('Информационный файл спрайта '+ExtractFileName(s)+' не найден');}

  try
    assign(f,fname);
    reset(f);
    readln(f,vs);
    j := Pos(' ',vs);
    fname := ExtractFilePath(fname)+Copy(vs,1,j-1);
    readln(f,w);
    readln(f,sp);
    Speed := sp;
    readln(f,num);
    for i:=1 to num do
    begin
      readln(f,vs);
      j := Pos(' ',vs);
      sname := Copy(vs,1,j-1);
      Delete(vs,1,j);
      vs := TrimLeft(vs);
      j := Pos(' ',vs);
      if j>0 then
        vs := Copy(vs,1,j-1);
      AddState(sname,StrToInt(vs));
    end;
    close(f);
  except
    on e: Exception do
    begin
      writeln(e);
      raise Exception.Create('Ошибка считывания из информационного файла спрайта');
    end;  
  end;
  inherited Init(x,y,w,fname);
  CheckStates;
end;

procedure SpriteABC.InitBy(g: SpriteABC);
var i: integer;
begin
  inherited InitBy(g);
  Init0;
  SetLength(States,g.States.Length);
  for i:=0 to States.Length-1 do
  begin
    States[i].Beg := g.States[i].Beg;
    States[i].Count := g.States[i].Count;
    States[i].Name := g.States[i].Name;
  end;
end;

constructor SpriteABC.Create(x,y: integer; fname: string);   
begin
  var s := ExtractFileExt(fname);
  if LowerCase(s) = infoext then
    InitWithStates(x,y,fname)
  else Init(x,y,fname);
  InternalDraw;
end;

constructor SpriteABC.Create(x,y,w: integer; fname: string); 
begin
  Init(x,y,w,fname);
  InternalDraw;
end;

constructor SpriteABC.Create(x,y,w: integer; p: Picture);
begin
  Init(x,y,w,p);
  InternalDraw;
end;

constructor SpriteABC.Create(g: SpriteABC);
begin
  InitBy(g);
end;

destructor SpriteABC.Destroy;
begin
  _Sprites.Remove(Self);
  inherited Destroy;
end;

procedure SpriteABC.SaveWithInfo(fname: string);
var
  s,fnameold: string;
  f: PABCSystem.text;
  i: integer;
begin
  CheckStates;
  s:=LowerCase(ExtractFileExt(fname));
  if (s<>'.bmp') and (s<>'.jpg') and (s<>'.gif') and (s<>'.png') then
    raise Exception.Create('Задан неверный формат графического файла');
  Save(fname);
  fnameold := fname;
  i := Pos(s,fname);
  Delete(fname,i,Length(s));
  fname := fname + infoext;
  assign(f,fname);
  rewrite(f);
  writeln(f,ExtractFileName(fnameold),' // имя файла спрайта');
  writeln(f,width,' // ширина кадра');
//  writeln(f,count,' // количество кадров');
  writeln(f,Speed,' // скорость');
  writeln(f,StateCount,' // количество состояний');
  for i:=0 to StateCount-1 do
    if i=0 then
      writeln(f,States[i].Name,' ',States[i].Count,' // имена состояний и количество кадров в них')
    else writeln(f,States[i].Name,' ',States[i].Count);
  close(f);
end;

procedure SpriteABC.CheckStates;
var
  s: integer;
  i: integer;
begin
  s := 0;
  for i:=0 to StateCount-1 do
    s := s + States[i].Count;
  if s<>Count then
    raise Exception.Create('Сумма кадров в состояниях спрайта отличается от общего количества кадров');
end;

procedure SpriteABC.Add(fname: string);
begin
//  Assert(StateCount=1,'при добавлении кадров количество состояний должно быть равно 1');
  inherited Add(fname);
  Inc(States[0].Count);
end;

procedure SpriteABC.NextTick;
begin
  if not act then exit;
  Inc(curtick);
  if curtick>ticks then
  begin
    NextFrame;
    curtick := 1;
  end;
end;

procedure SpriteABC.SetStateName(n: string);
var i,ind: integer;
begin
  ind := -1;
  for i:=0 to States.Length-1 do
    if States[i].Name = n then
    begin
      ind := i;
      break;
    end;
  if ind<>-1 then
    State := ind + 1;
end;

function SpriteABC.GetStateName: string;
begin
  Result := States[curst-1].Name;
end;

procedure SpriteABC.SetState(n: integer);
begin
  if curst=n then
    exit;
  if n<1 then
    n := 1;
  if n>StateCount then
    n := StateCount;
  curst := n;
  CurrentPicture := States[curst-1].Beg;
  Redraw;
end;

function SpriteABC.GetStateCount: integer;
begin
  Result := States.Length;
end;

procedure SpriteABC.SetSpeed(n: integer);
begin
  // пока нет нулевой скорости
  // сделать поправку на изменение тика при уменьшении скорости!
  if n<1 then n := 1;
  if n>10 then n := 10;
  case n of
1: ticks := 30;
2: ticks := 20;
3: ticks := 14;
4: ticks := 10;
5: ticks := 8;
6: ticks := 6;
7: ticks := 4;
8: ticks := 3;
9: ticks := 2;
10: ticks := 1;
  end;
end;

function SpriteABC.GetSpeed: integer;
begin
  case ticks of
30: Result := 1;
20: Result := 2;
14: Result := 3;
10: Result := 4;
8:  Result := 5;
6:  Result := 6;
4:  Result := 7;
3:  Result := 8;
2:  Result := 9;
1:  Result := 10;
  end;
end;

procedure SpriteABC.SetActive(b: boolean);
begin
  act:=b;
end;

procedure SpriteABC.NextFrame;
var n: integer;
begin
  n := Frame + 1;
  if n>States[curst-1].Count then
    n := 1;
  Frame := n;
//  Redraw; // это надо отключать по флагу!
end;

procedure SpriteABC.SetFrame(n: integer);
begin
  if Frame=n then
    exit;
  if n<1 then
    n := 1;
  if n>States[curst-1].Count then
    n := States[curst-1].Count;
  CurrentPicture := States[curst-1].Beg + n - 1;
end;

function SpriteABC.GetFrame: integer;
begin
  Result := CurrentPicture - States[curst-1].Beg + 1;
end;

function SpriteABC.FrameCount: integer;
begin
  Result := States[curst-1].Count;
end;

function SpriteABC.FrameBeg: integer;
begin
  Result := States[curst-1].Beg;
end;

procedure SpriteABC.AddState(name: string; count: integer);
begin
  if (States[0].Name='') then
  begin
//    StateBegs[1]:=1;
    States[0].Count := count;
    States[0].Name := name;
  end
  else
  begin
    var v := States.Length;
    SetLength(States,v+1);
    States[States.Length-1].Beg := States[States.Length-2].Beg + States[States.Length-2].Count;
    States[States.Length-1].Count := count;
    States[States.Length-1].Name := name;
//    StateBegs.Add(StateBegs[StateCount-1]+StateCounts[StateCount-1]);
//    StateCounts.Add(count);
//    StateNames.Add(name);
  end;
end;

function SpriteABC.Clone0: ObjectABC;
begin
  Result := new SpriteABC(Self);
end;

function SpriteABC.Clone: SpriteABC; 
begin
  Result := new SpriteABC(Self);
end;

procedure StartSprites;
begin
  _t.Start;
end;

procedure StopSprites;
begin
  _t.Stop;
end;

var k: integer;

procedure TimerProc(o: Object; e: System.Timers.ElapsedEventArgs);
var i: integer;
begin
  LockDrawingObjects;
  i := 0;
  while i<_Sprites.Count do
  begin
    SpriteABC(_Sprites[i]).NextTick;
    Inc(i);
  end;
  Inc(k);
  //SetWindowCaption(IntToStr(round(k*1000/Milliseconds)));
  RedrawObjects;
end;

var __initialized := false;

procedure __InitModule;
begin
  timerMs := 50; // дискретизация таймера. Потом допустима перекалибровка
  _Sprites := new System.Collections.ArrayList;
  _t := new System.Timers.Timer(timerMs);
  _t.Elapsed += TimerProc;
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
  StartSprites;
end;

initialization
  __InitModule;
finalization
  __FinalizeModule__;
end.