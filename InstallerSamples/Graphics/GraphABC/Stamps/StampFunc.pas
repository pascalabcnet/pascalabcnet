// Класс штампа графика функции
uses GraphABC;

type 
  FuncType = function (r: real): real;
  FuncStamp = class
    xs0,ys0,ws,hs: integer;
    xf0,yf0,wf,hf: real;
    f: FuncType;
    constructor (xs0p,ys0p,xs1p,ys1p: integer; xf0p,yf0p,xf1p,yf1p: real; ff: FuncType);
    begin
      SetScreenWindow(xs0p,ys0p,xs1p,ys1p);
      SetFuncWindow(xf0p,yf0p,xf1p,yf1p);
      f := ff;
    end;
    function WorldToScreenX(xf: real): integer;
    begin
      var a := ws/wf;
      var b := xs0-a*xf0;
      Result := Round(a * xf + b);
    end;
    function WorldToScreenY(yf: real): integer;
    begin
      var c := hs/hf;
      var d := ys0-c*yf0;
      Result := hs + 2*ys0 - Round(c * yf + d);
    end;
    procedure Stamp;
    const n = 100;
    begin
      Pen.Color := Color.Gray;
      Rectangle(xs0,ys0,xs0+ws,ys0+hs);
      Pen.Color := Color.Black;
      var x := xf0;
      var y := f(x);
      var h := wf/n;
      var xs := WorldToScreenX(x);
      var ys := WorldToScreenY(y);
      MoveTo(xs,ys);
      for var i:=1 to n do
      begin
        x += h;
        y := f(x);
        xs := WorldToScreenX(x);
        ys := WorldToScreenY(y);
        LineTo(xs,ys);
      end;  
    end;
    procedure SetScreenWindow(xs0p,ys0p,xs1p,ys1p: integer);
    begin
      xs0 := xs0p; ys0 := ys0p;
      ws := xs1p-xs0p; hs := ys1p-ys0p;
    end;
    procedure SetFuncWindow(xf0p,yf0p,xf1p,yf1p: real);
    begin
      xf0 := xf0p; yf0 := yf0p;
      wf := xf1p-xf0p; hf := yf1p-yf0p;
    end;
    procedure MoveOn(dx,dy: integer);
    begin
      xs0 += dx; ys0 += dy;
    end;
  end;
  
begin
  var fs := new FuncStamp(10,10,310,230,0,-2*Pi,2*Pi,2*Pi,x->x*sin(5*x));
  fs.Stamp;
  fs.MoveOn(320,0);
  fs.SetFuncWindow(-Pi,-1,Pi,1);
  fs.f := sin;
  fs.Stamp;
  fs.MoveOn(-320,240);
  fs.f := cos;
  fs.Stamp;
  fs.MoveOn(320,0);
  fs.SetFuncWindow(-2*Pi,-2,2*Pi,2);
  fs.f := x->sin(3*x)+sin(4*x);
  fs.Stamp;
end. 