// Класс штампа составного объекта
uses GraphABC;

type 
  TextStamp = class
    x,y,pt: integer;
    Text: string;
    constructor (xx,yy,ppt: integer; t: string);
    begin
      x := xx; y := yy;
      pt := ppt; 
      text := t;
    end;
    procedure Stamp;
    begin
      Font.Size := pt;
      Brush.Color := clWhite;
      TextOut(x,y,text);
    end;
    procedure MoveOn(dx,dy: integer);
    begin
      x += dx; y += dy;
    end;
  end;
  
  RectangleStamp = class
    x,y,w,h: integer;
    constructor (xx,yy,ww,hh: integer);
    begin
      x := xx; y := yy;
      w := ww; h := hh;
    end;
    procedure Stamp;
    begin
      Brush.Color := clRandom;
      Rectangle(x,y,x+w,y+h);
    end;
    procedure MoveOn(dx,dy: integer);
    begin
      x += dx; y += dy;
    end;
  end;
  
  RectWithTextStamp = class
    x,y,w,h: integer;
    Text: string;
    constructor (xx,yy,ww,hh: integer; t: string);
    begin
      x := xx; y := yy;
      w := ww; h := hh;
      text := t;
    end;
    procedure Draw;
    begin
      var r := new RectangleStamp(x,y,w,-h);
      var t := new TextStamp(x,y+3,10,Text);
      r.Stamp;
      t.Stamp;
    end;
    procedure MoveOn(dx,dy: integer);
    begin
      x += dx; y += dy;
    end;
  end;
  
begin
  var a: array of integer := (100,70,50,120,90,200,111,150,230,11,44);
  var rt := new RectWithTextStamp(100,300,30,a[0],IntToStr(a[0]));
  rt.Draw;
  for var i:=1 to a.Length-1 do
  begin
    rt.MoveOn(40,0);
    rt.h := a[i];
    rt.Text := IntToStr(a[i]);
    rt.Draw;
  end;
end. 