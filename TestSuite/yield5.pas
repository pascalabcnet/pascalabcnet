type
  c1<T1> = class
    
    function f1<T2>: sequence of T2;
    begin
      
      var s: T2;
      var s1: T1;
      yield s;
    end;
    
  end;
  
begin 
  var o := new c1<integer>;
  var i: integer;
  foreach var e in o.f1&<integer> do
  begin
    assert(e = 0);
    Inc(i);
  end;
  assert(i = 1);
end.