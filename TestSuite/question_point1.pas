procedure p1<T>(x: T);
begin
  var o := x;
  var ot := o?.GetType;
  assert(ot = typeof(integer));
end;

begin 
p1(2);
end.