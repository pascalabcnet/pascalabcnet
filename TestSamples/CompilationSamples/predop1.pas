type
  c = class;

  d = class
  public
    v: c;
  end;

  c = class(d)
  public
    vv: d;
  end;
  
var
  x: c;
  y: d;
  
begin
  x := new c;
  y := new d;
  x.vv := y;
  x.v := x;
  y.v := x;
  writeln(x.vv);
  writeln(x.v);
  writeln(y.v);
  readln;
end.