type
  t0 = class
  end;
  
procedure p1<T>;
where T: t0;
begin
  var o: T;
  var p := procedure -> o := o;
end;

begin
  p1&<t0>;
end.