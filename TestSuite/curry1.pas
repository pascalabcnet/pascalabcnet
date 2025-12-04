function Curry<T1,T2,T3>(f: (T1,T2)->T3; t: T2): T1->T3;
begin
  if f=nil then
    raise new System.ArgumentNullException('f');
  Result := x -> f(x,t)
end;

begin
  var f: (integer, integer)->integer := (x,y)->x+y;
  var f2: integer->integer := Curry(f,2);
  assert(f2(3)=5);
end.