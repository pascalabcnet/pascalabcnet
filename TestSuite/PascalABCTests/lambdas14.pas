var i: integer;
type
  t1 = class
    
    static procedure operator+=(a: t1; d: System.Action<integer>);
    begin
      i := 1;
    end;
    static procedure operator+=(a: t1; d: Action0);
    begin
      i := 2;
    end;
  end;
  
begin
  var a := new t1;
  a += ()->Writeln(1);
  assert(i = 2);
  a += (x: integer)->Writeln(1);
  assert(i = 1);
end.