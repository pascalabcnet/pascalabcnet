uses GraphABC;

type TByteArray = array of byte;
const frames = 25;
      size   = 250;
      dxy    = size div 2;
      dm     = 2*PI/1024;
      flameh = 4;            
      Light: byte = 255;
      
procedure FillPallete(ColorsTable: array of Color);
begin
  for var i:=0 to 255 do
    if(i<128) then
      ColorsTable[i] := RGB(i,0,i div 2)
    else
      ColorsTable[i] := RedColor(i);
end;

begin  
  //Создаюм буфер экрана
  var ScreenBuffer := new TByteArray[size+1];
  for var i:=0 to size do
    ScreenBuffer[i] := new byte[size+1];
  //Создаем палитру
  var ColorsTable := new Color[256];
  FillPallete(ColorsTable);
  //Настраиваем окно
  SetWindowSize(size,size);
  SetBrushColor(clBlack);
  FillRectangle(0,0,WindowWidth,WindowHeight);
  SetSmoothingOff;
  LockDrawing;
  //Поехали
  var x, y, s, tt, xx, yy: Integer;
  var dt := System.DateTime.Now;
  var ds := WindowWidth div 4;
  repeat
    tt := tt + 1;
    xx := dxy + Round(ds*Sin(tt*dm));
    yy := dxy + Round(ds*Cos(tt*dm));
    ScreenBuffer[xx,yy] := Light;
    SetPixel(xx,yy,ColorsTable[Light]);
    for var i:=0 to 5 do begin
      x := Random(size-1) + 1;
      y := Random(size-1) + 1;
      s := ScreenBuffer[Y,X];
      if s>=flameh then 
        s := s - flameh;
      if s=0 then 
        continue;
      ScreenBuffer[y-1,x+1] := s;
      ScreenBuffer[y-1,x  ] := s;
      ScreenBuffer[y-1,x-1] := s;
      ScreenBuffer[y+1,x  ] := s;
      var c := ColorsTable[s];
      SetPixel(y-1,x+1,c);
      SetPixel(y-1,x,  c);
      SetPixel(y-1,x-1,c);
      SetPixel(y+1,x,  c);
    end;
    if((system.datetime.Now-dt).TotalMilliseconds>1000/frames) then begin
      dt := System.Datetime.Now;
      Redraw;
    end;
  until False;
end.
