// 87.117.14.184
// Игра в 15
uses GraphABC,ABCObjects,ABCButtons,Events;

const
  n = 4;
  sz = 100;
  zz = 10;
  x0 = 20;
  y0 = 20;

var
  p: array [1..n,1..n] of SquareABC;
  digits: array [1..n*n-1] of integer;
  MeshButton: ButtonABC;
  StatusRect: RectangleABC;

  EmptyCellX,EmptyCellY: integer;
  MovesCount: integer;
  EndOfGame: boolean;  // True если все фишки стоят на своих местах

procedure Swap(var x,y: integer);
var v: integer;
begin
  v:=x;
  x:=y;
  y:=v;
end;

// Поменять местами две фишки
procedure Swap(var p,p1: SquareABC);
var
  v: SquareABC;
  i: integer;
begin
  v:=p;
  p:=p1;
  p1:=v;
  i:=p.Left;
  p.Left:=p1.Left;
  p1.Left:=i;
  i:=p.Top;
  p.Top:=p1.Top;
  p1.Top:=i;
end;

// Определить, являются ли клетки соседями
function Sosedi(x1,y1,x2,y2: integer): boolean;
begin
  Result:=(abs(x1-x2)=1) and (y1=y2) or (abs(y1-y2)=1) and (x1=x2)
end;

// Заполнить вспомогательный массив цифр
procedure FillDigitsArr;
var i: integer;
begin
  for i:=1 to n*n-1 do
    digits[i]:=i;
end;

// Перемешать вспомогательный массив цифр. Количество обменов должно быть четным
procedure MeshDigitsArr;
var i,x: integer;
begin
  for i:=1 to n*n-1 do
  begin
    repeat
      x:=Random(15)+1;
    until x<>i;
    Swap(digits[i],digits[x]);
  end;
  if n mod 2=0 then
    Swap(digits[1],digits[2]); // количество обменов должно быть четным
end;

// Заполнить двумерный массив фишек. Вместо пустой ячейки - белая фишка с числом 0
procedure Fill15ByDigitsArr;
var x,y,i: integer;
begin
  Swap(p[EmptyCellY,EmptyCellX],p[n,n]); // Переместить пустую фишку в правый нижний угол
  EmptyCellX:=n;
  EmptyCellY:=n;
  i:=1;
  for y:=1 to n do
  for x:=1 to n do
  begin
    if x*y=n*n then exit;
    p[y,x].Number:=digits[i];
    Inc(i);
  end;
end;

// Перемешать массив фишек
procedure Mesh15;
begin
  MeshDigitsArr;
  Fill15ByDigitsArr;
  MovesCount:=0;
  EndOfGame:=False;
  StatusRect.Text:='Количество ходов: '+IntToStr(MovesCount);
  StatusRect.Color:=RGB(200,200,255);
end;

// Создать массив фишек
procedure Create15;
var x,y: integer;
begin
  EmptyCellX:=n;
  EmptyCellY:=n;
  for x:=1 to n do
  for y:=1 to n do
  begin
    p[y,x]:=SquareABC.Create(x0+(x-1)*(sz+zz),y0+(y-1)*(sz+zz),sz,clMoneyGreen);
    p[y,x].BorderColor:=clGreen;
    p[y,x].BorderWidth:=2;
    p[y,x].TextScale:=0.7;
  end;
  p[EmptyCellY,EmptyCellX].Color:=clWhite;
  p[EmptyCellY,EmptyCellX].BorderColor:=clWhite;
  FillDigitsArr;
  MeshDigitsArr;
  Fill15ByDigitsArr;
end;

// Проверить, все ли фишки стоят на своих местах
function IsSolution: boolean;
var x,y,i: integer;
begin
  Result:=True;
  i:=1;
  for y:=1 to n do
  for x:=1 to n do
  begin
    if p[y,x].Number<>i then
    begin
      Result:=False;
      break;
    end;
    Inc(i);
    if i=n*n then i:=0;
  end;
end;

procedure MouseDown(x,y,mb: integer);
var fx,fy: integer;
begin
  if EndOfGame then // Если все фишки на своих местах, то не реагировать на мышь и ждать нажатия кнопки "Перемешать"
    exit;
  if ObjectUnderPoint(x,y)=nil then // Eсли мы щелкнули не на объекте, то не реагировать на мышь
    exit;
  fx:=(x-x0) div (sz+zz) + 1; // Вычислить координаты на доске для ячейки, на которой мы щелкнули мышью
  fy:=(y-y0) div (sz+zz) + 1;
  if (fx>n) or (fy>n) then
    exit;
  if Sosedi(fx,fy,EmptyCellX,EmptyCellY) then // Если ячейка соседствует с пустой, то поменять их местами
  begin
    Swap(p[EmptyCellY,EmptyCellX],p[fy,fx]);
    EmptyCellX:=fx;
    EmptyCellY:=fy;
    Inc(MovesCount);
    StatusRect.Text:='Количество ходов: '+IntToStr(MovesCount);
    if IsSolution then
    begin
      StatusRect.Text:='Победа! Сделано ходов: '+IntToStr(MovesCount);
      StatusRect.Color:=RGB(255,200,200);
      EndOfGame:=True;
    end
  end;
end;

begin
  SetSmoothingOff;
  SetWindowCaption('Игра в 15');
  SetWindowSize(2*x0+(sz+zz)*n-zz,2*y0+(sz+zz)*n-zz+90);
  
  EndOfGame:=False;
  Create15;
  MeshButton:=ButtonABC.Create((WindowWidth-200) div 2,2*y0+(sz+zz)*n-zz,200,'Перемешать',clLightGray);
  MeshButton.OnClick:=Mesh15;
  
  StatusRect:=RectangleABC.Create(0,WindowHeight-40,WindowWidth,40,RGB(200,200,255));
  StatusRect.TextVisible:=True;
  MovesCount:=0;
  StatusRect.Text:='Количество ходов: '+IntToStr(MovesCount);
  StatusRect.BorderWidth:=2;
  StatusRect.BorderColor:=RGB(80,80,255);//StatusRect.Color;

  OnMouseDown:=MouseDown;
end.
