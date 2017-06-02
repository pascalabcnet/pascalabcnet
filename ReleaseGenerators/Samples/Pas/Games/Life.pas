// Игра "Жизнь" на торе
// Оптимизация хешированием по равномерной сетке
uses GraphABC;

const 
/// Ширина клетки
  w = 4;
/// Количество клеток по ширине
  m = 300;
/// Количество клеток по высоте
  n = 220;
/// Отступ поля от левой границы окна
  x0 = 1;
/// Отступ поля от верхней границы окна
  y0 = 21;
  mm = m + 1;
  nn = n + 1;
/// Количество клеток сетки по горизонтали
  mk = 15;
/// Количество клеток сетки по вертикали
  nk = 10;

var
  a,b,sosedia,sosedib: array [0..nn,0..mm] of byte;
  obnovA,obnovB: array [1..nk,1..mk] of boolean;
  CountCells: integer;
  obn: boolean;
  gen: integer;
  hn,hm: integer;

/// Нарисовать ячейку
procedure DrawCell(i,j: integer);
begin
  DrawInBuffer := False;
  SetBrushColor(clBlack);
  FillRectangle(x0+(j-1)*w,y0+(i-1)*w,x0+j*w-1,y0+i*w-1);
  DrawInBuffer := True;
end;

/// Стереть ячейку
procedure ClearCell(i,j: integer);
begin
  DrawInBuffer := False;
  SetBrushColor(clWhite);
  FillRectangle(x0+(j-1)*w,y0+(i-1)*w,x0+j*w-1,y0+i*w-1);
  DrawInBuffer := True;
end;

/// Нарисовать все изменившиеся ячейки
procedure DrawConfiguration;
begin
  for var i:=1 to n do
  for var j:=1 to m do
  begin
    var bb := b[i,j];
    if a[i,j]<>bb then
      if bb=1 then DrawCell(i,j)
        else ClearCell(i,j);
  end;
end;

/// Нарисовать все ячейки
procedure DrawConfigurationFull;
begin
  for var i:=1 to n do
  for var j:=1 to m do
    if b[i,j]=1 then DrawCell(i,j)
      else ClearCell(i,j);
end;

/// Нарисовать поле
procedure DrawField;
begin
  Pen.Color := clLightGray;
  for var i:=0 to m do
  begin
    if i mod hm = 0 then
      Pen.Color := clGray
    else Pen.Color := clLightGray;
    Line(x0+i*w-1,y0,x0+i*w-1,y0+n*w);
  end;
  for var i:=0 to n do
  begin
    if i mod hn = 0 then
      Pen.Color := clGray
    else Pen.Color := clLightGray;
    Line(x0,y0+i*w-1,x0+m*w,y0+i*w-1);
  end;
end;

/// Увеличить массив соседей для данной клетки
procedure IncSosedi(i,j: integer);
var i1,i2,j1,j2: integer;
begin
  if i=1 then i1:=n else i1:=i-1;
  if i=n then i2:=1 else i2:=i+1;
  if j=1 then j1:=m else j1:=j-1;
  if j=m then j2:=1 else j2:=j+1;
  SosediB[i1,j1] += 1;
  SosediB[i1,j]  += 1;
  SosediB[i1,j2] += 1;
  SosediB[i,j1]  += 1;
  SosediB[i,j2]  += 1;
  SosediB[i2,j1] += 1;
  SosediB[i2,j]  += 1;
  SosediB[i2,j2] += 1;
end;

/// Уменьшить массив соседей для данной клетки
procedure DecSosedi(i,j: integer);
var i1,i2,j1,j2: integer;
begin
  if i=1 then i1:=n else i1:=i-1;
  if i=n then i2:=1 else i2:=i+1;
  if j=1 then j1:=m else j1:=j-1;
  if j=m then j2:=1 else j2:=j+1;
  SosediB[i1,j1] -= 1;
  SosediB[i1,j]  -= 1;
  SosediB[i1,j2] -= 1;
  SosediB[i,j1]  -= 1;
  SosediB[i,j2]  -= 1;
  SosediB[i2,j1] -= 1;
  SosediB[i2,j]  -= 1;
  SosediB[i2,j2] -= 1;
end;

/// Поставить ячейку в клетку (i,j)
procedure SetCell(i,j: integer);
begin
  if b[i,j]=0 then
  begin
    b[i,j] := 1;
    obn := True;
    IncSosedi(i,j);
  end;
  CountCells += 1;
end;

/// Убрать ячейку из клетки (i,j)
procedure UnSetCell(i,j: integer);
begin
  if b[i,j]=1 then
  begin
    b[i,j] := 0;
    obn := True;
    DecSosedi(i,j);
  end;
  CountCells -= 1;
end;

/// Инициализировать массивы и конфигурацию поля
procedure Init;
var 
  xc := n div 2;
  yc := m div 2;
begin
  for var i:=0 to n+1 do
  for var j:=0 to m+1 do
    b[i,j] := 0;
  a := b;
  SosediB := b;
  SosediA := SosediB;
  for var ik:=1 to nk do
  for var jk:=1 to mk do
    obnovB[ik,jk] := True;
  obnovA := obnovB;
  CountCells := 0;

  SetCell(xc,yc);
  SetCell(xc,yc+1);
  SetCell(xc,yc+2);
  SetCell(xc-1,yc+2);
  SetCell(xc+1,yc+1);
end;

/// Обработать ячейку
procedure ProcessCell(i,j: integer);
begin
  case SosediA[i,j] of
0..1,4..9:
    if b[i,j]=1 then
    begin
      b[i,j] := 0;
      obn := True;
      DecSosedi(i,j);
      ClearCell(i,j);
      Dec(CountCells);
    end;
3: if b[i,j]=0 then
    begin
      b[i,j] := 1;
      obn := True;
      IncSosedi(i,j);
      DrawCell(i,j);
      Inc(CountCells);
    end;
  end; {case}
end;

/// Перейти к следующему поколению
procedure NextGen;
begin
  for var ik:=1 to nk do
  begin
    for var jk:=1 to mk do
    begin
      obn := False;
      var ifirst := (ik-1)*hn+1;
      var ilast := (ik-1)*hn+hn;
      var jfirst := (jk-1)*hm+1;
      var jlast := (jk-1)*hm+hm;
      if obnovA[ik,jk] then
      begin
        for var i:=ifirst to ilast do
        for var j:=jfirst to jlast do
          ProcessCell(i,j);
      end
      else
      begin
        var ik1,jk1,ik2,jk2: integer;
        if ik=1 then ik1:=nk else ik1:=ik-1;
        if ik=nk then ik2:=1 else ik2:=ik+1;
        if jk=1 then jk1:=mk else jk1:=jk-1;
        if jk=mk then jk2:=1 else jk2:=jk+1;
        var l := obnovA[ik,jk1];
        var r := obnovA[ik,jk2];
        var u := obnovA[ik1,jk];
        var d := obnovA[ik2,jk];
        var lu := obnovA[ik1,jk1];
        var ld := obnovA[ik2,jk1];
        var ru := obnovA[ik1,jk2];
        var rd := obnovA[ik2,jk2];
        if u then
          for var j:=jfirst+1 to jlast-1 do
            ProcessCell(ifirst,j);
        if d then
          for var j:=jfirst+1 to jlast-1 do
            ProcessCell(ilast,j);
        if l then
          for var i:=ifirst+1 to ilast-1 do
            ProcessCell(i,jfirst);
        if r then
          for var i:=ifirst+1 to ilast-1 do
            ProcessCell(i,jlast);
        if u or l or lu then
          ProcessCell(ifirst,jfirst);
        if u or r or ru then
          ProcessCell(ifirst,jlast);
        if d or l or ld then
          ProcessCell(ilast,jfirst);
        if d or r or rd then
          ProcessCell(ilast,jlast);
      end;
      obnovB[ik,jk] := obn;
    end;
  end;
end;

/// Перерисовка содержимого окна
procedure LifeRedrawProc;
begin
  Redraw;
  DrawConfigurationFull;
end;

/// Вывод номера поколения и количества ячеек
procedure DrawInfo;
begin
  Brush.Color := clWhite;
  TextOut(25,0,'Поколение '+IntToStr(gen));
  TextOut(WindowWidth - 130,0,'Жителей: '+IntToStr(CountCells)+'    ');
end;

begin
  SetConsoleIO;
  if (m mod mk<>0) or (n mod nk<>0) then
  begin
    writeln('Размер кластера не согласован с размером поля. Программа завершена');
    exit
  end;
  hm := m div mk;
  hn := n div nk;
  SetWindowSize(x0+m*w,y0+n*w);
  CenterWindow;
  Window.Title := 'Игра "Жизнь"';
  Window.IsFixedSize := True;
  Font.Name := 'Arial';
  Font.Size := 10;
  Init;
  
  LockDrawing;
  DrawInfo;
  DrawField;
  DrawConfiguration;
  UnLockDrawing;
  
  var mil := Milliseconds;
  gen := 0;
  RedrawProc := LifeRedrawProc;
  while True do
  begin
    gen += 1;
    
    if gen mod 11 = 0 then
      DrawInfo;

    SosediA := SosediB;
    obnovA := obnovB;
    NextGen;
    
    if gen mod 1000 = 0 then
    begin
      var mil1 := Milliseconds;
      writeln(gen,'  ',(mil1-mil)/1000);
      mil := mil1;
    end;
  end;
end.
