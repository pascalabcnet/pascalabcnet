type
  t1 = class
    
    static procedure operator+=(a: t1; d: System.Action) := exit;
    
  end;
  
begin
  var a := new t1;
  a *= ()->Writeln(1);
end.