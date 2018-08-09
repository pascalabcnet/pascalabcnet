function f := 123;
 
begin
  var ff: ()->integer := f;
  var r: object := ff;
  Assert(r.Equals(123));
end.