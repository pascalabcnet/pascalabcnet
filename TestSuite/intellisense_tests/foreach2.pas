type
  t1 = class(System.Collections.Generic.IEnumerable<real>)
  end;
  
  t2 = class(System.Collections.IEnumerable)
    public function GetEnumerator: System.Collections.IEnumerator := Arr(new object).GetEnumerator;
  end;
  
begin
  foreach var o{@var o: real;@} in new t1 do
    writeln(o.GetType);
  foreach var o{@var o: Object;@} in new t2 do
    writeln(o.GetType);
end.