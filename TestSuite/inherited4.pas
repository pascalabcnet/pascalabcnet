type
  t0 = class
    function f1(i: integer); virtual := i;
  end;
  
  t1 = class(t0)  
    function f1(i: integer): integer; override;
    begin
      Result := inherited; // не разрешает
    end;  
  end;
  
begin
  var a := new t1;
  assert(a.f1(5) = 5);
end.