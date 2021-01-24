type
  t1 = class
    l: List<Action0> := new List<Action0>;
    static procedure operator+=(a: t1; d: Action0);
    begin
      a.l.Add(d);
    end;
    
  end;

var i: integer;
  
begin
  var a := new t1;
  a += ()->Inc(i);
  a.l[0]();
  assert(i = 1);
end.