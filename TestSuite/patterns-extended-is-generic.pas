type
  t1 = class end;
  
procedure p1<T>(l: List<t1>);
where T: t1;
begin
  l.RemoveAll(o->(o is T(var a)));
end;

begin 
  var l := Lst(new t1, nil);
  p1&<t1>(l);
  assert(l[0] = nil);
end.