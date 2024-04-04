// Игра "Жизнь" на торе
// Оптимизация хешированием по равномерной сетке
uses GraphWPF;

const 
/// Ширина клетки
  w = 4.0;
/// Количество клеток по ширине
  m = 300;
/// Количество клеток по высоте
  n = 220;
/// Отступ поля от левой границы окна
  x0 = 0;
/// Отступ поля от верхней границы окна
  y0 = 21;
/// Количество клеток сетки по горизонтали
  mk = 15;
/// Количество клеток сетки по вертикали
  nk = 10;

var
  configuration,NeighborsA,NeighborsB: array [0..n+1,0..m+1] of byte;
  updateA,updateB: array [1..nk,1..mk] of boolean;
  CountCells: integer;
  updateFlag: boolean;
  generation: integer;
  
  // визуальные компоненты для прорисовки как единого целого
  configurationVisual: DrawingVisual;
  infoVisual: DrawingVisual;

/// Нарисовать все изменившиеся ячейки
procedure DrawConfiguration;
begin
  DrawOnVisual(configurationVisual,dc->begin
    for var i:=1 to n do
    for var j:=1 to m do
      if configuration[i,j]=1 then
        DrawRectangleDC(dc,x0+(j-1)*w+0.5,y0+(i-1)*w+0.5,w-1,w-1,Brushes.Black,nil);
  end);  
end;

/// Нарисовать все изменившиеся ячейки
procedure DrawInfo;
begin
  DrawOnVisual(infoVisual,dc->begin
    TextOutDC(dc,14,0,'Поколение: '+generation);
    TextOutDC(dc,Window.Width - 110,0,'Жителей: '+CountCells);
  end);  
end;

/// Нарисовать поле
procedure DrawField;
begin
  var hm := m div mk;
  var hn := n div nk;
  Pen.Color := Colors.LightGray;
  for var i:=0 to m do
    if i mod hm <> 0 then
      Line(x0+i*w,y0,x0+i*w,y0+n*w);
  for var i:=0 to n do
    if i mod hn <> 0 then
      Line(x0,y0+i*w,x0+m*w,y0+i*w);
  Pen.Color := Colors.Gray;
  for var i:=0 to m do
    if i mod hm = 0 then
      Line(x0+i*w,y0,x0+i*w,y0+n*w);
  for var i:=0 to n do
    if i mod hn = 0 then
      Line(x0,y0+i*w,x0+m*w,y0+i*w);
  FlushDrawingToBitmap;
end;

/// Увеличить массив соседей для данной клетки
procedure IncNeighbors(i,j: integer);
begin
  var i1,i2,j1,j2: integer;
  if i=1 then i1:=n else i1:=i-1;
  if i=n then i2:=1 else i2:=i+1;
  if j=1 then j1:=m else j1:=j-1;
  if j=m then j2:=1 else j2:=j+1;
  NeighborsB[i1,j1] += 1;
  NeighborsB[i1,j]  += 1;
  NeighborsB[i1,j2] += 1;
  NeighborsB[i,j1]  += 1;
  NeighborsB[i,j2]  += 1;
  NeighborsB[i2,j1] += 1;
  NeighborsB[i2,j]  += 1;
  NeighborsB[i2,j2] += 1;
end;

/// Уменьшить массив соседей для данной клетки
procedure DecNeighbors(i,j: integer);
begin
  var i1,i2,j1,j2: integer;
  if i=1 then i1:=n else i1:=i-1;
  if i=n then i2:=1 else i2:=i+1;
  if j=1 then j1:=m else j1:=j-1;
  if j=m then j2:=1 else j2:=j+1;
  NeighborsB[i1,j1] -= 1;
  NeighborsB[i1,j]  -= 1;
  NeighborsB[i1,j2] -= 1;
  NeighborsB[i,j1]  -= 1;
  NeighborsB[i,j2]  -= 1;
  NeighborsB[i2,j1] -= 1;
  NeighborsB[i2,j]  -= 1;
  NeighborsB[i2,j2] -= 1;
end;

/// Поставить ячейку в клетку (i,j)
procedure SetCell(i,j: integer);
begin
  if configuration[i,j] = 0 then
  begin
    configuration[i,j] := 1;
    updateFlag := True;
    IncNeighbors(i,j);
    CountCells += 1;
  end;
end;

/// Убрать ячейку из клетки (i,j)
procedure UnSetCell(i,j: integer);
begin
  if configuration[i,j] = 1 then
  begin
    configuration[i,j] := 0;
    updateFlag := True;
    DecNeighbors(i,j);
    CountCells -= 1;
  end;
end;

/// Обработать ячейку (основные правила игры Жизнь)
procedure ProcessCell(i,j: integer);
begin
  case NeighborsA[i,j] of
0..1,4..9: UnSetCell(i,j);
3: SetCell(i,j);
  end; 
end;

/// Перейти к следующему поколению
procedure NextGen;
begin
  var hm := m div mk;
  var hn := n div nk;
  for var ik:=1 to nk do // двойной цикл по ячейкам хеширования
  begin
    for var jk:=1 to mk do
    begin
      updateFlag := False; // обновлены ли данные в ячейке хеширования
      var ifirst := (ik-1)*hn+1;
      var ilast := (ik-1)*hn+hn;
      var jfirst := (jk-1)*hm+1;
      var jlast := (jk-1)*hm+hm;
      if updateA[ik,jk] then
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
        var l := updateA[ik,jk1];
        var r := updateA[ik,jk2];
        var u := updateA[ik1,jk];
        var d := updateA[ik2,jk];
        var lu := updateA[ik1,jk1];
        var ld := updateA[ik2,jk1];
        var ru := updateA[ik1,jk2];
        var rd := updateA[ik2,jk2];
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
      updateB[ik,jk] := updateFlag;
    end;
  end;
end;

procedure InitWindow;
begin
  if (m mod mk<>0) or (n mod nk<>0) then
  begin
    Writeln('Размер кластера не согласован с размером поля. Программа завершена');
    exit
  end;
  Window.SetSize(x0+m*w,y0+n*w);
  Window.CenterOnScreen;
  Window.Title := 'Игра "Жизнь"';
  //Window.IsFixedSize := True;
  Font.Name := 'Arial';
  Font.Size := 14;
end;

/// Инициализировать массивы и конфигурацию поля
procedure InitArrays;
begin
  for var i:=0 to n+1 do
  for var j:=0 to m+1 do
    configuration[i,j] := 0;
  NeighborsB := configuration;
  NeighborsA := NeighborsB;
  for var ik:=1 to nk do
  for var jk:=1 to mk do
    updateB[ik,jk] := True;
  updateA := updateB;
  CountCells := 0;
end;

var yc := n div 2;
var xc := m div 2;

/// Инициализировать начальную конфигурацию
procedure InitConfiguration;
begin
  SetCell(yc,xc);
  SetCell(yc,xc+1);
  SetCell(yc,xc+2);
  SetCell(yc-1,xc+2);
  SetCell(yc+1,xc+1);
  
  {var tx := -3;
  var ty := -4;
  SetCell(tx + xc,ty + yc);
  SetCell(tx + xc,ty + yc+1);
  SetCell(tx + xc,ty + yc+2);
  SetCell(tx + xc-1,ty + yc+2);
  SetCell(tx + xc-2,ty + yc+1);
  
  SetCell(xc,yc);
  SetCell(xc+1,yc+1);
  SetCell(xc+1,yc-1);
  SetCell(xc+2,yc+1);
  SetCell(xc+2,yc-1);
  SetCell(xc+3,yc);}
end;



begin
  InitWindow;
  InitArrays;
  InitConfiguration;
  
  DrawField;
  
  // визуальный элемент для рисования конфигурации
  // при повторном рисовании на нем старое изображение автоматически стирается
  configurationVisual := CreateVisual;
  // визуальный элемент для рисования инфомации
  infoVisual := CreateVisual;
  
  DrawConfiguration;
  DrawInfo;
  
  MillisecondsDelta;
  generation := 0;
  while True do
  begin
    generation += 1;

    NeighborsA := NeighborsB;
    updateA := updateB;
    NextGen;
    //Sleep(30);
    DrawConfiguration;
    if generation mod 10 = 0 then
      DrawInfo;
    if generation mod 1000 = 0 then
      Writeln('Поколение ',generation,': ',MillisecondsDelta/1000,' с');
  end;
end.
